namespace Certify.ViewModels
{
    public class ForMySignaturesIndex
    {
        public int Id { get; set; }
        public DateTime SignedDate { get; set; }
        public bool? IsSigned { get; set; }
        public string Title { get; set; }
        public string FileURL { get; set; }

    }
}
