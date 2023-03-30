namespace Fantasista.TiffReader.Segments.Fields
{
    public class FieldValue
    {
        private uint _count;
        private uint _value;
        private TiffBinaryReader _reader;

        public FieldValue(uint count, uint value, TiffBinaryReader reader)
        {
            _count = count;
            _value = value; 
            _reader = reader;
        }

        public uint Count => _count;

        public ushort GetShort()
        {
            return (ushort)_value;
        } 

        public uint GetLong()
        {
            return _value;
        } 

        public RationalNumber GetRational()
        {
            var position = _reader.Position;
            var newPos = GetLong();
            _reader.Seek(GetLong());
            var numerator =_reader.ReadUInt32();
            var denominator = _reader.ReadUInt32();
            _reader.Seek(position);
            return new RationalNumber(numerator,denominator);                        
        }

        public string GetString()
        {
            var position = _reader.Position;
            var newPos = GetLong();
            _reader.Seek(GetLong());
            var str = _reader.ReadString();
            _reader.Seek(position);
            return str;                        

        }

        public byte[] GetBytes()
        {
            var newPos = GetLong();
            return _reader.ReadByteArray(newPos,_count);
        }


    }
}