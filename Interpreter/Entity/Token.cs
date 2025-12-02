using Interpreter.Enum;

namespace Interpreter.Entity
{
    // Record struct para permitir serialización/deserialización sencilla en cache
    public readonly record struct Token(TokenType Type, string Value, int Line, int Column)
    {
        public override string ToString()
        {
            return $"Token({Type}, '{Value}', L{Line}:C{Column})";
        }
    }
}
