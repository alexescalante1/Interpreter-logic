using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class LiteralExpression : IExpression
    {
        private readonly object _value;

        public LiteralExpression(object value)
        {
            _value = value;
        }

        public object? Evaluate(ScriptContext context)
        {
            return _value;
        }
    }
}
