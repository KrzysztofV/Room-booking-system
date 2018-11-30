using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// to jest od tych adnotacji [xxx]
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RezerwacjaSal.Models
{
    // Encja katedra
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Nazwa wydziału jest wymagana.")]
        [StringLength(50, ErrorMessage = "Nazwa wydziału nie może być dłuższa niż 50 znaków.")]
        public string Name { get; set; }

        [Range(1,10000000000, ErrorMessage = "Numer pracownika może zawierać wyłącznie liczby.")]
        public int Administrator { get; set; } 

        public ICollection<Building> Buildings { get; set; }

        public ICollection<ApplicationUser> AppUsers { get; set; }
    }
}
