using System.Runtime.Serialization;

namespace Fantasista.TiffReader.Image.Decompressors.Jpeg
{
    [Serializable]
    internal class TiffJpegMergerException : Exception
    {

        public TiffJpegMergerException(string? message) : base(message)
        {
        }
    }
}