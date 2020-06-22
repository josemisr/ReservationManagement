using System.Collections.Generic;
using Api.IServicesApi;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "TestKey")]
    [ApiController]
    public class ReservationApiController : ControllerBase
    {
        IReservationService _reservationService;
        public ReservationApiController(IReservationService reservationService)
        {
            this._reservationService = reservationService;
        }

        // GET: api/ReservationApi
        [HttpGet]
        public IActionResult GetReservations()
        {
            List<ReservationDto> reservations = this._reservationService.GetReservations();
            return Ok(reservations);
        }

        // GET: api/ReservationApi/Availability/5
        [HttpGet, Route("Availability")]
        public IActionResult GetAllAvailabilityReservations(int idEmployee, int idService, int idUser, string dateTime)
        {
            List<ReservationDto> reservations = this._reservationService.GetAllAvailabilityReservations(idEmployee, idService, idUser, dateTime);
            return Ok(reservations);
        }

        // GET: api/ReservationApi/5
        [HttpGet("{id}", Name = "GetReservation")]
        public IActionResult GetReservation(int id)
        {
            var result = this._reservationService.GetReservationById(id);
            return Ok(result);
        }

        // POST: api/ReservationApi
        [HttpPost]
        public IActionResult Post([FromBody] ReservationDto value)
        {
            var result = this._reservationService.AddReservation(value);
            return Ok(result);
        }

        // PUT: api/ReservationApi/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ReservationDto value)
        {
            var result = this._reservationService.UpdateReservation(value);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = this._reservationService.RemoveReservation(id);
            return Ok(result);
        }
    }
}
