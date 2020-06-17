using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReservationManagementApp.Models.Dto;
using ReservationManagementApp.Models.ServiceEmployeeModel;
using ReservationManagementApp.ServicesApp;

namespace ReservationManagementApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ServicesEmployeesController : Controller
    {
        private readonly IConfiguration _configuration;
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        public ServicesEmployeesController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: ServicesEmployees
        public IActionResult Index(int? idEmployee)
        {
            if (TempData["Error"] != null && !String.IsNullOrEmpty(TempData["Error"].ToString()))
                ModelState.AddModelError("Error", TempData["Error"].ToString());
            TempData.Clear();


            string responseBodyServicesEmployeeList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceEmployeeApi").GetAwaiter().GetResult();
            List<ServiceEmployeeDto> serviceEmployeeListDto = JsonConvert.DeserializeObject<List<ServiceEmployeeDto>>(responseBodyServicesEmployeeList);
            var servicesEmployeesList = serviceEmployeeListDto.Where(elem => elem.IdEmployee == idEmployee);

            string responseBodyServiceList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi").GetAwaiter().GetResult();
            List<ServiceDto> serviceDto = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBodyServiceList);
            ViewData["IdService"] = new SelectList(serviceDto, "Id", "Name");
            ServiceEmployeeModel serviceEmployeeModel = new ServiceEmployeeModel();
            serviceEmployeeModel.ServicesEmployeesList = servicesEmployeesList;
            serviceEmployeeModel.ServiceEmployee = new ServiceEmployeeDto();

            string responseBodyEmployee= this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + idEmployee.Value).GetAwaiter().GetResult();
            EmployeeDto employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
            serviceEmployeeModel.ServiceEmployee.IdEmployeeNavigation = employeeDto;
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
            ServiceEmployeeDto serviceEmployee = new ServiceEmployeeDto();
            serviceEmployee.IdEmployee = serviceEmployeeModel.ServiceEmployee.IdEmployee;
            serviceEmployee.IdService = serviceEmployeeModel.ServiceEmployee.IdService;

            string responseBodyServicesEmployeeList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceEmployeeApi").GetAwaiter().GetResult();
            List<ServiceEmployeeDto> serviceEmployeeListDto = JsonConvert.DeserializeObject<List<ServiceEmployeeDto>>(responseBodyServicesEmployeeList);


            if (serviceEmployeeListDto.FirstOrDefault(elem => elem.IdEmployee == serviceEmployee.IdEmployee && elem.IdService == serviceEmployee.IdService) != null)
            {
                TempData["Error"] = "This services is assigned";
                return RedirectToAction(nameof(Index), new
                {
                    idEmployee = serviceEmployee.IdEmployee
                });
            }
            if (serviceEmployeeModel.ServiceEmployee.IdService != 0)
            {
                string responseBodyServicesEmployee = await this._clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceEmployeeApi", JsonConvert.SerializeObject(serviceEmployee));
                ServiceEmployeeDto employeeDto = JsonConvert.DeserializeObject<ServiceEmployeeDto>(responseBodyServicesEmployee);
                return RedirectToAction(nameof(Index), new
                {
                    idEmployee = serviceEmployee.IdEmployee
                });
            }

            string responseBodyServicesList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi").GetAwaiter().GetResult();
            List<ServiceDto> serviceDto = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBodyServicesList);
            ViewData["IdService"] = new SelectList(serviceDto, "Id", "Name", serviceEmployee.IdService);
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

            string responseBodyServicesEmployeeList = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceEmployeeApi/" + id);
            ServiceEmployeeDto serviceEmployee = JsonConvert.DeserializeObject<ServiceEmployeeDto>(responseBodyServicesEmployeeList);
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
            string responseBodyServicesEmployeeList =await this._clientService.DeleteResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceEmployeeApi/" + id);
            ServiceEmployeeDto serviceEmployee = JsonConvert.DeserializeObject<ServiceEmployeeDto>(responseBodyServicesEmployeeList);
            return RedirectToAction(nameof(Index), new
            {
                idEmployee = serviceEmployee.IdEmployee
            });
        }

        private bool ServicesEmployeesExists(int id)
        {
            string responseBodyServicesEmployeeList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceEmployeeApi/" + id).GetAwaiter().GetResult();
            ServiceEmployeeDto serviceEmployee = JsonConvert.DeserializeObject<ServiceEmployeeDto>(responseBodyServicesEmployeeList);
            return serviceEmployee!= null;
        }
    }
}
