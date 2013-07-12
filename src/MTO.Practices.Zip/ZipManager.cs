namespace MTO.Practices.Common.Zip
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Ionic.Zip;

    public static class ZipManager
    {
        public static IList<ZipPart> Unzip(Stream stream)
        {
            IList<ZipPart> files = new List<ZipPart>();

            if (stream != null)
            {
                using (ZipFile zip = ZipFile.Read(stream))
                {
                    foreach (var part in zip)
                    {
                        if (!part.IsDirectory)
                        {
                            var zipPart = new ZipPart
                            {
                                InputStream = new MemoryStream(Convert.ToInt32(part.UncompressedSize)),
                                FileName = part.FileName.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar),
                                ContentType = string.Empty,
                                ContentLength = Convert.ToInt32(part.CompressedSize)
                            };

                            part.Extract(zipPart.InputStream);
                            files.Add(zipPart);
                        }
                    }
                }
            }

            return files;
        }
    }
}
