﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrimeSimulateur.Models;

namespace PrimeSimulateur.Controllers
{
    [Authorize]
    public class LogementsController : Controller
    {
        private readonly MyDbContext _context;

        public LogementsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Logements
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Logements.Include(l => l.Client);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Logements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logement = await _context.Logements
                .Include(l => l.Client)
                .FirstOrDefaultAsync(m => m.LogementId == id);
            if (logement == null)
            {
                return NotFound();
            }

            return View(logement);
        }

        // GET: Logements/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "email");
            return View();
        }

        // POST: Logements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LogementId,adresse,Ville,TypeEnergie,surface,ClientId")] Logement logement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "email", logement.ClientId);
            return View(logement);
        }

        // GET: Logements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logement = await _context.Logements.FindAsync(id);
            if (logement == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "email", logement.ClientId);
            return View(logement);
        }

        // POST: Logements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LogementId,adresse,Ville,TypeEnergie,surface,ClientId")] Logement logement)
        {
            if (id != logement.LogementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogementExists(logement.LogementId))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "email", logement.ClientId);
            return View(logement);
        }

        // GET: Logements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logement = await _context.Logements
                .Include(l => l.Client)
                .FirstOrDefaultAsync(m => m.LogementId == id);
            if (logement == null)
            {
                return NotFound();
            }

            return View(logement);
        }

        // POST: Logements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logement = await _context.Logements.FindAsync(id);
            _context.Logements.Remove(logement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogementExists(int id)
        {
            return _context.Logements.Any(e => e.LogementId == id);
        }
    }
}
