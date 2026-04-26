using System.ComponentModel.DataAnnotations;

namespace IncidentAPI_MayaraBouazra.Models
{
    public class Incident
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]

        [RegularExpression("LOW|MEDIUM|HIGH|CRITICAL", ErrorMessage = "Invalid severity")]
        public string Severity { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

