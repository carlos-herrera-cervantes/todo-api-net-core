using Microsoft.AspNetCore.Mvc;

namespace TodoApiNet.Models
{
    public class Request
    {
        #region snippet_Properties

        [FromQuery(Name = "sort")]
        public string Sort { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "paginate")]
        public bool Paginate { get; set; } = false;

        [FromQuery(Name = "relation")]
        public string[] Entities { get; set; } = new string[0];

        [FromQuery(Name = "filter")]
        public string[] Filters { get; set; } = new string[0];

        #endregion

        #region snippet_Deconstruct

        public void Deconstruct(out string sort, out int pageSize, out int page, out string[] entities, out string[] filters)
        {
            sort = Sort;
            pageSize = PageSize;
            page = Page;
            entities = Entities;
            filters = Filters;
        }

        public void Deconstruct(out int pageSize, out int page, out bool paginate)
        {
            pageSize = PageSize;
            page = Page;
            paginate = Paginate;
        }

        public void Deconstruct(out string[] entities, out string[] filters)
        {
            entities = Entities;
            filters = Filters;
        }

        #endregion
    }
}