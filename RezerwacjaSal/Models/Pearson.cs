using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// to jest od tych adnotacji [xxx]
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RezerwacjaSal.Models
{
    // encja człowiek
    public class Pearson
    {
        public int PearsonID { set; get; }

        [Required(ErrorMessage = "Numer osoby jest wymagany.")]
        [Range(0,100000, ErrorMessage = "Numer osoby może zawierać wyłącznie liczby z przedziału 0-100000")]
        public int PearsonNumber { get; set; } //primary key, domyślnie uznaje ID lub classnameID 

        
        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków oraz krótsze od 2.")]
        [RegularExpression(@"^[A-Z]+[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ""'\s-]*$", ErrorMessage = "Imię może zawierać wyłącznie znaki a-z oraz musi się zaczynać od dużej litery")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków oraz krótsze od 2.")]
        [RegularExpression(@"^[A-Z]+[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ""'\s-]*$", ErrorMessage = "Nazwisko może zawierać wyłącznie znaki a-z oraz musi się zaczynać od dużej litery")]
        public string LastName { get; set; }

        public bool Employee { get; set; } // czy jest pracownikiem uczelni

        [EmailAddress(ErrorMessage = "To nie jest poprawny adres e-mail.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "To nie przypomina telefunu.")]
        public string Phone { get; set; }

        [StringLength(200, ErrorMessage = "Notatka nie może być dłuższa niż 200 znaków.")]
        public string Note { get; set; }

        // Employment posiada PearsonID jako klucz obcy 
        public ICollection<Employment> Employments { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
