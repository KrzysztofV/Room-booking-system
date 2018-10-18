using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// to jest od tych adnotacji [xxx]
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RezerwacjaSal.Models
{
    public class Building
    {
        public int BuildingID { get; set; }

        [StringLength(50, ErrorMessage = "Nazwa budynku nie może być dłuższa niż 50 znaków.")]
        [Required(ErrorMessage = "Nazwa budynku jest wymagana.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Adres nie może być dłuższy niż 50 znaków.")]
        [Required(ErrorMessage = "Adres jest wymagany.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Wymagany jest wydział.")]
        public int DepartmentID { get; set; }

        public string GPS_N { get; set; }
        public string GPS_E { get; set; }

        // property jest rozpoznawane jako klucz obcy kiedy się nazywa <navigation property name><primary key property name>
        public Department Department { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
