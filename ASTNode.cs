public abstract class ASTNode
{
    public Token Token { get; set; }

    protected ASTNode(Token token)
    {
        Token = token;
    }
}

public class BinOp : ASTNode
{
    public ASTNode Left { get; set; }
    public ASTNode Right { get; set; }

    public BinOp(ASTNode left, Token op, ASTNode right) : base(op)
    {
        Left = left;
        Right = right;
    }
}

public class UnaryOp : ASTNode
{
    public ASTNode Expr { get; set; }

    public UnaryOp(Token op, ASTNode expr) : base(op)
    {
        Expr = expr;
    }
}

public class Num : ASTNode
{
    public Num(Token token) : base(token)
    {
    }
}

public class BoolNode : ASTNode
{
    public bool Value { get; set; }

    public BoolNode(Token token) : base(token)
    {
        Value = token.Type == TokenType.TRUE;
    }
}

public class CompOpNode : ASTNode
{
    public ASTNode Left { get; set; }
    public ASTNode Right { get; set; }

    public CompOpNode(ASTNode left, Token op, ASTNode right) : base(op)
    {
        Left = left;
        Right = right;
    }
}

public class LogicOpNode : ASTNode
{
    public ASTNode Left { get; set; }
    public ASTNode Right { get; set; }

    public LogicOpNode(ASTNode left, Token op, ASTNode right) : base(op)
    {
        Left = left;
        Right = right;
    }
}
