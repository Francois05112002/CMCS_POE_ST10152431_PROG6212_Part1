using System.ComponentModel.DataAnnotations;

namespace CMCS_POE_ST10152431_PROG6212_Part1.Models
{
    public class ReportIssueViewModel
    {
        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public List<string> Categories { get; set; } = new List<string>();

        public List<string> AttachmentFileNames { get; set; } = new List<string>();

        public int CurrentPoints { get; set; }

        public static List<string> GetDefaultCategories()
        {
            return new List<string>
            {
                "Sanitation",
                "Roads",
                "Utilities",
                "Street lights",
                "Other"
            };
        }
    }
}

