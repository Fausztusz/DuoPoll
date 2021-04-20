using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DuoPoll.Dal;
using DuoPoll.Dal.Entities;

namespace DuoPoll.MVC.Controllers
{
    public class TestController : Controller
    {
        private readonly DuoPollDbContext _context;

        public TestController(DuoPollDbContext context)
        {
            _context = context;
        }

        // GET: Test
        public async Task<IActionResult> Index()
        {
            return View(await _context.Polls.ToListAsync());
        }

        // GET: Test/Details/5
        [HttpGet("Test/Details/{url:length(32)}")]
        public async Task<IActionResult> Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .FirstOrDefaultAsync(m => m.Url == url);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // GET: Test/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url,Public,Status,Open,Close")] Poll poll)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        // GET: Test/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        // POST: Test/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url,Public,Status,Open,Close")] Poll poll)
        {
            if (id != poll.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.Id))
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
            return View(poll);
        }

        // GET: Test/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.Id == id);
        }
    }
}
