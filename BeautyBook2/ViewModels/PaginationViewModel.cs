namespace BeautyBook.Web.ViewModels
{
    public class PaginationViewModel
    {
        public int Page { get; set; }

        public int TotalPages { get; set; }

        // Current filters that must be kept when changing the page.
        public Dictionary<string, string> RouteValues { get; set; } = new();
    }
}