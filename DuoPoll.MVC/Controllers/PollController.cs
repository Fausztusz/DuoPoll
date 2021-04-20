using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace DuoPoll.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PollController : Controller
    {
        private UserManager<User> _userManager;
        private DuoPollDbContext _dbContext;

        public PollController(UserManager<User> userManager, DuoPollDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        // GET
        public IActionResult Show(Poll poll)
        {

            return View();
        }

        // GET
        [Authorize]
        public IActionResult Create()
        {
            return View("~/Views/Poll/Edit.cshtml");
        }

        // GET
        [Authorize]
        [HttpGet("/Polls/{id}/Edit")]
        public IActionResult Edit(Poll poll)
        {
            ViewBag.Poll = poll;
            return View("~/Views/Poll/Edit.cshtml");
        }

        // POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Poll poll)
        {
            var newPoll = new Poll
            {
                Name = poll.Name,
                Public = poll.Public,
                Open = poll.Open,
                Close = poll.Close,
                User = await _userManager.FindByNameAsync(this.User.GetDisplayName())
            };

            await _dbContext.Polls.AddAsync(newPoll);
            await _dbContext.SaveChangesAsync();

            return Redirect("/Poll/Show/" + newPoll.Id);
        }

        [HttpPost("/Polls/{id}")]
        public IActionResult Update(Poll poll)
        {
            return View("~/Views/Poll/Show.cshtml" + poll.Id);
        }


        public IActionResult Statistics()
        {
            return View();
        }
    }
}