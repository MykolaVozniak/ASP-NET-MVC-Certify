

namespace Data.Models
{
    public class Signature
    {
        public int Id { get; set; }
        public bool? IsSigned { get; set; }
        public DateTime SingedDate { get; set; }


        public int DocumentId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Document Document { get; set; }
    }
}
