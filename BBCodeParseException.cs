namespace bbsharp;

public class BBCodeParseException : Exception
{
    internal BBCodeParseException(string Message, int Position) : base(Message + " at position " + Position)
    {
        this.Position = Position;
    }

    /// <summary>
    ///     The position of the character which caused the parse error
    /// </summary>
    public int Position { get; internal set; }
}