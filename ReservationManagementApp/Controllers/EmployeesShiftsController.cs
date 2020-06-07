using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationManagementApp.Models;
using ReservationManagementApp.Models.EmployeeShiftModel;

namespace ReservationManagementApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class EmployeesShiftsController : Controller
    {
        private readonly ReservationManagementDbContext _context;

        public EmployeesShiftsController(ReservationManagementDbContext context)
        {
            _context = context;
        }

        // GET: EmployeesShifts
        public IActionResult Index(int? idEmployee)
        {
            AddModelErrors();
            var reservationManagementDbContext = _context.EmployeesShifts.Where(elem => elem.IdEmployee == idEmployee).Include(e => e.IdEmployeeNavigation);
            EmployeeShiftModel employeeShiftModel = new EmployeeShiftModel();
            employeeShiftModel.EmployeesShifts = reservationManagementDbContext;
            employeeShiftModel.EmployeeShifts = new EmployeesShifts();
            employeeShiftModel.EmployeeShifts.WorkDay = DateTime.Now;
            employeeShiftModel.EndDate = DateTime.Now;
            employeeShiftModel.EmployeeShifts.IdEmployeeNavigation = _context.Employees.FirstOrDefault(elem => elem.Id == idEmployee.Value);
            employeeShiftModel.EmployeeShifts.IdEmployee = idEmployee.Value;

            return View(employeeShiftModel);
        }


        // POST: EmployeesShifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeShiftModel employeeShiftModel)
        {
            List<EmployeesShifts> AllShiftsToAdd = GetShiftsForEmployee(employeeShiftModel);
            if (ValidateemployeeShiftModel(employeeShiftModel))
            {
                if (AllShiftsToAdd.Count() > 0)//Validaciones
                {
                    foreach (EmployeesShifts shift in AllShiftsToAdd)
                    {
                        if (ValidateShift(shift))
                        {
                            _context.Add(shift);
                        }
                        else
                        {
                           return RedirectToAction(nameof(Index), new
                            {
                                idEmployee = employeeShiftModel.EmployeeShifts.IdEmployee
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new
                    {
                        idEmployee = employeeShiftModel.EmployeeShifts.IdEmployee
                    });
                }
            }
          
                return RedirectToAction(nameof(Index), new
                    {
                        idEmployee = employeeShiftModel.EmployeeShifts.IdEmployee
                    });
        }

        // GET: EmployeesShifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeesShifts = await _context.EmployeesShifts
                .Include(e => e.IdEmployeeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeesShifts == null)
            {
                return NotFound();
            }

            return View(employeesShifts);
        }

        // POST: EmployeesShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeesShifts = await _context.EmployeesShifts.FindAsync(id);
            _context.EmployeesShifts.Remove(employeesShifts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = employeesShifts.IdEmployee
            });
        }

        private bool EmployeesShiftsExists(int id)
        {
            return _context.EmployeesShifts.Any(e => e.Id == id);
        }

        private List<EmployeesShifts> GetShiftsForEmployee(EmployeeShiftModel employeeShiftModel)
        {
            List<EmployeesShifts> employeesShifts = new List<EmployeesShifts>();
            var dates = new List<DateTime>();

            for (var dt = employeeShiftModel.EmployeeShifts.WorkDay; dt <= employeeShiftModel.EndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            foreach (DateTime date in dates)
            {
                EmployeesShifts employeeShift = new EmployeesShifts();
                employeeShift.InitHour = employeeShiftModel.EmployeeShifts.InitHour;
                employeeShift.EndHour = employeeShiftModel.EmployeeShifts.EndHour;
                employeeShift.WorkDay = date;
                employeeShift.IdEmployee = employeeShiftModel.EmployeeShifts.IdEmployee;
                employeesShifts.Add(employeeShift);
            }
            return employeesShifts;
        }
        private bool ValidateemployeeShiftModel(EmployeeShiftModel employeeShiftModel)
        {

            if (employeeShiftModel.EmployeeShifts.WorkDay > employeeShiftModel.EndDate)
            {
                TempData["ErrorDate"] = "The End Date must be greather than the Init Date";
            }
            if (employeeShiftModel.EmployeeShifts.InitHour > employeeShiftModel.EmployeeShifts.EndHour)
            {
                TempData["ErrorHours"] = "The End Date must be greather than the Init Date";
            }
            if (employeeShiftModel.EmployeeShifts.InitHour > 24 || employeeShiftModel.EmployeeShifts.InitHour <0)
            {
                TempData["ErrorInitHour"] = "The Init Hour must be a number between 0 and 24";
            }
            if (employeeShiftModel.EmployeeShifts.EndHour > 24 || employeeShiftModel.EmployeeShifts.EndHour < 0)
            {
                TempData["ErrorEndHour"] = "The End Hour must be a number between 0 and 24";
            }
            if (employeeShiftModel.EmployeeShifts.InitHour == employeeShiftModel.EmployeeShifts.EndHour)
            {
                TempData["ErrorSameHour"] = "The init Hour must be different to End Hour";
            }
            if (TempData.Count>0)
                return false;
            return true;
        }

        private void AddModelErrors()
        {
            if (TempData["ErrorDate"] != null && !String.IsNullOrEmpty(TempData["ErrorDate"].ToString()))
                ModelState.AddModelError("ErrorDate", TempData["ErrorDate"].ToString());
            if (TempData["ErrorHours"] != null && !String.IsNullOrEmpty(TempData["ErrorHours"].ToString()))
                ModelState.AddModelError("ErrorHours", TempData["ErrorHours"].ToString());
            if (TempData["InitEndHour"] != null && !String.IsNullOrEmpty(TempData["InitEndHour"].ToString()))
                ModelState.AddModelError("InitEndHour", TempData["InitEndHour"].ToString());
            if (TempData["ErrorEndHour"] != null && !String.IsNullOrEmpty(TempData["ErrorEndHour"].ToString()))
                ModelState.AddModelError("ErrorEndHour", TempData["ErrorEndHour"].ToString());
            if (TempData["Error"] != null && !String.IsNullOrEmpty(TempData["Error"].ToString()))
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            if (TempData["ErrorSameHour"] != null && !String.IsNullOrEmpty(TempData["ErrorSameHour"].ToString()))
                ModelState.AddModelError("ErrorSameHour", TempData["ErrorSameHour"].ToString());
            TempData.Clear();
        }
        private bool ValidateShift(EmployeesShifts shift)
        {
            if(_context.EmployeesShifts.FirstOrDefault(elem=> elem.IdEmployee == shift.IdEmployee
            && elem.WorkDay == shift.WorkDay 
            && ((elem.InitHour < shift.InitHour && elem.EndHour > shift.InitHour)
            || (elem.InitHour < shift.EndHour && elem.EndHour > shift.EndHour)
            || (elem.InitHour == shift.InitHour && elem.EndHour == shift.EndHour)
            || (elem.InitHour > shift.InitHour && elem.EndHour < shift.EndHour))
            ) != null)
            {
                TempData["Error"] = "The shift " + shift.InitHour + " - " + shift.EndHour + " is already included on day " + shift.WorkDay;
                return false;
            }

            return true;
        }
    }
}
