namespace Fantasista.TiffReader.Image.Decompressors
{
    public class PackbitDecompressor : IDecompressor
    {

        public byte[] Decompress(byte[] compressedData)
        {
            List<byte> result = new List<byte>();
            var index = 0;
            while (index<compressedData.Length)
            {
                var sb = (sbyte)compressedData[index++];
                if (sb>0) 
                    for (var x=0;x<sb;x++)
                        result.Add(compressedData[index++]);
                else if (sb==-128)
                    result.Add((byte)sb);
                else
                {
                    var nextByte = compressedData[index++];
                    for (var x=0;x<0-sb+1;x++)
                        result.Add(nextByte);
                }
            }
            return result.ToArray();
        }
    }
}