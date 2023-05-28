public class Interpreter
{
    private Parser _parser;
    private Dictionary<string, dynamic> _variables = new Dictionary<string, dynamic>();

    public Interpreter(Parser parser)
    {
        _parser = parser;
    }

    public Parser Parser 
    { 
        get { return _parser; } 
        set { _parser = value; } 
    }

    public void Visit(PrintNode node)
    {
        // Evaluate the expression and convert the result to a string.
        var value = Visit(node.Expression).ToString();

        // Print the value.
        Console.WriteLine(value);
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
        else if (node is VarNode)
        {
            return Visit((VarNode)node);
        }
        else if (node is PrintNode)
        {
            Visit((PrintNode)node);
            return "";
            //TODO: find a fix for return null have replaced it with return "" but that adds a new line
        }
        else
        {
            throw new Exception($"Unexpected node type {node.GetType()}.");
        }
    }

    public dynamic Visit(VarNode node)
    {
        string varName = node.Token.Value;
        if (!_variables.ContainsKey(varName))
        {
            throw new Exception($"Variable {varName} not defined.");
        }
        return _variables[varName];
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
        else if (node.Token.Type == TokenType.ASSIGN)
        {
            var varName = ((VarNode)node.Left).Token.Value;
            var value = Visit(node.Right);
            _variables[varName] = value;
            return value;
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
