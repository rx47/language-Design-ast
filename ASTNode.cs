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

public class StringNode : ASTNode
{
    public string Value { get; set; }
    public StringNode(Token token) : base(token)
    {
        Value = token.Value;
    }
}

public class ConcatOp : ASTNode
{
    public ASTNode Left { get; set; }
    public ASTNode Right { get; set; }
    public ConcatOp(ASTNode left, Token op, ASTNode right) : base(op)
    {
        Left = left;
        Right = right;
    }
}

public class VarNode : ASTNode
{
    public string Value { get; set; }
    public VarNode(Token token) : base(token)
    {
        Value = token.Value;
    }
}

public class PrintNode : ASTNode
{
    public ASTNode Expression { get; set; }
    public PrintNode(Token token, ASTNode expression) : base(token)
    {
        Expression = expression;
    }
}

public class InputNode : ASTNode
{
    public string Prompt { get; set; }

    public InputNode(Token token, string prompt) : base(token)
    {
        Prompt = prompt;
    }
}

public class IfNode : ASTNode
{
    public ASTNode Condition { get; set; }
    public ASTNode TrueBlock { get; set; }
    public ASTNode? FalseBlock { get; set; }

    public IfNode(ASTNode condition, ASTNode trueBlock, ASTNode? falseBlock, Token token) : base(token)
    {
        Condition = condition;
        TrueBlock = trueBlock;
        FalseBlock = falseBlock;
    }
}

public class ElseIfNode : ASTNode
{
    public ASTNode Condition { get; set; }
    public ASTNode Block { get; set; }

    public ElseIfNode(ASTNode condition, ASTNode block, Token token) : base(token)
    {
        Condition = condition;
        Block = block;
    }
}

public class ElseNode : ASTNode
{
    public ASTNode Block { get; set; }

    public ElseNode(ASTNode block, Token token) : base(token)
    {
        Block = block;
    }
}

public class BlockNode : ASTNode
{
    public List<ASTNode> Statements { get; set; }

    public BlockNode(Token token, List<ASTNode> statements) : base(token)
    {
        Statements = statements;
    }
}

public class WhileNode : ASTNode
{
    public ASTNode Condition { get; set; }
    public ASTNode Block { get; set; }

    public WhileNode(ASTNode condition, ASTNode block, Token token) : base(token)
    {
        Condition = condition;
        Block = block;
    }
}

public class AssignNode : ASTNode
{
    public string VarName { get; set; }
    public ASTNode Value { get; set; }

    public AssignNode(Token token, string varName, ASTNode value) : base(token)
    {
        VarName = varName;
        Value = value;
    }
}

public class FunctionNode : ASTNode
{
    public string Name { get; set; }
    public List<string> Parameters { get; set; }
    public ASTNode Block { get; set; }

    public FunctionNode(Token token, string name, List<string> parameters, ASTNode block) : base(token)
    {
        Name = name;
        Parameters = parameters;
        Block = block;
    }
}

public class FunctionCallNode : ASTNode
{
    public string Name { get; set; }
    public List<ASTNode> Arguments { get; set; }

    public FunctionCallNode(Token token, string name, List<ASTNode> arguments) : base(token)
    {
        Name = name;
        Arguments = arguments;
    }
}