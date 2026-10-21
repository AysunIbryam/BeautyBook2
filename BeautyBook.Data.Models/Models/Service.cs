using System.ComponentModel.DataAnnotations;

namespace BeautyBook.Data.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 1000)]
        public decimal Price { get; set; }

        [Range(15, 480)]
        public int DurationMinutes { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }

}
