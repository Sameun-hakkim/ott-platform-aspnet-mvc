using System.ComponentModel.DataAnnotations;

namespace OTTPlatform.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public int VideoID { get; set; }
        public string? CommentsName { get; set; }
        public string? UserName { get; set; }
        public int? Ratings { get; set; }

    }
}
