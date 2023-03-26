using System.Runtime.Serialization;

namespace Fantasista.TiffReader.Segments.Fields
{
    [Serializable]
    internal class FieldValueException : Exception
    {

        public FieldValueException(string? message) : base(message)
        {
        }

    }
}