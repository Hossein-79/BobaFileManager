using BobaFileManager.Models;
using BobaFileManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BobaFileManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INetherumService _netherumService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, INetherumService netherumService, IUserService userService)
        {
            _logger = logger;
            _netherumService = netherumService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string address, string signature)
        {
            var massage = "Login with " + address;
            var publicAddress = _netherumService.VerifySignature(massage, address, signature);

            if (publicAddress is null)
                return Json(false);

            var user = new User { Address = publicAddress };
            await _userService.TryAdd(user);

            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Address)},
                    CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            });

            return Json(true);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
