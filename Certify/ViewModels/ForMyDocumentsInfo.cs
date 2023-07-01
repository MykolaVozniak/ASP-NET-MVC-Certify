using System.ComponentModel.DataAnnotations;

namespace Certify.ViewModels
{
    public class ForMyDocumentsInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileURL { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UserId { get; set; }


        public string UserName { get; set; }
        public bool? IsSigned { get; set; }
    }
}
