using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Buildings
{
    public class IndexModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public IndexModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }
        public int DepartmentIdRoute { get; set; }
        public IList<Building> Building { get; set; }
        public IList<Department> Department { get; set; }

        public async Task OnGetAsync(int? departmentid)
        {
            DepartmentIdRoute = departmentid ?? 0;

            if (departmentid != null)
            {
                Building = await _context.Buildings
                    .Where(b => b.DepartmentID == departmentid)
                    .AsNoTracking()
                    .OrderBy(n=>n.Name)
                    .ToListAsync();

                Department = await _context.Departments
                    .Where(b => b.DepartmentID == departmentid)
                    .AsNoTracking()
                    .ToListAsync();
            }
            if (departmentid == null || departmentid == 0)
            {
                Building = await _context.Buildings
                    .AsNoTracking()
                    .OrderBy(n => n.Name)
                    .ToListAsync();

                Department = await _context.Departments
                    .AsNoTracking()
                    .ToListAsync();
            }
            
        }
    }
}
