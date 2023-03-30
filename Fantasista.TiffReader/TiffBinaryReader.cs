using Fantasista.TiffReader.Segments.Fields;

namespace Fantasista.TiffReader
{
    public enum Endianness
    {
        Little,
        Big
    }

    public class TiffBinaryReader
    {
        private BinaryReader _reader;
        private Endianness _endianess;

        public long Position => _reader.BaseStream.Position;
        public TiffBinaryReader(Stream stream)
        {
            _reader = new BinaryReader(stream); 
            _endianess = BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;           
        }

        public void Seek(long offset)
        {
            _reader.BaseStream.Seek(offset,SeekOrigin.Begin);
        }

        public void SetEndianness(Endianness endianess)
        {
            _endianess = endianess;
        }

        public byte[] ReadByteArray(uint offset, uint length)
        {
            var curPos = this.Position;
            Seek(offset);
            var result = _reader.ReadBytes((int)length);
            Seek(curPos);
            return result;
        }

        public UInt16 ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadNext(2),0);
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }

        public UInt32 ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadNext(4),0);
        }

        public FieldValue ReadFieldValue(FieldType type)
        {
            var count = ReadUInt32();
            if (type==FieldType.Short) {
                var part1 = ReadUInt16();
                var part2 = ReadUInt16();
                return new FieldValue(count,part1,this);
            }
            else 
                return new FieldValue(count,ReadUInt32(),this);

        }

        private byte[] ReadNext(byte size)
        {
            var byteArr = _reader.ReadBytes(size);
            return FixEndianess(ref byteArr);
        }

        public byte[] FixEndianess(ref byte[] byteArr)
        {
            if (_endianess == Endianness.Big && BitConverter.IsLittleEndian || _endianess == Endianness.Little && !BitConverter.IsLittleEndian)
                byteArr = byteArr.Reverse().ToArray();
            return byteArr;
        }


    }
}