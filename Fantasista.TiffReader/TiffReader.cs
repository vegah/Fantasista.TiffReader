using Fantasista.TiffReader.Segments;

namespace Fantasista.TiffReader;
public class TiffReader
{
    private Stream _s;
    private TiffBinaryReader _br;

    public TiffReader(Stream s)
    {
        _s = s;
        _br = new TiffBinaryReader(s);
    }

    public IEnumerable<TiffImage> Read()
    {
        var header = new Header(_br);
        header.Read();
        var nextIfd = header.FirstIFDOffset;
        Console.WriteLine(nextIfd);
        while (nextIfd!=0)
        {
            Console.WriteLine($"New IFD at offset {nextIfd}");
            var ifd = new IFD(_br,nextIfd);
            ifd.ReadIFD();
            var image = new TiffImage(_br,ifd);
            image.Read();
            yield return image;
            nextIfd = ifd.NextOffset;
        }
    }

    

}
