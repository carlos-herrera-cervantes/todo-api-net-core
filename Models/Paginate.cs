namespace TodoApiNet.Models
{
    public class Paginate
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int RemainingDocuments { get; set; } = 0;

        public int TotalDocuments { get; set; } = 0;
    }
}