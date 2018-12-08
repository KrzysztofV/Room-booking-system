using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RezerwacjaSal.Areas.Identity.Pages.Account
{
    public class RegisterSuccesModel : PageModel
    {
        public string ReturnUrl { get; private set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }
    }
}