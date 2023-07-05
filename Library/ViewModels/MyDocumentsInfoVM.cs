using System.ComponentModel.DataAnnotations;

namespace Certify.Library.ViewModels
{
    public class MyDocumentsInfoVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileURL { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UserId { get; set; }


        public List<string> SignedTrueUsers { get; set; }

        public List<string> SignedNullUsers { get; set; }

        public List<string> SignedFalseUsers { get; set; }

        public bool IsUserOwner { get; set; }
        public bool IsUserSignatuer { get; set; }
        public string UserName { get; set; }
        public bool? IsSigned { get; set; }
    }
}
