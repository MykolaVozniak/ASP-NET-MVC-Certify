using System.ComponentModel.DataAnnotations;

namespace Certify.Models
{
    public class Document
    {
        public int Id { get; set; }
        [Required, StringLength(80, MinimumLength = 1)]
        public string Title { get; set; }
        [Required]
        public string FileURL { get; set; }
        [StringLength(500, MinimumLength = 2)]
        public string? ShortDescription { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UserId { get; set; }


        public User User { get; set; }
        public ICollection<Signature> Signatures { get; set; }

    }
}
