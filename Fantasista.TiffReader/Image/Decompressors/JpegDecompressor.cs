using JpegLibrary;

namespace Fantasista.TiffReader.Image.Decompressors
{
    public class JpegDecompressor : IDecompressor
    {
        private byte[] _quanitzationTable;
        JpegHuffmanDecodingTable? _huffmanTable;

        public JpegDecompressor(byte[] quantizationTable)
        {
            _quanitzationTable = quantizationTable;

        }
        public byte[] Decompress(byte[] compressedData)
        {
            var parsedQuantizationTable = new JpegQuantizationTable();
            int bytesConsumed = 0;
            var readOnlySpan = new System.Buffers.ReadOnlySequence<byte>(_quanitzationTable);
            if (!JpegQuantizationTable.TryParse(0,0,readOnlySpan,out parsedQuantizationTable,ref bytesConsumed))
            {
                throw new JpegDecompressionException("Quantization table is not in correct format");            
            }

            var decoder = new JpegDecoder();
            decoder.SetInput(new ReadOnlyMemory<byte>(compressedData));
            decoder.Identify(); 
            Console.WriteLine("NoC :"+decoder.NumberOfComponents);
            Console.WriteLine("Precision :"+decoder.Precision);
            byte[] output = new byte[decoder.Width*decoder.Height*decoder.NumberOfComponents];
            decoder.SetQuantizationTable(parsedQuantizationTable);
            if (_huffmanTable!=null)
                decoder.SetHuffmanTable(_huffmanTable);
            decoder.SetOutputWriter(new JpegBufferOutputWriter8Bit(decoder.Width,decoder.Height,2,output));
            decoder.Decode();
            _huffmanTable = decoder.GetHuffmanTable(true,0);
            _huffmanTable = decoder.GetHuffmanTable(true,1);

            return new byte[0];            
        }

    }
}