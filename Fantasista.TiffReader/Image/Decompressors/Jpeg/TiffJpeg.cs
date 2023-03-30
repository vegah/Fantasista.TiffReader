namespace Fantasista.TiffReader.Image.Decompressors.Jpeg
{
    /*
    Messy class for creating a valid jpeg image from the partly jpeg images stored in a tiff.
    It will 
    1) Add the quantizationtable if it is not set
    2) Add the huffmantable from the first section, if it is not set in the first part.     
    */
    public class TiffJpeg
    {
        private byte[]? _quantizationTable;
        private List<byte[]> _huffmanTables;

        private class MarkerPosition
        {
            public ushort Marker {get;set;}
            public long Position {get;set;}
        }

        public TiffJpeg(byte[]? quantizationTable)
        {
            _quantizationTable = quantizationTable;
            _huffmanTables = new List<byte[]>();
        }

        public byte[] GetTiffJpegAsNormalJpeg(byte[] content)
        {
            var allMarkers = FindAllInterestingMarkers(content);
            if (_quantizationTable!=null)
            {
                var allJpegTableMarkers = FindAllInterestingMarkers(_quantizationTable);
            }
            var quantizationTableMarker = allMarkers.FirstOrDefault(x=>x.Marker==0xFFDB);
            if (quantizationTableMarker==null && _quantizationTable!=null)
                content = Insert(allMarkers[1].Position,content,_quantizationTable.Skip(2).Take(_quantizationTable.Length-4));
            var huffmanTableMarkers = allMarkers.FirstOrDefault(x=>x.Marker==0xFFC4);
            if (huffmanTableMarkers==null)
            {
                foreach (var htable in _huffmanTables)
                    content = Insert(allMarkers[1].Position,content,htable);
            }

            return content;
        }

        private byte[] Insert(long position, byte[] originalContent,IEnumerable<byte> toInsert)
        {
            return originalContent.Take((int)position).Concat(toInsert).Concat(originalContent.Skip((int)position)).ToArray();
        }

        private List<MarkerPosition> FindAllInterestingMarkers(byte[] content)
        {
            using (var stream = new MemoryStream(content))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var first = ReadShort(reader);
                    if (first!=0xFFD8) throw new TiffJpegMergerException("Missing FFD8 marker");
                    return FindRestOfInterestingMarkers(reader);    
                }
            }
        }

        private List<MarkerPosition> FindRestOfInterestingMarkers(BinaryReader reader)
        {
            ushort current=0xFFD8; 
            var positions = new List<MarkerPosition>();
            while (current!=0xFFD9 && current!=0xFFDA)
            {
                var currentPos = reader.BaseStream.Position;
                var marker = ReadShort(reader);
                if (marker!=0xFFD9 && marker!=0xFFD0 && marker!=0xFF01)
                {
                    var length = ReadShort(reader);
                    reader.BaseStream.Seek(-4,SeekOrigin.Current);
                    var data = reader.ReadBytes(length+2);
                    if (marker==0xFFC4)
                    {
                        _huffmanTables.Add(data);
                    }
                    positions.Add(new MarkerPosition {Marker=marker,Position=currentPos});
                }
                current=marker;
            }
            return positions;
        }

        public ushort ReadShort(BinaryReader reader)
        {
            return BitConverter.ToUInt16(reader.ReadBytes(2).Reverse().ToArray());
        }
    }
}