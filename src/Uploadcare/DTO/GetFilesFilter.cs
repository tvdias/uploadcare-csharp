using System;

namespace Uploadcare.DTO
{
    public class GetFilesFilter
    {
        /// <summary>
        /// true to only include removed files in the response, false to include existing files. Defaults to false.
        /// </summary>
        public bool OnlyRemoved { get; set; }

        /// <summary>
        /// true to only include files that were stored, false to include temporary ones. The default is unset: both stored and not stored files are returned.
        /// </summary>
        public bool? Stored { get; set; }

        /// <summary>
        /// A preferred amount of files in a list for a single response. Defaults to 100, while the maximum is 1000.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Specifies the property the files are sorted by.
        /// </summary>
        public SortProperty Sort { get; set; }

        /// <summary>
        /// Specifies the direction files are sorted
        /// </summary>
        public Direction SortDirection { get; set; }

        /// <summary>
        /// A starting point for filtering files when DatetimeUploaded is used as sorting property.
        /// </summary>
        public DateTime? FromDatetimeUploaded { get; set; }

        /// <summary>
        /// A starting point for filtering files when Size is used as sorting property.
        /// </summary>
        public int FromSize { get; set; }

        public enum SortProperty
        {
            // TODO: Proper naming
            datetime_uploaded = 0,
            size
        }

        public enum Direction
        {
            Ascending = 0,
            Descending,
        }
    }
}