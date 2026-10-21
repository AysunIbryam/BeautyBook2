using System.ComponentModel.DataAnnotations;

namespace BeautyBook.Data.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public int ServiceId { get; set; }

        public Service Service { get; set; } = null!;
    }

}
