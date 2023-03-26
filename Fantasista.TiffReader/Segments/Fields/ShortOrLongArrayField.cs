namespace Fantasista.TiffReader.Segments.Fields
{
    public class ShortOrLongArrayField : Field
    {
        public ShortOrLongArrayField(Tag tag, FieldType type, TiffBinaryReader reader) : base(tag, type, reader)
        {
        }

        public uint[] GetArrayOfLongs()
        {
            var array = new uint[_value.Count];
            var oldPos = _reader.Position;
            _reader.Seek(_value.GetLong());
            for (var x=0;x<_value.Count;x++)
            {
                if (_type==FieldType.Short)
                    array[x]=_reader.ReadUInt16();
                else
                    array[x]=_reader.ReadUInt32();
            }
            _reader.Seek(oldPos);
            return array;
        }

        public override string ToString()
        {
            return $"Field: {_tag} of type {_type}[] with length {_value.Count}";
        }
    }
}