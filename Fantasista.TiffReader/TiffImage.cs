using System.Runtime.InteropServices;
using BitMiracle.LibJpeg.Classic;
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
            var colorComponentsField = GetSoft<SingleShortOrLongValueField>(Tag.SamplesPerPixel);
            if (colorComponentsField==null) ColorComponents=1;
            else ColorComponents=colorComponentsField.Value;            
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
                case CompressionType.Packbits:
                {
                    return new PackbitDecompressor();
                }
                case CompressionType.NoCompression:
                    return new NoCompressionDecompressor();
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
            var field = GetSoft<T>(tag);
            if (field==null)
                throw new MissingTagException($"Tag {tag} is missing, but is required");
            return field;
        }

        private T? GetSoft<T>(Tag tag) where T:Field
        {
            if (_ifd.Contains(tag)) return _ifd[tag] as T;
            else return null;
        }

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public uint ColorComponents { get; private set; }

        public byte[] RawData { get; private set; }

        public void WriteToRgbJpeg(Stream s) 
        {
                jpeg_compress_struct compressor = new jpeg_compress_struct();
                compressor.jpeg_stdio_dest(s);
                compressor.Image_height = (int)Height;
                compressor.Image_width = (int)Width;
                compressor.Input_components = 3;
                compressor.In_color_space = J_COLOR_SPACE.JCS_RGB;
                compressor.jpeg_set_defaults();
                compressor.jpeg_start_compress(true);
                var converter = new RgbConverter(RawData,(int)Width,(int)Height,(int)ColorComponents);
                var convertedData = converter.Convert();
                var rowData = new byte[1][];
                var currentRow = 0;
                while (compressor.Next_scanline < compressor.Image_height)
                {
                    rowData[0] = convertedData.Skip(currentRow*(int)Width*3).Take((int)Width*3).ToArray();
                    compressor.jpeg_write_scanlines(rowData,1);
                    currentRow++;
                }
                compressor.jpeg_finish_compress();
        }

    }
}