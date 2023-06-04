public class Parser
{
    private List<Token> _tokens;
    private int _currentTokenIndex;
    private Token _currentToken;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _currentTokenIndex = 0;
        _currentToken = _tokens[_currentTokenIndex];
    }

    
    private void Eat(TokenType tokenType)
    {
        if (_currentToken.Type == tokenType)
        {
            _currentTokenIndex++;
            if (_currentTokenIndex < _tokens.Count)
            {
                _currentToken = _tokens[_currentTokenIndex];
            }
            else
            {
                _currentToken = new Token(TokenType.EOF, String.Empty, -1);
            }
        }
        else
        {
            throw new Exception($"Expected token type {tokenType}, but found {_currentToken.Type} instead at line {_currentToken.LineNumber}.");
        }
    }


    // the lower the following methods are in the file, the higher their precedence
    // factor is the highest precedence
    private ASTNode Factor()
    {
        Token token = _currentToken;
        if (token.Type == TokenType.INTEGER)
        {
            Eat(TokenType.INTEGER);
            return new Num(token);
        }
        else if (token.Type == TokenType.LPAREN)
        {
            Eat(TokenType.LPAREN);
            ASTNode node = LogicExpr();
            Eat(TokenType.RPAREN);
            return node;
        }
        else if (token.Type == TokenType.TRUE)
        {
            Eat(TokenType.TRUE);
            return new BoolNode(token);
        }
        else if (token.Type == TokenType.FALSE)
        {
            Eat(TokenType.FALSE);
            return new BoolNode(token);
        }
        else if (token.Type == TokenType.MINUS)
        {
            Eat(TokenType.MINUS);
            ASTNode node = new UnaryOp(token, Factor());
            return node;
        }
        else if (token.Type == TokenType.NOT)
        {
            Eat(TokenType.NOT);
            ASTNode node = new UnaryOp(token, Factor());
            return node;
        }
        else if (token.Type == TokenType.STRING)
        {
            Eat(TokenType.STRING);
            return new StringNode(token);
        }
        else if (token.Type == TokenType.IDENTIFIER)
        {
            Eat(TokenType.IDENTIFIER);
            if (_currentToken.Type == TokenType.LPAREN)
            {
                return FunctionCallStatement(token.Value, token);
            }
            else
            {
                return new VarNode(token);
            }
        }
        else if (token.Type == TokenType.INPUT)
        {
            return InputStatement();
        }
        else if (token.Type == TokenType.FUNCTION)
        {
            return FunctionStatement();
        }
        else
        {
            throw new Exception($"Unexpected token type {token.Type} at line {token.LineNumber}.");
        }
    }

    // term is the second highest precedence
    private ASTNode Term()
    {
        ASTNode node = Factor();

        while (_currentToken.Type == TokenType.MULTIPLY || _currentToken.Type == TokenType.DIVIDE)
        {
            Token token = _currentToken;
            if (token.Type == TokenType.MULTIPLY)
            {
                Eat(TokenType.MULTIPLY);
            }
            else if (token.Type == TokenType.DIVIDE)
            {
                Eat(TokenType.DIVIDE);
            }

            node = new BinOp(node, token, Factor());
        }
        return node;
    }

    // expr is the third highest precedence
    private ASTNode Expr()
    {
        ASTNode node = Term();

        while (_currentToken.Type == TokenType.PLUS || _currentToken.Type == TokenType.MINUS)
        {
            Token token = _currentToken;
            if (token.Type == TokenType.PLUS)
            {
                Eat(TokenType.PLUS);
            }
            else if (token.Type == TokenType.MINUS)
            {
                Eat(TokenType.MINUS);
            }
            node = new BinOp(node, token, Term());
        }
        return node;
    }

    // compexpr is the fourth highest precedence
    private ASTNode CompExpr()
    {
        ASTNode node = Expr();

        while (_currentToken.Type == TokenType.EQUAL || _currentToken.Type == TokenType.NOT_EQUAL ||
            _currentToken.Type == TokenType.LESS_THAN || _currentToken.Type == TokenType.GREATER_THAN ||
            _currentToken.Type == TokenType.LESS_THAN_OR_EQUAL || _currentToken.Type == TokenType.GREATER_THAN_OR_EQUAL)
        {
            Token token = _currentToken;

            if (token.Type == TokenType.EQUAL)
            {
                Eat(TokenType.EQUAL);
            }
            else if (token.Type == TokenType.NOT_EQUAL)
            {
                Eat(TokenType.NOT_EQUAL);
            }
            else if (token.Type == TokenType.LESS_THAN)
            {
                Eat(TokenType.LESS_THAN);
            }
            else if (token.Type == TokenType.GREATER_THAN)
            {
                Eat(TokenType.GREATER_THAN);
            }
            else if (token.Type == TokenType.LESS_THAN_OR_EQUAL)
            {
                Eat(TokenType.LESS_THAN_OR_EQUAL);
            }
            else if (token.Type == TokenType.GREATER_THAN_OR_EQUAL)
            {
                Eat(TokenType.GREATER_THAN_OR_EQUAL);
            }

            node = new CompOpNode(node, token, Expr());
        }

        return node;
    }

    // logicexpr is the fifth highest precedence
    private ASTNode LogicExpr()
    {
        ASTNode node = CompExpr();

        while (_currentToken.Type == TokenType.AND || _currentToken.Type == TokenType.OR)
        {
            Token token = _currentToken;
            if (token.Type == TokenType.AND)
            {
                Eat(TokenType.AND);
            }
            else if (token.Type == TokenType.OR)
            {
                Eat(TokenType.OR);
            }

            node = new LogicOpNode(node, token, CompExpr());
        }

        return node;
    }

    public List<ASTNode> Parse()
    {
        var statements = new List<ASTNode>();

        while (_currentToken.Type != TokenType.EOF)
        {
            if (_currentToken.Type == TokenType.PRINT)
            {
                statements.Add(PrintStatement());
            }
            else if (_currentToken.Type == TokenType.IF)
            {
                statements.Add(IfStatement());
            }
            else if (_currentToken.Type == TokenType.ELSE)
            {
                statements.Add(ElseStatement());
            }
            else if (_currentToken.Type == TokenType.WHILE)
            {
                statements.Add(WhileStatement());
            }
            else if (_currentToken.Type == TokenType.IDENTIFIER)
            {
                statements.Add(IdentifierStatement());
            }
            else if (_currentToken.Type == TokenType.INPUT)
            {
                statements.Add(InputStatement());
            }
            else if (_currentToken.Type == TokenType.INTEGER ||
                    _currentToken.Type == TokenType.LPAREN ||
                    _currentToken.Type == TokenType.MINUS ||
                    _currentToken.Type == TokenType.NOT ||
                    _currentToken.Type == TokenType.TRUE ||
                    _currentToken.Type == TokenType.FALSE ||
                    _currentToken.Type == TokenType.STRING)
            {
                statements.Add(LogicExpr());
            }
            else if (_currentToken.Type == TokenType.FUNCTION)
            {
                statements.Add(FunctionStatement());
            }
            else if (_currentToken.Type == TokenType.RETURN)
            {
                statements.Add(ReturnStatement());
            }
            else
            {
                throw new Exception($"Unexpected token type {_currentToken.Type} at line {_currentToken.LineNumber}.");
            }

            if (_currentToken.Type != TokenType.EOF)
            {
                Eat(TokenType.SEMICOLON);
            }
        }

        return statements;
    }

    // identifier statement is either a function call or a variable assignment
    private ASTNode IdentifierStatement()
    {
        Token token = _currentToken;
        string identifierName = _currentToken.Value;
        Eat(TokenType.IDENTIFIER);

        if (_currentToken.Type == TokenType.LPAREN)
        {
            return FunctionCallStatement(identifierName, token);
        }
        else if (_currentToken.Type == TokenType.ASSIGN)
        {
            Token assignToken = _currentToken;
            Eat(TokenType.ASSIGN);
            ASTNode right = LogicExpr();
            return new BinOp(new VarNode(token), assignToken, right);
        }
        else
        {
            return new VarNode(token);
        }
    }

    private ASTNode AssignmentStatement()
    {
        Token token = _currentToken;
        Eat(TokenType.IDENTIFIER);
        if (_currentToken.Type == TokenType.ASSIGN)
        {
            Token assignToken = _currentToken;
            Eat(TokenType.ASSIGN);
            ASTNode right = LogicExpr();
            return new BinOp(new VarNode(token), assignToken, right);
        }
        else
        {
            return new VarNode(token);
        }
    }

    private ASTNode PrintStatement()
    {
        Eat(TokenType.PRINT);
        var node = new PrintNode(_currentToken, LogicExpr());
        Eat(TokenType.RPAREN);
        return node;
    }

    private ASTNode InputStatement()
    {
        Token token = _currentToken;
        Eat(TokenType.INPUT);
        string prompt = _currentToken.Value;
        Eat(TokenType.STRING);
        Eat(TokenType.RPAREN);
        return new InputNode(token, prompt);
    }

    private ASTNode IfStatement()
    {
        Eat(TokenType.IF);
        Eat(TokenType.LPAREN);
        var condition = LogicExpr();
        Eat(TokenType.RPAREN);
        Eat(TokenType.LBRACE);
        var trueBlock = Statements();
        Eat(TokenType.RBRACE);
        ASTNode? falseBlock = null;
        if (_currentToken.Type == TokenType.ELIF)
        {
            falseBlock = ElseIfStatement();
        }
        else if (_currentToken.Type == TokenType.ELSE)
        {
            falseBlock = ElseStatement();
        }
        return new IfNode(condition, trueBlock, falseBlock, new Token(TokenType.IF, "if", _currentToken.LineNumber));
    }

    private ASTNode ElseIfStatement()
    {
        Eat(TokenType.ELIF);
        Eat(TokenType.LPAREN);
        var condition = LogicExpr();
        Eat(TokenType.RPAREN);
        Eat(TokenType.LBRACE);
        var trueBlock = Statements();
        Eat(TokenType.RBRACE);
        ASTNode? falseBlock = null;
        if (_currentToken.Type == TokenType.ELIF)
        {
            falseBlock = ElseIfStatement();
        }
        else if (_currentToken.Type == TokenType.ELSE)
        {
            falseBlock = ElseStatement();
        }
        return new IfNode(condition, trueBlock, falseBlock, new Token(TokenType.IF, "if", _currentToken.LineNumber));
    }

    private ASTNode ElseStatement()
    {
        Eat(TokenType.ELSE);
        Eat(TokenType.LBRACE);
        var statements = Statements();
        Eat(TokenType.RBRACE);
        return new ElseNode(statements, new Token(TokenType.ELSE, "else", _currentToken.LineNumber));
    }

    private ASTNode WhileStatement()
    {
        Eat(TokenType.WHILE);
        Eat(TokenType.LPAREN);
        var condition = LogicExpr();
        Eat(TokenType.RPAREN);
        Eat(TokenType.LBRACE);
        var block = Statements();
        Eat(TokenType.RBRACE);
        return new WhileNode(condition, block, new Token(TokenType.WHILE, "while", _currentToken.LineNumber));
    }

    // function is a keyword followed by an identifier, a list of parameters surrounded by parentheses, and a block
    private ASTNode FunctionStatement()
    {
        Eat(TokenType.FUNCTION);
        string name = _currentToken.Value;
        Eat(TokenType.IDENTIFIER);
        Eat(TokenType.LPAREN);
        var parameters = new List<string>();
        while (_currentToken.Type != TokenType.RPAREN)
        {
            parameters.Add(_currentToken.Value);
            Eat(TokenType.IDENTIFIER);
            if (_currentToken.Type == TokenType.COMMA)
            {
                Eat(TokenType.COMMA);
            }
        }
        Eat(TokenType.RPAREN);
        Eat(TokenType.LBRACE);
        var block = Statements();
        Eat(TokenType.RBRACE);
        return new FunctionNode(new Token(TokenType.FUNCTION, name, _currentToken.LineNumber), name, parameters, block);
    }

    // function call is an identifier followed by a list of arguments surrounded by parentheses
    private ASTNode FunctionCallStatement(string functionName, Token token)
    {
        Eat(TokenType.LPAREN);
        var arguments = new List<ASTNode>();
        while (_currentToken.Type != TokenType.RPAREN)
        {
            arguments.Add(LogicExpr());
            if (_currentToken.Type == TokenType.COMMA)
            {
                Eat(TokenType.COMMA);
            }
        }
        Eat(TokenType.RPAREN);
        return new FunctionCallNode(token, functionName, arguments);
    }

    private ASTNode ReturnStatement()
    {
        Eat(TokenType.RETURN);
        var node = new ReturnNode(_currentToken, LogicExpr());
        Eat(TokenType.SEMICOLON);
        return node;
    }

    // block is a list of statements surrounded by curly braces
    private BlockNode Statements()
    {
        var statements = new List<ASTNode>();

        while (_currentToken.Type != TokenType.RBRACE && _currentToken.Type != TokenType.EOF)
        {
            if (_currentToken.Type == TokenType.PRINT)
            {
                statements.Add(PrintStatement());
                if (_currentToken.Type == TokenType.SEMICOLON)
                {
                    Eat(TokenType.SEMICOLON);
                }
            }
            else if (_currentToken.Type == TokenType.IF)
            {
                statements.Add(IfStatement());
            }
            else if (_currentToken.Type == TokenType.WHILE)
            {
                statements.Add(WhileStatement());
            }
            else if (_currentToken.Type == TokenType.IDENTIFIER)
            {
                statements.Add(AssignmentStatement());
                if (_currentToken.Type == TokenType.SEMICOLON)
                {
                    Eat(TokenType.SEMICOLON);
                }
            }
            else if (_currentToken.Type == TokenType.RETURN)
            {
                statements.Add(ReturnStatement());
            }
            else
            {
                statements.Add(LogicExpr());
                if (_currentToken.Type == TokenType.SEMICOLON)
                {
                    Eat(TokenType.SEMICOLON);
                }
            }
        }
        int lineNumber = statements.Any() ? statements.First().Token.LineNumber : -1;
        return new BlockNode(new Token(TokenType.BLOCK, "block", lineNumber), statements);
    }

}
