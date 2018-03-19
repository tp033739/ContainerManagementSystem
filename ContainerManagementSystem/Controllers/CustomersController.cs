using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContainerManagementSystem.Data;
using ContainerManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace ContainerManagementSystem.Controllers
{
    [Authorize(Roles = Roles.AdministratorOrAgent)]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Roles.Administrator))
            {
                return View(await _context.Customer.ToListAsync());
            }

            var customers = await _context.Customer
                .Where(c => c.AgentUserId == _userManager.GetUserId(User))
                .ToListAsync();

            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OrganizationName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.AgentUserId = _userManager.GetUserId(User);
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OrganizationName")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            var savedCustomer = await GetCustomer(id);
            if (savedCustomer == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.AgentUserId = savedCustomer.AgentUserId;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            if (User.IsInRole(Roles.Administrator))
            {
                return _context.Customer
                    .Any(c => c.Id == id);
            }
            else
            {
                return _context.Customer
                    .Where(c => c.AgentUserId == _userManager.GetUserId(User))
                    .Any(c => c.Id == id);
            }
        }

        private async Task<Customer> GetCustomer(int? id)
        {
            if (User.IsInRole(Roles.Administrator))
            {
                return await _context.Customer
                    .SingleOrDefaultAsync(c => c.Id == id);
            }
            else
            {
                return await _context.Customer
                    .Where(c => c.AgentUserId == _userManager.GetUserId(User))
                    .SingleOrDefaultAsync(c => c.Id == id);
            }
        }
    }
}
