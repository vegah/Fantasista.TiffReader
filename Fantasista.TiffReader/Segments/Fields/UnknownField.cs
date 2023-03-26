namespace Fantasista.TiffReader.Segments.Fields
{
    public class UnknownField : Field
    {
        private ushort _originalType;

        public UnknownField(Tag tag, FieldType type, TiffBinaryReader reader, ushort originalType) : base(tag, type, reader)
        {
            _originalType = originalType;
        }

        public override string ToString()
        {
            return $"Field: {_tag} - {_tagNumber} - (original : {_originalType})";
        }
    }
}