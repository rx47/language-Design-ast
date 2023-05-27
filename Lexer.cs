public class Lexer
{
    private string _input;
    private int _position;
    private char _currentChar;

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
        _currentChar = _input[_position];
    }

    private void Advance()
    {
        _position++;
        if (_position > _input.Length - 1)
        {
            _currentChar = '\0';
        }
        else
        {
            _currentChar = _input[_position];
        }
    }

    private void SkipWhitespace()
    {
        while (_currentChar != '\0' && char.IsWhiteSpace(_currentChar))
        {
            Advance();
        }
    }

    private double Number()
    {
        var result = "";
        while (_currentChar != '\0' && char.IsDigit(_currentChar))
        {
            result += _currentChar;
            Advance();
        }
        if (_currentChar == '.')
        {
            result += _currentChar;
            Advance();
            while (_currentChar != '\0' && char.IsDigit(_currentChar))
            {
                result += _currentChar;
                Advance();
            }
        }
        return double.Parse(result);
    }

    public Token GetNextToken()
    {
        while (_currentChar != '\0')
        {
            if (char.IsWhiteSpace(_currentChar))
            {
                SkipWhitespace();
                continue;
            }

            if (char.IsDigit(_currentChar))
            {
                return new Token(TokenType.INTEGER, Number().ToString());
            }

            if (_currentChar == '+')
            {
                Advance();
                return new Token(TokenType.PLUS, "+");
            }

            if (_currentChar == '-')
            {
                Advance();
                return new Token(TokenType.MINUS, "-");
            }

            if (_currentChar == '*')
            {
                Advance();
                return new Token(TokenType.MULTIPLY, "*");
            }

            if (_currentChar == '/')
            {
                Advance();
                return new Token(TokenType.DIVIDE, "/");
            }

            if (_currentChar == '(')
            {
                Advance();
                return new Token(TokenType.LPAREN, "(");
            }

            if (_currentChar == ')')
            {
                Advance();
                return new Token(TokenType.RPAREN, ")");
            }

            throw new Exception("Error parsing input");
        }

        return new Token(TokenType.EOF, string.Empty);
    }
}
