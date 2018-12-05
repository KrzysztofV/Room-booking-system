using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Reservations
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DetailsModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }
        public int BuildingIdRoute { get; set; }
        public int DepartmentIdRoute { get; set; }
        public DateTime Date { get; private set; }

        public Reservation Reservation { get; set; }


        public async Task<IActionResult> OnGetAsync(int reservationid, int buildingid, int departmentid, string date)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;


            Reservation = await _context.Reservations
                .Include(r => r.ApplicationUser)
                .Include(r => r.Room)
                .ThenInclude(b=>b.Building)
                .ThenInclude(d=>d.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ReservationID == reservationid);

            if (Reservation == null)
                return NotFound();

            return Page();
        }
    }
}
