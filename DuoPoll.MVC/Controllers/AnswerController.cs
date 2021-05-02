using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace DuoPoll.MVC.Controllers
{
    internal class PollUrl
    {
        private PollUrl(){}
        public string Url { get; set; }
    }
    [ApiController]
    [Route("api/[controller]")]
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
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        // PUT: api/Answer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Answer>> PutAnswer(int id,object json)
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
            try
            {
                _context.Answers.Update(answer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Answer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer()
        {
            var poll = await _context.Polls
                .Include(p => p.Answers)
                .FirstOrDefaultAsync(p => p.Url == Request.Form["Url"].ToString());

            if (poll == null || !CanEdit(poll))
            {
                return Unauthorized();
            }

            var answer = new Answer
            {
                Title = Request.Form["Title"],
                Media = Request.Form["Media"],
            };
            poll.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return Json(new {id = answer.Id});
        }

        // DELETE: api/Answer/5
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
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
            return poll.UserId == int.Parse( User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
        }
    }
}