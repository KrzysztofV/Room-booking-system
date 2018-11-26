using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;
        public IndexModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }
        public IList<Building> Buildings { get; set; }
        public IList<Room> Rooms { get; set; }
        public IList<Reservation> Reservations { get; set; }
        public int BuildingIdRoute { get; set; }
        public DateTime Now { get; set; }
        public async Task OnGetAsync (int? buildingid)
        {
            
            Buildings = await _context.Buildings
                .OrderBy(n => n.Name)
                .Include(r => r.Department)
                .AsNoTracking()
                .ToListAsync();

            BuildingIdRoute = buildingid ?? Buildings.Select(r => r.BuildingID).FirstOrDefault();


            Rooms = await _context.Rooms
                .OrderBy(n => n.Number)
                .Where(r => r.BuildingID == BuildingIdRoute)
                .AsNoTracking()
                .ToListAsync();

            Now = new DateTime();
            Now = DateTime.Now;

            Reservations = await _context.Reservations
                .Include(r => r.ApplicationUser)
                .AsNoTracking()
                .ToListAsync();


        }
    }
}
