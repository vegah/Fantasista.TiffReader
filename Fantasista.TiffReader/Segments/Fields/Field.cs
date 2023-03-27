namespace Fantasista.TiffReader.Segments.Fields
{
    public abstract class Field
    {
        protected Tag _tag;
        protected TiffBinaryReader _reader;
        protected FieldType _type;
        protected ushort _tagNumber;
        protected FieldValue _value;

        private static Dictionary<Tag,Func<Tag,FieldType,TiffBinaryReader,Field>> _fieldsMap = new Dictionary<Tag, Func<Tag, FieldType, TiffBinaryReader,Field>>()
        {
            {Tag.ImageLength,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.ImageWidth,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.Compression,(a,b,c)=>new CompressionField(a,b,c)},
            {Tag.PhotometricInterpretation,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.ResolutionUnit,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.XResolution,(a,b,c)=>new SingleRationalNumberField(a,b,c)},
            {Tag.YResolution,(a,b,c)=>new SingleRationalNumberField(a,b,c)},
            {Tag.StripOffsets,(a,b,c)=>new ShortOrLongArrayField(a,b,c)},
            {Tag.RowsPerStrip,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.StripByteCounts,(a,b,c)=>new ShortOrLongArrayField(a,b,c)},
            {Tag.BitsPerSample,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.PlanarConfiguration,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.SamplesPerPixel,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.ImageDescription,(a,b,c)=>new StringValueField(a,b,c)},
            {Tag.JPEGTables,(a,b,c)=>new ByteArrayField(a,b,c)},
            {Tag.Orientation,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.FillOrder,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.ExtraSamples,(a,b,c)=>new ShortOrLongArrayField(a,b,c)},
            {Tag.NewSubfileType,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.PageNumber,(a,b,c)=>new SingleShortOrLongValueField(a,b,c)},
            {Tag.WhitePoint,(a,b,c)=>new SingleRationalNumberField(a,b,c)},




        };


        protected Field(Tag tag, FieldType type, TiffBinaryReader reader)
        {
            _tag = tag;
            _reader = reader; 
            _type = type;
            _tagNumber = (ushort)tag;
            _value = reader.ReadFieldValue();
        }

        public Tag Tag => _tag;

        public static Field CreateField(ushort tag, FieldType type, TiffBinaryReader reader)
        {
            var etag = Tag.Unknown;
            if (Enum.IsDefined(typeof(Tag),tag))
                etag = (Tag)tag;                        
            return _fieldsMap.ContainsKey(etag) 
                ? _fieldsMap[etag](etag,type,reader) 
                : new UnknownField(etag,type,reader,tag); 
        }

        public override string ToString()
        {
            return $"Field: {_tag} - {_tagNumber}";
        }
    }
}