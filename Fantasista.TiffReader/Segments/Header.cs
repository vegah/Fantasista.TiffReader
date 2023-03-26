using Fantasista.TiffReader.Segments.Exceptions;

namespace Fantasista.TiffReader.Segments
{
    public class Header
    {
        private TiffBinaryReader _reader;

        public uint FirstIFDOffset { get; private set; }

        public Header(TiffBinaryReader reader)
        {
            _reader = reader;
        }

        public void Read()
        {
            ReadAndSetByteOrder();  
            ReadMagicByte();
            ReadFirstIFDOffset();  
        }

        private void ReadFirstIFDOffset()
        {
            FirstIFDOffset = _reader.ReadUInt32(); 
        }

        private void ReadMagicByte()
        {
            var magic = _reader.ReadUInt16();
            if (magic!=42) throw new HeaderException($"Magic byte expected as 42 - was {magic}");
        }

        private void ReadAndSetByteOrder()
        {
            var byteorder = _reader.ReadUInt16();
            if (byteorder==0x4949)
                _reader.SetEndianness(Endianness.Little);
            else if (byteorder==0x4d4d)
                _reader.SetEndianness(Endianness.Big);
            else
                throw new HeaderException($"The header must declare endianness. Was 0x{byteorder.ToString("x2")}, expected 0x4949 or 0x4d4d");
        }


    }
}