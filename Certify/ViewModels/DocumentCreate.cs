using System.ComponentModel.DataAnnotations;

namespace Certify.ViewModels
{
    public class DocumentCreate
    {
        [Required(ErrorMessage = ("Dawn blat"))]
        public IFormFile UploadedFile { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required(ErrorMessage = ("Dawn blat"))]
        public string Title { get; set; }

        [StringLength(150, MinimumLength = 2)]
        public string? ShortDescription { get; set; }
        [Required(ErrorMessage =("Dawn blat"))]
        public string UserEmail { get; set; }
    }
}
