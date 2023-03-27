using JpegLibrary;

namespace Fantasista.TiffReader.Image.Decompressors
{
    public class JpegDecompressor : IDecompressor
    {
        private byte[] _quanitzationTable;
        private JpegDecoder _decoder;
        private JpegHuffmanDecodingTable? _dcHuffmanTable;
        private JpegHuffmanDecodingTable? _acHuffmanTable;

        public JpegDecompressor(byte[] quantizationTable)
        {
            _quanitzationTable = quantizationTable;
            _decoder = new JpegDecoder();
            var parsedQuantizationTable = new JpegQuantizationTable();
            var readOnlySpan = new System.Buffers.ReadOnlySequence<byte>(_quanitzationTable);
            int bytesConsumed = 0;
            if (!JpegQuantizationTable.TryParse(0,0,readOnlySpan,out parsedQuantizationTable,ref bytesConsumed))
            {
                throw new JpegDecompressionException("Quantization table is not in correct format");            
            }
            Console.WriteLine("Bytes consumed "+bytesConsumed);
            _decoder.SetQuantizationTable(parsedQuantizationTable);            

        }
        public byte[] Decompress(byte[] compressedData)
        {

            _decoder.SetInput(new ReadOnlyMemory<byte>(compressedData));
            _decoder.Identify(); 
            byte[] output = new byte[_decoder.Width*_decoder.Height*_decoder.NumberOfComponents];
            var outputWriter = new JpegBufferOutputWriter8Bit(_decoder.Width,_decoder.Height,_decoder.NumberOfComponents,output);
            _decoder.SetOutputWriter(outputWriter);
            _decoder.Decode();
            return output.Where((v,i)=>i%2==0).ToArray();            
        }

    }
}