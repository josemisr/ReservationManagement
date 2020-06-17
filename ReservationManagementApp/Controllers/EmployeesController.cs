using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReservationManagementApp.Models.Dto;
using ReservationManagementApp.ServicesApp;

namespace ReservationManagementApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _configuration;
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        public EmployeesController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            string responseBodyEmployeeList = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi");
            List<EmployeeDto> employeeList = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseBodyEmployeeList);
            return View(employeeList);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string responseBodyEmployee = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + id);
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Surname2,IdCard")] EmployeeDto employee)
        {
            if (ModelState.IsValid)
            {
                string responseBodyEmployee = await this._clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi", JsonConvert.SerializeObject(employee));
                EmployeeDto employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
                if (employeeDto != null)
                {
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = employeeDto.Id
                    });
                }
                else { return View(employee); }

            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string responseBodyEmployee = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + id);
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Surname2,IdCard")] EmployeeDto employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string responseBodyEmployee = await this._clientService.PutResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + employee.Id, JsonConvert.SerializeObject(employee));
                    EmployeeDto employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employee.Id))
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
            if (id == null)
            {
                return NotFound();
            }

            string responseBodyEmployee = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + id);
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            string responseBodyEmployee = this._clientService.DeleteResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + id).GetAwaiter().GetResult();
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
            string responseBodyEmployee = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + id).GetAwaiter().GetResult();
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
            return employee != null;
        }
    }
}
