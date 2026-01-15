using System.ComponentModel.DataAnnotations;

namespace OTTPlatform.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public string? UserType { get; set; }
        public string? UserName { get; set; }
        public string? password { get; set; }
        public bool? UserStatus { get; set; }

    }
}
