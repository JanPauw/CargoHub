using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CargoHubWeb.Data;
using CargoHubWeb.Models;
using Microsoft.AspNetCore.Http;

namespace CargoHubWeb.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeesController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return _db.Employees != null ?
                        View(await _db.Employees.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
        }

        //Disabling an Employee Account
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(int? EmpNumber)
        {
            if (EmpNumber != null)
            {
                var obj = _db.Employees.Find(EmpNumber);

                if (obj != null)
                {
                    obj.Role = "Disabled";
                    _db.Employees.Update(obj);
                }

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var employee = await _db.Employees
                .FirstOrDefaultAsync(m => m.Number == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? Name, string? Role)
        {
            Encrypt enc = new Encrypt();
            Employee obj = new Employee();
            obj.Name = Name;
            obj.Role = Role;
            obj.Password = enc.EncryptString("???");

            if (ModelState.IsValid)
            {
                _db.Add(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var employee = await _db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Number,Password,Name,Role")] Employee employee)
        {
            if (id != employee.Number)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(employee);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Number))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var employee = await _db.Employees
                .FirstOrDefaultAsync(m => m.Number == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Employees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employee = await _db.Employees.FindAsync(id);
            if (employee != null)
            {
                _db.Employees.Remove(employee);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return (_db.Employees?.Any(e => e.Number == id)).GetValueOrDefault();
        }

        //Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(int? Number, string? Password)
        {
            Encrypt enc = new Encrypt();
            var obj = _db.Employees.Find(Number);

            if (obj == null)
            {
                TempData["ErrorMessage"] = "Invalid Employee Number!";
                return Login();
            }

            if (obj.Password == enc.EncryptString("???"))
            {
                TempData["ErrorMessage"] = "Need to Register Employee Number!";
                return Login();
            }

            if (obj.Role == "Disabled")
            {
                TempData["ErrorMessage"] = "Account Disabled! Contact Admin if you think this is a mistake!";
                return Login();
            }

            if (Password == null || Password.Length < 8)
            {
                TempData["ErrorMessage"] = "Invalid Password!";
                return Login();
            }

            if (obj.Password != enc.EncryptString(Password))
            {
                TempData["ErrorMessage"] = "Invalid Combination!";
                return Login();
            }

            HttpContext.Session.SetInt32("Number", obj.Number);
            HttpContext.Session.SetString("Password", obj.Password);
            HttpContext.Session.SetString("Name", obj.Name.ToUpper());
            HttpContext.Session.SetString("Role", obj.Role);
            HttpContext.Session.SetString("logged_in", "true");

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        //Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(int? Number, string? Password, string? ConfirmPassword)
        {
            Encrypt enc = new Encrypt();
            var obj = _db.Employees.Find(Number);

            if (obj == null)
            {
                TempData["ErrorMessage"] = "Invalid Employee Number! Try logging in.";
                return Register();
            }

            if (Password != ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match!";
                return Register();
            }

            if (Password != null && Password.Length < 8)
            {
                TempData["ErrorMessage"] = "Passwords needs to be 8 characters!";
                return Register();
            }

            if (obj.Password != enc.EncryptString("???"))
            {
                TempData["ErrorMessage"] = "Employee already registered! Contact admin, to change your password if needed.";
                return Register();
            }

            if (Password != null)
            {
                obj.Password = enc.EncryptString(Password);
            }

            _db.Update(obj);
            _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Disabled()
        {
            return _db.Employees != null ?
                        View(await _db.Employees.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
        }

        // GET: Employees/Enable/5
        public async Task<IActionResult> Enable(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var employee = await _db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        //Enabling an Employee Account
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult EnablePOST(int? Number, string? Role)
        {
            if (Number != null && Role != null)
            {
                var obj = _db.Employees.Find(Number);

                if (obj != null)
                {
                    obj.Role = Role;
                    _db.Employees.Update(obj);
                }

                _db.SaveChanges();
            }

            return RedirectToAction("Disabled");
        }
    }
}
