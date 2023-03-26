using Fantasista.TiffReader.Exceptions;
using Fantasista.TiffReader.Image;
using Fantasista.TiffReader.Image.Decompressors;
using Fantasista.TiffReader.Segments;
using Fantasista.TiffReader.Segments.Fields;

namespace Fantasista.TiffReader
{
    public class TiffImage
    {
        private TiffBinaryReader _reader;
        private IFD _ifd;

        public TiffImage(TiffBinaryReader reader,IFD ifd)
        {
            _reader = reader;
            _ifd = ifd;
        }

        public void Read()
        {
            GetStrips();
        }

        private IDecompressor CreateDecompressor(CompressionType type)
        {
            switch (type)
            {
                case CompressionType.JPEG:
                {
                    var quantizationTableField = GetHard<ByteArrayField>(Tag.JPEGTables);        
                    return new JpegDecompressor(quantizationTableField.Value);
                }
                default:
                {
                    throw new UnimplementedCompressionAlgorithmException(type);
                }
            }
        }

        private void GetStrips()
        {
            var stripOffsets = GetHard<ShortOrLongArrayField>(Tag.StripOffsets);
            var stripByteCounts = GetHard<ShortOrLongArrayField>(Tag.StripByteCounts);
            var compression = GetHard<CompressionField>(Tag.Compression);
            var decompressor = CreateDecompressor(compression.Value);
            
            var offsets = stripOffsets.GetArrayOfLongs();
            var byteCounts = stripByteCounts.GetArrayOfLongs();
            if (offsets.Length!=byteCounts.Length)
                throw new InconsistentStripsException($"Number of offsets : {offsets}, number of bytecounts : {byteCounts} ");

            for (var offsetNo =0;offsetNo<offsets.Length;offsetNo++)
            {
                var strip = new Strip(offsets[offsetNo],byteCounts[offsetNo],_reader,decompressor);

            }    


        }

        private T GetHard<T>(Tag tag) where T: Field
        {
            var field = _ifd[tag] as T;
            if (field==null)
                throw new MissingTagException($"Tag {tag} is missing, but is required");
            return field;
        }
    }
}