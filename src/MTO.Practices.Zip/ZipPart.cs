namespace MTO.Practices.Common.Zip
{
    using System.IO;

    public class ZipPart
    {
        public Stream InputStream { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public int ContentLength { get; set; }
    }
}
