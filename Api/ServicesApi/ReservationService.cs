using Api.IServicesApi;
using Api.Models;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ServicesApi
{
    public class ReservationService : IReservationService
    {
        private readonly IMapper _mapper;
        private ReservationOperations db = new ReservationOperations();
        private EmployeeShiftOperations dbEmployeeShiftOerations = new EmployeeShiftOperations();
        private ServiceOperations dbServiceOperations = new ServiceOperations();
        private EmployeeOperations dbEmployeeOperations = new EmployeeOperations();
        public ReservationService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<ReservationDto> GetReservations()
        {
            List<ReservationDto> reservationsDto = new List<ReservationDto>();
            List<Reservations> reservations = db.GetAllReservations();
            reservationsDto = _mapper.Map<List<Reservations>, List<ReservationDto>>(reservations);
            return reservationsDto;
        }

        public List<ReservationDto> GetAllAvailabilityReservations(int idEmployee, int idService, int idUser, string dateTime)
        {
            ReservationDto reservationDto = new ReservationDto()
            {
                IdEmployee = idEmployee,
                IdService = idService,
                IdUser = idUser,
                Date = Convert.ToDateTime(dateTime)
            };
            Services service = dbServiceOperations.GetByPk(reservationDto.IdService);
            Employees employee = dbEmployeeOperations.GetByPk(reservationDto.IdEmployee);
            reservationDto.IdEmployeeNavigation = _mapper.Map<Employees, EmployeeDto>(employee);
            reservationDto.IdServiceNavigation = _mapper.Map<Services, ServiceDto>(service);

            List<ReservationDto> reservationsDto = GetAvailability(reservationDto);
            return reservationsDto;
        }
        public ReservationDto GetReservationById(int id)
        {
            var result = db.GetByPk(id);
            return _mapper.Map<Reservations, ReservationDto>(result);
        }
        public ReservationDto AddReservation(ReservationDto reservationDto)
        {
            Reservations reservationDb = new Reservations();
            reservationDb = _mapper.Map<ReservationDto, Reservations>(reservationDto);
            var result = db.CreateReservation(reservationDb);
            return _mapper.Map<Reservations, ReservationDto>(result);
        }
        public ReservationDto UpdateReservation(ReservationDto reservationDto)
        {
            Reservations reservationDb = new Reservations();
            reservationDb = _mapper.Map<ReservationDto, Reservations>(reservationDto);
            var result = db.UpdateReservation(reservationDb);
            return _mapper.Map<Reservations, ReservationDto>(result);
        }
        public ReservationDto RemoveReservation(int id)
        {
            var result = db.DeleteReservation(id);
            return _mapper.Map<Reservations, ReservationDto>(result);
        }

        private List<ReservationDto> GetAvailability(ReservationDto reservationDto)
        {
            List<ReservationDto> possibleReservations = new List<ReservationDto>();
            List<ReservationDto> currentReservations = GetCurrentsReservations(reservationDto);
            possibleReservations = GetPossibleReservations(reservationDto);
            if (possibleReservations != null && possibleReservations.Count > 0)
            {
                possibleReservations = possibleReservations.Where(elem => currentReservations.FirstOrDefault(elem2 => elem.Date == elem2.Date) == null).ToList();
            }
            return possibleReservations;

        }

        private List<ReservationDto> GetCurrentsReservations(ReservationDto reservation)
        {
            List<Reservations> reservationsList = db.GetAllReservations();
            List<ReservationDto> reservationListDto = _mapper.Map<List<Reservations>, List<ReservationDto>>(reservationsList);

            List<ReservationDto> currentReservations = reservationListDto
               .Where(elem => elem.IdEmployee == reservation.IdEmployee &&
               elem.Date.Date == reservation.Date.Date).ToList();
            return reservationListDto;
        }

        private List<ReservationDto> GetPossibleReservations(ReservationDto reservation)
        {
            List<ReservationDto> possibleReservations = new List<ReservationDto>();
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
                        possibleReservations.Add(possibleReservation);
                    }
                }
            return possibleReservations;
        }
    }
}
