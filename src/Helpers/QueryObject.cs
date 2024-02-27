using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basic_api.Helpers
{
    public class QueryObject
    {
        public string? Search { get; set; } = null;
        public string? Gender { get; set; } = null;
        public string? StartDate { get; set; } = null;
        public string? EndDate { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? SortBy { get; set; } = "Id";
        public bool IsDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}