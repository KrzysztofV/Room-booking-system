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

namespace RezerwacjaSal.Pages.People
{
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;
        private List<int> AllPearsonNumbers;

        public CreateModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty] // wiązanie modelu
        public Pearson Pearson { get; set; }

        [BindProperty]
        public Employment Employment { get; set; }

        [BindProperty]
        public Employment SecondEmployment { get; set; }

        [BindProperty]
        public bool SecondEmploymentChecked { get; set; }

        [BindProperty] //aby można było z tego skorzystać na stronie html i mogło zostać użyte potem w onPost
        public bool SetAutoPearsonNumber { get; set; } = true;

        [BindProperty]
        [Required(ErrorMessage = "ID jest wymagane.")]
        [Range(1,100000, ErrorMessage = "Tylko liczby w zakresie 1-100000")]
        public int ManualPearsonNumber { get; set; }

        public int AutoPearsonNumber { get; set; }

        public string DuplicatePearsonNumberExistError { get; set; }
        public int FreeNumber { get; private set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        public async Task<IActionResult> OnGet(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Ustalenie nowego ID dla nowej osoby
            AllPearsonNumbers = await _context.People
                .Select(i => i.PearsonNumber)
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
                if (AllPearsonNumbers.Contains(FreeNumber)) FreeNumber++;
                else break;
            }

            AutoPearsonNumber = FreeNumber;

            ManualPearsonNumber = AutoPearsonNumber;

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            AllPearsonNumbers = await _context.People
                .Select(i => i.PearsonNumber)
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
                if (AllPearsonNumbers.Contains(FreeNumber)) FreeNumber++;
                else break;
            }

            if (AllPearsonNumbers.Contains(ManualPearsonNumber) && !SetAutoPearsonNumber)   // własna validacja numeru pracownika
            {
                AutoPearsonNumber = FreeNumber;
                DuplicatePearsonNumberExistError = "Doopanuj się! Ten numer jest już zajęty.";
                return Page();
            }

            var newPearson = new Pearson();

            // obsługa osoby
            if (await TryUpdateModelAsync<Pearson>(
                newPearson,
                "Pearson",   // Prefix for form value.
                 s => s.FirstName, s => s.LastName, s => s.Employee, s => s.Email, s => s.Phone, s => s.Note))
            {

                if (SetAutoPearsonNumber) newPearson.PearsonNumber = FreeNumber;
                else newPearson.PearsonNumber = ManualPearsonNumber;

                _context.People.Add(newPearson);
                await _context.SaveChangesAsync();
            }

            // obsługa zatrudnienia jeśli osoba jest pracownikiem
            if (newPearson.Employee)
            {
                var emptyEmployment = new Employment();

                if (await TryUpdateModelAsync<Employment>(
                emptyEmployment,
                "Employment",
                s => s.PearsonID, s => s.DepartmentID, s => s.Position)) // employmentID jest nadawane automatycznie - pacz Employment.cs
                {
                    emptyEmployment.PearsonID = newPearson.PearsonID; // przekazanie ID pracownika do encji employment
                    _context.Employments.Add(emptyEmployment);
                    await _context.SaveChangesAsync();
                }

                if (SecondEmploymentChecked) // drugie zatrudnienie
                {
                    var emptySecondEmployment = new Employment();

                    if (await TryUpdateModelAsync<Employment>(
                    emptySecondEmployment,
                    "SecondEmployment",
                    s => s.PearsonID, s => s.DepartmentID, s => s.Position)) // employmentID jest nadawane automatycznie - pacz Employment.cs
                    {
                        emptySecondEmployment.PearsonID = newPearson.PearsonID; // przekazanie ID pracownika do encji employment
                        _context.Employments.Add(emptySecondEmployment);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToPage("./Index");
        }
    }
}

