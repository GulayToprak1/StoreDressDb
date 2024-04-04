using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace StoreDressDb.website.Models
{
    public class DressViewModel
    {
        public Dress Dress { get; set; }
        public Category Category { get; set; }
        public Designer Designer { get; set; }
        public List<Dress> Dresses { get; set; }
    }
}
