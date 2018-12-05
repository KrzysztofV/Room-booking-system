using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Reservations
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public EditModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public DateTime Date { get; private set; }
        [BindProperty]
        public Reservation Reservation { get; set; }
        public IList<Room> Rooms { get; set; }

        private List<int> AllNumbers;

        public IList<ApplicationUser> AppUsers { get; set; }
        public int BuildingIdRoute { get; private set; }
        public int DepartmentIdRoute { get; private set; }
        public int ReservationIdRoute { get; set; }
        [BindProperty]
        public int Number { get; set; }
        [BindProperty]
        public int RoomID { get; private set; }
        public List<string> Hours { get; private set; }

        [BindProperty]
        public string DateInputString { get; set; }
        public string ErrorString { get; private set; }
        public IList<Reservation> Reservations { get; set; }
        public string NumberError { get; private set; }
        [BindProperty]
        public DateTime StartTime { get; set; }
        [BindProperty]
        public DateTime EndTime { get; set; }

        public async Task<IActionResult> OnGetAsync(int reservationid, int buildingid, int departmentid, string date)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;
            ReservationIdRoute = reservationid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            Reservation = await _context.Reservations
                .Include(r => r.ApplicationUser)
                .Include(r => r.Room)
                .ThenInclude(r => r.Building)
                .ThenInclude(r => r.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ReservationID == reservationid);

            Number = Reservation.ApplicationUser.Number;

            Rooms = await _context.Rooms
                .Where(b => b.BuildingID == Reservation.Room.BuildingID)
                .AsNoTracking()
                .ToListAsync();

            if (Reservation == null)
                return NotFound();

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

            ViewData["StartTime"] = new SelectList(Hours, Reservation.StartTime.ToShortTimeString());
            ViewData["EndTime"] = new SelectList(Hours, Reservation.StartTime.ToShortTimeString());


            // lista, atrybut ustawiany, atrybut wyświetlany, domyślnie wybrana pozycja z listy 
            ViewData["Rooms"] = new SelectList(Rooms, "RoomID", "Number", Reservation.RoomID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int reservationid, int roomid, int buildingid, int departmentid, string date)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            DateTime.TryParse(DateInputString, out var ParseStringDate);

            ReservationIdRoute = reservationid;

            if (!ModelState.IsValid)
                return Page();

            AllNumbers = await _context.AppUsers
                .Select(i => i.Number)
                .ToListAsync();

            AppUsers = await _context.AppUsers
                .AsNoTracking()
                .ToListAsync();

            Reservations = await _context.Reservations
                .Where(r => r.ReservationID!= reservationid)
                .Where(b => b.Room.BuildingID == buildingid)
                .Where(d => d.Date == ParseStringDate)
                .Where(r => r.RoomID == Reservation.RoomID)
                .AsNoTracking()
                .ToListAsync();

            Reservation = await _context.Reservations
                .Include(r => r.ApplicationUser)
                .Include(r => r.Room)
                .ThenInclude(r => r.Building)
                .ThenInclude(r => r.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ReservationID == reservationid);

            Rooms = await _context.Rooms
                .Where(b => b.BuildingID == Reservation.Room.BuildingID)
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



            ViewData["StartTime"] = new SelectList(Hours, Reservation.StartTime.ToShortTimeString());
            ViewData["EndTime"] = new SelectList(Hours, Reservation.EndTime.ToShortTimeString());


            // lista, atrybut ustawiany, atrybut wyświetlany, domyślnie wybrana pozycja z listy 
            ViewData["Rooms"] = new SelectList(Rooms, "RoomID", "Number", Reservation.RoomID);

            if (StartTime == EndTime) ErrorString = "Rezerwacja musi trwać co najmniej 30 minut.";
            if (StartTime > EndTime) ErrorString = "Rezerwacja nie może się zacząć później niż skończyć.";

            foreach (var item in Reservations)
            {
                if (StartTime == item.StartTime || EndTime == item.EndTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (EndTime < item.EndTime && StartTime < item.StartTime && Reservation.EndTime > item.StartTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (EndTime > item.EndTime && StartTime < item.StartTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (EndTime > item.EndTime && StartTime > item.StartTime && Reservation.StartTime < item.EndTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
                if (EndTime < item.EndTime && StartTime > item.StartTime) ErrorString = "W tym czasie trwa inna rezerwacja.";
            }


            if (ErrorString != null) return Page();

            if (!AllNumbers.Contains(Number))
            {
                NumberError = String.Format("Nie ma takiego człeka o numerze: {0}", Number);
                return Page();
            }

            var reservationToUpdate = await _context.Reservations
                .SingleOrDefaultAsync(m => m.ReservationID == reservationid);

            reservationToUpdate.Id = AppUsers.Where(i => i.Number == Number).Select(i => i.Id).FirstOrDefault();
            reservationToUpdate.RoomID = RoomID;
            reservationToUpdate.StartTime = StartTime;
            reservationToUpdate.EndTime = EndTime;

            reservationToUpdate.Date = ParseStringDate;
            if (await TryUpdateModelAsync<Reservation>(
                reservationToUpdate,
                "Reservation",
                s => s.RoomID, s => s.Note))
            {
                await _context.SaveChangesAsync();
            }

            return Redirect("./Details" + "?" + "reservationid=" + reservationToUpdate.ReservationID.ToString() + "&buildingid=" + BuildingIdRoute.ToString() + "&departmentid=" + DepartmentIdRoute.ToString() + "&date=" + Date.ToShortDateString());
        }
    }
}
