using System;
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
    [Authorize(Roles = "Administrator,Agent")]
    public class ShippingSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShippingSchedules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShippingSchedule.Include(s => s.Vessel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShippingSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingSchedule = await _context.ShippingSchedule
                .Include(s => s.Vessel)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (shippingSchedule == null)
            {
                return NotFound();
            }

            return View(shippingSchedule);
        }

        // GET: ShippingSchedules/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Name");
            return View();
        }

        // POST: ShippingSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DepartureTime,ArrivalTime,DepartureLocation,ArrivalLocation,VesselId")] ShippingSchedule shippingSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Name", shippingSchedule.VesselId);
            return View(shippingSchedule);
        }

        // GET: ShippingSchedules/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingSchedule = await _context.ShippingSchedule.SingleOrDefaultAsync(m => m.Id == id);
            if (shippingSchedule == null)
            {
                return NotFound();
            }
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Name", shippingSchedule.VesselId);
            return View(shippingSchedule);
        }

        // POST: ShippingSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepartureTime,ArrivalTime,DepartureLocation,ArrivalLocation,VesselId")] ShippingSchedule shippingSchedule)
        {
            if (id != shippingSchedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingScheduleExists(shippingSchedule.Id))
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
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Name", shippingSchedule.VesselId);
            return View(shippingSchedule);
        }

        // GET: ShippingSchedules/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingSchedule = await _context.ShippingSchedule
                .Include(s => s.Vessel)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (shippingSchedule == null)
            {
                return NotFound();
            }

            return View(shippingSchedule);
        }

        // POST: ShippingSchedules/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shippingSchedule = await _context.ShippingSchedule.SingleOrDefaultAsync(m => m.Id == id);
            _context.ShippingSchedule.Remove(shippingSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingScheduleExists(int id)
        {
            return _context.ShippingSchedule.Any(e => e.Id == id);
        }
    }
}
