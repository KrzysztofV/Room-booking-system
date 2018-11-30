﻿using System;
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

namespace RezerwacjaSal.Pages.AppUsers
{
    public class CreateModel : PageModel
    {
        private readonly RezerwacjaSalContext _context;
        private List<int> AllNumbers;

        public CreateModel(
            RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] // wiązanie modelu
        public ApplicationUser ApplicationUser { get; set; }


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

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;


        

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

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

            // znalezienie wolnego numeru (wyszukuje też lukę np. 1,2,..,4,5)
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

                // na podstawie Register.cshtml.cs
                var newApplicationUser = new ApplicationUser
            {
                UserName = ApplicationUser.Email,
                Email = ApplicationUser.Email,
                FirstName = ApplicationUser.FirstName,
                LastName = ApplicationUser.LastName,
                PhoneNumber = ApplicationUser.PhoneNumber,
                Note = ApplicationUser.Note,
                Number = ApplicationUser.Number,
                Employment = ApplicationUser.Employment,
                DepartmentID = ApplicationUser.DepartmentID,
                EmailConfirmed = true
            };
            
            var result = await _userManager.CreateAsync(newApplicationUser, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newApplicationUser);
                await _signInManager.SignInAsync(newApplicationUser, isPersistent: false);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            
            return RedirectToPage("./Index");
        }
    }
}

