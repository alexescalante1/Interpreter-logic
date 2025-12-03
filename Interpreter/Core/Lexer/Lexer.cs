using Interpreter.Entity;
using Interpreter.Enum;
using System.Text;

namespace Interpreter.Core.Lexer
{
    public class Lexer
    {
        private static readonly Dictionary<string, TokenType> Keywords = new()
        {
            { "NUMERIC", TokenType.INT },
            { "DECIMAL", TokenType.DECIMAL },
            { "STRING", TokenType.STRING },
            { "BOOL", TokenType.BOOL },
            { "LIST", TokenType.LIST },
            { "OBJECT", TokenType.OBJECT },
            { "IF", TokenType.IF },
            { "ELSE", TokenType.ELSE },
            { "ELSEIF", TokenType.ELSEIF },
            { "SWITCH", TokenType.SWITCH },
            { "CASE", TokenType.CASE },
            { "DEFAULT", TokenType.DEFAULT },
            { "WHILE", TokenType.WHILE },
            { "FOR", TokenType.FOR },
            { "BREAK", TokenType.BREAK },
            { "CONTINUE", TokenType.CONTINUE },
            { "AND", TokenType.AND },
            { "OR", TokenType.OR },
            { "NOT", TokenType.NOT },
            { "true", TokenType.TRUE },
            { "false", TokenType.FALSE }
        };

        public List<Token> Tokenize(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("El código fuente no puede estar vacío");

            var length = source.Length;
            var tokens = new List<Token>(Math.Max(4, length / 2));
            var line = 1;
            var column = 1;
            var i = 0;

            while (i < length)
            {
                var c = source[i];

                if (char.IsWhiteSpace(c))
                {
                    if (c == '\n')
                    {
                        line++;
                        column = 1;
                    }
                    else
                    {
                        column++;
                    }
                    i++;
                    continue;
                }

                if (c == '/' && i + 1 < length && source[i + 1] == '/')
                {
                    while (i < length && source[i] != '\n')
                    {
                        i++;
                    }
                    continue;
                }

                if (c == '"')
                {
                    var startColumn = column;
                    i++;
                    column++;
                    var sb = new StringBuilder();

                    while (i < length && source[i] != '"')
                    {
                        if (source[i] == '\\' && i + 1 < length)
                        {
                            i++;
                            column++;
                            char escapeChar = source[i];
                            sb.Append(escapeChar switch
                            {
                                'n' => '\n',
                                't' => '\t',
                                'r' => '\r',
                                '"' => '"',
                                '\\' => '\\',
                                _ => escapeChar
                            });
                        }
                        else
                        {
                            sb.Append(source[i]);
                        }
                        i++;
                        column++;
                    }

                    if (i >= length)
                        throw new Exception($"String sin cerrar en línea {line}");

                    tokens.Add(new Token(TokenType.STRING_LITERAL, sb.ToString(), line, startColumn));
                    i++;
                    column++;
                    continue;
                }

                if (char.IsDigit(c))
                {
                    var startColumn = column;
                    var sb = new StringBuilder();
                    var hasDot = false;

                    while (i < length && (char.IsDigit(source[i]) || source[i] == '.'))
                    {
                        if (source[i] == '.')
                        {
                            // Solo un punto y debe estar seguido de digito para considerar decimal
                            if (hasDot || i + 1 >= length || !char.IsDigit(source[i + 1]))
                            {
                                break;
                            }
                            hasDot = true;
                        }

                        sb.Append(source[i]);
                        i++;
                        column++;
                    }

                    tokens.Add(new Token(TokenType.NUMBER, sb.ToString(), line, startColumn));
                    continue;
                }

                if (char.IsLetter(c) || c == '_')
                {
                    var startColumn = column;
                    var sb = new StringBuilder();

                    while (i < length && (char.IsLetterOrDigit(source[i]) || source[i] == '_'))
                    {
                        sb.Append(source[i]);
                        i++;
                        column++;
                    }

                    var word = sb.ToString();
                    var tokenType = Keywords.TryGetValue(word, out var type) ? type : TokenType.IDENTIFIER;
                    tokens.Add(new Token(tokenType, word, line, startColumn));
                    continue;
                }

                if (i + 1 < length)
                {
                    var c1 = source[i];
                    var c2 = source[i + 1];
                    TokenType? type = null;
                    string tokenValue = "";

                    if (c1 == '=' && c2 == '=')
                    {
                        type = TokenType.EQUAL;
                        tokenValue = "==";
                    }
                    else if (c1 == '!' && c2 == '=')
                    {
                        type = TokenType.NOT_EQUAL;
                        tokenValue = "!=";
                    }
                    else if (c1 == '<' && c2 == '=')
                    {
                        type = TokenType.LESS_EQUAL;
                        tokenValue = "<=";
                    }
                    else if (c1 == '>' && c2 == '=')
                    {
                        type = TokenType.GREATER_EQUAL;
                        tokenValue = ">=";
                    }

                    if (type.HasValue)
                    {
                        tokens.Add(new Token(type.Value, tokenValue, line, column));
                        i += 2;
                        column += 2;
                        continue;
                    }
                }

                var token = c switch
                {
                    '=' => new Token(TokenType.ASSIGN, "=", line, column),
                    '+' => new Token(TokenType.PLUS, "+", line, column),
                    '-' => new Token(TokenType.MINUS, "-", line, column),
                    '*' => new Token(TokenType.MULTIPLY, "*", line, column),
                    '/' => new Token(TokenType.DIVIDE, "/", line, column),
                    '%' => new Token(TokenType.MODULO, "%", line, column),
                    '<' => new Token(TokenType.LESS, "<", line, column),
                    '>' => new Token(TokenType.GREATER, ">", line, column),
                    '(' => new Token(TokenType.LPAREN, "(", line, column),
                    ')' => new Token(TokenType.RPAREN, ")", line, column),
                    '{' => new Token(TokenType.LBRACE, "{", line, column),
                    '}' => new Token(TokenType.RBRACE, "}", line, column),
                    '[' => new Token(TokenType.LBRACKET, "[", line, column),
                    ']' => new Token(TokenType.RBRACKET, "]", line, column),
                    ';' => new Token(TokenType.SEMICOLON, ";", line, column),
                    ':' => new Token(TokenType.COLON, ":", line, column),
                    ',' => new Token(TokenType.COMMA, ",", line, column),
                    '.' => new Token(TokenType.DOT, ".", line, column),
                    _ => throw new Exception($"Carácter inesperado '{c}' en línea {line}, columna {column}")
                };

                tokens.Add(token);
                i++;
                column++;
            }

            tokens.Add(new Token(TokenType.EOF, "", line, column));
            return tokens;
        }
    }
}
