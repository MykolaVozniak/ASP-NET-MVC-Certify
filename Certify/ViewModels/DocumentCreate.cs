using System.ComponentModel.DataAnnotations;

namespace Certify.ViewModels
{
    public class DocumentCreate
    {
        public IFormFile UploadedFile { get; set; }

        [Required, StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(150, MinimumLength = 2)]
        public string? ShortDescription { get; set; }
        public List<string> UserId { get; set; }
    }
}
