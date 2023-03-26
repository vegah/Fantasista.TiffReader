using System.Runtime.Serialization;

namespace Fantasista.TiffReader.Image.Decompressors
{
    [Serializable]
    internal class JpegDecompressionException : Exception
    {

        public JpegDecompressionException(string? message) : base(message)
        {
        }

    }
}