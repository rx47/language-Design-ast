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
