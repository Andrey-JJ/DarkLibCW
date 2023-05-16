using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Edition
    {
        public int Id { get; set; }
        [Display(Name = "Название издательства")]
        public string Name { get; set; }
    }
}
