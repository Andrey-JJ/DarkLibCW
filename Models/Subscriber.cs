using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        public string? MidName { get; set; }
        [Display(Name = "ФИО")]
        public string FullName { get => $"{LastName} {Name} {MidName}"; }
        public string ShortName { get => $"{LastName} {Name.Substring(0, 1)}. {MidName.Substring(0, 1)}."; }
        public string? UserName { get; set; }
    }
}
