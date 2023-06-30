using System.ComponentModel.DataAnnotations;

namespace Certify.ViewModels
{
    public class DocumentIndex
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileURL { get; set; }

        public int CountTrue { get; set; }

        public int CountMax { get; set; }
    }
}
