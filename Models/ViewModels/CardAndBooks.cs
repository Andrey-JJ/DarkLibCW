namespace DarkLibCW.Models.ViewModels
{
    public class CardAndBooks
    {
        public CatalogCard CatalogCard { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
