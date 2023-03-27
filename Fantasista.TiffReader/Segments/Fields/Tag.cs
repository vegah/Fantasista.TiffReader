namespace Fantasista.TiffReader.Segments.Fields
{
    public enum Tag : ushort
    {
        NewSubfileType = 254,
        ImageWidth=256,

        BitsPerSample = 258,

        ImageLength=257,
        Compression=259,
        PhotometricInterpretation=262,
        FillOrder = 266,
        ImageDescription = 270,
        Orientation = 274,
        StripOffsets=273,
        SamplesPerPixel = 277,
        RowsPerStrip=278,

        StripByteCounts=279,

        PlanarConfiguration = 284, 

        XResolution=282,
        YResolution=283,

        ResolutionUnit=296,
        PageNumber=297,
        WhitePoint=318,
        PrimaryChromaticities=319,
        ExtraSamples=338,
        JPEGTables=347,

        Unknown = 0xffff
    }
}