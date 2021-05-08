using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace DuoPoll.MVC.Controllers
{
    internal class PollUrl
    {
        private PollUrl()
        {
        }

        public string Url { get; set; }
    }

    internal class Vote
    {
        private Vote()
        {
        }

        public string Side { get; set; }
    }

    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class AnswerController : Controller
    {
        private readonly DuoPollDbContext _context;

        public AnswerController(DuoPollDbContext context)
        {
            _context = context;
        }

        // GET: api/Answer
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
        {
            return await _context.Answers.ToListAsync();
        }

        // GET: api/Answer/5
        [HttpGet("{url:length(32)}")]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswer(string url)
        {
            var poll = await _context.Polls
                .AsNoTracking()
                .Where(p => p.Url == url)
                .FirstOrDefaultAsync();

            var answers = await _context.Answers
                .Where(a => a.PollId == poll.Id)
                .Include(a => a.Choices)
                .Include(a => a.Losses)
                .ToListAsync();

            if (answers.Count < 2) return StatusCode(422, "Not enough answers");

            var userId = GetIdOrHash();

            var choices = await _context.Choices.ToListAsync();

            const int limit = 50;
            int left = 0, right = 0;
            for (var i = 0; i < limit; i++)
            {
                do
                {
                    left = new Random().Next(0, answers.Count);
                    right = new Random().Next(0, answers.Count);
                } while (left == right);

                var exists = choices.Find(c =>
                    (c.AnswerId == answers[left].Id && c.LoserId == answers[right].Id)
                    || (c.AnswerId == answers[right].Id && c.LoserId == answers[left].Id)
                    && c.UserIdentity == userId);

                if (exists == null) break;
                if (i == limit - 1) return StatusCode(404, "No new question found");
            }

            HttpContext.Session.SetInt32("left", answers[left].Id);
            HttpContext.Session.SetInt32("right", answers[right].Id);

            return Json(new
            {
                left = new {Title = answers[left].Title, Media = answers[left].Media},
                right = new {Title = answers[right].Title, Media = answers[right].Media}
            });
        }

        [HttpPost("{url:length(32)}/Vote")]
        public async Task<IActionResult> PostVote(string url, object json)
        {
            var vote = JsonConvert.DeserializeObject<Vote>(json.ToString());

            var left = HttpContext.Session.GetInt32("left");
            var right = HttpContext.Session.GetInt32("right");

            if (left == 0 || left == null || right == 0 || right == null || left == right)
            {
                return BadRequest();
            }

            var choice = new Choice();
            if (vote.Side == "left")
            {
                choice.AnswerId = (int) left;
                choice.LoserId = (int) right;
                choice.UserIdentity = GetIdOrHash();
            }
            else if (vote.Side == "right")
            {
                choice.AnswerId = (int) left;
                choice.LoserId = (int) right;
                choice.UserIdentity = GetIdOrHash();
            }
            else
                return BadRequest();

            await _context.Choices.AddAsync(choice);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("left", 0);
            HttpContext.Session.SetInt32("right", 0);

            return Json("Ok");
        }

        // PUT: api/Answer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Answer>> PutAnswer(int id, object json)
        {
            // I hate this
            var answer = JsonConvert.DeserializeObject<Answer>(json.ToString());
            var url = JsonConvert.DeserializeObject<PollUrl>(json.ToString());

            var poll = await _context.Polls
                .AsNoTracking()
                .Include(p => p.Answers)
                .FirstOrDefaultAsync(p => p.Url == url.Url);

            if (id != answer.Id || poll == null)
            {
                return BadRequest();
            }

            if (!CanEdit(poll))
            {
                return Unauthorized();
            }

            answer.PollId = poll.Id;
            _context.Entry(answer).State = EntityState.Modified;

            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();

            return Json("Ok");
        }

        // POST: api/Answer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer(object json)
        {
            var answer = JsonConvert.DeserializeObject<Answer>(json.ToString());
            var url = JsonConvert.DeserializeObject<PollUrl>(json.ToString());

            var poll = await _context.Polls
                .Include(p => p.Answers)
                .FirstOrDefaultAsync(p => p.Url == url.Url);

            if (poll == null || !CanEdit(poll))
            {
                return Unauthorized();
            }

            if (poll.Status == Poll.StatusType.Draft)
            {
                poll.Status = Poll.StatusType.Open;
            }

            poll.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return Json(new {id = answer.Id});
        }

        // DELETE: api/Answer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? throw new InvalidOperationException());
        }

        private string GetIdOrHash()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? GetIp();

            return userId;
        }

        private string GetIp()
        {
            if (HttpContext.Connection.RemoteIpAddress != null)
                return ComputeSha256Hash(HttpContext.Connection.RemoteIpAddress.ToString());
            return "NO IP";
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using var sha256Hash = SHA256.Create();
            // ComputeHash - returns byte array
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }

            return builder.ToString();
        }

        private bool CanEdit(Poll poll)
        {
            return poll.UserId == GetUserId();
        }
    }
}