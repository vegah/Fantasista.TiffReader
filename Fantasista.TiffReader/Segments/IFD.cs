using Fantasista.TiffReader.Segments.Fields;

namespace Fantasista.TiffReader.Segments
{
    public class IFD
    {
        private TiffBinaryReader _reader;
        private uint _offset;
        private ushort _entries;
        private Dictionary<Tag,Field> _fields;

        public IFD(TiffBinaryReader reader,uint offset)
        {
            _reader = reader;
            _offset = offset;
            _fields = new Dictionary<Tag, Field>();
        }

        public uint NextOffset { get; private set; }

        public void ReadIFD()
        {
            _reader.Seek(_offset);
            _entries = _reader.ReadUInt16();
            for (var x=0;x<_entries;x++)
            {
                ReadIFDEntry();
            }
            NextOffset = _reader.ReadUInt32();
        }

        private void ReadIFDEntry()
        {
            var tag = _reader.ReadUInt16();
            var fieldType = _reader.ReadUInt16();
            var field = Field.CreateField(tag,(FieldType)fieldType,_reader);
            _fields.Add(field.Tag,field);
            Console.WriteLine($"{field} at pos {_reader.Position}");
        }

        public Boolean Contains(Tag tag) => _fields.ContainsKey(tag);

        public Field this[Tag tag] => _fields[tag];
    }
}