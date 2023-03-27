using System.IO;
using Xunit;

namespace Fantasista.TiffReader.Tests
{
    public class TestSuite
    {
        [Fact]
        public void All_ParseExampleFiles()
        {
            // var stream = File.OpenRead("/home/vegardb/Downloads/at3_1m4_01.tif");
            var stream = File.OpenRead("/home/vegardb/Downloads/aab.tif");
            var reader = new TiffReader(stream);
            reader.Read();
        }
    }
}