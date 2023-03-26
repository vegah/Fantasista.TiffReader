namespace Fantasista.TiffReader.Exceptions
{
    public class MissingTagException : Exception
    {
        public MissingTagException(string message) : base(message)
        {}
    }
}