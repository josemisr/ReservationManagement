using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Operations
{
    public class ReservationOperations
    {
        private ReservationManagementDbContext db = new ReservationManagementDbContext();
        public List<Reservations> GetAllReservations()
        {
            List<Reservations> reservations = db.Reservations.Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdServiceNavigation).Include(r => r.IdUserNavigation).ToList();
            return reservations;
        }
        public Reservations CreateReservation(Reservations reservation)
        {
            db.Add(reservation);
            db.SaveChanges();
            return reservation;
        }
        public Reservations DeleteReservation(int id)
        {
            var reservation = GetByPk(id);
            db.Remove(reservation);
            db.SaveChanges();
            return reservation;
        }

        public Reservations UpdateReservation(Reservations reservation)
        {
            db.Update(reservation);
            db.SaveChanges();
            return reservation;
        }

        public Reservations GetByPk(int id)
        {
            var reservation = db.Reservations.Find(id);
            return reservation;
        }
    }
}
