using System.Collections.Generic;

namespace Uploadcare.DTO
{
    public class PaginatedResult<T>
    {
        public string Next { get; set; }

        public string Previous { get; set; }

        public int Total { get; set; }

        public int PerPage { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}