using System.Runtime.InteropServices;
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
        private List<Strip> _strips;

        public TiffImage(TiffBinaryReader reader,IFD ifd)
        {
            _reader = reader;
            _ifd = ifd;
            _strips = new List<Strip>();
            RawData = new byte[0];
        }

        public void Read()
        {
            GetBasicInformation();
            GetStrips();
            MergeRawData();
        }

        public void GetBasicInformation()
        {
            Height = GetHard<SingleShortOrLongValueField>(Tag.ImageLength).Value;
            Width = GetHard<SingleShortOrLongValueField>(Tag.ImageWidth).Value;
            ColorComponents = GetHard<SingleShortOrLongValueField>(Tag.SamplesPerPixel).Value;
        }

        private void MergeRawData()
        {
            var arr = new byte[0];
            foreach (var strip in _strips)
            {
                arr = arr.Concat(strip.RawDecompressedData).ToArray();
            }
            RawData = arr;
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
                _strips.Add(new Strip(offsets[offsetNo],byteCounts[offsetNo],_reader,decompressor));
            }    


        }

        private T GetHard<T>(Tag tag) where T: Field
        {
            var field = _ifd[tag] as T;
            if (field==null)
                throw new MissingTagException($"Tag {tag} is missing, but is required");
            return field;
        }

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public uint ColorComponents { get; private set; }

        public byte[] RawData { get; private set; }

    }
}