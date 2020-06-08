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
            var reservationManagementDbContext = _context.ServicesEmployees.Where(elem => elem.IdEmployee == idEmployee).Include(s => s.IdEmployeeNavigation).Include(s => s.IdServiceNavigation);
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name");
            ServiceEmployeeModel serviceEmployeeModel = new ServiceEmployeeModel();
            serviceEmployeeModel.ServicesEmployees = reservationManagementDbContext;
            serviceEmployeeModel.ServicesEmployee = new ServicesEmployees();
            serviceEmployeeModel.ServicesEmployee.IdEmployeeNavigation = _context.Employees.FirstOrDefault(elem => elem.Id == idEmployee.Value);
            serviceEmployeeModel.ServicesEmployee.IdEmployee = idEmployee.Value;
            return View(serviceEmployeeModel);
        }

        // POST: ServicesEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceEmployeeModel serviceEmployeeModel)
        {
            ServicesEmployees servicesEmployee = new ServicesEmployees();
            servicesEmployee.IdEmployee = serviceEmployeeModel.ServicesEmployee.IdEmployee;
            servicesEmployee.IdService = serviceEmployeeModel.ServicesEmployee.IdService;
            if (_context.ServicesEmployees.FirstOrDefault(elem => elem.IdEmployee == servicesEmployee.IdEmployee && elem.IdService == servicesEmployee.IdService) != null)
            {
                TempData["Error"] = "This services is assigned";
                return RedirectToAction(nameof(Index), new
                {
                    idEmployee = servicesEmployee.IdEmployee
                });
            }
            if (serviceEmployeeModel.ServicesEmployee.IdService != 0)
            {
                _context.Add(servicesEmployee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new
                {
                    idEmployee = servicesEmployee.IdEmployee
                });
            }

            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "IdCard", servicesEmployee.IdEmployee);
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name", servicesEmployee.IdService);
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = servicesEmployee.IdEmployee
            });
        }

        // GET: ServicesEmployees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.ServicesEmployees
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdServiceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservations == null)
            {
                return NotFound();
            }

            return View(reservations);
        }


        // POST: ServicesEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicesEmployees = await _context.ServicesEmployees.FindAsync(id);
            _context.ServicesEmployees.Remove(servicesEmployees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = servicesEmployees.IdEmployee
            });
        }

        private bool ServicesEmployeesExists(int id)
        {
            return _context.ServicesEmployees.Any(e => e.Id == id);
        }
    }
}
