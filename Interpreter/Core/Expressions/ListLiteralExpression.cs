using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class ListLiteralExpression : IExpression
    {
        public ListLiteralExpression()
        {
        }

        public object Evaluate(ScriptContext context)
        {
            return new List<object>();
        }
    }
}
