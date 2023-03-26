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
            var bytes = BitConverter.GetBytes(_value);
            var shortPart = bytes.Take(4).ToArray();
            return BitConverter.ToUInt16(_reader.FixEndianess(ref shortPart));
        } 

        public uint GetLong()
        {
            var bytes = BitConverter.GetBytes(_value);
            return BitConverter.ToUInt32(_reader.FixEndianess(ref bytes));            
        } 

        public RationalNumber GetRational()
        {
            var position = _reader.Position;
            var newPos = GetLong();
            Console.WriteLine("NewPost "+newPos);
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