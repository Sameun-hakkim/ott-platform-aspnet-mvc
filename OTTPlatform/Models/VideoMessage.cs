using System.ComponentModel.DataAnnotations;

namespace OTTPlatform.Models
{
    public class VideoMessage
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string? UserMessage { get; set; }
        public bool MsgStatus { get; set; }
        public string? Username { get; set; }

    }
}
