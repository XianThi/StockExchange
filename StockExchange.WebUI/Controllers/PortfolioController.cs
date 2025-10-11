using Microsoft.AspNetCore.Mvc;
using StockService;
using System.Globalization;

namespace StockExchange.WebUI.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly StockServiceClient _client;

        public PortfolioController(StockServiceClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            try
            {
                var portfolioResponse = await _client.GetPortfolioAsync(userId.Value);

                if (portfolioResponse.Success)
                {
                    return View(portfolioResponse);
                }

                TempData["Error"] = portfolioResponse.Message;
                return RedirectToAction("Index", "Stock");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Portföy yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "Stock");
            }
        }

        public async Task<IActionResult> Transactions()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            try
            {
                var transactionsResponse = await _client.GetTransactionHistoryAsync(userId.Value);

                if (transactionsResponse.Success)
                {
                    return View(transactionsResponse);
                }

                TempData["Error"] = transactionsResponse.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "İşlem geçmişi yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSellOrder([FromBody] SellOrderRequest model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { error = "Kullanıcı girişi gerekli." });

            try
            {
                model.UserId = userId.Value;
                var response = await _client.AddSellOrderAsync(model);

                if (response.Success == true)
                {
                    return Json(new { success = true, message = response.Message });
                }
                else
                {
                    return BadRequest(new { error = "Satın alma işlemi başarısız oldu." + response.Message ?? "" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPortfolioData()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { error = "Kullanıcı girişi gerekli." });

            try
            {
                var portfolioResponse = await _client.GetPortfolioAsync(userId.Value);

                if (portfolioResponse.Success)
                {
                    return Json(new
                    {
                        success = true,
                        summary = portfolioResponse.Summary,
                        items = portfolioResponse.Items
                    });
                }
                else
                {
                    return BadRequest(new { error = portfolioResponse.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseHistory(string symbol)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { error = "Kullanıcı girişi gerekli." });

            try
            {
                var purchaseHistory = await _client.GetUserStockPurchaseHistoryAsync(userId.Value, symbol);

                return Json(new
                {
                    success = true,
                    purchaseHistory = purchaseHistory
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
