namespace Fantasista.TiffReader.Image.Decompressors
{
    public class NoCompressionDecompressor : IDecompressor
    {
        public NoCompressionDecompressor() 
        {

        }

        public byte[] Decompress(byte[] compressedData)
        {
            return compressedData;
        }
    }
}