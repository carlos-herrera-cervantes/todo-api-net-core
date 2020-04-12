using Microsoft.AspNetCore.Mvc;

namespace TodoApiNet.Models
{
    public class Request
    {
        [FromQuery(Name = "sort")]
        public string Sort { get; set; }
    }
}