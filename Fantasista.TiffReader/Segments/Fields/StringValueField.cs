namespace Fantasista.TiffReader.Segments.Fields
{
    internal class StringValueField : Field
    {
        private string _str;

        public StringValueField(Tag tag, FieldType type, TiffBinaryReader reader) : base(tag, type, reader)
        {
            _str = _value.GetString();
        }

        public string Value => _str;

        public override string ToString()
        {
            return $"Field: {_tag} of type {_type} with value {Value}";
        }
    }
}