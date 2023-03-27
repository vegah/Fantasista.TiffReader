using BitMiracle.LibJpeg;
using BitMiracle.LibJpeg.Classic;
using Fantasista.TiffReader.Image.Decompressors.Jpeg;

namespace Fantasista.TiffReader.Image.Decompressors
{
    public class JpegDecompressor : IDecompressor
    {
        private byte[]? _quanitzationTable;
        private TiffJpeg _tiffJpeg;

        public JpegDecompressor(byte[]? quantizationTable)
        {
            _quanitzationTable = quantizationTable;
            _tiffJpeg = new TiffJpeg(quantizationTable);

        }
        public byte[] Decompress(byte[] compressedData)
        {
            var decompressor = new jpeg_decompress_struct();
            var mergedJpeg = _tiffJpeg.GetTiffJpegAsNormalJpeg(compressedData);

            decompressor.jpeg_stdio_src(new MemoryStream(mergedJpeg));
            decompressor.jpeg_read_header(true);
            decompressor.jpeg_start_decompress();
            var read = 0;
            var result = new byte[0];
            while (read<decompressor.Image_height)
            {
                var tempBuffer = new byte[decompressor.Image_height][];
                tempBuffer = tempBuffer.Select(x=>new byte[decompressor.Image_width*decompressor.Num_components]).ToArray();
                var new_read= decompressor.jpeg_read_scanlines(tempBuffer,decompressor.Image_height);
                result = result.Concat(tempBuffer.Take(new_read).SelectMany(x=>x)).ToArray();
                read+=new_read;
            }
            decompressor.jpeg_finish_decompress();
            return result;
        }

    }
}