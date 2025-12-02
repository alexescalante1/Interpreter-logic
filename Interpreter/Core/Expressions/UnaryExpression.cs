using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class UnaryExpression : IExpression
    {
        private readonly string _operator;
        private readonly IExpression _operand;

        public UnaryExpression(string op, IExpression operand)
        {
            _operator = op;
            _operand = operand;
        }

        public object? Evaluate(ScriptContext context)
        {
            var value = _operand.Evaluate(context);

            return _operator switch
            {
                "NOT" => !IsTruthy(value),
                "-" => Negate(value),
                _ => throw new Exception($"Operador unario desconocido: {_operator}")
            };
        }

        private object Negate(object value)
        {
            if (value is int i) return -i;
            if (value is double d) return -d;
            throw new Exception($"No se puede negar");
        }

        private bool IsTruthy(object value)
        {
            if (value is bool b) return b;
            if (value == null) return false;
            return true;
        }
    }
}
