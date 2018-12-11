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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using RezerwacjaSal.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;

namespace RezerwacjaSal.Pages.AppUsers
{
    [Authorize(Roles = "administrator")]

    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSalContext _context;
        private List<int> AllNumbers;

        public CreateModel(
            RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty] // wiązanie modelu
        public ApplicationUser ApplicationUser { get; set; }

        public int FreeNumber { get; private set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public class InputModel
        {
            // TODO nie działa walidacja hasła
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Nowe hasło i jego poteierdzenie się nie zgadzają.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string RoleName { get; set; }
            [Required]
            public bool EmailConfirmed { get; set; }

            public bool PhoneConfirmed { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGet(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Ustalenie nowego numeru dla nowej osoby
            AllNumbers = await _context.AppUsers
                .Select(i => i.Number)
                .ToListAsync();

            // Przekazanie parametrów URL
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // Znalezienie wolnego numeru
            FreeNumber = 1;
            while (true)
            {
                if (AllNumbers.Contains(FreeNumber)) FreeNumber++;
                else break;
            }

            // Listy wyborów
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", _roleManager.Roles.Where(r => r.Name == "użytkownik").Select(r => r.Name).First());

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {

            if (!ModelState.IsValid)
            {
                ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", _roleManager.Roles.Where(r => r.Name == "użytkownik").Select(r => r.Name).First());
                return Page();
            }

            // Przekazanie parametrów URL
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // Znalezienie wolnego numeru
            AllNumbers = await _context.AppUsers
                .Select(i => i.Number)
                .ToListAsync();
            FreeNumber = 1;
            while (true)
            {
                if (AllNumbers.Contains(FreeNumber)) FreeNumber++;
                else break;
            }

            // Nowy użytkownik
            var newApplicationUser = new ApplicationUser
            {
                UserName = ApplicationUser.Email,
                Email = ApplicationUser.Email,
                FirstName = ApplicationUser.FirstName,
                LastName = ApplicationUser.LastName,
                PhoneNumber = ApplicationUser.PhoneNumber,
                Note = ApplicationUser.Note,
                Number = FreeNumber,
                Employment = ApplicationUser.Employment,
                DepartmentID = ApplicationUser.DepartmentID,
                EmailConfirmed = Input.EmailConfirmed,
                PhoneNumberConfirmed = Input.PhoneConfirmed
            };
            
            // Zapisanie użytkownika i jego roli
            var createUserResult = await _userManager.CreateAsync(newApplicationUser, Input.Password);
            var updateReoleResult = await _userManager.AddToRoleAsync(newApplicationUser, Input.RoleName);
            if (createUserResult.Succeeded && createUserResult.Succeeded) _logger.LogInformation("Utworzono nowego użytkownika");
            foreach (var error in createUserResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
            foreach (var error in updateReoleResult.Errors) ModelState.AddModelError(string.Empty, error.Description);

            return RedirectToPage("./Index");
        }
    }
}

