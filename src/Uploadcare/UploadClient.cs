using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Uploadcare.DTO;
using Uploadcare.Helpers;
using File = System.IO.File;

namespace Uploadcare
{
    public class UploadClient : ClientBase
    {
        public UploadClient(Configuration configuration, HttpClient httpClient = null)
            : base(configuration, httpClient)
        {
        }

        /// <summary>
        /// <para>Synchronously uploads the file to Uploadcare.</para>
        /// <para>The calling thread will be busy until the upload is finished.</para>
        /// </summary>
        /// <returns> An Uploadcare file </returns>
        /// <exception cref="UploadFailureException"> </exception>
        public Task<UploadResult> UploadAsync(FileInfo fileInfo, bool? store = null)
        {
            var file = File.ReadAllBytes(fileInfo.FullName);

            return this.UploadAsync(fileInfo.Name, file, store);
        }

        /// <summary>
        /// <para>Synchronously uploads the file to Uploadcare.</para>
        /// <para>The calling thread will be busy until the upload is finished.</para>
        /// </summary>
        /// <returns> An Uploadcare file </returns>
        /// <exception cref="UploadFailureException"> </exception>
        public async Task<UploadResult> UploadAsync(string filename, byte[] file, bool? store = null)
        {
            using (var multipartContent = new MultipartFormDataContent()
                {
                    { new StringContent(this._configuration.PublicKey), "UPLOADCARE_PUB_KEY" }
                })
            {
                if (!store.HasValue)
                {
                    multipartContent.Add(new StringContent("auto"), "UPLOADCARE_STORE");
                }
                else
                {
                    multipartContent.Add(new StringContent($"{(store.Value ? 1 : 0)}"), "UPLOADCARE_STORE");
                }

                multipartContent.Add(new ByteArrayContent(file), "file", filename);

                using (var request = new HttpRequestMessage(HttpMethod.Post, UrlHelper.UploadBase()))
                {
                    request.Content = multipartContent;

                    return await ExecuteRequestAsync<UploadResult>(this._httpClient, request).ConfigureAwait(false);
                }
            }
        }
    }
}