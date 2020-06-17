using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReservationManagementApp.Models.Dto;
using ReservationManagementApp.Models.EmployeeShiftModel;
using ReservationManagementApp.ServicesApp;

namespace ReservationManagementApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class EmployeesShiftsController : Controller
    {
        private readonly IConfiguration _configuration;
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        public EmployeesShiftsController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: EmployeesShifts
        public IActionResult Index(int? idEmployee, DateTime? workDay)
        {
            AddModelErrors();
            string responseBodyEmployeesShiftsList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeShiftApi").GetAwaiter().GetResult();
            List<EmployeeShiftDto> employeeShift = JsonConvert.DeserializeObject<List<EmployeeShiftDto>>(responseBodyEmployeesShiftsList);
            var employeesShiftsList = employeeShift.Where(elem => elem.IdEmployee == idEmployee
            && ((workDay != null && elem.WorkDay == workDay) || (workDay == null && elem.WorkDay >= DateTime.Today))).OrderBy(elem => elem.WorkDay);/*.Include(e => e.IdEmployeeNavigation);*/
            EmployeeShiftModel employeeShiftModel = new EmployeeShiftModel();
            employeeShiftModel.EmployeeShiftsList = employeesShiftsList;
            employeeShiftModel.EmployeeShift = new EmployeeShiftDto();

            string responseBodyEmployee = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + idEmployee).GetAwaiter().GetResult();
            EmployeeDto employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);


            employeeShiftModel.EmployeeShift.IdEmployeeNavigation = employeeDto;
            employeeShiftModel.EmployeeShift.IdEmployee = idEmployee.Value;
            employeeShiftModel.EmployeeShift.WorkDay = workDay == null ? DateTime.Now : workDay.Value;
            employeeShiftModel.EndDate = workDay == null ? DateTime.Now : workDay.Value;

            return View(employeeShiftModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter(EmployeeShiftModel employeeShiftModel)
        {
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = employeeShiftModel.EmployeeShift.IdEmployee,
                workDay = employeeShiftModel.EmployeeShift.WorkDay
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearFilter(EmployeeShiftModel employeeShiftModel)
        {
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = employeeShiftModel.EmployeeShift.IdEmployee
            });
        }


        // POST: EmployeesShifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeShiftModel employeeShiftModel)
        {
            List<EmployeeShiftDto> allShiftsToAdd = GetShiftsForEmployee(employeeShiftModel);
            if (ValidateemployeeShiftModel(employeeShiftModel))
            {
                if (allShiftsToAdd.Count() > 0)//Validaciones
                {
                    foreach (EmployeeShiftDto shift in allShiftsToAdd)
                    {
                        if (ValidateShift(shift))
                        {
                            string responseBodyEmployeeShift = await this._clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeShiftApi", JsonConvert.SerializeObject(shift));
                        }
                        else
                        {
                            return RedirectToAction(nameof(Index), new
                            {
                                idEmployee = employeeShiftModel.EmployeeShift.IdEmployee
                            });
                        }
                    }
                    return RedirectToAction(nameof(Index), new
                    {
                        idEmployee = employeeShiftModel.EmployeeShift.IdEmployee
                    });
                }
            }
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = employeeShiftModel.EmployeeShift.IdEmployee
            });
        }

        // GET: EmployeesShifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string responseBodyEmployeeShift = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeShiftApi/" + id);
            EmployeeShiftDto employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDto>(responseBodyEmployeeShift);
            var employeesShifts = employeeShift;
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
            string responseBodyEmployeeShift = await this._clientService.DeleteResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeShiftApi/" + id);
            EmployeeShiftDto employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDto>(responseBodyEmployeeShift);
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = employeeShift.IdEmployee
            });
        }

        private bool EmployeesShiftsExists(int id)
        {
            string responseBodyEmployeeShift = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeShiftApi/" + id).GetAwaiter().GetResult();
            EmployeeDto employeeShift = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployeeShift);
            return employeeShift != null;
        }

        private List<EmployeeShiftDto> GetShiftsForEmployee(EmployeeShiftModel employeeShiftModel)
        {
            List<EmployeeShiftDto> employeesShifts = new List<EmployeeShiftDto>();
            var dates = new List<DateTime>();

            for (var dt = employeeShiftModel.EmployeeShift.WorkDay; dt <= employeeShiftModel.EndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            foreach (DateTime date in dates)
            {
                EmployeeShiftDto employeeShift = new EmployeeShiftDto();
                employeeShift.InitHour = employeeShiftModel.EmployeeShift.InitHour;
                employeeShift.EndHour = employeeShiftModel.EmployeeShift.EndHour;
                employeeShift.WorkDay = date;
                employeeShift.IdEmployee = employeeShiftModel.EmployeeShift.IdEmployee;
                employeesShifts.Add(employeeShift);
            }
            return employeesShifts;
        }
        private bool ValidateemployeeShiftModel(EmployeeShiftModel employeeShiftModel)
        {
            if (employeeShiftModel.EmployeeShift.WorkDay > employeeShiftModel.EndDate)
            {
                TempData["ErrorDate"] = "The End Date must be greather than the Init Date";
            }
            if (employeeShiftModel.EmployeeShift.InitHour > employeeShiftModel.EmployeeShift.EndHour)
            {
                TempData["ErrorHours"] = "The End Date must be greather than the Init Date";
            }
            if (employeeShiftModel.EmployeeShift.InitHour > 24 || employeeShiftModel.EmployeeShift.InitHour < 0)
            {
                TempData["ErrorInitHour"] = "The Init Hour must be a number between 0 and 24";
            }
            if (employeeShiftModel.EmployeeShift.EndHour > 24 || employeeShiftModel.EmployeeShift.EndHour < 0)
            {
                TempData["ErrorEndHour"] = "The End Hour must be a number between 0 and 24";
            }
            if (employeeShiftModel.EmployeeShift.InitHour == employeeShiftModel.EmployeeShift.EndHour)
            {
                TempData["ErrorSameHour"] = "The init Hour must be different to End Hour";
            }
            if (TempData.Count > 0)
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
        private bool ValidateShift(EmployeeShiftDto shift)
        {
            string responseBodyEmployeeShift = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeShiftApi").GetAwaiter().GetResult();
            List<EmployeeShiftDto> employeeShift = JsonConvert.DeserializeObject<List<EmployeeShiftDto>>(responseBodyEmployeeShift);
            if (employeeShift.FirstOrDefault(elem => elem.IdEmployee == shift.IdEmployee
             && elem.WorkDay == shift.WorkDay
             && ((elem.InitHour < shift.InitHour && elem.EndHour > shift.InitHour)
             || (elem.InitHour < shift.EndHour && elem.EndHour > shift.EndHour)
             || (elem.InitHour == shift.InitHour && elem.EndHour <= shift.EndHour)
             || (elem.InitHour >= shift.InitHour && elem.EndHour <= shift.EndHour)
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
