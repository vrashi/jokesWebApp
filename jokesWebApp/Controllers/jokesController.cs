﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jokesWebApp.Data;
using jokesWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace jokesWebApp.Controllers
{
    public class jokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public jokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: jokes
        public async Task<IActionResult> Index()
        {
            return View(await _context.joke.ToListAsync());
        }

        // GET: jokes/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: jokes/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.joke.Where(j => j.JokeQuestion.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        [Authorize]
        // GET: jokes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer")] joke joke)
        {
            if (joke.JokeQuestion.ToLower() == joke.JokeAnswer.ToLower()) {
                ModelState.AddModelError("JokeQuestion", "The joke question cannot be same as joke answer.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                TempData["success"] = "New joke created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: jokes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokeQuestion,JokeAnswer")] joke joke)
        {
            if (id != joke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Joke updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!jokeExists(joke.Id))
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
            return View(joke);
        }

        // GET: jokes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // POST: jokes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var joke = await _context.joke.FindAsync(id);
            if (joke != null)
            {
                _context.joke.Remove(joke);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Joke deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool jokeExists(int id)
        {
            return _context.joke.Any(e => e.Id == id);
        }
    }
}
