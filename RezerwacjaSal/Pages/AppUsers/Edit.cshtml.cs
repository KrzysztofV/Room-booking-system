using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RezerwacjaSal.Areas.Identity.Pages.Account;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.AppUsers
{
    [Authorize(Roles = "administrator")]

    public class EditModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public EditModel(
            RezerwacjaSal.Data.RezerwacjaSalContext context,
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

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public bool ChangePassword { get; set; }



        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Nowe hasło i jego powierdzenie się nie zgadzają.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string RoleName { get; set; }

            [Required]
            [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "Niepoprawny numer telefonu")]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

        }

        public async Task<IActionResult> OnGet(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Przekazanie parametrów URL
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // Wczytanie danego użytkownika
            ApplicationUser = await _context.AppUsers
                .Include(e => e.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            // Nie ma takiego użytkownika pod danym Id
            if (ApplicationUser == null)
                return NotFound();

            var email = await _userManager.GetEmailAsync(ApplicationUser);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(ApplicationUser);
            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
            };
            // Listy wyborów
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            var userRoles = await _userManager.GetRolesAsync(ApplicationUser);
            ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", userRoles.First());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            // Przekazanie parametrów URL
            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            // Aktualizacja osoby
            var appUserToUpdate = await _context.AppUsers.FindAsync(id);

            if (!ModelState.IsValid)
            {
                ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
                var userRoles = await _userManager.GetRolesAsync(appUserToUpdate);
                ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", userRoles.First());
                return Page();
            }

            // Zmiana hasła
            if (ChangePassword)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(appUserToUpdate);
                var updatePasswordResult = await _userManager.ResetPasswordAsync(appUserToUpdate, token, Input.Password);
                if (updatePasswordResult.Succeeded) _logger.LogInformation("Zaktualizowano hasło");
                else
                {
                    foreach (var error in updatePasswordResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
                    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
                    var userRoles = await _userManager.GetRolesAsync(appUserToUpdate);
                    ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", userRoles.First());
                    return Page();
                }
            }

            // Zmiana roli
            var userRole = await _userManager.GetRolesAsync(appUserToUpdate);
            if (userRole.First() != Input.RoleName)
            {
                var removeReoleResult = await _userManager.RemoveFromRolesAsync(appUserToUpdate, await _userManager.GetRolesAsync(appUserToUpdate));
                var updateReoleResult = await _userManager.AddToRoleAsync(appUserToUpdate, Input.RoleName);
                if (updateReoleResult.Succeeded && removeReoleResult.Succeeded)
                {
                    _logger.LogInformation("Zaktualizowano rolę");
                }
                else
                {
                    foreach (var error in updateReoleResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
                    ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
                    var userRoles = await _userManager.GetRolesAsync(appUserToUpdate);
                    ViewData["RoleNames"] = new SelectList(_roleManager.Roles, "Name", "Name", userRoles.First());
                    return Page();
                }
            }

            // Zmiana email i nazwy użytkownika
            var email = await _userManager.GetEmailAsync(appUserToUpdate);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(appUserToUpdate, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(appUserToUpdate);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
                var setUsernameResult = await _userManager.SetUserNameAsync(appUserToUpdate, Input.Email);
                if (!setUsernameResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(appUserToUpdate);
                    throw new InvalidOperationException($"Unexpected error occurred setting username for user with ID '{userId}'.");
                }
            }

            // Zmiana telefonu
            var phoneNumber = await _userManager.GetPhoneNumberAsync(appUserToUpdate);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(appUserToUpdate, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(appUserToUpdate);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }



            // Aktualizacja pozostałych danych osoby
            if (await TryUpdateModelAsync<ApplicationUser>(
                appUserToUpdate,
                "ApplicationUser",
                 s => s.Employment,
                 s => s.DepartmentID,
                 s => s.FirstName,
                 s => s.LastName,
                 s => s.Note,
                 s => s.EmailConfirmed,
                 s => s.PhoneNumberConfirmed))
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { sortOrder = SortOrderRoute, currentFilter = CurrentFilterRoute, searchString = SearchStringRoute, pageIndex = PageIndexRoute, pageSize = PageSizeRoute });


        }

        private bool AppUserExists(int id)
        {
            return _context.AppUsers.Any(e => Int32.Parse(e.Id) == id);
        }
    }
}
