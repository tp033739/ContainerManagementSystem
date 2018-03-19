using System;
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
    public class ShippingSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShippingSchedules
        public async Task<IActionResult> Index(string departureLocation, string arrivalLocation)
        {
            ViewData["DepartureLocation"] = departureLocation;
            ViewData["ArrivalLocation"] = arrivalLocation;

            IQueryable<ShippingSchedule> context = _context.ShippingSchedule.Include(s => s.Vessel);

            if (!String.IsNullOrWhiteSpace(departureLocation))
                context = context.Where(s => s.DepartureLocation.Contains(departureLocation));
            if (!String.IsNullOrWhiteSpace(arrivalLocation))
                context = context.Where(s => s.ArrivalLocation.Contains(arrivalLocation));

            return View(await context.ToListAsync());
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
        [Authorize(Roles = Roles.Administrator)]
        public IActionResult Create()
        {
            ViewData["VesselId"] = new SelectList(_context.Vessel, "Id", "Name");
            return View();
        }

        // POST: ShippingSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Roles.Administrator)]
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
        [Authorize(Roles = Roles.Administrator)]
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
        [Authorize(Roles = Roles.Administrator)]
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
        [Authorize(Roles = Roles.Administrator)]
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
        [Authorize(Roles = Roles.Administrator)]
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
