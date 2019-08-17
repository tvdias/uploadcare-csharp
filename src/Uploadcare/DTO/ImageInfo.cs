using System;

namespace Uploadcare.DTO
{
    public class ImageInfo
    {
        public string ColorMode { get; set; }

        public string Orientation { get; set; }

        public string Format { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string GeoLocation { get; set; }

        public DateTime? DatetimeOriginal { get; set; }

        public int[] Dpi { get; set; }

        public bool Sequence { get; set; }
    }
}