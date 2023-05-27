public class TokenType
{
    public static readonly TokenType NUMBER = new TokenType("NUMBER");
    public static readonly TokenType PLUS = new TokenType("PLUS");
    public static readonly TokenType MINUS = new TokenType("MINUS");
    public static readonly TokenType MULTIPLY = new TokenType("MULTIPLY");
    public static readonly TokenType DIVIDE = new TokenType("DIVIDE");
    public static readonly TokenType INTEGER = new TokenType("INTEGER");
    public static readonly TokenType LPAREN = new TokenType("LPAREN");
    public static readonly TokenType RPAREN = new TokenType("RPAREN");
    public static readonly TokenType EOF = new TokenType("EOF");

    public string Name { get; set; }

    private TokenType(string name)
    {
        Name = name;
    }
}