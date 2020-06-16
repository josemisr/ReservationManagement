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
    public class ReservationService:IReservationService
    {
        private readonly IMapper _mapper;
        private ReservationOperations db = new ReservationOperations();
        public ReservationService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<ReservationDto> GetReservations()
        {
            List<ReservationDto> reservationsDto = new List<ReservationDto>();
            List<Reservations> reservations = db.GetAllReservations();
            reservationsDto =_mapper.Map<List<Reservations>, List<ReservationDto>>(reservations);
            return reservationsDto;
        }
        public ReservationDto GetReservationById(int id)
        {
            var result= db.GetByPk(id);
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
    }
}
