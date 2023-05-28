using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Book
    {
        // Основная информация
        public int Id { get; set; }
        public int CatalogCardId { get; set; }
        [Display(Name = "Название книги")]
        public CatalogCard? CatalogCard { get; set; }
        public int StatusId { get; set; }
        [Display(Name = "Состояние книги")]
        public Status? Status { get; set; } // "Выдана", "Забронирована", "Не выдана"
    }
}
