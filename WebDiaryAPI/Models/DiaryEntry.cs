using System.ComponentModel.DataAnnotations;

namespace WebDiaryAPI.Models
{
    public class DiaryEntry
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "No data to display.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "This title should be between 4 and 51 characters.")]
        public required string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "No data to display.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "This entry should be between 4 and 1001 characters.")]
        public string Entry { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
