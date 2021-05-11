using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
using DuoPoll.Dal.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer _;
        private readonly PollService _pollService;
        private readonly AnswerService _answerService;
        private readonly ChoiceService _choiceService;

        public AnswerController(IStringLocalizer<AnswerController> localisation,
            AnswerService answerService, PollService pollService, ChoiceService choiceService)
        {
            _ = localisation;
            _pollService = pollService;
            _answerService = answerService;
            _choiceService = choiceService;
        }

        // GET: api/Answer/5
        [HttpGet("{url:length(32)}")]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswer(string url)
        {
            var answers = await _answerService.GetAnswers(url);
            if (answers.Count < 2) return StatusCode(422, _["Not enough answers"].ToString());

            var choices = await _choiceService.GetChoices(GetIdOrHash());

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
                    || (c.AnswerId == answers[right].Id && c.LoserId == answers[left].Id));

                if (exists == null) break;
                if (i == limit - 1) return StatusCode(404, _["No new question found!"].ToString());
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
        public async Task<IActionResult> PostVote(string url, ChoiceHeader choiceHeader)
        {
            var left = HttpContext.Session.GetInt32("left");
            var right = HttpContext.Session.GetInt32("right");

            if (left == 0 || left == null || right == 0 || right == null || left == right)
            {
                return BadRequest();
            }

            var choice = new Choice();
            if (choiceHeader.Side == "left")
            {
                choice.AnswerId = (int) left;
                choice.LoserId = (int) right;
                choice.UserIdentity = GetIdOrHash();
            }
            else if (choiceHeader.Side == "right")
            {
                choice.AnswerId = (int) right;
                choice.LoserId = (int) left;
                choice.UserIdentity = GetIdOrHash();
            }
            else
                return BadRequest();

            await _choiceService.CreateChoice(choice);

            HttpContext.Session.SetInt32("left", 0);
            HttpContext.Session.SetInt32("right", 0);

            return Json("Ok");
        }

        // PUT: api/Answer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Answer>> PutAnswer(int id,
            [Bind("Id,Title,Media,Url")] AnswerHeader answerHeader)
        {
            var poll = await _pollService.GetPollWithAnswers(answerHeader.Url);

            if (id != answerHeader.Id || poll == null)
            {
                return BadRequest();
            }

            if (!CanEdit(poll))
            {
                return Unauthorized();
            }

            await _answerService.UpdateAnswer(answerHeader, poll.Id);
            return Json("Ok");
        }

        // POST: api/Answer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer(AnswerHeader answerHeader)
        {
            var poll = await _pollService.GetPollWithAnswers(answerHeader.Url);

            if (poll == null || !CanEdit(poll))
            {
                return Unauthorized();
            }

            if (poll.Status == Poll.StatusType.Draft)
            {
                poll.Status = Poll.StatusType.Open;
            }

            var answer = await _answerService.CreateAnswer(answerHeader, poll.Id);

            return Json(new {id = answer.Id});
        }

        // DELETE: api/Answer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _answerService.GetAnswer(id);

            if (answer == null)
            {
                return NotFound();
            }

            if (!CanEdit(answer.Poll))
            {
                return Unauthorized();
            }

            await _answerService.DeleteAnswer(answer);
            return Ok();
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