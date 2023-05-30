public class Parser
{
    private Lexer _lexer;
    private Token _currentToken;

    public Parser(Lexer lexer)
    {
        _lexer = lexer;
        _currentToken = _lexer.GetNextToken();
    }

    private void Eat(TokenType tokenType)
    {
        if (_currentToken.Type == tokenType)
        {
            _currentToken = _lexer.GetNextToken();
        }
        else
        {
            throw new Exception($"Unexpected token type {tokenType} but found {_currentToken.Type} instead.");
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

    public ASTNode Parse()
    {
        if (_currentToken.Type == TokenType.PRINT) 
        {
            Eat(TokenType.PRINT);
            ASTNode expression = LogicExpr();
            return new PrintNode(_currentToken, expression);
        }
        else 
        {
            return LogicExpr();
        }
    }

}
