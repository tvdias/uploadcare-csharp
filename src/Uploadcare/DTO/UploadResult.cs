using System;
using Newtonsoft.Json;

namespace Uploadcare.DTO
{
    public class UploadResult
    {
        [JsonProperty("file")]
        public Guid FileId { get; set; }
    }
}