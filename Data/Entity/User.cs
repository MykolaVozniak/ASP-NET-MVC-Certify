using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime Birthdate { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Firstname { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Lastname { get; set; }
        public ICollection<Signature> Signatures { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
