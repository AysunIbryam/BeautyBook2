namespace BeautyBook.Services.Core.Models
{
    public class PagedResult<T>
    {
        public PagedResult(
            IReadOnlyList<T> items,
            int page,
            int pageSize,
            int totalItems)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        public IReadOnlyList<T> Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public int TotalPages => CalculateTotalPages(TotalItems, PageSize);

        public static int CalculateTotalPages(int totalItems, int pageSize)
        {
            return Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
        }

        // Keeps the requested page inside the valid range.
        public static int ClampPage(int page, int totalItems, int pageSize)
        {
            return Math.Clamp(page, 1, CalculateTotalPages(totalItems, pageSize));
        }
    }
}