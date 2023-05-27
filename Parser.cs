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
            throw new Exception($"Expected token type {tokenType} but found {_currentToken.Type} instead.");
        }
    }

    private ASTNode Factor()
    {
        var token = _currentToken;
        if (token.Type == TokenType.PLUS)
        {
            Eat(TokenType.PLUS);
            return new UnaryOp(token, Factor());
        }
        else if (token.Type == TokenType.MINUS)
        {
            Eat(TokenType.MINUS);
            return new UnaryOp(token, Factor());
        }
        else if (token.Type == TokenType.INTEGER)
        {
            Eat(TokenType.INTEGER);
            return new Num(token);
        }
        else if (token.Type == TokenType.LPAREN)
        {
            Eat(TokenType.LPAREN);
            var node = Expr();
            Eat(TokenType.RPAREN);
            return node;
        }
        else
        {
            throw new Exception($"Unexpected token type {token.Type}.");
        }
    }


    private ASTNode Term()
    {
        var node = Factor();

        while (_currentToken.Type == TokenType.MULTIPLY || _currentToken.Type == TokenType.DIVIDE)
        {
            var token = _currentToken;
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

    public ASTNode Expr()
    {
        var node = Term();

        while (_currentToken.Type == TokenType.PLUS || _currentToken.Type == TokenType.MINUS)
        {
            var token = _currentToken;
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



    public ASTNode Parse()
    {
        return Expr();
    }
}