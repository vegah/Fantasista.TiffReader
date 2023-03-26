namespace Fantasista.TiffReader.Segments.Fields
{
    public class SingleShortOrLongValueField : Field
    {
        private ushort _ssize;
        private uint _lsize;

        public SingleShortOrLongValueField(Tag tag, FieldType type, TiffBinaryReader reader) : base(tag, type, reader)
        {
            if (type==FieldType.Short)
                _ssize = _value.GetShort();
            else if (type==FieldType.Long)
                _lsize = _value.GetLong();
            else throw new FieldValueException("ImageLength or ImageHeight must be Short or Long");
        }

        public uint Value => _type == FieldType.Short ? _ssize : _lsize;

        public override string ToString()
        {
            return $"Field: {_tag} of type {_type} with value {Value}";
        }
    }
}