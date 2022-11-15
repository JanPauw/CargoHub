using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CargoHubWeb.Data;
using CargoHubWeb.Models;

namespace CargoHubWeb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Customer).Include(o => o.EmployeeNumNavigation).Include(o => o.FromDepotNavigation).Include(o => o.ToDepotNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.EmployeeNumNavigation)
                .Include(o => o.FromDepotNavigation)
                .Include(o => o.ToDepotNavigation)
                .FirstOrDefaultAsync(m => m.Number == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["EmployeeNum"] = new SelectList(_context.Employees.Where(x => x.Role == "Driver"), "Number", "Name");
            ViewData["FromDepot"] = new SelectList(_context.Depots, "Id", "Address");
            ViewData["ToDepot"] = new SelectList(_context.Depots, "Id", "Address");

            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            (
            string? Description, int? Weight,
            int? ToDepot, int? FromDepot, int? CustomerId,
            string? Status, int? EmployeeNum
            )
        {
            Order order = new Order();

            #region ViewData
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["EmployeeNum"] = new SelectList(_context.Employees.Where(x => x.Role == "Driver"), "Number", "Name");
            ViewData["FromDepot"] = new SelectList(_context.Depots, "Id", "Address");
            ViewData["ToDepot"] = new SelectList(_context.Depots, "Id", "Address");

            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            #endregion

            #region Input Validation
            //Description
            if (!string.IsNullOrWhiteSpace(Description))
            {
                order.Description = Description;
            }
            else
            {
                TempData["error"] = "Description cannot be empty!";
                return View();
            }

            //Weight
            if (Weight > 0)
            {
                try
                {
                    order.Weight = (int)Weight;
                }
                catch (Exception)
                {
                    TempData["error"] = "Weight can only be a Number!";
                    return View();
                }
            }
            else
            {
                TempData["error"] = "Weight can not be less than 0!";
                return View();
            }

            //To Depot
            if (ToDepot != null && _context.Depots.Find(ToDepot) != null)
            {
                order.ToDepot = (int)ToDepot;
            }
            else
            {
                TempData["error"] = "Invalid Origin Depot Selected!";
                return View();
            }

            //From Depot
            if (FromDepot != null && _context.Depots.Find(FromDepot) != null)
            {
                order.FromDepot = (int)FromDepot;
            }
            else
            {
                TempData["error"] = "Invalid Destination Selected!";
                return View();
            }

            //Customer ID
            if (CustomerId != null && _context.Customers.Find(CustomerId) != null)
            {
                order.CustomerId = (int)CustomerId;
            }
            else
            {
                TempData["error"] = "Invalid Customer Selected!";
                return View();
            }

            //Status
            if (!string.IsNullOrWhiteSpace(Status))
            {
                order.Status = Status;
            }
            else
            {
                TempData["error"] = "Invalid Status Selected!";
                return View();
            }

            //Employee Number
            if (EmployeeNum != null && _context.Employees.Find(EmployeeNum) != null)
            {
                order.EmployeeNum = (int)EmployeeNum;
            }
            else
            {
                TempData["error"] = "Invalid Employee Selected!";
                return View();
            }
            #endregion

            order.Customer = _context.Customers.Find(CustomerId);
            order.EmployeeNumNavigation = _context.Employees.Find(EmployeeNum);
            order.ToDepotNavigation = _context.Depots.Find(ToDepot);
            order.FromDepotNavigation = _context.Depots.Find(FromDepot);
            order.Date = DateTime.Now;

            order.Number = OrderNumGen(order.Customer.Name);

            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string OrderNumGen(string name)
        {
            string answer = "";
            answer += name.Substring(0, 2).ToUpper();

            Random r = new Random();
            int num = r.Next(10000000, 100000000);

            answer += num.ToString();

            if (_context.Orders.Where(x => x.Number == answer).FirstOrDefault() != null)
            {
                return OrderNumGen(name);
            }

            return answer;
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["EmployeeNum"] = new SelectList(_context.Employees, "Number", "Number", order.EmployeeNum);
            ViewData["FromDepot"] = new SelectList(_context.Depots, "Id", "Id", order.FromDepot);
            ViewData["ToDepot"] = new SelectList(_context.Depots, "Id", "Id", order.ToDepot);

            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            ViewData["Depots"] = _context.Depots.ToList();
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string id, string Description, int Weight,
            int ToDepot, int FromDepot, int CustomerId,
            string Status, int EmployeeNum
            )
        {
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            ViewData["Depots"] = _context.Depots.ToList();

            //Check if Order Exists
            Order order = _context.Orders.Find(id);

            if (order == null)
            {
                return View();
            }

            #region Input Validation
            //Description
            if (!string.IsNullOrWhiteSpace(Description))
            {
                order.Description = Description;
            }
            else
            {
                TempData["error"] = "Description cannot be empty!";
                return View();
            }

            //Weight
            if (Weight > 0)
            {
                try
                {
                    order.Weight = (int)Weight;
                }
                catch (Exception)
                {
                    TempData["error"] = "Weight can only be a Number!";
                    return View();
                }
            }
            else
            {
                TempData["error"] = "Weight can not be less than 0!";
                return View();
            }

            //To Depot
            if (ToDepot != null && _context.Depots.Find(ToDepot) != null)
            {
                order.ToDepot = (int)ToDepot;
            }
            else
            {
                TempData["error"] = "Invalid Origin Depot Selected!";
                return View();
            }

            //From Depot
            if (FromDepot != null && _context.Depots.Find(FromDepot) != null)
            {
                order.FromDepot = (int)FromDepot;
            }
            else
            {
                TempData["error"] = "Invalid Destination Selected!";
                return View();
            }

            //Customer ID
            if (CustomerId != null && _context.Customers.Find(CustomerId) != null)
            {
                order.CustomerId = (int)CustomerId;
            }
            else
            {
                TempData["error"] = "Invalid Customer Selected!";
                return View();
            }

            //Status
            if (!string.IsNullOrWhiteSpace(Status))
            {
                order.Status = Status;
            }
            else
            {
                TempData["error"] = "Invalid Status Selected!";
                return View();
            }

            //Employee Number
            if (EmployeeNum != null && _context.Employees.Find(EmployeeNum) != null)
            {
                order.EmployeeNum = (int)EmployeeNum;
            }
            else
            {
                TempData["error"] = "Invalid Employee Selected!";
                return View();
            }
            #endregion

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["EmployeeNum"] = new SelectList(_context.Employees, "Number", "Number", order.EmployeeNum);
            ViewData["FromDepot"] = new SelectList(_context.Depots, "Id", "Id", order.FromDepot);
            ViewData["ToDepot"] = new SelectList(_context.Depots, "Id", "Id", order.ToDepot);

            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction("Details", "Orders", new { id = id });
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.EmployeeNumNavigation)
                .Include(o => o.FromDepotNavigation)
                .Include(o => o.ToDepotNavigation)
                .FirstOrDefaultAsync(m => m.Number == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.Number == id);
        }
    }
}
