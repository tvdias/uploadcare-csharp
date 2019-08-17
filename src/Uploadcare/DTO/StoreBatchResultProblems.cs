using System.Collections.Generic;

namespace Uploadcare.DTO
{
    public class StoreBatchResultProblems : Dictionary<string, string>
    {
        public string FileId { get; set; }

        public string ProblemDescription { get; set; }
    }
}