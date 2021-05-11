using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
using DuoPoll.Dal.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace DuoPoll.MVC.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class PollController : Controller
    {
        private readonly PollService _pollService;

        public PollController(PollService pollService)
        {
            _pollService = pollService;
        }

        // GET
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _pollService.Index());
        }

        // GET
        [AllowAnonymous]
        [HttpGet("Poll/Details/{url:length(32)}")]
        public async Task<IActionResult> Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var poll = await _pollService.Details(url);

            if (poll == null)
            {
                return NotFound();
            }

            ViewData["Owner"] = this.User.GetDisplayName() == poll.User.UserName;

            return View(poll);
        }

        // GET
        [Authorize]
        public IActionResult Create()
        {
            var poll = new Poll();
            return View("~/Views/Poll/Edit.cshtml", poll);
        }

        // GET
        [Authorize]
        [HttpGet("Poll/Edit/{url:length(32)}")]
        public async Task<IActionResult> Edit(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var poll = await _pollService.GetPollWithAnswers(url);
            if (poll == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Edit Poll";
            ViewData["Current"] = "polls.update";
            return View("~/Views/Poll/Edit.cshtml", poll);
        }

        // POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PollHeader pollHeader)
        {
            pollHeader.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                          throw new InvalidOperationException());

            var poll = await _pollService.Create(pollHeader);

            return Redirect("/Poll/Details/" + poll.Url);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Poll/Edit/{url:length(32)}")]
        public async Task<IActionResult> Update(string url, [Bind("Name,Url,Public,Status,Open,Close")]
            PollHeader pollHeader)
        {
            if (url != pollHeader.Url)
            {
                return NotFound();
            }

            pollHeader.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                          throw new InvalidOperationException());

            if (ModelState.IsValid)
            {
                Poll poll;
                try
                {
                    poll = await _pollService.Update(pollHeader);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return View("~/Views/Poll/Edit.cshtml", poll);
            }

            return NotFound();
        }

        // GET
        [AllowAnonymous]
        [HttpGet("Poll/Statistics/{url:length(32)}")]
        public IActionResult Statistics(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            ViewData["Url"] = url;

            return View("~/Views/Poll/Statistics.cshtml");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Statistics()
        {
            return View(await _pollService.Statistics());
        }
    }
}