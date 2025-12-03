using System;
using System.Globalization;
using Interpreter.Core.Block;
using Interpreter.Core.Expressions;
using Interpreter.Core.Statements;
using Interpreter.Entity;
using Interpreter.Enum;
using Interpreter.Interface;

namespace Interpreter.Core.Parser
{
    public class Parser
    {
        private Token[] _tokens = Array.Empty<Token>();
        private int _current;
        private int _length;

        private static readonly TokenType[] DeclarationTypes = { TokenType.INT, TokenType.DECIMAL, TokenType.STRING, TokenType.BOOL, TokenType.LIST, TokenType.OBJECT };
        private static readonly TokenType[] EqualityOperators = { TokenType.EQUAL, TokenType.NOT_EQUAL };
        private static readonly TokenType[] ComparisonOperators = { TokenType.LESS, TokenType.LESS_EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL };
        private static readonly TokenType[] AdditiveOperators = { TokenType.PLUS, TokenType.MINUS };
        private static readonly TokenType[] MultiplicativeOperators = { TokenType.MULTIPLY, TokenType.DIVIDE, TokenType.MODULO };
        private static readonly TokenType[] UnaryOperators = { TokenType.NOT, TokenType.MINUS };

        public List<IStatement> Parse(List<Token> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            _tokens = tokens.ToArray();
            _length = _tokens.Length;
            _current = 0;

            var statements = new List<IStatement>(Math.Max(16, _length / 4));

            while (_current < _length && _tokens[_current].Type != TokenType.EOF)
            {
                try
                {
                    var statement = ParseStatement();
                    if (statement != null)
                        statements.Add(statement);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error de parseo en línea {_tokens[_current].Line}: {ex.Message}", ex);
                }
            }

            return statements;
        }

        private IStatement? ParseStatement()
        {
            var currentType = _tokens[_current].Type;

            // Declaraciones de variables
            if (currentType == TokenType.INT || currentType == TokenType.DECIMAL ||
                currentType == TokenType.STRING || currentType == TokenType.BOOL ||
                currentType == TokenType.LIST || currentType == TokenType.OBJECT)
            {
                _current++;
                return ParseVariableDeclaration();
            }

            // Control de flujo
            if (currentType == TokenType.IF)
            {
                _current++;
                return ParseIfStatement();
            }

            if (currentType == TokenType.SWITCH)
            {
                _current++;
                return ParseSwitchStatement();
            }

            if (currentType == TokenType.WHILE)
            {
                _current++;
                return ParseWhileStatement();
            }

            if (currentType == TokenType.FOR)
            {
                _current++;
                return ParseForStatement();
            }

            if (currentType == TokenType.BREAK)
            {
                _current++;
                if (_tokens[_current].Type != TokenType.SEMICOLON)
                    throw new Exception("Se esperaba ';'");
                _current++;
                return new BreakStatement();
            }

            if (currentType == TokenType.CONTINUE)
            {
                _current++;
                if (_tokens[_current].Type != TokenType.SEMICOLON)
                    throw new Exception("Se esperaba ';'");
                _current++;
                return new ContinueStatement();
            }

            // Identificadores (asignación, llamadas, propiedades)
            if (currentType == TokenType.IDENTIFIER)
            {
                if (_current + 1 < _length)
                {
                    var nextType = _tokens[_current + 1].Type;

                    if (nextType == TokenType.LPAREN)
                        return ParseFunctionCall();

                    if (nextType == TokenType.DOT)
                        return ParsePropertyAssignment();

                    if (nextType == TokenType.ASSIGN)
                        return ParseAssignment();
                }
            }

            // Bloques
            if (currentType == TokenType.LBRACE)
            {
                _current++;
                return ParseBlock();
            }

            _current++;
            return null;
        }

        private IStatement ParseVariableDeclaration()
        {
            var type = _tokens[_current - 1].Value;

            if (_tokens[_current].Type != TokenType.IDENTIFIER)
                throw new Exception("Se esperaba nombre de variable");

            var name = _tokens[_current++].Value;

            if (_tokens[_current].Type != TokenType.ASSIGN)
                throw new Exception("Se esperaba '='");
            _current++;

            IExpression value;

            if (type == "LIST" && _tokens[_current].Type == TokenType.LBRACKET)
            {
                _current++; // [
                if (_tokens[_current].Type != TokenType.RBRACKET)
                    throw new Exception("Se esperaba ']'");
                _current++; // ]
                value = new ListLiteralExpression();
            }
            else
            {
                value = ParseExpression();
            }

            if (_tokens[_current].Type != TokenType.SEMICOLON)
                throw new Exception("Se esperaba ';'");
            _current++;

            return new VariableDeclarationStatement(type, name, value);
        }

        private IStatement ParseAssignment()
        {
            var name = _tokens[_current++].Value;

            if (_tokens[_current].Type != TokenType.ASSIGN)
                throw new Exception("Se esperaba '='");
            _current++;

            var value = ParseExpression();

            if (_tokens[_current].Type != TokenType.SEMICOLON)
                throw new Exception("Se esperaba ';'");
            _current++;

            return new AssignmentStatement(name, value);
        }

        private IStatement ParsePropertyAssignment()
        {
            var objectName = _tokens[_current++].Value;

            if (_tokens[_current].Type != TokenType.DOT)
                throw new Exception("Se esperaba '.'");
            _current++;

            if (_tokens[_current].Type != TokenType.IDENTIFIER)
                throw new Exception("Se esperaba nombre de propiedad después de '.'");

            var propertyName = _tokens[_current++].Value;

            if (_tokens[_current].Type != TokenType.ASSIGN)
                throw new Exception("Se esperaba '='");
            _current++;

            var value = ParseExpression();

            if (_tokens[_current].Type != TokenType.SEMICOLON)
                throw new Exception("Se esperaba ';'");
            _current++;

            return new PropertyAssignmentStatement(objectName, propertyName, value);
        }

        private IStatement ParseIfStatement()
        {
            if (_tokens[_current].Type != TokenType.LPAREN)
                throw new Exception("Se esperaba '('");
            _current++;

            var condition = ParseExpression();

            if (_tokens[_current].Type != TokenType.RPAREN)
                throw new Exception("Se esperaba ')'");
            _current++;

            var thenBranch = ParseBlockOrStatement();
            IStatement? elseBranch = null;

            if (_tokens[_current].Type == TokenType.ELSEIF)
            {
                _current++;
                elseBranch = ParseIfStatement();
            }
            else if (_tokens[_current].Type == TokenType.ELSE)
            {
                _current++;
                elseBranch = ParseBlockOrStatement();
            }

            return new IfStatement(condition, thenBranch, elseBranch);
        }

        private IStatement ParseSwitchStatement()
        {
            if (_tokens[_current].Type != TokenType.LPAREN)
                throw new Exception("Se esperaba '('");
            _current++;

            var expression = ParseExpression();

            if (_tokens[_current].Type != TokenType.RPAREN)
                throw new Exception("Se esperaba ')'");
            _current++;

            if (_tokens[_current].Type != TokenType.LBRACE)
                throw new Exception("Se esperaba '{'");
            _current++;

            var cases = new List<CaseBlock>();
            BlockStatement? defaultCase = null;

            while (_current < _length && _tokens[_current].Type != TokenType.RBRACE)
            {
                if (_tokens[_current].Type == TokenType.CASE)
                {
                    _current++;

                    if (_tokens[_current].Type != TokenType.LPAREN)
                        throw new Exception("Se esperaba '('");
                    _current++;

                    var caseValue = ParseExpression();

                    if (_tokens[_current].Type != TokenType.RPAREN)
                        throw new Exception("Se esperaba ')'");
                    _current++;

                    if (_tokens[_current].Type != TokenType.COLON)
                        throw new Exception("Se esperaba ':'");
                    _current++;

                    var caseStatements = new List<IStatement>();
                    while (_current < _length && _tokens[_current].Type != TokenType.CASE &&
                           _tokens[_current].Type != TokenType.DEFAULT &&
                           _tokens[_current].Type != TokenType.RBRACE)
                    {
                        var stmt = ParseStatement();
                        if (stmt != null) caseStatements.Add(stmt);
                    }

                    cases.Add(new CaseBlock(caseValue, new BlockStatement(caseStatements)));
                }
                else if (_tokens[_current].Type == TokenType.DEFAULT)
                {
                    _current++;

                    if (_tokens[_current].Type != TokenType.COLON)
                        throw new Exception("Se esperaba ':'");
                    _current++;

                    var defaultStatements = new List<IStatement>();
                    while (_current < _length && _tokens[_current].Type != TokenType.RBRACE)
                    {
                        var stmt = ParseStatement();
                        if (stmt != null) defaultStatements.Add(stmt);
                    }

                    defaultCase = new BlockStatement(defaultStatements);
                }
                else
                {
                    _current++;
                }
            }

            if (_tokens[_current].Type != TokenType.RBRACE)
                throw new Exception("Se esperaba '}'");
            _current++;

            return new SwitchStatement(expression, cases, defaultCase);
        }

        private IStatement ParseWhileStatement()
        {
            if (_tokens[_current].Type != TokenType.LPAREN)
                throw new Exception("Se esperaba '('");
            _current++;

            var condition = ParseExpression();

            if (_tokens[_current].Type != TokenType.RPAREN)
                throw new Exception("Se esperaba ')'");
            _current++;

            var body = ParseBlockOrStatement();

            return new WhileStatement(condition, body);
        }

        private IStatement ParseForStatement()
        {
            if (_tokens[_current].Type != TokenType.LPAREN)
                throw new Exception("Se esperaba '('");
            _current++;

            IStatement? initializer = null;
            var currentType = _tokens[_current].Type;

            if (currentType == TokenType.INT || currentType == TokenType.STRING ||
                currentType == TokenType.DECIMAL || currentType == TokenType.BOOL ||
                currentType == TokenType.LIST || currentType == TokenType.OBJECT)
            {
                _current++;
                initializer = ParseVariableDeclaration();
            }
            else if (currentType == TokenType.IDENTIFIER)
            {
                initializer = ParseAssignment();
            }

            var condition = ParseExpression();

            if (_tokens[_current].Type != TokenType.SEMICOLON)
                throw new Exception("Se esperaba ';'");
            _current++;

            var increment = ParseAssignmentWithoutSemicolon();

            if (_tokens[_current].Type != TokenType.RPAREN)
                throw new Exception("Se esperaba ')'");
            _current++;

            var body = ParseBlockOrStatement();

            return new ForStatement(initializer, condition, increment, body);
        }

        private IStatement ParseAssignmentWithoutSemicolon()
        {
            if (_tokens[_current].Type != TokenType.IDENTIFIER)
                throw new Exception("Se esperaba un identificador");

            var name = _tokens[_current++].Value;

            if (_tokens[_current].Type != TokenType.ASSIGN)
                throw new Exception("Se esperaba '='");
            _current++;

            var value = ParseExpression();

            return new AssignmentStatement(name, value);
        }

        private IStatement ParseFunctionCall()
        {
            var name = _tokens[_current++].Value;

            if (_tokens[_current].Type != TokenType.LPAREN)
                throw new Exception("Se esperaba '('");
            _current++;

            var arguments = new List<IExpression>();

            if (_tokens[_current].Type != TokenType.RPAREN)
            {
                do
                {
                    arguments.Add(ParseExpression());
                } while (_tokens[_current].Type == TokenType.COMMA && ++_current >= 0);
            }

            if (_tokens[_current].Type != TokenType.RPAREN)
                throw new Exception("Se esperaba ')'");
            _current++;

            if (_tokens[_current].Type != TokenType.SEMICOLON)
                throw new Exception("Se esperaba ';'");
            _current++;

            return new FunctionCallStatement(name, arguments);
        }

        private IStatement ParseBlockOrStatement()
        {
            if (_tokens[_current].Type == TokenType.LBRACE)
            {
                _current++;
                return ParseBlock();
            }

            return ParseStatement();
        }

        private BlockStatement ParseBlock()
        {
            var statements = new List<IStatement>();

            while (_current < _length && _tokens[_current].Type != TokenType.RBRACE)
            {
                var stmt = ParseStatement();
                if (stmt != null) statements.Add(stmt);
            }

            if (_tokens[_current].Type != TokenType.RBRACE)
                throw new Exception("Se esperaba '}'");
            _current++;

            return new BlockStatement(statements);
        }

        private IExpression ParseExpression() => ParseLogicalOr();

        private IExpression ParseLogicalOr()
        {
            var left = ParseLogicalAnd();

            while (_tokens[_current].Type == TokenType.OR)
            {
                var op = _tokens[_current++].Value;
                var right = ParseLogicalAnd();
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private IExpression ParseLogicalAnd()
        {
            var left = ParseEquality();

            while (_tokens[_current].Type == TokenType.AND)
            {
                var op = _tokens[_current++].Value;
                var right = ParseEquality();
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private IExpression ParseEquality()
        {
            var left = ParseComparison();

            while (_tokens[_current].Type == TokenType.EQUAL ||
                   _tokens[_current].Type == TokenType.NOT_EQUAL)
            {
                var op = _tokens[_current++].Value;
                var right = ParseComparison();
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private IExpression ParseComparison()
        {
            var left = ParseAddition();

            var type = _tokens[_current].Type;
            while (type == TokenType.LESS || type == TokenType.LESS_EQUAL ||
                   type == TokenType.GREATER || type == TokenType.GREATER_EQUAL)
            {
                var op = _tokens[_current++].Value;
                var right = ParseAddition();
                left = new BinaryExpression(left, op, right);
                type = _tokens[_current].Type;
            }

            return left;
        }

        private IExpression ParseAddition()
        {
            var left = ParseMultiplication();

            while (_tokens[_current].Type == TokenType.PLUS ||
                   _tokens[_current].Type == TokenType.MINUS)
            {
                var op = _tokens[_current++].Value;
                var right = ParseMultiplication();
                left = new BinaryExpression(left, op, right);
            }

            return left;
        }

        private IExpression ParseMultiplication()
        {
            var left = ParseUnary();

            var type = _tokens[_current].Type;
            while (type == TokenType.MULTIPLY || type == TokenType.DIVIDE ||
                   type == TokenType.MODULO)
            {
                var op = _tokens[_current++].Value;
                var right = ParseUnary();
                left = new BinaryExpression(left, op, right);
                type = _tokens[_current].Type;
            }

            return left;
        }

        private IExpression ParseUnary()
        {
            var type = _tokens[_current].Type;

            if (type == TokenType.NOT || type == TokenType.MINUS)
            {
                var op = _tokens[_current++].Value;
                var right = ParseUnary();
                return new UnaryExpression(op, right);
            }

            return ParsePrimary();
        }

        private IExpression ParseObjectLiteral()
        {
            var properties = new Dictionary<string, IExpression>();

            if (_tokens[_current].Type == TokenType.RBRACE)
            {
                _current++;
                return new ObjectLiteralExpression(properties);
            }

            do
            {
                if (_tokens[_current].Type != TokenType.IDENTIFIER)
                    throw new Exception("Se esperaba nombre de propiedad");

                var propName = _tokens[_current++].Value;
                var nextType = _tokens[_current].Type;

                if (nextType == TokenType.COMMA || nextType == TokenType.RBRACE)
                {
                    properties[propName] = new VariableExpression(propName);
                }
                else
                {
                    if (nextType != TokenType.COLON)
                        throw new Exception("Se esperaba ':' después del nombre de propiedad");
                    _current++;

                    var value = ParseExpression();
                    properties[propName] = value;
                }

            } while (_tokens[_current].Type == TokenType.COMMA && ++_current >= 0);

            if (_tokens[_current].Type != TokenType.RBRACE)
                throw new Exception("Se esperaba '}'");
            _current++;

            return new ObjectLiteralExpression(properties);
        }

        private IExpression ParsePrimary()
        {
            var type = _tokens[_current].Type;

            if (type == TokenType.TRUE)
            {
                _current++;
                return new LiteralExpression(true);
            }

            if (type == TokenType.FALSE)
            {
                _current++;
                return new LiteralExpression(false);
            }

            if (type == TokenType.NUMBER)
            {
                var value = _tokens[_current++].Value;
                if (value.Contains("."))
                {
                    var number = double.Parse(value, CultureInfo.InvariantCulture);
                    return new LiteralExpression(number);
                }
                else
                {
                    var number = int.Parse(value, CultureInfo.InvariantCulture);
                    return new LiteralExpression(number);
                }
            }

            if (type == TokenType.STRING_LITERAL)
            {
                var value = _tokens[_current++].Value;
                return new LiteralExpression(value);
            }

            if (type == TokenType.LBRACE)
            {
                _current++;
                return ParseObjectLiteral();
            }

            if (type == TokenType.IDENTIFIER)
            {
                var name = _tokens[_current++].Value;
                var nextType = _tokens[_current].Type;

                if (nextType == TokenType.LPAREN)
                {
                    _current++;
                    var arguments = new List<IExpression>();

                    if (_tokens[_current].Type != TokenType.RPAREN)
                    {
                        do
                        {
                            arguments.Add(ParseExpression());
                        } while (_tokens[_current].Type == TokenType.COMMA && ++_current >= 0);
                    }

                    if (_tokens[_current].Type != TokenType.RPAREN)
                        throw new Exception("Se esperaba ')'");
                    _current++;

                    return new FunctionCallExpression(name, arguments);
                }

                if (nextType == TokenType.LBRACKET)
                {
                    _current++;
                    var index = ParseExpression();

                    if (_tokens[_current].Type != TokenType.RBRACKET)
                        throw new Exception("Se esperaba ']'");
                    _current++;

                    return new ListAccessExpression(name, index);
                }

                if (nextType == TokenType.DOT)
                {
                    _current++;

                    if (_tokens[_current].Type != TokenType.IDENTIFIER)
                        throw new Exception("Se esperaba nombre de propiedad después de '.'");

                    var propertyName = _tokens[_current++].Value;
                    return new PropertyAccessExpression(name, propertyName);
                }

                return new VariableExpression(name);
            }

            if (type == TokenType.LPAREN)
            {
                _current++;
                var expr = ParseExpression();

                if (_tokens[_current].Type != TokenType.RPAREN)
                    throw new Exception("Se esperaba ')'");
                _current++;

                return expr;
            }

            throw new Exception($"Expresión inesperada: {_tokens[_current].Value}");
        }
    }
}
