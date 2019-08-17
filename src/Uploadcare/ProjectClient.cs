using System.Net.Http;
using System.Threading.Tasks;
using Uploadcare.DTO;
using Uploadcare.Helpers;

namespace Uploadcare
{
    public class ProjectClient : ClientBase
    {
        public ProjectClient(Configuration configuration, HttpClient httpClient = null)
            : base(configuration, httpClient)
        {
        }

        public async Task<Project> GetAsync()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, UrlHelper.ApiProject()))
            {
                return await ExecuteRequestAsync<Project>(this._httpClient, request).ConfigureAwait(false);
            }
        }
    }
}