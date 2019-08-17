using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Uploadcare.DTO
{
    public class StoreResult
    {
        public string Status { get; set; }

        public IEnumerable<StoreBatchResultProblems> Problems { get; set; }

        public File Result
            => this.Results?.FirstOrDefault();

        [JsonProperty("result")]
        public IEnumerable<File> Results { get; set; }
    }
}