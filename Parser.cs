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
                _currentToken = new Token(TokenType.EOF, String.Empty);
            }
        }
        else
        {
            throw new Exception($"Expected token type {tokenType}, but found {_currentToken.Type} instead.");
        }
    }

    // the lower the following methods are in the file, the higher their precedence
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
            return new VarNode(token);
        }
        else if (token.Type == TokenType.INPUT)
        {
            Eat(TokenType.INPUT);
            string prompt = _currentToken.Value;
            Eat(TokenType.STRING);
            return new InputNode(token, prompt);
        }

        else
        {
            throw new Exception($"Unexpected token type {token.Type}.");
        }
    }

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

    public ASTNode? Parse()
    {
        ASTNode? node;
        if (_currentToken.Type == TokenType.PRINT)
        {
            node = PrintStatement();
        }
        else if (_currentToken.Type == TokenType.IF)
        {
            node = IfStatement();
        }
        else if (_currentToken.Type == TokenType.ELSE)
        {
            node = ElseStatement();
        }
        else if (_currentToken.Type == TokenType.WHILE)
        {
            node = WhileStatement();
        }
        else if (_currentToken.Type == TokenType.IDENTIFIER)
        {
            node = AssignmentStatement();
        }
        else if (_currentToken.Type == TokenType.EOF)
        {
            // Handle EOF, for example by returning a special EOF node or null.
            node = null;
        }
        else
        {
            node = LogicExpr();
        }
        return node;
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
        return node;
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
        if (_currentToken.Type == TokenType.ELSE)
        {
            falseBlock = ElseStatement();
        }
        
        return new IfNode(condition, trueBlock, falseBlock, new Token(TokenType.IF, "if"));
    }


    private ASTNode ElseStatement()
    {
        Eat(TokenType.ELSE);
        Eat(TokenType.LBRACE);
        var statements = Statements();
        Eat(TokenType.RBRACE);
        return new ElseNode(statements, new Token(TokenType.ELSE, "else"));
    }

    private BlockNode Statements()
    {
        var statements = new List<ASTNode>();

        while (_currentToken.Type != TokenType.RBRACE && _currentToken.Type != TokenType.EOF)
        {
            if (_currentToken.Type == TokenType.PRINT)
            {
                statements.Add(PrintStatement());
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
            }
            if (_currentToken.Type == TokenType.SEMICOLON)
            {
                Eat(TokenType.SEMICOLON);
            }
        }
        return new BlockNode(new Token(TokenType.BLOCK, "block"), statements);
    }
    

    private ASTNode WhileStatement()
    {
        Eat(TokenType.WHILE);
        Eat(TokenType.LPAREN);
        var condition = LogicExpr();
        Eat(TokenType.RPAREN);

        Eat(TokenType.LBRACE);
        var trueBlock = Statements();
        Eat(TokenType.RBRACE);

        return new WhileNode(condition, trueBlock, new Token(TokenType.WHILE, "while"));
    }

}
