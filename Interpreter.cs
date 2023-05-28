public class Interpreter
{
    private Parser _parser;

    public Interpreter(Parser parser)
    {
        _parser = parser;
    }

    private dynamic Visit(ASTNode node)
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
        else if (node is BoolNode)
        {
            return Visit((BoolNode)node);
        }
        else if (node is CompOpNode)
        {
            return Visit((CompOpNode)node);
        }
        else if (node is LogicOpNode)
        {
            return Visit((LogicOpNode)node);
        }
        else if (node is StringNode)
        {
            return Visit((StringNode)node);
        }
        else if (node is ConcatOp)
        {
            return Visit((ConcatOp)node);
        }
        else
        {
            throw new Exception($"Unexpected node type {node.GetType()}.");
        }
    }

    private string Visit(StringNode node)
    {
        return node.Value;
    }

    private string Visit(ConcatOp node)
    {
        return Visit(node.Left) + Visit(node.Right);
    }


    private double Visit(Num node)
    {
        return double.Parse(node.Token.Value);
    }

    private bool Visit(BoolNode node)
    {
        return node.Value;
    }

    private dynamic Visit(UnaryOp node)
    {
        if (node.Token.Type == TokenType.PLUS)
        {
            return +Visit(node.Expr);
        }
        else if (node.Token.Type == TokenType.MINUS)
        {
            return -Visit(node.Expr);
        }
        else if (node.Token.Type == TokenType.NOT)
        {
            return !Visit(node.Expr);
        }
        else
        {
            throw new Exception($"Unexpected token type {node.Token.Type}.");
        }
    }

    private dynamic Visit(BinOp node)
    {
        if (node.Token.Type == TokenType.PLUS)
        {
            var left = Visit(node.Left);
            var right = Visit(node.Right);

            if(left is double && right is double)
                return (double)left + (double)right;
            else if(left is string && right is string)
                return (string)left + (string)right;
            else
                throw new Exception("Type mismatch in addition operation");
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

    private bool Visit(CompOpNode node)
    {
        if (node.Token.Type == TokenType.EQUAL)
        {
            return Visit(node.Left) == Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.NOT_EQUAL)
        {
            return Visit(node.Left) != Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.LESS_THAN)
        {
            return Visit(node.Left) < Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.LESS_THAN_OR_EQUAL)
        {
            return Visit(node.Left) <= Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.GREATER_THAN)
        {
            return Visit(node.Left) > Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.GREATER_THAN_OR_EQUAL)
        {
            return Visit(node.Left) >= Visit(node.Right);
        }
        else
        {
            throw new Exception($"Unexpected token type {node.Token.Type}.");
        }
    }

    private bool Visit(LogicOpNode node)
    {
        if (node.Token.Type == TokenType.AND)
        {
            return Visit(node.Left) && Visit(node.Right);
        }
        else if (node.Token.Type == TokenType.OR)
        {
            return Visit(node.Left) || Visit(node.Right);
        }
        else
        {
            throw new Exception($"Unexpected token type {node.Token.Type}.");
        }
    }

    public dynamic Interpret()
    {
        var tree = _parser.Parse();
        return Visit(tree);
    }
}
