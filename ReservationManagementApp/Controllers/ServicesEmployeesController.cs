using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationManagementApp.Models;
using ReservationManagementApp.Models.ServiceEmployeeModel;

namespace ReservationManagementApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ServicesEmployeesController : Controller
    {
        private readonly ReservationManagementDbContext _context;

        public ServicesEmployeesController(ReservationManagementDbContext context)
        {
            _context = context;
        }

        // GET: ServicesEmployees
        public IActionResult Index(int? idEmployee)
        {
            if (TempData["Error"] != null && !String.IsNullOrEmpty(TempData["Error"].ToString()))
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            TempData.Clear();
            var servicesEmployeesList = _context.ServicesEmployees.Where(elem => elem.IdEmployee == idEmployee).Include(s => s.IdEmployeeNavigation).Include(s => s.IdServiceNavigation);
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name");
            ServiceEmployeeModel serviceEmployeeModel = new ServiceEmployeeModel();
            serviceEmployeeModel.ServicesEmployeesList = servicesEmployeesList;
            serviceEmployeeModel.ServiceEmployee = new ServicesEmployees();
            serviceEmployeeModel.ServiceEmployee.IdEmployeeNavigation = _context.Employees.FirstOrDefault(elem => elem.Id == idEmployee.Value);
            serviceEmployeeModel.ServiceEmployee.IdEmployee = idEmployee.Value;
            return View(serviceEmployeeModel);
        }

        // POST: ServicesEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceEmployeeModel serviceEmployeeModel)
        {
            ServicesEmployees serviceEmployee = new ServicesEmployees();
            serviceEmployee.IdEmployee = serviceEmployeeModel.ServiceEmployee.IdEmployee;
            serviceEmployee.IdService = serviceEmployeeModel.ServiceEmployee.IdService;
            if (_context.ServicesEmployees.FirstOrDefault(elem => elem.IdEmployee == serviceEmployee.IdEmployee && elem.IdService == serviceEmployee.IdService) != null)
            {
                TempData["Error"] = "This services is assigned";
                return RedirectToAction(nameof(Index), new
                {
                    idEmployee = serviceEmployee.IdEmployee
                });
            }
            if (serviceEmployeeModel.ServiceEmployee.IdService != 0)
            {
                _context.Add(serviceEmployee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new
                {
                    idEmployee = serviceEmployee.IdEmployee
                });
            }

            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name", serviceEmployee.IdService);
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = serviceEmployee.IdEmployee
            });
        }

        // GET: ServicesEmployees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceEmployee = await _context.ServicesEmployees
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdServiceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceEmployee == null)
            {
                return NotFound();
            }

            return View(serviceEmployee);
        }


        // POST: ServicesEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceEmployee = await _context.ServicesEmployees.FindAsync(id);
            if (serviceEmployee != null)
            {
                _context.ServicesEmployees.Remove(serviceEmployee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = serviceEmployee.IdEmployee
            });
        }

        private bool ServicesEmployeesExists(int id)
        {
            return _context.ServicesEmployees.Any(e => e.Id == id);
        }
    }
}
