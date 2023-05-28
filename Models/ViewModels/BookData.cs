namespace DarkLibCW.Models.ViewModels
{
    public class BookData
    {
        public Book Book { get; set; }
        public IEnumerable<Issue> Issues { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
