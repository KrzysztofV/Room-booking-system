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
    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteModel(
            RezerwacjaSal.Data.RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }
        public int BuildingIdRoute { get; private set; }
        public int DepartmentIdRoute { get; private set; }
        public DateTime Date { get; private set; }
        public int ReservationIdRoute { get; private set; }
        public ApplicationUser CurrentUser { get; private set; }

        public async Task<IActionResult> OnGetAsync(int reservationid, int buildingid, int departmentid, string date)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            ReservationIdRoute = reservationid;

            Reservation = await _context.Reservations
                .Include(r => r.ApplicationUser)
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
            CurrentUser = await _userManager.GetUserAsync(base.User);
            if (CurrentUser == null)
            {
                return base.NotFound($"Unable to load user with ID '{_userManager.GetUserId(base.User)}'.");
            }

            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            DateTime.TryParse(date, out var ParseDate);
            Date = ParseDate;

            Reservation = await _context.Reservations.FindAsync(reservationid);

            

            if (Reservation != null)
            {
                if (CurrentUser.Id == Reservation.Id)
                {
                    _context.Reservations.Remove(Reservation);
                    await _context.SaveChangesAsync();
                }
            }
            return Redirect("./Index" + "?" + "buildingid=" + BuildingIdRoute.ToString() + "&departmentid=" + DepartmentIdRoute.ToString() + "&date=" + Date.ToShortDateString());
        }
    }
}
