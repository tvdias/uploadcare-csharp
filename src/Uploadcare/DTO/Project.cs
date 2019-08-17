using System.Collections.Generic;
using Newtonsoft.Json;

namespace Uploadcare.DTO
{
    public class Project
    {
        public bool AutostoreEnabled { get; set; }

        public IEnumerable<Collaborator> Collaborators { get; set; }

        public string Name { get; set; }

        [JsonProperty("pub_key")]
        public string PublicKey { get; set; }
    }
}