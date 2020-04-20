using Microsoft.AspNetCore.Mvc;

namespace TodoApiNet.Models
{
    public class Request
    {
        [FromQuery(Name = "sort")]
        public string Sort { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "paginate")]
        public bool Paginate { get; set; } = false; 
    }
}