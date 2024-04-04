using StoreDressDb;

namespace StoreDress.panel.Models
{
    public class DressCreateModel
    {
        public Dress Dress { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Designer>? Designers { get; set; }
    }
}
