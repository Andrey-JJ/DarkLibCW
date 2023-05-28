namespace DarkLibCW.Models.ViewModels
{
    public class CategoryBooks
    {
        public Category Category { get; set; }
        public IEnumerable<CatalogCard> CatalogCards { get; set; }
    }
}
