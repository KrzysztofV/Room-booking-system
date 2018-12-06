using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DetailsModel(
            RezerwacjaSal.Data.RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public int BuildingIdRoute { get; set; }
        public int DepartmentIdRoute { get; set; }
        public DateTime Date { get; private set; }

        public Reservation Reservation { get; set; }
        public ApplicationUser CurrentUser { get; set; }


        public async Task<IActionResult> OnGetAsync(int reservationid, int buildingid, int departmentid, string date)
        {
            CurrentUser = await _userManager.GetUserAsync(base.User);
            if (CurrentUser == null)
            {
                return base.NotFound($"Unable to load user with ID '{_userManager.GetUserId(base.User)}'.");
            }
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
