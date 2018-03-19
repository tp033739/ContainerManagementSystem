using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContainerManagementSystem.Data;
using ContainerManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ContainerManagementSystem.Controllers
{
    [Authorize(Roles = Roles.AdministratorOrAgent)]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var context = _context.Booking
                .Include(b => b.Customer)
                .Include(b => b.ShippingSchedule);

            if (User.IsInRole(Roles.Administrator))
            {
                return View(await context.ToListAsync());
            }
            else
            {
                return View(await context.Where(b => b.AgentUserId == _userManager.GetUserId(User)).ToListAsync());
            }
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name");
            ViewData["ShippingScheduleId"] = new SelectList(_context.ShippingSchedule, "Id", "Description");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CargoWeightInKilograms,NumberOfContainers,CustomerId,ShippingScheduleId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.AgentUserId = _userManager.GetUserId(User);
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", booking.CustomerId);
            ViewData["ShippingScheduleId"] = new SelectList(_context.ShippingSchedule, "Id", "Description", booking.ShippingScheduleId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", booking.CustomerId);
            ViewData["ShippingScheduleId"] = new SelectList(_context.ShippingSchedule, "Id", "Description", booking.ShippingScheduleId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CargoWeightInKilograms,NumberOfContainers,CustomerId,ShippingScheduleId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            var savedBooking = await GetBooking(id);
            if (savedBooking == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    booking.AgentUserId = savedBooking.AgentUserId;
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", booking.CustomerId);
            ViewData["ShippingScheduleId"] = new SelectList(_context.ShippingSchedule, "Id", "Description", booking.ShippingScheduleId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await GetBooking(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            if (User.IsInRole(Roles.Administrator))
            {
                return _context.Booking
                    .Any(b => b.Id == id);
            }
            else
            {
                return _context.Booking
                    .Where(b => b.AgentUserId == _userManager.GetUserId(User))
                    .Any(b => b.Id == id);
            }
        }

        private async Task<Booking> GetBooking(int? id)
        {
            var context = _context.Booking
                .Include(b => b.Customer)
                .Include(b => b.ShippingSchedule);

            if (User.IsInRole(Roles.Administrator))
            {
                return await context.SingleOrDefaultAsync(b => b.Id == id);
            }
            else
            {
                return await context.Where(b => b.AgentUserId == _userManager.GetUserId(User))
                    .SingleOrDefaultAsync(b => b.Id == id);
            }
        }
    }
}
