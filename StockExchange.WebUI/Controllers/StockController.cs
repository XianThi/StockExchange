using Microsoft.AspNetCore.Mvc;
using StockExchange.WebUI.Models;
using StockService;
using System.Reflection;

namespace StockExchange.WebUI.Controllers
{
    public class StockController : Controller
    {
        private readonly StockServiceClient _client;
        public StockController(StockServiceClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks(int page = 1, int pageSize = 25)
        {
            try
            {
                var response = await _client.GetStocksPagedAsync(page, pageSize);

                return Json(new
                {
                    stocks = response.Stocks,
                    totalCount = response.TotalCount,
                    totalPages = response.TotalPages,
                    currentPage = response.PageNumber,
                    pageSize = response.PageSize
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPriceHistory(string symbol)
        {
            try
            {
                var priceHistories = await _client.GetPriceHistoryWithSymbolAsync(symbol);
                if (priceHistories == null)
                    return NotFound(new { error = "Fiyat bilgisi bulunamadı." });
                return Json(priceHistories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Buy([FromBody] BuyRequestModel model)
        {
            return BadRequest(new { error = "Satın alma işlemi geçici olarak devre dışı bırakıldı." });
            // satın alma işlemi artık buyorder ile yapılacak
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { error = "Kullanıcı girişi gerekli." });
            try
            {
                var request = new BuyStockRequest
                {
                    UserId = userId.Value,
                    StockId = model.StockId,
                    Quantity = model.Quantity
                };
                var response = await _client.BuyStockAsync(request);
                
                if (response.Success == true)
                {
                    HttpContext.Session.SetString("Balance", response.NewBalance.ToString());
                    return Json(new { success = true, newBalance = response.NewBalance});
                }
                else
                {
                    return BadRequest(new { error = "Satın alma işlemi başarısız oldu."+ response.Message ?? "" });
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBuyOrder([FromBody] BuyOrderRequest model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { error = "Kullanıcı girişi gerekli." });
            try
            {
                model.UserId = (int)userId;
                var response = await _client.AddBuyOrderAsync(model);

                if (response.Success == true)
                {
                    return Json(new { success = true, message = response.Message});
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
    }
}
