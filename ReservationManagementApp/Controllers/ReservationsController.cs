using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReservationManagementApp.Models.Dto;
using ReservationManagementApp.Models.Reservation;
using ReservationManagementApp.ServicesApp;

namespace ReservationManagementApp.Controllers
{
    public class ReservationsController : Controller
    {

        private readonly IConfiguration _configuration;
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        public ReservationsController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Reservations
        public IActionResult Index()
        {
            string responseBodyServiceList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi").GetAwaiter().GetResult();
            List<ServiceDto> servicesList = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBodyServiceList);
            ViewData["IdService"] = new SelectList(servicesList, "Id", "Name");

            if (TempData["Date"] != null && !String.IsNullOrEmpty(TempData["Date"].ToString()))
                ModelState.AddModelError("reservation.Date", TempData["Date"].ToString());

            if (TempData["IdService"] != null && !String.IsNullOrEmpty(TempData["IdService"].ToString()))
                ModelState.AddModelError("reservation.IdService", TempData["IdService"].ToString());

            TempData.Clear();

            ReservationModel reservationModel = new ReservationModel();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                UserDto user = JsonConvert.DeserializeObject<UserDto>(HttpContext.Session.GetString("User"));

                string responseBodyReservationsList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ReservationApi").GetAwaiter().GetResult();
                List<ReservationDto> reservationsList = JsonConvert.DeserializeObject<List<ReservationDto>>(responseBodyReservationsList);
                reservationModel.Reservations = reservationsList.Where(elem => elem.IdUser == user.Id && elem.Date >= DateTime.Today);
            }

            return View(reservationModel);
        }
        public IActionResult New([Bind("Id,IdService,IdEmployee,IdUser,Date")] ReservationDto reservation)
        {
            if (TempData["ErrorCreate"] != null)
            {
                ModelState.AddModelError("ErrorCreate", TempData["ErrorCreate"].ToString());
                TempData.Clear();
            }
            string responseBodyService = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ServiceApi/" + reservation.IdService).GetAwaiter().GetResult();
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBodyService);
            reservation.IdServiceNavigation = service;

            string responseBodyEmployee = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi/" + reservation.IdEmployee).GetAwaiter().GetResult();
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
            reservation.IdEmployeeNavigation = employee;

            string responseBodyEmployeesList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/EmployeeApi").GetAwaiter().GetResult();
            List<EmployeeDto> employeesList = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseBodyEmployeesList);
            ViewData["IdEmployee"] = new SelectList(employeesList
                .Where(elem => elem.ServicesEmployees.FirstOrDefault(elem2 => elem2.IdService == reservation.IdService) != null
                                && elem.EmployeesShifts.FirstOrDefault(elem2 => elem2.WorkDay.Date == reservation.Date.Date) != null)
                , "Id", "Name");

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                UserDto user = JsonConvert.DeserializeObject<UserDto>(HttpContext.Session.GetString("User"));
                reservation.IdUser = user.Id;
            }
            ReservationModel reseservationModel = new ReservationModel();
            reseservationModel.Reservation = reservation;

            string responseBodyReservationAvailabilityList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ReservationApi/Availability?idEmployee=" + reservation.IdEmployee + "&idService=" + reservation.IdService + "&idUser=" + reservation.IdUser + "&datetime=" + reservation.Date.ToString()).GetAwaiter().GetResult();
            List<ReservationDto> reservationAvailabilityList = JsonConvert.DeserializeObject<List<ReservationDto>>(responseBodyReservationAvailabilityList);
            reseservationModel.Reservations = reservationAvailabilityList;
            return View(reseservationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search([Bind("Id,IdService,IdEmployee,IdUser,Date")] ReservationDto reservation)
        {
            if (ModelState.IsValid && reservation.IdService != 0)
            {
                return RedirectToAction(nameof(New), new
                {
                    idService = reservation.IdService,
                    idEmployee = reservation.IdEmployee,
                    date = reservation.Date
                });
            }
            if (ModelState["reservation.Date"] != null)
                TempData["Date"] = ModelState["reservation.Date"].Errors.Count() > 0 ? "Select a Date" : "";
            if (ModelState["reservation.IdService"] != null)
                TempData["IdService"] = ModelState["reservation.IdService"].Errors.Count() > 0 ? "Select a Service" : "";
            return RedirectToAction(nameof(Index));
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdService,IdEmployee,IdUser,Date")] ReservationDto reservation)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                {
                    UserDto user = JsonConvert.DeserializeObject<UserDto>(HttpContext.Session.GetString("User"));
                    reservation.IdUser = user.Id;
                }
                await this._clientService.PostResponse(this._configuration["AppSettings:ApiRest"] + "api/ReservationApi", JsonConvert.SerializeObject(reservation));
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorCreate"] = "The data is not correct.";
            return RedirectToAction(nameof(New), reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string responseBodyReservationList = await this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ReservationApi");
            List<ReservationDto> reservationList = JsonConvert.DeserializeObject<List<ReservationDto>>(responseBodyReservationList);
            var reservations = reservationList
                .FirstOrDefault(m => m.Id == id);
            if (reservations == null)
            {
                return NotFound();
            }

            return View(reservations);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this._clientService.DeleteResponse(this._configuration["AppSettings:ApiRest"] + "api/ReservationApi/" + id);
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationsExists(int id)
        {
            string responseBodyReservationList = this._clientService.GetResponse(this._configuration["AppSettings:ApiRest"] + "api/ReservationApi/" + id).GetAwaiter().GetResult();
            List<ReservationDto> reservationList = JsonConvert.DeserializeObject<List<ReservationDto>>(responseBodyReservationList);
            return reservationList.Any(e => e.Id == id);
        }
    }
}
