using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public EditModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        private List<int> AllPearsonNumbers { get; set; }
        public string AdministratorIdError { get; set; }
        public string ManagerIdError { get; set; }
        [BindProperty]
        public Department Department { get; set; }
        public IEnumerable<Pearson> People { get; set; }
        public string DuplicateNameExistError { get; private set; }
        private List<string> AllOtherDepartmentsNames { get; set; }

        public async Task<IActionResult> OnGetAsync(int departmentid)
        {
            Department = await _context.Departments
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.DepartmentID == departmentid);

            People = await _context.People
                .AsNoTracking()
                .ToListAsync();

            if (Department == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int departmentid)
        {
            AllPearsonNumbers = await _context.People
                .Select(i => i.PearsonNumber)
                .ToListAsync();

            AllOtherDepartmentsNames = await _context.Departments
                .Where(r => r.Name != _context.Departments.SingleOrDefault(m => m.DepartmentID == departmentid).Name)
                .Select(i => i.Name)
                .ToListAsync();

            People = await _context.People
                .AsNoTracking()
                .ToListAsync();

            if (AllOtherDepartmentsNames.Contains(Department.Name))
            {
                DuplicateNameExistError = "Już istnieje wydział o podanej nazwie";
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            if (!AllPearsonNumbers.Contains(Department.Administrator))
            {
                AdministratorIdError = String.Format("Nie ma takiego człeka o numerze: {0}", Department.Administrator);

                if (!AllPearsonNumbers.Contains(Department.Manager))
                    ManagerIdError = String.Format("Nie ma takiego człeka o numerze: {0}", Department.Manager);

                return Page();
            }

            var departmentToUpdate = await _context.Departments.FindAsync(departmentid);

            if (await TryUpdateModelAsync<Department>(departmentToUpdate, "Department", s => s.Name, s => s.Administrator, s => s.Manager))
                await _context.SaveChangesAsync();

            return Page();
        }

    }
}
