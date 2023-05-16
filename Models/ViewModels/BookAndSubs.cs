namespace DarkLibCW.Models.ViewModels
{
    public class BookAndSubs
    {
        public int CardId { get; set; }
        public Book book { get; set; }
        public IEnumerable<Subscriber> Subscribers { get; set; }
    }
}
