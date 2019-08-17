using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Uploadcare.DTO;
using Uploadcare.Helpers;

namespace Uploadcare
{
    public class FileClient : ClientBase
    {
        public FileClient(Configuration configuration, HttpClient httpClient = null)
            : base(configuration, httpClient)
        {
        }

        /// <summary>
        /// Copy original files or their modified versions to a custom storage or the default one.
        /// Source files MAY either be stored or just uploaded and MUST NOT be deleted.
        /// </summary>
        /// <param name="sourceFileId">UUID of a file subjected to copy.</param>
        /// <param name="makePublic">Applicable to custom storage only. MUST be either true or false. true to make copied files available via public links, false to reverse the behavior.</param>
        /// <param name="pattern">Applies to custom storage usage scenario only. The parameter is used to specify file names Uploadcare passes to a custom storage. In case the parameter is omitted, we use pattern of your custom storage.
        ///
        /// Allowed values:
        /// ${default} = ${uuid}/${auto_filename}
        /// ${auto_filename} = ${filename} ${effects} ${ext}
        /// ${effects} = processing operations put into a CDN URL
        /// ${filename} = original filename, no extension
        /// ${uuid} = file UUID
        /// ${ext} = file extension, leading dot, e.g. .jpg
        /// </param>
        /// <param name="store">The parameter only applies to the Uploadcare storage and MUST be either true or false. true to store files while copying.If stored, files won’t be automatically deleted after a 24-hour period. false to not store files, default.</param>
        /// <param name="target">Identifies a custom storage name related to your project. Implies you are copying a file to a specified custom storage. Keep in mind you can have multiple storages associated with a single S3 bucket.</param>
        /// <remarks>Copying is synchronous: new files can be used upon a request completion. Hence, copy requests take more time for large files.</remarks>
        public async Task<CopyResult> CopyAsync(Guid sourceFileId, bool? makePublic = false, string pattern = null, bool? store = false, string target = null)
        {
            var sourceString = sourceFileId.ToString();

            return await this.CopyInternal(sourceString, makePublic, pattern, store, target).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy original files or their modified versions to a custom storage or the default one.
        /// Source files MAY either be stored or just uploaded and MUST NOT be deleted.
        /// </summary>
        /// <param name="source">A CDN URL of a file subjected to copy.</param>
        /// <param name="target">Identifies a custom storage name related to your project. Implies you are copying a file to a specified custom storage. Keep in mind you can have multiple storages associated with a single S3 bucket.</param>
        /// <param name="store">The parameter only applies to the Uploadcare storage and MUST be either true or false. true to store files while copying.If stored, files won’t be automatically deleted after a 24-hour period. false to not store files, default.</param>
        /// <param name="makePublic">Applicable to custom storage only. MUST be either true or false. true to make copied files available via public links, false to reverse the behavior.</param>
        /// <param name="pattern">Applies to custom storage usage scenario only. The parameter is used to specify file names Uploadcare passes to a custom storage. In case the parameter is omitted, we use pattern of your custom storage.
        ///
        /// Allowed values:
        /// ${default} = ${uuid}/${auto_filename}
        /// ${auto_filename} = ${filename} ${effects} ${ext}
        /// ${effects} = processing operations put into a CDN URL
        /// ${filename} = original filename, no extension
        /// ${uuid} = file UUID
        /// ${ext} = file extension, leading dot, e.g. .jpg
        /// </param>
        /// <remarks>Copying is synchronous: new files can be used upon a request completion. Hence, copy requests take more time for large files.</remarks>
        public async Task<CopyResult> CopyAsync(Uri source, string target, bool store = false, bool makePublic = false, string pattern = null)
        {
            return await this.CopyInternal(source.ToString(), makePublic, pattern, store, target).ConfigureAwait(false);
        }

        public async Task<DeleteResult> DeleteAsync(Guid fileId)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, UrlHelper.ApiFiles(fileId)))
            {
                return await ExecuteRequestAsync<DeleteResult>(this._httpClient, request).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(IEnumerable<Guid> imageIds)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, UrlHelper.ApiFilesStorage()))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(imageIds), Encoding.UTF8, "application/json");

                await ExecuteRequestAsync(this._httpClient, request).ConfigureAwait(false);
            }
        }

        public async Task<DeleteResult> DeleteStorageAsync(Guid fileId)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, UrlHelper.ApiFilesStorage(fileId)))
            {
                return await ExecuteRequestAsync<DeleteResult>(this._httpClient, request).ConfigureAwait(false);
            }
        }

        public async Task<File> GetAsync(Guid fileId)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, UrlHelper.ApiFiles(fileId)))
            {
                return await ExecuteRequestAsync<File>(this._httpClient, request).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get a paginated file list.
        /// </summary>
        /// <param name="filter"></param>
        public async Task<PaginatedResult<File>> GetListAsync(GetFilesFilter filter)
        {
            var pathSB = new StringBuilder("?ordering=");

            if (filter.SortDirection.Equals(GetFilesFilter.Direction.Descending))
            {
                pathSB.Append("-");
            }

            pathSB.Append(filter.Sort);

            if (filter.OnlyRemoved)
            {
                pathSB.Append("&removed=true");
            }

            if (filter.Stored.HasValue)
            {
                pathSB.Append("&stored=").Append(filter.Stored.Value ? "true" : "false");
            }

            if (filter.Limit > 0)
            {
                pathSB.Append("&limit=").Append(filter.Limit);
            }

            if (filter.Sort.Equals(GetFilesFilter.SortProperty.datetime_uploaded)
                && filter.FromDatetimeUploaded.HasValue)
            {
                pathSB.Append("&from=").Append(filter.FromDatetimeUploaded.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
            }
            else if (filter.Sort.Equals(GetFilesFilter.SortProperty.size)
                && filter.FromSize > 0)
            {
                pathSB.Append("&from=").Append(filter.FromSize);
            }

            var uri = new Uri(UrlHelper.ApiFiles(), pathSB.ToString());

            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await ExecuteRequestAsync<PaginatedResult<File>>(this._httpClient, request).ConfigureAwait(false);
            }
        }

        public async Task<StoreResult> StoreAsync(Guid imageId)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, UrlHelper.ApiFilesStorage()))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(imageId), Encoding.UTF8, "application/json");

                return await ExecuteRequestAsync<StoreResult>(this._httpClient, request).ConfigureAwait(false);
            }
        }

        public async Task<StoreBatchResult> StoreAsync(IEnumerable<Guid> imageIds)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, UrlHelper.ApiFilesStorage()))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(imageIds), Encoding.UTF8, "application/json");

                return await ExecuteRequestAsync<StoreBatchResult>(this._httpClient, request).ConfigureAwait(false);
            }
        }

        private async Task<CopyResult> CopyInternal(string source, bool? makePublic, string pattern, bool? store, string target)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, UrlHelper.ApiFiles()))
            {
                var contentSB = new StringBuilder($"source={source}");

                if (makePublic.HasValue)
                {
                    contentSB.Append("&makePublic=").Append(makePublic.Value);
                }

                if (!string.IsNullOrEmpty(pattern))
                {
                    contentSB.Append("&pattern=").Append(pattern);
                }

                if (store.HasValue)
                {
                    contentSB.Append("&store=").Append(store.Value);
                }

                if (!string.IsNullOrEmpty(target))
                {
                    contentSB.Append("&target=").Append(target);
                }

                request.Content = new StringContent(contentSB.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");

                return await ExecuteRequestAsync<CopyResult>(this._httpClient, request).ConfigureAwait(false);
            }
        }
    }
}