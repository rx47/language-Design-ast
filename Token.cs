public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
    public int LineNumber { get; }

    public Token(TokenType type, string value, int lineNumber)
    {
        Type = type;
        Value = value;
        LineNumber = lineNumber;
    }

    public override string ToString()
    {
        return $"Type: line: {LineNumber}), {Type}, Value: {Value}";
    }

}