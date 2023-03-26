namespace Fantasista.TiffReader.Segments.Fields
{
    public class RationalNumber
    {
        private uint _numerator;
        private uint _denominator;

        public RationalNumber(uint numerator, uint denominator)
        {
            _numerator = numerator;
            _denominator = denominator;
        }

        public override string ToString()
        {
            return $"{_numerator}/{_denominator} ({_numerator/_denominator})";
        }
    }
}