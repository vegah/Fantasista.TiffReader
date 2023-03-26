namespace Fantasista.TiffReader.Segments.Fields
{
    public class SingleRationalNumberField : Field
    {
        private RationalNumber _rationalValue;

        public SingleRationalNumberField(Tag tag, FieldType type, TiffBinaryReader reader) : base(tag, type, reader)
        {
            _rationalValue = _value.GetRational();
        }

        public RationalNumber Value => _rationalValue;

        public override string ToString()
        {
            return $"Field: {_tag} of type {_type} with value {Value}";
        }
    }
}