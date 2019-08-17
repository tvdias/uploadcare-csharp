using System;

namespace Uploadcare.Helpers
{
    public static class UrlHelper
    {
        private static readonly Uri API = new Uri("https://api.uploadcare.com");
        private static readonly Uri CDN = new Uri("https://ucarecdn.com");
        private static readonly Uri UPLOAD = new Uri("https://upload.uploadcare.com");

        /// <summary>
        /// Creates a URL to a project resource.
        /// </summary>
        public static Uri ApiProject()
        {
            return new Uri(API, "project/");
        }

        /// <summary>
        /// Creates a URL to a file resource.
        /// </summary>
        /// <param name="fileId"> UploadcareFile UUID
        /// </param>
        public static Uri ApiFiles(Guid? fileId = null)
        {
            var filePath = fileId.HasValue ? $"{fileId}/" : string.Empty;

            return new Uri(API, $"files/{filePath}");
        }

        /// <summary>
        /// Creates a URL to the storage action for a file (saving the file).
        /// </summary>
        /// <param name="fileId"> UploadcareFile UUID
        /// </param>
        public static Uri ApiFilesStorage(Guid? fileId = null)
        {
            var filePath = fileId.HasValue ? $"{fileId}/storage/" : string.Empty;

            return new Uri(API, $"files/{filePath}");
        }

        /// <summary>
        /// Creates a URL to the file upload endpoint.
        /// </summary>
        public static Uri UploadBase()
        {
            return new Uri(UPLOAD, "base/");
        }

        /// <summary>
        /// Creates a URL for URL upload.
        /// </summary>
        /// <param name="sourceUrl"> URL to upload from </param>
        /// <param name="pubKey"> Public key
        /// </param>
        public static Uri UploadFromUrl(string sourceUrl, string pubKey, bool? store = null)
        {
            var path = $"from_url/?source_url={sourceUrl}&pub_key={pubKey}";

            if (store.HasValue)
            {
                path += $"&store={(store.Value ? 1 : 0)}";
            }

            return new Uri(UPLOAD, path);
        }

        /// <summary>
        /// Creates a URL for URL upload status (e.g. progress).
        /// </summary>
        /// <param name="token"> Token, received after a URL upload request
        /// </param>
        public static Uri UploadFromUrlStatus(string token)
        {
            return new Uri(UPLOAD, "from_url/status/?token=" + token);
        }
    }
}