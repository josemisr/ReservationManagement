using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationManagementApp.Models;
using ReservationManagementApp.Models.Reservation;

namespace ReservationManagementApp.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationManagementDbContext _context;

        public ReservationsController(ReservationManagementDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public IActionResult Index()
        {
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name");

            if (TempData["Date"] != null && !String.IsNullOrEmpty(TempData["Date"].ToString()))
                ModelState.AddModelError("reservation.Date", TempData["Date"].ToString());

            if (TempData["IdService"]!= null && !String.IsNullOrEmpty(TempData["IdService"].ToString()))
                ModelState.AddModelError("reservation.IdService", TempData["IdService"].ToString());

            TempData.Clear();

            ReservationModel reservationModel = new ReservationModel();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                Users user = JsonSerializer.Deserialize<Users>(HttpContext.Session.GetString("User"));
                reservationModel.Reservations = _context.Reservations.Where(elem => elem.IdUser ==user.Id);
            }
           
            return View(reservationModel);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.Reservations
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdServiceNavigation)
                .Include(r => r.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservations == null)
            {
                return NotFound();
            }

            return View(reservations);
        }

        // GET: Reservations/Create
        public IActionResult New([Bind("Id,IdService,IdEmployee,IdUser,Date")] Reservations reservation)
        {
            if (ModelState.IsValid)
            {
                if (reservation.IdService == 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                reservation.IdServiceNavigation = _context.Services.FirstOrDefault(elem => elem.Id == reservation.IdService);
                reservation.IdEmployeeNavigation = _context.Employees.FirstOrDefault(elem => elem.Id == reservation.IdEmployee);
                ViewData["IdEmployee"] = new SelectList(_context.Employees
                    .Include(r => r.ServicesEmployees)
                    .Include(r => r.EmployeesShifts)
                    .Where(elem => elem.ServicesEmployees.FirstOrDefault(elem2 => elem2.IdService == reservation.IdService) != null
                                    && elem.EmployeesShifts.FirstOrDefault(elem2 => elem2.WorkDay == reservation.Date) != null)
                    , "Id", "Name");

                if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                {
                    Users user = JsonSerializer.Deserialize<Users>(HttpContext.Session.GetString("User"));
                    reservation.IdUser = user.Id;
                }

                if (reservation.IdEmployee == 0)
                {
                    reservation.IdEmployee = 0;
                }

                ReservationModel reseservationModel = new ReservationModel();
                reseservationModel.Reservation = reservation;
                reseservationModel.Reservations = Getavailability(reservation);//.Where(elem=>elem.IdEmployeeNavigation.Id==reservation.IdEmployee);
                return View(reseservationModel);
            }
            if (ModelState["reservation.Date"] != null)
                TempData["Date"] = ModelState["reservation.Date"].Errors.Count() > 0 ? "Select a Date" : "";
            if (ModelState["reservation.IdService"] != null)
                TempData["IdService"] = ModelState["reservation.IdService"].Errors.Count() > 0 ? "Select a Service" : "";
            return RedirectToAction(nameof(Index));
        }

        private List<Reservations> Getavailability(Reservations reservation)
        {
            List<Reservations> currentReservations = GetCurrentsReservations(reservation);
            List<Reservations> possibleReservations = GetPossibleReservations(reservation);
            possibleReservations = possibleReservations.Where(elem => currentReservations.FirstOrDefault(elem2=> elem.Date == elem2.Date) == null ).ToList();
            return possibleReservations;
        }

        private List<Reservations> GetCurrentsReservations(Reservations reservation)
        {
            List<Reservations> currentReservations = _context.Reservations
               .Where(elem => elem.IdEmployee == reservation.IdEmployee &&
               elem.Date.Date == reservation.Date.Date && elem.IdService == reservation.IdService).ToList();
            return currentReservations;
        }

        private List<Reservations> GetPossibleReservations(Reservations reservation)
        {
            List<Reservations> possibleReservations = new List<Reservations>();

            EmployeesShifts employeeShifts = _context.EmployeesShifts.FirstOrDefault(elem => elem.IdEmployee == reservation.IdEmployee);
            if (employeeShifts != null)
            {
                for (int i = employeeShifts.InitHour; i <= employeeShifts.EndHour; i++)
                {
                    Reservations possibleReservation = new Reservations();
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

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdService,IdEmployee,IdUser,Date")] Reservations reservation)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                {
                    Users user = JsonSerializer.Deserialize<Users>(HttpContext.Session.GetString("User"));
                    reservation.IdUser = user.Id;
                }
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "IdCard", reservation.IdEmployee);
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name", reservation.IdService);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Email", reservation.IdUser);
            ReservationModel reseservationModel = new ReservationModel();
            reseservationModel.Reservation = reservation;
            reseservationModel.Reservations = _context.Reservations;
            return View(reseservationModel);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.Reservations.FindAsync(id);
            if (reservations == null)
            {
                return NotFound();
            }
            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "IdCard", reservations.IdEmployee);
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name", reservations.IdService);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Email", reservations.IdUser);
            return View(reservations);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdService,IdEmployee,IdUser,Date")] Reservations reservations)
        {
            if (id != reservations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationsExists(reservations.Id))
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
            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "IdCard", reservations.IdEmployee);
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name", reservations.IdService);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Email", reservations.IdUser);
            return View(reservations);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.Reservations
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdServiceNavigation)
                .Include(r => r.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var reservations = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationsExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
