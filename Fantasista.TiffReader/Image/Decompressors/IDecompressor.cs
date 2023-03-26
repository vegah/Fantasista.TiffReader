namespace Fantasista.TiffReader.Image.Decompressors
{
    public interface IDecompressor
    {
         byte[] Decompress(byte[] compressedData);
    }
}