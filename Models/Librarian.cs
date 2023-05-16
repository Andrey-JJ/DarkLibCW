using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Librarian
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        public string? MidName { get; set; }
        [Display(Name = "Возраст")]
        public int? Age { get; set; }
        [Display(Name = "ФИО")]
        public string FullName { get => $"{LastName} {Name} {MidName}"; }
        [Display(Name = "Пол")]
        public string Gender { get; set; }
        public string UserName { get; set; }
    }
}
