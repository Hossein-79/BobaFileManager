using BobaFileManager.Helpers;
using BobaFileManager.Models;
using BobaFileManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private readonly IBundlrService _bundlrService;
        private readonly IUserFileService _userFileService;

        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IUserFileService userFileService, IWebHostEnvironment environment, INetherumService netherumService, IUserService userService, IBundlrService bundlrService)
        {
            _logger = logger;
            _netherumService = netherumService;
            _userService = userService;
            _bundlrService = bundlrService;
            _userFileService = userFileService;

            _hostEnvironment = environment;
        }

        public async Task<IActionResult> Index(string text)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUser(User.Identity.Name);
                var files = await _userFileService.GetUserFiles(user.UserId);

                ViewBag.Balance = user.Balance;

                return View("Dashboard", files);
            }
            return View();
        }

        //[Authorize]
        //public async Task<IActionResult> UploadText(string text)
        //{
        //    var user = await _userService.GetUser(User.Identity.Name);


        //    //System.IO.File.WriteAllText(path, text);
        //    long length = new System.IO.FileInfo(path).Length;

        //    var fee = _bundlrService.GetUploadFee(length);

        //    return Json(new { length, path, fee });
        //}

        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            //var rrr = new UserFile
            //{
            //    UserFileId = 1,
            //    UserId = 1,
            //    FileName = "ticket_1037149696.pdf",
            //    Extension = ".pdf",
            //    Length = 162593,
            //    LocalName = "1-839598.pdf",
            //    ArweaveUrl = "https://arweave.net/4mcsgP3kVbT-0I4UEhwmE6F3OJ2wNryhWYHWQP5-Z2w",
            //    IsUploaded = true,
            //    UploadTime = DateTime.UtcNow,
            //};
            //return PartialView("_UserFilePartial", rrr);

            if (file is null || file.Length <= 0)
                return Json(new { Success = false, Msg = "Select File" });

            if (file.Length > 20000000L)
                return Json(new { Success = false, Msg = "File size must under 20MB" });

            var user = await _userService.GetUser(User.Identity.Name);

            var fee = _bundlrService.GetUploadFee(file.Length);
            if (user.Balance < fee)
                return Json(new { Success = false, Msg = "Not enough balance" });

            var extension = file.FileName[file.FileName.LastIndexOf(".")..];
            var fileName = $"{user.UserId}-{Helper.GenerateRandomNumber(6)}{extension}";
            var path = _hostEnvironment.ContentRootPath + $"\\UserFiles\\{fileName}";

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileInfo = new UserFile
            {
                UserId = user.UserId,
                FileName = file.FileName,
                Extension = extension,
                IsUploaded = false,
                ArweaveUrl = null,
                Length = file.Length,
                LocalName = fileName,
                UploadTime = DateTime.UtcNow,
            };
            await _userFileService.Add(fileInfo);


            var arweaveUrl = _bundlrService.UploadFile(path);
            if (arweaveUrl is null)
                return Json(new { Success = false, Msg = "upload to blockchain exeption" });

            fileInfo.ArweaveUrl = arweaveUrl;
            fileInfo.IsUploaded = true;
            await _userFileService.Update(fileInfo);

            user.Balance -= fee;
            await _userService.Update(user);

            return PartialView("_UserFilePartial", fileInfo);
        }

        public IActionResult GetFee(long length)
        {
            var fee = _bundlrService.GetUploadFee(length);
            //var fee = 0.000089631437673069m;
            return Json(fee);
        }

        public IActionResult Test()
        {
            var t = _bundlrService.Test(10000);
            return Content(t);
        }

        public async Task<IActionResult> Login(string address, string signature)
        {
            if (User.Identity.IsAuthenticated)
                return Json(true);

            var massage = "Login with " + address;
            var publicAddress = _netherumService.VerifySignature(massage, address, signature);

            if (publicAddress is null)
                return Json(false);

            var user = new User
            {
                Address = publicAddress,
                Balance = 0.500000000000000000m,
            };
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Index));
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
