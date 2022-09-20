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
    public class CustomersController : Controller
    {
        private readonly Data.ApplicationDbContext _db;

        public CustomersController(Data.ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string? SearchOption, string? SearchInput)
        {
            if (SearchOption == null || SearchInput == null)
            {
                return _db.Customers != null ?
                        View(await _db.Customers.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }

            switch (SearchOption)
            {
                case "Id":
                    return _db.Customers != null ?
                        View(await _db.Customers.Where(x => x.Id.ToString().Contains(SearchInput)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
                case "Name":
                    return _db.Customers != null ?
                        View(await _db.Customers.Where(x => x.Name.Contains(SearchInput)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
                case "Address":
                    return _db.Customers != null ?
                        View(await _db.Customers.Where(x => x.Address.Contains(SearchInput)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
                case "Province":
                    return _db.Customers != null ?
                        View(await _db.Customers.Where(x => x.Province.Contains(SearchInput)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
                case "Email":
                    return _db.Customers != null ?
                        View(await _db.Customers.Where(x => x.Email.Contains(SearchInput)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
                case "Phone":
                    return _db.Customers != null ?
                        View(await _db.Customers.Where(x => x.PhoneNumber.Contains(SearchInput)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }

            return _db.Customers != null ?
                        View(await _db.Customers.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Customers == null)
            {
                return NotFound();
            }

            var customer = await _db.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Province,Email,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Add(customer);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Customers == null)
            {
                return NotFound();
            }

            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Province,Email,PhoneNumber")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(customer);
                    await _db.SaveChangesAsync();
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
            if (id == null || _db.Customers == null)
            {
                return NotFound();
            }

            var customer = await _db.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_db.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            var customer = await _db.Customers.FindAsync(id);
            if (customer != null)
            {
                _db.Customers.Remove(customer);
            }
            
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_db.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
