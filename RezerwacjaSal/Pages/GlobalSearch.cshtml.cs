using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages
{
    public class GlobalSearchModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public GlobalSearchModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public string SearchString { get; set; }
        public IList<Reservation> Reservations { get;set; }
        public IList<ApplicationUser> AppUsers { get; set; }
        public IList<Department> Departments { get; set; }
        public IList<Room> Rooms { get; set; }
        public IList<Building> Buildings { get; set; }

        public async Task OnGetAsync(string searchString)
        {
            SearchString = searchString;


            Int32.TryParse(SearchString, out int SearchStringInt);

            Reservations = await _context.Reservations
                .Where(r => r.ReservationID.Equals(SearchStringInt))
                .Include(r => r.ApplicationUser)
                .Include(r => r.Room)
                .ThenInclude(r => r.Building)
                .ThenInclude(r => r.Department)
                .AsNoTracking()
                .ToListAsync();

            AppUsers = await _context.AppUsers
                .Where(r => r.Number.Equals(SearchStringInt) || r.FirstName.Contains(SearchString) || r.LastName.Contains(SearchString))
                .AsNoTracking()
                .ToListAsync();

            Departments = await _context.Departments
                .Where(r => r.Name.Contains(SearchString))
                .AsNoTracking()
                .ToListAsync();

            Rooms = await _context.Rooms
                .Where(r => r.Number.Equals(SearchStringInt))
                .Include(r => r.Building)
                .ThenInclude(r =>r.Department)
                .AsNoTracking()
                .ToListAsync();

            Buildings = await _context.Buildings
                .Where(r => r.Name.Contains(SearchString))
                .Include(r => r.Department)
                .AsNoTracking()
                .ToListAsync();


        }
    }
}
