using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class BinaryExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly string _operator;
        private readonly IExpression _right;

        public BinaryExpression(IExpression left, string op, IExpression right)
        {
            _left = left;
            _operator = op;
            _right = right;
        }

        public object Evaluate(ScriptContext context)
        {
            var left = _left.Evaluate(context);
            var right = _right.Evaluate(context);

            return _operator switch
            {
                "+" => Add(left, right),
                "-" => Subtract(left, right),
                "*" => Multiply(left, right),
                "/" => Divide(left, right),
                "%" => Modulo(left, right),
                "<" => Compare(left, right) < 0,
                ">" => Compare(left, right) > 0,
                "<=" => Compare(left, right) <= 0,
                ">=" => Compare(left, right) >= 0,
                "==" => AreEqual(left, right),
                "!=" => !AreEqual(left, right),
                "AND" => IsTruthy(left) && IsTruthy(right),
                "OR" => IsTruthy(left) || IsTruthy(right),
                _ => throw new Exception($"Operador binario desconocido: {_operator}")
            };
        }

        private object Add(object left, object right)
        {
            if (left is string || right is string) return $"{left}{right}";
            if (TryGetNumber(left, out var l, out var lIsDouble) && TryGetNumber(right, out var r, out var rIsDouble))
            {
                var result = l + r;
                return (lIsDouble || rIsDouble) ? result : (object)(int)result;
            }
            throw new Exception($"No se puede sumar {left?.GetType().Name ?? "null"} y {right?.GetType().Name ?? "null"}");
        }

        private object Subtract(object left, object right)
        {
            if (TryGetNumber(left, out var l, out var lIsDouble) && TryGetNumber(right, out var r, out var rIsDouble))
            {
                var result = l - r;
                return (lIsDouble || rIsDouble) ? result : (object)(int)result;
            }
            throw new Exception($"No se puede restar");
        }

        private object Multiply(object left, object right)
        {
            if (TryGetNumber(left, out var l, out var lIsDouble) && TryGetNumber(right, out var r, out var rIsDouble))
            {
                var result = l * r;
                return (lIsDouble || rIsDouble) ? result : (object)(int)result;
            }
            throw new Exception($"No se puede multiplicar");
        }

        private object Divide(object left, object right)
        {
            if (TryGetNumber(left, out var l, out var lIsDouble) && TryGetNumber(right, out var r, out var rIsDouble))
            {
                if (r == 0) throw new Exception("Division por cero");
                var result = l / r;
                return (lIsDouble || rIsDouble) ? result : (object)(int)result;
            }
            throw new Exception($"No se puede dividir");
        }

        private object Modulo(object left, object right)
        {
            if (left is int l && right is int r)
            {
                if (r == 0) throw new Exception("Modulo por cero");
                return l % r;
            }
            throw new Exception($"No se puede calcular modulo con operandos no enteros");
        }

        private int Compare(object left, object right)
        {
            if (left is int l && right is int r) return l.CompareTo(r);
            if (TryGetNumber(left, out var ld, out _) && TryGetNumber(right, out var rd, out _)) return ld.CompareTo(rd);
            if (left is string ls && right is string rs) return string.Compare(ls, rs, StringComparison.Ordinal);
            throw new Exception($"No se puede comparar");
        }

        private bool AreEqual(object left, object right)
        {
            if (TryGetNumber(left, out var l, out _) && TryGetNumber(right, out var r, out _))
            {
                return Math.Abs(l - r) < double.Epsilon;
            }
            if (left == null && right == null) return true;
            if (left == null || right == null) return false;
            return left.Equals(right);
        }

        private bool IsTruthy(object value)
        {
            if (value is bool b) return b;
            if (value == null) return false;
            return true;
        }

        private bool TryGetNumber(object value, out double number, out bool isDouble)
        {
            switch (value)
            {
                case int i:
                    number = i;
                    isDouble = false;
                    return true;
                case double d:
                    number = d;
                    isDouble = true;
                    return true;
                default:
                    number = 0;
                    isDouble = false;
                    return false;
            }
        }
    }
}
