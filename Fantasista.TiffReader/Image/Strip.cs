using Fantasista.TiffReader.Image.Decompressors;
using Fantasista.TiffReader.Segments;

namespace Fantasista.TiffReader.Image
{
    public class Strip
    {
        private uint _offset;
        private uint _byteCount;
        private TiffBinaryReader _reader;

        public Strip(uint offset, uint byteCount, TiffBinaryReader reader, IDecompressor decompressor)
        {
            _offset = offset;
            _byteCount = byteCount;
            _reader = reader;

            var bytes = _reader.ReadByteArray(offset,byteCount);
            decompressor.Decompress(bytes);

            File.WriteAllBytes("/home/vegardb/Downloads/temp/"+offset+".jpg",bytes);
        }


    }
}