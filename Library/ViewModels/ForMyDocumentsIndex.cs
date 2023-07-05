namespace Certify.Library.ViewModels
{
    public class ForMyDocumentsIndex
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileURL { get; set; }
        public int CurrentSignaturesCount { get; set; }
        public int MaxSignaturesCount { get; set; }
        public bool? IsSigned { get; set; }
    }
}
