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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private List<int> AllNumbers;
        public int FreeNumber { get; private set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

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

        public class InputModel
        {
            public ApplicationUser ApplicationUser { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Nowe hasło i jego potwierdzenie się nie zgadzają.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string RoleName { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGet(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Wszystkie zajęte numery osób
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

            Input = new InputModel
            {
            };

            // Listy wyborów
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", _roleManager.Roles.Where(r => r.Name == "użytkownik").Select(r => r.Name).First());

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Przekazanie parametrów URL
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            if (ModelState.IsValid)
            {
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

                Input.ApplicationUser.Number = FreeNumber;
                Input.ApplicationUser.UserName = Input.ApplicationUser.Email;

                // Zapisanie użytkownika i jego roli
                var createUserResult = await _userManager.CreateAsync(Input.ApplicationUser, Input.Password);
                if (createUserResult.Succeeded)
                {
                    _logger.LogInformation("Utworzono nowego użytkownika");
                    var updateReoleResult = await _userManager.AddToRoleAsync(Input.ApplicationUser, Input.RoleName);
                    if (updateReoleResult.Succeeded)
                    {
                        _logger.LogInformation("Dodano rolę nowemu użytkownikowi.");
                        return RedirectToPage("./Index");
                    }
                    else
                        foreach (var error in updateReoleResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                    foreach (var error in createUserResult.Errors) ModelState.AddModelError(string.Empty, error.Description);

            }
            // If we got this far, something failed, redisplay form
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", _roleManager.Roles.Where(r => r.Name == "użytkownik").Select(r => r.Name).First());
            return Page();


        }
    }
}

