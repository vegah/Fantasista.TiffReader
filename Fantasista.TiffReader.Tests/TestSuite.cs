using System;
using System.IO;
using Xunit;

namespace Fantasista.TiffReader.Tests
{
    public class TestSuite
    {
        [Fact]
        public void All_ParseExampleFiles()
        {
            var tiffExamplesDirectory = "../../../../testimages";
            var files = Directory.GetFiles(tiffExamplesDirectory);
            foreach (var file in files)
            {
                using var stream = File.OpenRead(file);
                var reader = new TiffReader(stream);
                Console.WriteLine($"Extracting from : {file}");
                var fileNo = 0;
                foreach (var image in reader.Read())
                {
                    Console.WriteLine($"Width :{image.Width}, Height: ${image.Height}, Components: {image.ColorComponents} - size: {image.Width*image.Height*image.ColorComponents}");
                    Console.WriteLine($"Output buffer : {image.RawData.Length}");
                    var justfileName = Path.GetFileNameWithoutExtension(file);
                    using var outstream =  File.OpenWrite("../../../../testimages/out/"+justfileName+(fileNo++)+".jpg");
                    image.WriteToRgbJpeg(outstream);

                }
            }
        }

        [Fact]
        public void TestOddIfd()
        {
            var file = "../../../../testimages/odd_ifd.tif";
            using var stream = File.OpenRead(file);
            var reader = new TiffReader(stream);
            Console.WriteLine($"Extracting from : {file}");
            var fileNo = 0;
            foreach (var image in reader.Read())
            {
                Console.WriteLine($"Width :{image.Width}, Height: ${image.Height}, Components: {image.ColorComponents} - size: {image.Width*image.Height*image.ColorComponents}");
                Console.WriteLine($"Output buffer : {image.RawData.Length}");
                using var outstream =  File.OpenWrite("../../../../testimages/out/odd_ifd"+(fileNo++)+".jpg");
                image.WriteToRgbJpeg(outstream);
            }
        }

        [Fact]
        public void Test2ComponentShit()
        {
            var file = "../../../../testimages/185492.tiff";
            using var stream = File.OpenRead(file);
            var reader = new TiffReader(stream);
            Console.WriteLine($"Extracting from : {file}");
            var fileNo = 0;
            foreach (var image in reader.Read())
            {
                Console.WriteLine($"Width :{image.Width}, Height: ${image.Height}, Components: {image.ColorComponents} - size: {image.Width*image.Height*image.ColorComponents}");
                Console.WriteLine($"Output buffer : {image.RawData.Length}");
                using var outstream =  File.OpenWrite("../../../../testimages/out/odd_ifd"+(fileNo++)+".jpg");
                image.WriteToRgbJpeg(outstream);
            }
        }


    }
}