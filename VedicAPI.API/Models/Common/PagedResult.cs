namespace VedicAPI.API.Models.Common
{
    /// <summary>
    /// Generic paged result wrapper for list endpoints
    /// </summary>
    /// <typeparam name="T">Type of items in the page</typeparam>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    }
}
