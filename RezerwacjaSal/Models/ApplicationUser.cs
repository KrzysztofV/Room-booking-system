using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RezerwacjaSal.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Numer osoby jest wymagany.")]
        [Range(0, 100000, ErrorMessage = "Numer osoby może zawierać wyłącznie liczby z przedziału 0-100000")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków oraz krótsze od 2.")]
        [RegularExpression(@"^[A-Z]+[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ""'\s-]*$", ErrorMessage = "Imię może zawierać wyłącznie znaki a-z oraz musi się zaczynać od dużej litery")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków oraz krótsze od 2.")]
        [RegularExpression(@"^[A-Z]+[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ""'\s-]*$", ErrorMessage = "Nazwisko może zawierać wyłącznie znaki a-z oraz musi się zaczynać od dużej litery")]
        public string LastName { get; set; }

        [StringLength(200, ErrorMessage = "Notatka nie może być dłuższa niż 200 znaków.")]
        public string Note { get; set; }


        public int? DepartmentID { get; set; }
        [StringLength(100, ErrorMessage = "Max 100 znaków.")]

        public string Employment { get; set; }


        public Department Department { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
