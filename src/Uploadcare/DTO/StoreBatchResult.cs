using System.Collections.Generic;

namespace Uploadcare.DTO
{
    public class StoreBatchResult
    {
        public string Status { get; set; }

        public IEnumerable<StoreBatchResultProblems> Problems { get; set; }

        public IEnumerable<File> Result { get; set; }
    }
}