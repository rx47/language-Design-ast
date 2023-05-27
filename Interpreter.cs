public class Interpreter
{
    private Parser _parser;

    public Interpreter(Parser parser)
    {
        _parser = parser;
    }

    private double Visit(ASTNode node)
    {
        if (node is Num)
        {
            return Visit((Num)node);
        }
        else if (node is BinOp)
        {
            return Visit((BinOp)node);
        }
        else if (node is UnaryOp)
        {
            return Visit((UnaryOp)node);
        }
        else
        {
            throw new Exception($"Unexpected node type {node.GetType()}.");
        }
    }

    private double Visit(Num node)
    {
        return double.Parse(node.Token.Value);
    }

    private double Visit(UnaryOp node)
    {
        if (node.Token.Type == TokenType.PLUS)
        {
            return +Visit(node.Expr);
        }
        else if (node.Token.Type == TokenType.MINUS)
        {
            return -Visit(node.Expr);
        }
        else
        {
            throw new Exception($"Unexpected token type {node.Token.Type}.");
        }
    }

    private double Visit(BinOp node)
    {
        if (node.Token.Type == TokenType.PLUS)
        {
            return Visit(node.Left) + Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.MINUS)
        {
            return Visit(node.Left) - Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.MULTIPLY)
        {
            return Visit(node.Left) * Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.DIVIDE)
        {
            return Visit(node.Left) / Visit(node.Right);
        }
        else
        {
            throw new Exception($"Unexpected token type {node.Token.Type}.");
        }
    }

    public double Interpret()
    {
        var tree = _parser.Parse();
        return Visit(tree);
    }
}
