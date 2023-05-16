using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        public string? MidName { get; set; }
        [Display(Name = "ФИО")]
        public string FullName { get => $"{Name.Substring(0, 1)}. {MidName.Substring(0, 1)}. {LastName}"; }
        public ICollection<CatalogCard>? CatalogCards { get; set; }
        public Author() { CatalogCards = new List<CatalogCard>(); }
    }
}
