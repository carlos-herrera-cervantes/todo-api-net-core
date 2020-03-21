using System.Collections.Generic;

namespace TodoApiNet.Models
{
    public class JsonLocalization
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValue { get; set; } = new Dictionary<string, string>();
    }
}