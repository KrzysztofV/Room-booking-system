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

    public class DetailsModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DetailsModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public Room Room { get; set; }
        public int BuildingIdRoute { get; set; }
        public int DepartmentIdRoute { get; private set; }

        public async Task<IActionResult> OnGetAsync(int roomid, int buildingid, int departmentid)
        {
            BuildingIdRoute = buildingid;
            DepartmentIdRoute = departmentid;

            Room = await _context.Rooms
                .Include(r => r.Building)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.RoomID == roomid);

            if (Room == null)
                return NotFound();

            return Page();
        }
    }
}
