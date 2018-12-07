using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.Departments
{
    [Authorize(Roles = "administrator")]
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;
        public CreateModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Department Department { get; set; }
        private List<int> AllAppUsersIDs { get; set; }
        private List<string> AllDepartmentsNames { get; set; }
        public string AdministratorIdError { get; set; }
        public string DuplicateNameExistError { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            AllAppUsersIDs = await _context.AppUsers
                .Select(i => i.Number)
                .ToListAsync();

            AllDepartmentsNames = await _context.Departments
                .Select(i => i.Name)
                .ToListAsync();

            if (!AllAppUsersIDs.Contains(Department.Administrator))
            {
                AdministratorIdError = String.Format("Nie ma takiego człeka dla ID: {0}", Department.Administrator);
                return Page();
            }
            if (AllDepartmentsNames.Contains(Department.Name))
            {
                DuplicateNameExistError = "Już istnieje wydział o podanej nazwie";
                return Page();
            }

            var newDepartment = new Department();

            if (await TryUpdateModelAsync<Department>(
                newDepartment,
                "Department",   // Prefix for form value.
                 s => s.Name, s => s.Administrator))
            {
                _context.Departments.Add(newDepartment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}