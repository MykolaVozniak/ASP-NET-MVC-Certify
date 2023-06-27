using Certify.Models;

namespace Certify.ViewModels
{
    public class DocumentInfo
    {
        public Document? DocumentDI { get; set; }

        public List<string>? SignedFalse { get; set; }

        public List<string>? SignedTrue { get; set; }

        public List<string>? SignedNull { get; set; }
    }
}
