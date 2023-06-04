public class TokenType
{   //define token types
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
    public static readonly TokenType IDENTIFIER = new TokenType("IDENTIFIER");
    public static readonly TokenType ASSIGN = new TokenType("ASSIGN");
    public static readonly TokenType PRINT = new TokenType("PRINT");
    public static readonly TokenType INPUT = new TokenType("INPUT");
    public static readonly TokenType IF = new TokenType("IF");
    public static readonly TokenType ELIF = new TokenType("ELIF");
    public static readonly TokenType ELSE = new TokenType("ELSE");
    public static readonly TokenType WHILE = new TokenType("WHILE");
    public static readonly TokenType LBRACE = new TokenType("LBRACE");
    public static readonly TokenType RBRACE = new TokenType("RBRACE");
    public static readonly TokenType BLOCK = new TokenType("BLOCK");
    public static readonly TokenType NEWLINE = new TokenType("NEWLINE");
    public static readonly TokenType SEMICOLON = new TokenType("SEMICOLON");
    public static readonly TokenType FUNCTION = new TokenType("FUNCTION");
    public static readonly TokenType COMMA = new TokenType("COMMA");
    public static readonly TokenType RETURN = new TokenType("RETURN");
    public string Name { get; set; }

    //initialize the token type
    private TokenType(string name)
    {
        Name = name;
    }
    //override ToString() to return the token type name
    public override string ToString()
    {
        return Name;
    }
}