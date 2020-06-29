using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AutoMapper;
using DataAccess.Operations;
using FunctionsReservation.Models;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace FunctionsReservation.Functions
{
    public class FunctionsReservationApi
    {
        private readonly IMapper _mapper;
        private ReservationOperations db = new ReservationOperations();
        private EmployeeShiftOperations dbEmployeeShiftOerations = new EmployeeShiftOperations();
        private ServiceOperations dbServiceOperations = new ServiceOperations();
        private EmployeeOperations dbEmployeeOperations = new EmployeeOperations();

        public FunctionsReservationApi(IMapper mapper)
        {
            _mapper = mapper;
        }

        [FunctionName("GetReservations")]
        public IActionResult GetReservations(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ReservationFunctionApi")] HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            List<Reservations> reservations = db.GetAllReservations();
            List<ReservationDto> responseMessage = _mapper.Map<List<Reservations>, List<ReservationDto>>(reservations);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetAllAvailabilityReservations")]
        public IActionResult GetAllAvailabilityReservations(
       [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ReservationFunctionApi/Availability")] HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var query = System.Web.HttpUtility.ParseQueryString(req.RequestUri.Query);
            string result = query.Get("result");
            ReservationDto reservationDto = new ReservationDto()
            {
                IdEmployee = Convert.ToInt32(query.Get("idEmployee")),
                IdService = Convert.ToInt32(query.Get("idService")),
                IdUser = Convert.ToInt32(query.Get("idUser")),
                Date = Convert.ToDateTime(query.Get("dateTime"))
            };
            Services service = dbServiceOperations.GetByPk(reservationDto.IdService);
            Employees employee = dbEmployeeOperations.GetByPk(reservationDto.IdEmployee);
            reservationDto.IdEmployeeNavigation = _mapper.Map<Employees, EmployeeDto>(employee);
            reservationDto.IdServiceNavigation = _mapper.Map<Services, ServiceDto>(service);

            List<ReservationDto> responseMessage = GetAvailability(reservationDto);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetReservation")]
        public IActionResult GetReservation(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ReservationFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.GetByPk(id);
            ReservationDto responseMessage = _mapper.Map<Reservations, ReservationDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("AddReservation")]
        public async Task<IActionResult> AddReservation(
            [HttpTrigger(AuthorizationLevel.Function, "Post", Route = "ReservationFunctionApi")]HttpRequestMessage req, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            ReservationDto reservationDto = JsonSerializer.Deserialize<ReservationDto>(content);
            Reservations reservationDb = _mapper.Map<ReservationDto, Reservations>(reservationDto);
            var result = db.CreateReservation(reservationDb);
            ReservationDto responseMessage = _mapper.Map<Reservations, ReservationDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpdateReservation")]
        public async Task<IActionResult> UpdateReservation(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "ReservationFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var content = await req.Content.ReadAsStringAsync();
            ReservationDto reservationDto = JsonSerializer.Deserialize<ReservationDto>(content);
            Reservations reservationDb = _mapper.Map<ReservationDto, Reservations>(reservationDto);
            var result = db.UpdateReservation(reservationDb);
            ReservationDto responseMessage = _mapper.Map<Reservations, ReservationDto>(result);
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("RemoveReservation")]
        public IActionResult RemoveReservation(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "ReservationFunctionApi/{id:int}")] HttpRequestMessage req, int id, ExecutionContext context)
        {
            /*Validate JWT*/
            if ((SecurityJwt.ValidateToken(req.Headers.Authorization, context.FunctionAppDirectory)) == null)
            {
                return new UnauthorizedResult();
            }
            /*Code*/
            var result = db.DeleteReservation(id);
            ReservationDto responseMessage = _mapper.Map<Reservations, ReservationDto>(result);
            return new OkObjectResult(responseMessage);
        }

        private List<ReservationDto> GetAvailability(ReservationDto reservationDto)
        {
            List<ReservationDto> possibleReservationsList = new List<ReservationDto>();
            List<ReservationDto> currentReservationsList = GetCurrentsReservations(reservationDto);
            possibleReservationsList = GetPossibleReservations(reservationDto);
            if (possibleReservationsList != null && possibleReservationsList.Count > 0)
            {
                possibleReservationsList = possibleReservationsList.Where(elem => currentReservationsList.FirstOrDefault(elem2 => elem.Date == elem2.Date) == null).ToList();
            }
            return possibleReservationsList;

        }

        private List<ReservationDto> GetCurrentsReservations(ReservationDto reservation)
        {
            List<Reservations> reservationsList = db.GetAllReservations();
            List<ReservationDto> reservationsListDto = _mapper.Map<List<Reservations>, List<ReservationDto>>(reservationsList);

            List<ReservationDto> currentReservations = reservationsListDto
               .Where(elem => elem.IdEmployee == reservation.IdEmployee &&
               elem.Date.Date == reservation.Date.Date).ToList();
            return currentReservations;
        }

        private List<ReservationDto> GetPossibleReservations(ReservationDto reservation)
        {
            List<ReservationDto> possibleReservationsList = new List<ReservationDto>();
            List<EmployeesShifts> employeesShiftsList = dbEmployeeShiftOerations.GetAllEmployeesShifts();
            employeesShiftsList = employeesShiftsList.Where(elem => elem.IdEmployee == reservation.IdEmployee && elem.WorkDay == reservation.Date).OrderBy(elem => elem.InitHour).ToList();
            List<EmployeeShiftDto> employeesShiftsListDto = _mapper.Map<List<EmployeesShifts>, List<EmployeeShiftDto>>(employeesShiftsList);
            foreach (EmployeeShiftDto employeeShiftDto in employeesShiftsListDto)
                if (employeeShiftDto != null)
                {
                    for (int i = employeeShiftDto.InitHour; i < employeeShiftDto.EndHour; i++)
                    {
                        ReservationDto possibleReservation = new ReservationDto();
                        possibleReservation.IdEmployee = reservation.IdEmployee;
                        possibleReservation.IdService = reservation.IdService;
                        possibleReservation.IdUser = reservation.IdUser;
                        possibleReservation.IdServiceNavigation = reservation.IdServiceNavigation;
                        possibleReservation.IdEmployeeNavigation = reservation.IdEmployeeNavigation;
                        DateTime possibleDatetime = reservation.Date.AddHours(i);
                        possibleReservation.Date = possibleDatetime;
                        possibleReservationsList.Add(possibleReservation);
                    }
                }
            return possibleReservationsList;
        }
    }
}
