using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoPoll.Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DuoPoll.MVC.Controllers
{
    internal class PollResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Media { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public double WinPercentage { get; set; }
    }

    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class StatisticsController : Controller
    {
        private readonly DuoPollDbContext _context;

        public StatisticsController(DuoPollDbContext context)
        {
            _context = context;
        }

        [HttpGet("{url:length(32)}")]
        public async Task<IActionResult> GetStatistics(string url)
        {
            var poll = await _context.Polls
                .Where(p => p.Url == url)
                .Include(p => p.Answers)
                .ThenInclude(a => a.Choices).FirstOrDefaultAsync();

            var list = new List<PollResult>();
            foreach (var pollAnswer in poll.Answers)
            {
                list.Add(new PollResult
                {
                    Id = pollAnswer.Id,
                    Title = pollAnswer.Title ?? "",
                    Media = pollAnswer.Media ?? "",
                    Wins = pollAnswer.Choices?.Count ?? 0,
                    Loses = pollAnswer.Losses?.Count ?? 0,
                    WinPercentage = Math.Round((double) (pollAnswer.Choices?.Count ?? 0)
                        / (((pollAnswer.Choices?.Count ?? 1) == 0 ? 1 : pollAnswer.Choices?.Count ?? 1) +
                           (pollAnswer.Losses?.Count ?? 0)) * 100, 1),
                });
            }

            return Json(new {Name = poll.Name, Answers = list.OrderByDescending(p => p.WinPercentage)});
        }
    }
}