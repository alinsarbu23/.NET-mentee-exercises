namespace AirportTool.Infrastructure.DTOs.Common
{
    /// <summary>
    /// For getting flights with pagination details.
    /// </summary>
    public class PagedResultDto<T>
    {
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
