using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Reservations
{
    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DeleteModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }
        public int BuildingIdRoute { get; private set; }
        public int DepartmentIdRoute { get; private set; }
        public DateTime Date { get; private set; }
        public int ReservationIdRoute { get; private set; }

        public async Task<IActionResult> OnGetAsync(int reservationid, int buildingid, int departmentid, string date)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            ReservationIdRoute = reservationid;

            Reservation = await _context.Reservations
                .Include(r => r.Pearson)
                .Include(r => r.Room)
                .ThenInclude(r => r.Building)
                .ThenInclude(r => r.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ReservationID == reservationid);

            if (Reservation == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int reservationid, int buildingid, int departmentid, string date)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            Reservation = await _context.Reservations.FindAsync(reservationid);

            if (Reservation != null)
            {
                _context.Reservations.Remove(Reservation);
                await _context.SaveChangesAsync();
            }

            return Redirect("./Index" + "?" + "buildingid=" + BuildingIdRoute.ToString() + "&departmentid=" + DepartmentIdRoute.ToString() + "&date=" + Date.ToShortDateString());
        }
    }
}
