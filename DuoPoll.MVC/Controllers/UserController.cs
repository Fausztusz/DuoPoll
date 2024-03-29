﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using DuoPoll.Dal;
using DuoPoll.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace DuoPoll.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DuoPollDbContext _dbContext;

        public UserController(DuoPollDbContext dbContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Poll()
        {
            ViewData["Title"] = "My Polls";
            ViewData["current"] = "user.polls";

            return View("~/Views/Poll/Index.cshtml",
                _dbContext.Polls.Where(p =>
                        p.User.Id == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.AsInt())
                    .Include(p => p.Answers)
                    .Include(p => p.User)
                    .ToList());
        }

        [System.Web.Mvc.HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            if (culture != "en" && culture != "hu")
                return BadRequest();

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1),
                    IsEssential = false,
                }
            );
            return Redirect(Request.Headers["Referer"].ToString().IsEmpty()
                ? "/"
                : Request.Headers["Referer"].ToString());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}