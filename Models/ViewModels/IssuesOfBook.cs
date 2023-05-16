namespace DarkLibCW.Models.ViewModels
{
    public class IssuesOfBook
    {
        public Book Book { get; set; }
        public IEnumerable<Issue> Issues { get; set; }
    }
}
