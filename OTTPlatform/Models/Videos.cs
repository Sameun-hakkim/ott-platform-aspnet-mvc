using System.ComponentModel.DataAnnotations;

namespace OTTPlatform.Models
{
    public class Videos
    {
        [Key]
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public int LanguageID { get;set; }
        [Required]
        public string? VideoName { get; set; }
        public string? VideoDescription {  get; set; }
        public string? VideoURL {  get; set; }
        public string? TemplateURL {  get; set; }
        public string? CategoryName { get; set; }
        public string? LanguageName { get; set; }
        public bool VideoStatus { get; set; }
        public int Ratings { get; set; }
        public int RatingsCount { get; set; }



    }
}
