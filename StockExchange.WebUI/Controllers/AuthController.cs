using Microsoft.AspNetCore.Mvc;
using StockExchange.Core.Models.UserModel;
using StockService;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace StockExchange.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly StockServiceClient _client;

        public AuthController(StockServiceClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(StockService.LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var response = await _client.LoginAsync(request);
            
            if (response.Success==1)
            {
                HttpContext.Session.SetInt32("UserId", response.UserId);
                HttpContext.Session.SetString("Username", response.Username);
                HttpContext.Session.SetString("Email", response.Email);
                HttpContext.Session.SetString("Balance", response.Balance.ToString(CultureInfo.InvariantCulture));
                return RedirectToAction("Index", "Stock");
            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View(request);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Stock");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(StockService.CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var response = await _client.CreateUserAsync(request);
            if (response.Success == 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            ModelState.AddModelError("", "Kayıt işlemi başarısız oldu. Lütfen bilgilerinizi kontrol edin.");
            return View(request);
        }
    }
}
