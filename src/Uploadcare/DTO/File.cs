using System;

namespace Uploadcare.DTO
{
    public class File
    {
        /// <summary>
        /// Date and time when a file was removed, if any.
        /// </summary>
        public DateTime? DatetimeRemoved { get; set; }

        /// <summary>
        /// Date and time of the last store request, if any.
        /// </summary>
        public DateTime? DatetimeStored { get; set; }

        /// <summary>
        /// Date and time when a file was uploaded.
        /// </summary>
        public DateTime DatetimeUploaded { get; set; }

        /// <summary>
        /// Image meta (if a file is an image).
        /// </summary>
        public ImageInfo ImageInfo { get; set; }

        /// <summary>
        /// true, if your file is an image and can be processed via Image Processing.
        /// </summary>
        /// <remarks>
        /// Please note, our processing engine does not treat all image files as such.
        /// Some of those may not be supported due to file sizes, resolutions or formats.
        /// In the case, the flag is set to false. false otherwise.
        /// </remarks>
        public bool? IsImage { get; set; }

        /// <summary>
        /// If a file is ready and not deleted, it is available on CDN.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// File MIME type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Publicly available file CDN URL. Available if a file is not deleted.
        /// </summary>
        public string OriginalFileUrl { get; set; }

        /// <summary>
        /// Original name of an uploaded file.
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// File size in bytes.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// API resource URL for a particular file.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// File UUID.
        /// </summary>
        public Guid Uuid { get; set; }

        public string Source { get; set; }
    }
}