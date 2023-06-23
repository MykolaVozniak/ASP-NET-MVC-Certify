using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User : IdentityUser
    {
        
        public DateTime Birthdate { get; set; }
        [ StringLength (100, MinimumLength = 2)]
        public string FirstName { get; set; }
        [StringLength(100, MinimumLength =2)]
        public string LastName { get; set; }
        public ICollection<Signature> Signatures { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
