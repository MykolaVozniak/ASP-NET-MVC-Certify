using System.ComponentModel.DataAnnotations;

namespace Certify.ViewModels
{
    public class DocumentCreate
    {
        [Required(ErrorMessage = ("Add file"))]
        public IFormFile UploadedFile { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required(ErrorMessage = ("Enter name file"))]
        public string Title { get; set; }

        [StringLength(250, MinimumLength = 2)]
        public string? ShortDescription { get; set; }
        [Required(ErrorMessage ="Miminum one user")]
        public string UserEmail { get; set; }
    }
}
