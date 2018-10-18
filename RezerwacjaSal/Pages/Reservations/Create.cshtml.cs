using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public CreateModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }
        public Room Room { get; set; }
        public IList<Pearson> People { get; set; }
        public IList<Reservation> Reservations { get; set; }

        public List<String> Hours;
        public DateTime Date { get; private set; }

        private List<int> AllPearsonNumbers;

        public int BuildingIdRoute { get; set; }
        public int DepartmentIdRoute { get; set; }
        public DateTime StartTime { get; private set; }
        public int RoomId { get; set; }
        [BindProperty]
        public int PearsonNumber { get; set; }

        public async Task<IActionResult> OnGetAsync( int roomid, int buildingid, int departmentid, string date, string time)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;
            
            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            DateTime.TryParse(date + " " + time, out var ParseTime);
            StartTime = ParseTime;

            RoomId = roomid;

            Room = await _context.Rooms
                    .Where(i => i.RoomID == roomid)
                    .Include(b=>b.Building)
                    .ThenInclude(d=>d.Department)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();


            Reservations = await _context.Reservations
                            .Include(r => r.Pearson)
                            .Include(r => r.Room)
                            .ThenInclude(b => b.Building)
                            .Where(b => b.Room.BuildingID == buildingid)
                            .Where(d => d.Date == Date)
                            .AsNoTracking()
                            .ToListAsync();

            Hours = new List<String>();
            Hours.Add(new DateTime(2018, 1, 1, 8, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 8, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 9, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 9, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 10, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 10, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 11, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 11, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 12, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 12, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 13, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 13, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 14, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 14, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 15, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 15, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 16, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 16, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 17, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 17, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 18, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 18, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 19, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 19, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 20, 00, 0).ToShortTimeString());

            ViewData["StartTime"] = new SelectList(Hours, StartTime.ToShortTimeString());
            ViewData["EndTime"] = new SelectList(Hours, StartTime.AddMinutes(30).ToShortTimeString());

            return Page();
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        [BindProperty]
        public string DateInputString { get; set; }
        public string ErrorString { get; private set; }
        public string PearsonNumberError { get; private set; }

        public async Task<IActionResult> OnPostAsync(int roomid, int buildingid, int departmentid, string date)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            AllPearsonNumbers = await _context.People
                .Select(i => i.PearsonNumber)
                .ToListAsync();

            RoomId = roomid;

            if (!ModelState.IsValid)
                return Page();

            Reservations = await _context.Reservations
                .Where(b => b.Room.BuildingID == buildingid)
                .Where(d => d.Date == Date)
                .Where(r => r.RoomID == Reservation.RoomID)
                .AsNoTracking()
                .ToListAsync();

            Room = await _context.Rooms
                .Where(i => i.RoomID == roomid)
                .Include(b => b.Building)
                .ThenInclude(d => d.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            People = await _context.People
                .AsNoTracking()
                .ToListAsync();

            Hours = new List<String>();
            Hours.Add(new DateTime(2018, 1, 1, 8, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 8, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 9, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 9, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 10, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 10, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 11, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 11, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 12, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 12, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 13, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 13, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 14, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 14, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 15, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 15, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 16, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 16, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 17, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 17, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 18, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 18, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 19, 00, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 19, 30, 0).ToShortTimeString());
            Hours.Add(new DateTime(2018, 1, 1, 20, 00, 0).ToShortTimeString());

            ViewData["StartTime"] = new SelectList(Hours, StartTime.ToShortTimeString());
            ViewData["EndTime"] = new SelectList(Hours, StartTime.AddMinutes(30).ToShortTimeString());

            if (Reservation.StartTime == Reservation.EndTime) ErrorString = "Rezerwacja musi trwać co najmniej 30 minut.";
            if (Reservation.StartTime > Reservation.EndTime) ErrorString = "Rezerwacja nie może się zacząć później niż skończyć.";

            foreach (var item in Reservations)
            {
                if (Reservation.StartTime == item.StartTime || Reservation.EndTime == item.EndTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (Reservation.EndTime < item.EndTime && Reservation.StartTime < item.StartTime && Reservation.EndTime > item.StartTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (Reservation.EndTime > item.EndTime && Reservation.StartTime < item.StartTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (Reservation.EndTime > item.EndTime && Reservation.StartTime > item.StartTime && Reservation.StartTime < item.EndTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (Reservation.EndTime < item.EndTime && Reservation.StartTime > item.StartTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
            }

            if (ErrorString != null) return Page();

            if (!AllPearsonNumbers.Contains(PearsonNumber))
            {
                PearsonNumberError = String.Format("Nie ma takiego człeka o numerze: {0}", PearsonNumber);
                return Page();
            }

            var newReservation = new Reservation();

            DateTime.TryParse(DateInputString, out var ParseStringDate);
            newReservation.Date = ParseStringDate;

            newReservation.PearsonID = People.Where(i => i.PearsonNumber == PearsonNumber).Select(i => i.PearsonID).FirstOrDefault();

            if (await TryUpdateModelAsync<Reservation>(
                newReservation,
                "Reservation",
                 s => s.Note, s => s.StartTime, s => s.EndTime, s => s.RoomID))
            {
                _context.Reservations.Add(newReservation);
                await _context.SaveChangesAsync();
            }

            return Redirect("./Details" + "?" + "reservationid=" + newReservation.ReservationID.ToString() + "&buildingid=" + BuildingIdRoute.ToString() + "&departmentid=" + DepartmentIdRoute.ToString() + "&date=" + Date.ToShortDateString());
        }
    }
}