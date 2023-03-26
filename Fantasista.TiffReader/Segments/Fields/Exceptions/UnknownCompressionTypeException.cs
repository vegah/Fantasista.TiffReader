namespace Fantasista.TiffReader.Segments.Fields.Exceptions
{
    public class UnknownCompressionTypeException : Exception
    {
        public UnknownCompressionTypeException(ushort value) : base($"Unknown compression type : {value}")
        {
            
        }
    }
}