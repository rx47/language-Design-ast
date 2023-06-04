public class Lexer
{
    private string _input;
    private int _position;
    private char _currentChar;
    private int _lineNumber;

    //initialize the lexer
    public Lexer(string input)
    {
        _input = input;
        _position = 0;
        _lineNumber = 1;
        _currentChar = _input.Length > 0 ? _input[_position] : '\0';
    }

    // skip whitespace
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
            if (_currentChar == '\n')
            {
                _lineNumber++;
            }
        }
    }

    // look at the next character without advancing
    private char Peek()
    {
        int peekPosition = _position + 1;
        if (peekPosition > _input.Length - 1)
        {
            return '\0';
        }
        else
        {
            return _input[peekPosition];
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

    private string String()
    {
        Advance();
        var result = "";
        while (_currentChar != '\0' && _currentChar != '"')
        {
            result += _currentChar;
            Advance();
        }
        Advance();
        return result;
    }

    private string Identifier()
    {
        var result = "";
        while (_currentChar != '\0' && (char.IsLetterOrDigit(_currentChar) || _currentChar == '_'))
        {
            result += _currentChar;
            Advance();
        }
        return result;
    }

    public List<Token> GetAllTokens()
    {
        List<Token> tokens = new List<Token>();
        Token token = GetNextToken();
        while (token.Type != TokenType.EOF)
        {
            tokens.Add(token);
            token = GetNextToken();
        }
        tokens.Add(new Token(TokenType.EOF, "", _lineNumber));
        return tokens;
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
                return new Token(TokenType.INTEGER, Number().ToString(), _lineNumber);
            }

            if (_currentChar == '+')
            {
                Advance();
                return new Token(TokenType.PLUS, "+", _lineNumber);
            }

            if (_currentChar == '-')
            {
                Advance();
                return new Token(TokenType.MINUS, "-", _lineNumber);
            }

            if (_currentChar == '*')
            {
                Advance();
                return new Token(TokenType.MULTIPLY, "*", _lineNumber);
            }

            if (_currentChar == '/')
            {
                Advance();
                return new Token(TokenType.DIVIDE, "/", _lineNumber);
            }

            if (_currentChar == '(')
            {
                Advance();
                return new Token(TokenType.LPAREN, "(", _lineNumber);
            }

            if (_currentChar == ')')
            {
                Advance();
                return new Token(TokenType.RPAREN, ")", _lineNumber);
            }

            if (_currentChar == '=')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.EQUAL, "==", _lineNumber);
                }
                else
                {
                    Advance();
                    return new Token(TokenType.ASSIGN, "=", _lineNumber);
                }
            }

            if (_currentChar == '!')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.NOT_EQUAL, "!=", _lineNumber);
                }
                else
                {
                    Advance();
                    return new Token(TokenType.NOT, "!", _lineNumber);
                }
            }

            if (_currentChar == '<')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.LESS_THAN_OR_EQUAL, "<=", _lineNumber);
                }
                else
                {
                    Advance();
                    return new Token(TokenType.LESS_THAN, "<", _lineNumber);
                }
            }

            if (_currentChar == '>')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.GREATER_THAN_OR_EQUAL, ">=", _lineNumber);
                }
                else
                {
                    Advance();
                    return new Token(TokenType.GREATER_THAN, ">", _lineNumber);
                }
            }

            if (_currentChar == '&')
            {
                if (Peek() == '&')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.AND, "&&", _lineNumber);
                }
            }

            if (_currentChar == '|')
            {
                if (Peek() == '|')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.OR, "||", _lineNumber);
                }
            }

            if (_position + 4 <= _input.Length && _input.Substring(_position, 4) == "true")
            {
                _position += 4;
                Advance();
                return new Token(TokenType.TRUE, "true", _lineNumber);
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "false")
            {
                _position += 5;
                Advance();
                return new Token(TokenType.FALSE, "false", _lineNumber);
            }

            if (_currentChar == '"')
            {
                return new Token(TokenType.STRING, String(), _lineNumber);
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "print" &&
            (_position + 5 == _input.Length || char.IsWhiteSpace(_input[_position + 5]) || _input[_position + 5] == '('))
            {
                _position += 5;
                Advance();
                return new Token(TokenType.PRINT, "print", _lineNumber);
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "input" &&
            (_position + 5 == _input.Length || char.IsWhiteSpace(_input[_position + 5]) || _input[_position + 5] == '('))
            {
                _position += 5;
                Advance();
                return new Token(TokenType.INPUT, "input", _lineNumber);
            }

            if (_position + 2 <= _input.Length && _input.Substring(_position, 2) == "if" &&
            (_position + 2 == _input.Length || char.IsWhiteSpace(_input[_position + 2]) || _input[_position + 2] == '('))
            {
                _position += 2;
                Advance();
                return new Token(TokenType.IF, "if", _lineNumber);
            }

            if (_position + 7 <= _input.Length && _input.Substring(_position, 7) == "else if" &&
            (_position + 7 == _input.Length || char.IsWhiteSpace(_input[_position + 7]) || _input[_position + 7] == '('))
            {
                _position += 7;
                Advance();
                return new Token(TokenType.ELIF, "else if", _lineNumber);
            }

            if (_position + 4 <= _input.Length && _input.Substring(_position, 4) == "else" &&
            (_position + 4 == _input.Length || char.IsWhiteSpace(_input[_position + 4]) || _input[_position + 4] == '('))
            {
                _position += 4;
                Advance();
                return new Token(TokenType.ELSE, "else", _lineNumber);
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "while" &&
            (_position + 5 == _input.Length || char.IsWhiteSpace(_input[_position + 5]) || _input[_position + 5] == '('))
            {
                _position += 5;
                Advance();
                return new Token(TokenType.WHILE, "while", _lineNumber);
            }

            if (_position + 3 <= _input.Length && _input.Substring(_position, 3) == "def" &&
            (_position + 3 == _input.Length || char.IsWhiteSpace(_input[_position + 3]) || _input[_position + 3] == '('))
            {
                _position += 3;
                Advance();
                return new Token(TokenType.FUNCTION, "def", _lineNumber);
            }

            if (_position + 6 <= _input.Length && _input.Substring(_position, 6) == "return" &&
            (_position + 6 == _input.Length || char.IsWhiteSpace(_input[_position + 6]) || _input[_position + 6] == '('))
            {
                _position += 6;
                Advance();
                return new Token(TokenType.RETURN, "return", _lineNumber);
            }

            if (_currentChar == ',')
            {
                Advance();
                return new Token(TokenType.COMMA, ",", _lineNumber);
            }

            if (_currentChar == '\n')
            {
                Advance();
                return new Token(TokenType.NEWLINE, "\n", _lineNumber);
            }

            if (_currentChar == '{')
            {
                Advance();
                return new Token(TokenType.LBRACE, "{", _lineNumber);
            }

            if (_currentChar == '}')
            {
                Advance();
                return new Token(TokenType.RBRACE, "}", _lineNumber);
            }

            if (_currentChar == ';')
            {
                Advance();
                return new Token(TokenType.SEMICOLON, ";", _lineNumber);
            }

            // keep this at the end
            if (char.IsLetter(_currentChar) || _currentChar == '_')
            {
                return new Token(TokenType.IDENTIFIER, Identifier(), _lineNumber);
            }
            throw new Exception($"Error parsing input at position {_position}: Unexpected character '{_currentChar}'");
        }
        return new Token(TokenType.EOF, string.Empty, _lineNumber);
    }
}
