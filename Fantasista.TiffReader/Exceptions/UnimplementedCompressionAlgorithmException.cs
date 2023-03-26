using Fantasista.TiffReader.Segments.Fields;

namespace Fantasista.TiffReader.Exceptions
{
    public class UnimplementedCompressionAlgorithmException : Exception
    {

        public UnimplementedCompressionAlgorithmException(CompressionType type) : base($"Currently {type} is not implemented as a compression algorithm.")
        {
        }
    }
}