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
    public static readonly TokenType TRUE = new TokenType("TRUE");
    public static readonly TokenType FALSE = new TokenType("FALSE");
    public static readonly TokenType EQUAL = new TokenType("EQUAL");
    public static readonly TokenType NOT_EQUAL = new TokenType("NOT_EQUAL");
    public static readonly TokenType LESS_THAN = new TokenType("LESS_THAN");
    public static readonly TokenType LESS_THAN_OR_EQUAL = new TokenType("LESS_THAN_OR_EQUAL");
    public static readonly TokenType GREATER_THAN = new TokenType("GREATER_THAN");
    public static readonly TokenType GREATER_THAN_OR_EQUAL = new TokenType("GREATER_THAN_OR_EQUAL");
    public static readonly TokenType AND = new TokenType("AND");
    public static readonly TokenType OR = new TokenType("OR");
    public static readonly TokenType NOT = new TokenType("NOT");
    public static readonly TokenType STRING = new TokenType("STRING");

    public string Name { get; set; }

    private TokenType(string name)
    {
        Name = name;
    }
}