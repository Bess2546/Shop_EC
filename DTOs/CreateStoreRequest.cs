using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.DTOs
{
    public class CreateStoreRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}