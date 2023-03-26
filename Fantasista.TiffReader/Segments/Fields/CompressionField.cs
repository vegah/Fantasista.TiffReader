using Fantasista.TiffReader.Segments.Fields.Exceptions;

namespace Fantasista.TiffReader.Segments.Fields
{
    public enum CompressionType : ushort
    {
        NoCompression = 1,
        CCITT = 2,
        Packbits = 32773,
        CCITT_G3 = 3,
        CCITT_G4 = 4,
        LZW = 5,
        OLDJPEG = 6,
        JPEG = 7,
        DEFLATE = 8

    }

    public class CompressionField : SingleShortOrLongValueField
    {
        private ushort _readValue;

        public CompressionField(Tag tag, FieldType type, TiffBinaryReader reader) : base(tag, type, reader)
        {
            _readValue = _value.GetShort();
        }

        public new CompressionType Value 
        {
            get 
            {
                if (!Enum.IsDefined(typeof(CompressionType),_readValue))
                    throw new UnknownCompressionTypeException(_readValue);
                return (CompressionType)_readValue;
            }
        }

        public override string ToString()
        {
            return $"Field: {_tag} of type {_type} with value {Value}";
        }


    }
}