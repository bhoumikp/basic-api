using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basic_api.Wrappers
{
    public class PaginatedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T>? Items { get; set; }
    }
}