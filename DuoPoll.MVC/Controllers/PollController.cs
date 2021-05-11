using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
using DuoPoll.Dal.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace DuoPoll.MVC.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class PollController : Controller
    {
        private readonly DuoPollDbContext _dbContext;
        private readonly PollService _pollService;

        public PollController(DuoPollDbContext dbContext, PollService pollService)
        {
            _dbContext = dbContext;
            _pollService = pollService;
        }

        // GET
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Polls
                .Where(p => p.Public)
                .Where(p => p.Status == Poll.StatusType.Open)
                .Where(p => p.Answers.Count > 0)
                .Include(p => p.Answers)
                .Include(p => p.User)
                .ToListAsync());
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

            var poll = await _dbContext.Polls
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Url == url);
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

            var poll = await _dbContext.Polls
                .Include(p => p.Answers)
                .FirstOrDefaultAsync(p => p.Url == url);
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
            var poll = await _dbContext.Polls
                .Where(p => p.Answers.Count > 0)
                .Where(p => p.Public)
                .Where(p => p.Status != Poll.StatusType.Draft)
                .Include(p => p.User)
                .Include(p => p.Answers)
                .ThenInclude(a => a.Choices)
                .ToListAsync();

            return View(poll);
        }

        private bool PollExists(int id)
        {
            return _dbContext.Polls.Any(e => e.Id == id);
        }

        private bool PollExists(string url)
        {
            return _dbContext.Polls.Any(e => e.Url == url);
        }
    }
}