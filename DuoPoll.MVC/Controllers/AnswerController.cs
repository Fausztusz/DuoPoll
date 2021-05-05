using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [Microsoft.AspNetCore.Mvc.HttpGet("{url:length(32)}")]
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

            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException()
            );

            const int limit = 20;
            int left = 0, right = 0;
            for (var i = 0; i < limit; i++)
            {
                left = new Random().Next(1, answers.Count);
                do
                {
                    right = new Random().Next(1, answers.Count);
                } while (left == right);

                var exists = await _context.Choices
                    .Where(c =>
                        (c.AnswerId == left && c.LoserId == right)
                        || (c.AnswerId == right && c.LoserId == left)
                        && c.UserId == userId)
                    .FirstOrDefaultAsync();

                if (exists == null) break;
                if (i == limit - 1) return StatusCode(404, "No new question found");
            }

            HttpContext.Session.SetInt32("left", left);
            HttpContext.Session.SetInt32("right", right);

            return Json(new
            {
                left = new {Title = answers[left].Title, Media = answers[left].Media},
                right = new {Title = answers[right].Title, Media = answers[right].Media}
            });
        }

        // PUT: api/Answer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Answer>> PutAnswer(int id, object json)
        {
            // I hate this
            var answer = JsonConvert.DeserializeObject<Answer>(json.ToString());
            var url = JsonConvert.DeserializeObject<PollUrl>(json.ToString());
            if (id != answer.Id)
            {
                return BadRequest();
            }

            var poll = await _context.Polls
                .AsNoTracking()
                .Include(p => p.Answers)
                .FirstOrDefaultAsync(p => p.Url == url.Url);

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

        private bool CanEdit(Poll poll)
        {
            return poll.UserId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                            throw new InvalidOperationException());
        }
    }
}