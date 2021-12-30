using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PictureOrganizer
{
    public static class Helper
    {
        private static Regex r = new Regex(":");

        public static DateTime? GetDateTakenFromImage(string path)
        {
            try
            {
                var data = ImageMetadataReader.ReadMetadata(path);
                var subIfdDirectory = data.OfType<ExifSubIfdDirectory>().FirstOrDefault();

                var tagDateTimeOriginal = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

                if (!string.IsNullOrEmpty(tagDateTimeOriginal))
                {
                    var value = r.Replace(tagDateTimeOriginal, "-", 2);
                    return DateTime.Parse(value);
                }

                return null;
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}
