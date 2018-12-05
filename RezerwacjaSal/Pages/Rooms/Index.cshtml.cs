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

namespace RezerwacjaSal.Pages.Rooms
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public IndexModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public IList<Room> Room { get;set; }
        public IList<Building> Buildings { get; set; }
        public int DepartmentIdRoute { get; set; }
        public int BuildingIdRoute { get; private set; }

        public async Task OnGetAsync(int? buildingid, int? departmentid)
        {
            DepartmentIdRoute = departmentid ?? 0;
            BuildingIdRoute = buildingid ?? 0;

            if (buildingid == null || buildingid == 0)
                Room = await _context.Rooms
                        .OrderBy(n=>n.Number)
                        .Include(r => r.Building)
                        .AsNoTracking()
                        .ToListAsync();
            else
                Room = await _context.Rooms
                        .Where(b => b.BuildingID == buildingid)
                        .OrderBy(n => n.Number)
                        .Include(r => r.Building)
                        .AsNoTracking()
                        .ToListAsync();

            Buildings = await _context.Buildings
                .AsNoTracking()
                 .ToListAsync();

        }
    }
}
