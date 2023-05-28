namespace DarkLibCW.Models.ViewModels
{
    public class SubscriberBooks
    {
        public Subscriber Subscriber { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
        public IEnumerable<Issue> Issues { get; set; }
    }
}
