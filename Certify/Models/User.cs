using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Certify.Models
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime Birthdate { get; set; }
        [Required,StringLength(100, MinimumLength = 2)]
        public string Firstname { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Lastname { get; set; }
        public ICollection<Signature> Signatures { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
