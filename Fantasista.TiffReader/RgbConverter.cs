namespace Fantasista.TiffReader
{
    public class RgbConverter
    {
        private byte[] _rawData;
        private readonly int _width;
        private readonly int _height;
        private int _components;

        public RgbConverter(byte[] rawData, int width, int height, int components )
        {
            _rawData = rawData;
            _width = width;
            _height = height;
            _components = components;
        }

        public byte[] Convert()
        {
            if (_components==1)
                return Convert1Component();
            if (_components==2)
                return Convert2Components();
            if (_components==4)
                return Convert4Components();
            if (_components==3)
                return _rawData;
            else 
                throw new Exception("Not recognizable number of components");
        }

        private byte[] Convert4Components()
        {
            var output = new byte[_width*_height*3];
            var index = 0;
            for (var x=0;x<_rawData.Length;x++)
            {
                var oneIndex = x+1;
                if (x%4!=0)
                    output[index++]=_rawData[x];
            }
            return output;
        }

        private byte[] Convert2Components()
        {
            var output = new byte[_width*_height*3];
            var index = 0;
            for (var x=0;x<_rawData.Length;x+=2)
            {
                var arr = new [] {_rawData[x+1], _rawData[x]};
                var rgb16 = BitConverter.ToUInt16(arr);
                var r = rgb16>>8;
                output[index++]=(byte)r;
                output[index++]=(byte)r;
                output[index++]=(byte)r;
            }
            return output;
        }

        private byte[] Convert1Component()
        {
            var output = new byte[_width*_height*3];
            for (var x=0;x<_rawData.Length;x++)
            {
                output[x*3]=_rawData[x];
                output[x*3+1]=_rawData[x];
                output[x*3+2]=_rawData[x];
            }
            return output;
        }
    }
}