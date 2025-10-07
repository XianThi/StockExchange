using Microsoft.AspNetCore.Mvc;
using StockExchange.WebUI.Models;
using StockService;

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

        [HttpPost]
        public async Task<IActionResult> Buy([FromBody] BuyRequestModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { error = "Kullanıcı girişi gerekli." });
            try
            {
                var request = new BuyStockRequest
                {
                    UserId = userId.Value,
                    Symbol = model.Symbol,
                    Quantity = model.Quantity
                };
                var response = await _client.BuyStockAsync(request);
                if (response.Success == 1)
                {
                    HttpContext.Session.SetString("Balance", response.UpdatedUser.Balance.ToString());
                    return Json(new { success = true, newBalance = response.UpdatedUser.Balance });
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
    }
}
