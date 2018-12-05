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

namespace RezerwacjaSal.Pages.Buildings
{
    [Authorize]

    public class DetailsModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DetailsModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }
        public int DepartmentIdRoute { get; set; }
        public Building Building { get; set; }
        public string MapE { get; set; }
        public string MapN { get; set; }
        public int MapZoom { get; set; }

        public async Task<IActionResult> OnGetAsync(int buildingid, int departmentid)
        {
            DepartmentIdRoute = departmentid;

            Building = await _context.Buildings
                .Include(b => b.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BuildingID == buildingid);

            if(Building.GPS_E != null && Building.GPS_N != null)
            {
                MapE = Building.GPS_E;
                MapN = Building.GPS_N;
                MapZoom = 19;
            }
            else
            {
                MapE = "19.9829";
                MapN = "51.4404";
                MapZoom = 5;
            }

            if (Building == null)
                return NotFound();

            return Page();
        }
    }
}
