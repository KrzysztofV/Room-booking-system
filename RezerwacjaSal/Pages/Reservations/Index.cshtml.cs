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
    public class IndexModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public IndexModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public int BuildingIdRoute { get; private set; }
        public int DepartmentIdRoute { get; set; }

        public Building Building { get; set; }
        public IList<Room> Rooms { get; set; }
        public IList<Pearson> People { get; set; }
        public IList<Reservation> Reservations { get; set; }
        public IList<DateTime> Hours { get; set; }
        public DateTime Date { get; set; }

        public async Task OnGetAsync(int? buildingid, int? departmentid, string date)
        {
            if (date == null)
                Date = DateTime.Today;
            else
            {
                DateTime.TryParse(date, out var ParseDatetime);
                Date = ParseDatetime;
            }

            Hours = new List<DateTime>();
            Hours.Add(new DateTime(2018, 1, 1, 8, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 8, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 9, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 9, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 10, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 10, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 11, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 11, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 12, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 12, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 13, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 13, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 14, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 14, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 15, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 15, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 16, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 16, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 17, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 17, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 18, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 18, 30, 0));
            Hours.Add(new DateTime(2018, 1, 1, 19, 00, 0));
            Hours.Add(new DateTime(2018, 1, 1, 19, 30, 0));

            BuildingIdRoute = buildingid ?? await _context.Buildings.Select(r => r.BuildingID).FirstOrDefaultAsync();
            DepartmentIdRoute = departmentid ?? 0;

            Building = await _context.Buildings // tu wystarczy jeden budynek 
                .Where(i => i.BuildingID == BuildingIdRoute)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            Rooms = await _context.Rooms
                .Where(i => i.BuildingID == BuildingIdRoute)
                .OrderBy(n => n.Number)
                .AsNoTracking()
                .ToListAsync();

            People = await _context.People
                .AsNoTracking()
                .ToListAsync();

            Reservations = await _context.Reservations
                            .Include(r => r.Pearson)
                            .Include(r => r.Room)
                            // kaskada rezerwacja<-pokój<-budynek, aby dostać się do ID budynku trzeba "przejść" przez pokój 
                            .ThenInclude(b => b.Building)
                            // rezerwacje dla danego ID budynku
                            .Where(b => b.Room.BuildingID == BuildingIdRoute)
                            .Where(d => d.Date == Date.Date)
                            .AsNoTracking()
                            .ToListAsync();
        }
    
    }
    
}
