public class Lexer
{
    private string _input;
    private int _position;
    private char _currentChar;

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
        _currentChar = _input.Length > 0 ? _input[_position] : '\0';
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

            if (_currentChar == '=')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.EQUAL, "==");
                }
                else
                {
                    Advance();
                    return new Token(TokenType.ASSIGN, "=");
                }
            }

            if (_currentChar == '!')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.NOT_EQUAL, "!=");
                }
                else
                {
                    Advance();
                    return new Token(TokenType.NOT, "!");
                }
            }

            if (_currentChar == '<')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.LESS_THAN_OR_EQUAL, "<=");
                }
                else
                {
                    Advance();
                    return new Token(TokenType.LESS_THAN, "<");
                }
            }

            if (_currentChar == '>')
            {
                if (Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.GREATER_THAN_OR_EQUAL, ">=");
                }
                else
                {
                    Advance();
                    return new Token(TokenType.GREATER_THAN, ">");
                }
            }

            if (_currentChar == '&')
            {
                if (Peek() == '&')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.AND, "&&");
                }
            }

            if (_currentChar == '|')
            {
                if (Peek() == '|')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.OR, "||");
                }
            }

            if (_position + 4 <= _input.Length && _input.Substring(_position, 4) == "true")
            {
                _position += 4;
                Advance();
                return new Token(TokenType.TRUE, "true");
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "false")
            {
                _position += 5;
                Advance();
                return new Token(TokenType.FALSE, "false");
            }

            if (_currentChar == '"')
            {
                return new Token(TokenType.STRING, String());
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "print" &&
            (_position + 5 == _input.Length || char.IsWhiteSpace(_input[_position + 5]) || _input[_position + 5] == '('))
            {
                _position += 5;
                Advance();
                return new Token(TokenType.PRINT, "print");
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "input" &&
            (_position + 5 == _input.Length || char.IsWhiteSpace(_input[_position + 5]) || _input[_position + 5] == '('))
            {
                _position += 5;
                Advance();
                return new Token(TokenType.INPUT, "input");
            }

            if (_position + 2 <= _input.Length && _input.Substring(_position, 2) == "if" &&
            (_position + 2 == _input.Length || char.IsWhiteSpace(_input[_position + 2]) || _input[_position + 2] == '('))
            {
                _position += 2;
                Advance();
                return new Token(TokenType.IF, "if");
            }

            if (_position + 4 <= _input.Length && _input.Substring(_position, 4) == "else" &&
            (_position + 4 == _input.Length || char.IsWhiteSpace(_input[_position + 4]) || _input[_position + 4] == '('))
            {
                _position += 4;
                Advance();
                return new Token(TokenType.ELSE, "else");
            }

            if (_position + 5 <= _input.Length && _input.Substring(_position, 5) == "while" &&
            (_position + 5 == _input.Length || char.IsWhiteSpace(_input[_position + 5]) || _input[_position + 5] == '('))
            {
                _position += 5;
                Advance();
                return new Token(TokenType.WHILE, "while");
            }

            if (_currentChar == '\n')
            {
                Advance();
                return new Token(TokenType.NEWLINE, "\n");
            }

            if (_currentChar == '{')
            {
                Advance();
                return new Token(TokenType.LBRACE, "{");
            }

            if (_currentChar == '}')
            {
                Advance();
                return new Token(TokenType.RBRACE, "}");
            }

            if (_currentChar == ';')
            {
                Advance();
                return new Token(TokenType.SEMICOLON, ";");
            }

            // identifier should not be checked before print because print is a keyword
            if (char.IsLetter(_currentChar) || _currentChar == '_')
            {
                return new Token(TokenType.IDENTIFIER, Identifier());
            }
            throw new Exception($"Error parsing input at position {_position}: Unexpected character '{_currentChar}'");
        }
        return new Token(TokenType.EOF, string.Empty);
    }
}
