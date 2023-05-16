namespace DarkLibCW.Models.ViewModels
{
    public class CategoryAndBooks
    {
        public Category Category { get; set; }
        public IEnumerable<CatalogCard> Books { get; set; }
    }
}
