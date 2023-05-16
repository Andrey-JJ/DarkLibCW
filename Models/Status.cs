using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Status
    {
        public int Id { get; set; }
        [Display(Name = "Название статуса")]
        public string Type { get; set; }
    }
}
