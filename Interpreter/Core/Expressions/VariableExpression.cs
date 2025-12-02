using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class VariableExpression : IExpression
    {
        private readonly string _name;

        public VariableExpression(string name)
        {
            _name = name;
        }

        public object Evaluate(ScriptContext context)
        {
            return context.GetVariable(_name);
        }
    }
}
