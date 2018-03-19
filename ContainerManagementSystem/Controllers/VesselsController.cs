﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContainerManagementSystem.Data;
using ContainerManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace ContainerManagementSystem.Controllers
{
    [Authorize(Roles = Roles.AdministratorOrAgent)]
    public class VesselsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VesselsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vessels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vessel.ToListAsync());
        }

        // GET: Vessels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vessel == null)
            {
                return NotFound();
            }

            return View(vessel);
        }

        // GET: Vessels/Create
        [Authorize(Roles = Roles.Administrator)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vessels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Capacity")] Vessel vessel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vessel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vessel);
        }

        // GET: Vessels/Edit/5
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessel.SingleOrDefaultAsync(m => m.Id == id);
            if (vessel == null)
            {
                return NotFound();
            }
            return View(vessel);
        }

        // POST: Vessels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Capacity")] Vessel vessel)
        {
            if (id != vessel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vessel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VesselExists(vessel.Id))
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
            return View(vessel);
        }

        // GET: Vessels/Delete/5
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vessel = await _context.Vessel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vessel == null)
            {
                return NotFound();
            }

            return View(vessel);
        }

        // POST: Vessels/Delete/5
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vessel = await _context.Vessel.SingleOrDefaultAsync(m => m.Id == id);
            _context.Vessel.Remove(vessel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VesselExists(int id)
        {
            return _context.Vessel.Any(e => e.Id == id);
        }
    }
}
