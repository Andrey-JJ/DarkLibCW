using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Category
    {
        // Основная информация
        public int Id { get; set; }
        [Display(Name = "Название категории")]
        public string Name { get; set; }
    }
}
