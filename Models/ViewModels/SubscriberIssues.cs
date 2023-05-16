namespace DarkLibCW.Models.ViewModels
{
    public class SubscriberIssues
    {
        public Subscriber Subscriber { get; set; }
        public IEnumerable<Issue> Issues { get; set; }
    }
}
