using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class AssignmentStatement : IStatement
    {
        private readonly string _name;
        private readonly IExpression _value;

        public AssignmentStatement(string name, IExpression value)
        {
            _name = name;
            _value = value;
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            var value = _value.Evaluate(context);
            context.SetVariable(_name, value);
            return ExecutionResult.Normal();
        }
    }
}
