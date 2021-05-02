using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
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
        private UserManager<User> _userManager;
        private DuoPollDbContext _dbContext;

        [BindProperty( SupportsGet = true )]
        public int PollId { get; set; }

        [BindProperty]
        public PollHeader SelectedPoll { get; set; }

        public PollController(UserManager<User> userManager, DuoPollDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // GET
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Polls
                .Where(p => p.Public == true)
                .Include(p => p.User).ToListAsync());
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
                .Include(p=>p.User)
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
            return View("~/Views/Poll/Edit.cshtml");
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
                .Include(p=>p.Answers)
                .FirstOrDefaultAsync(p=>p.Url == url);
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
        public async Task<IActionResult> Create(Poll poll)
        {
            poll.UserId = int.Parse( User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());

            await _dbContext.Polls.AddAsync(poll);
            await _dbContext.SaveChangesAsync();

            return Redirect("/Poll/Details/" + poll.Url);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Poll/Edit/{url:length(32)}")]
        public async Task<IActionResult> Update(string url,[Bind("Id,Name,Url,Public,Status,Open,Close")] Poll poll)
        {

            if (url != poll.Url)
            {
                return NotFound();
            }
            poll.UserId = int.Parse( User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(poll);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.Url))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Poll/Details.cshtml", poll);
        }

        public IActionResult Statistics()
        {
            return View();
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