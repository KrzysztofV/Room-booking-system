using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.AppUsers
{
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;
        private List<int> AllNumbers;

        public CreateModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty] // wiązanie modelu
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public Employment Employment { get; set; }

        [BindProperty]
        public Employment SecondEmployment { get; set; }

        [BindProperty]
        public bool SecondEmploymentChecked { get; set; }

        [BindProperty] //aby można było z tego skorzystać na stronie html i mogło zostać użyte potem w onPost
        public bool SetAutoNumber { get; set; } = true;

        [BindProperty]
        [Required(ErrorMessage = "ID jest wymagane.")]
        [Range(1,100000, ErrorMessage = "Tylko liczby w zakresie 1-100000")]
        public int ManualNumber { get; set; }

        public int AutoNumber { get; set; }

        public string DuplicateNumberExistError { get; set; }
        public int FreeNumber { get; private set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        public async Task<IActionResult> OnGet(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Ustalenie nowego ID dla nowej osoby
            AllNumbers = await _context.AppUsers
                .Select(i => i.Number)
                .ToListAsync();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // znalezienie wolnego numeru (wyszukuje też lukę)
            FreeNumber = 1;
            while (true)
            {
                if (AllNumbers.Contains(FreeNumber)) FreeNumber++;
                else break;
            }

            AutoNumber = FreeNumber;

            ManualNumber = AutoNumber;

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            AllNumbers = await _context.AppUsers
                .Select(i => i.Number)
                .ToListAsync();

            if (!ModelState.IsValid)
                return Page();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // znalezienie wolnego numeru (wyszukuje też lukę)
            FreeNumber = 1;
            while (true)
            {
                if (AllNumbers.Contains(FreeNumber)) FreeNumber++;
                else break;
            }

            if (AllNumbers.Contains(ManualNumber) && !SetAutoNumber)   // własna validacja numeru pracownika
            {
                AutoNumber = FreeNumber;
                DuplicateNumberExistError = "Doopanuj się! Ten numer jest już zajęty.";
                return Page();
            }

            var newApplicationUser = new ApplicationUser();

            // obsługa osoby
            if (await TryUpdateModelAsync<ApplicationUser>(
                newApplicationUser,
                "ApplicationUser",   // Prefix for form value.
                 s => s.FirstName, s => s.LastName, s => s.Employee, s => s.Email, s => s.Phone, s => s.Note))
            {

                if (SetAutoNumber) newApplicationUser.Number = FreeNumber;
                else newApplicationUser.Number = ManualNumber;

                _context.AppUsers.Add(newApplicationUser);
                await _context.SaveChangesAsync();
            }

            // obsługa zatrudnienia jeśli osoba jest pracownikiem
            if (newApplicationUser.Employee)
            {
                var emptyEmployment = new Employment();

                if (await TryUpdateModelAsync<Employment>(
                emptyEmployment,
                "Employment",
                s => s.Id, s => s.DepartmentID, s => s.Position)) // employmentID jest nadawane automatycznie - pacz Employment.cs
                {
                    emptyEmployment.Id = Int32.Parse(newApplicationUser.Id); // przekazanie ID pracownika do encji employment
                    _context.Employments.Add(emptyEmployment);
                    await _context.SaveChangesAsync();
                }

                if (SecondEmploymentChecked) // drugie zatrudnienie
                {
                    var emptySecondEmployment = new Employment();

                    if (await TryUpdateModelAsync<Employment>(
                    emptySecondEmployment,
                    "SecondEmployment",
                    s => s.Id, s => s.DepartmentID, s => s.Position)) // employmentID jest nadawane automatycznie - pacz Employment.cs
                    {
                        emptySecondEmployment.Id = Int32.Parse(newApplicationUser.Id); // przekazanie ID pracownika do encji employment
                        _context.Employments.Add(emptySecondEmployment);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToPage("./Index");
        }
    }
}

