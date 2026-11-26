using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyBook.Data.Models.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = null!;

        [StringLength(500)]
        public string? Bio { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
