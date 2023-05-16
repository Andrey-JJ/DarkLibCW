using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class CatalogCard
    {
        // Основная информация
        public int Id { get; set; }
        [Display(Name = "Название книги")]
        public string Title { get; set; }
        [Display(Name = "Издательство")]
        public int EditionId { get; set; }
        
        public Edition? Edition { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy")]
        [Display(Name = "Дата издания")]
        public DateTime EditionDate { get; set; }

        [Display(Name = "Автор/Авторы")]
        public ICollection<Author>? Author { get; set; }
        //До
        [Display(Name = "Объем книги")]
        public int? Volume { get; set; }
        [Display(Name = "Изображение")]
        public string? Image { get; set; }
        // Внешний ключ
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public CatalogCard() { Author = new List<Author>(); }
    }
}
