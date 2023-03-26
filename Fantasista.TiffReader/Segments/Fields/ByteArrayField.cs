namespace Fantasista.TiffReader.Segments.Fields
{
    public class ByteArrayField : Field
    {
        private byte[] _bytes;

        public ByteArrayField(Tag tag, FieldType type, TiffBinaryReader reader) : base(tag, type, reader)
        {
            _bytes = _value.GetBytes();
        }

        public byte[] Value => _bytes;

        public override string ToString()
        {
            return $"Field: {_tag} of type {_type}[]/byte[] with length {_bytes.Length}";
        }
        
    }
}