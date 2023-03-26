using System;
using System.IO;
using Fantasista.TiffReader.Segments;
using Fantasista.TiffReader.Segments.Exceptions;
using Xunit;

namespace Fantasista.TiffReader.Tests.SegmentsTests
{
    public class HeaderTests
    {
        [Fact]
        public void Header_ItAcceptsValidLittleEndianByteOrder()
        {
            var littleEndianStream = new MemoryStream(new byte[] {0x49, 0x49,42,0,14,0,0,0});
            var reader = new TiffBinaryReader(littleEndianStream);
            var header = new Header(reader);
            header.Read();
            Assert.Equal(14u,header.FirstIFDOffset);
        }

        [Fact]
        public void Header_ItAcceptsValidBigEndianByteOrder()
        {
            var littleEndianStream = new MemoryStream(new byte[] {0x4d, 0x4d,00,42,0,0,0,14});
            var reader = new TiffBinaryReader(littleEndianStream);
            var header = new Header(reader);
            header.Read();
            Assert.Equal(14u,header.FirstIFDOffset);
        }

        [Fact]
        public void Header_ItGivesExceptionWhenNonValidByteOrder()
        {
            var littleEndianStream = new MemoryStream(new byte[] {0x2d, 0x2d,42,00,0,0,0,14});
            var reader = new TiffBinaryReader(littleEndianStream);
            var header = new Header(reader);
            Assert.Throws<HeaderException>(()=>header.Read());
        }


    }
}