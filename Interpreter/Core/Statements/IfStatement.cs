using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class IfStatement : IStatement
    {
        private readonly IExpression _condition;
        private readonly IStatement _thenBranch;
        private readonly IStatement? _elseBranch;

        public IfStatement(IExpression condition, IStatement thenBranch, IStatement? elseBranch = null)
        {
            _condition = condition;
            _thenBranch = thenBranch;
            _elseBranch = elseBranch;
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            var conditionValue = _condition.Evaluate(context);

            if (IsTruthy(conditionValue))
            {
                return _thenBranch.Execute(context);
            }
            else if (_elseBranch != null)
            {
                return _elseBranch.Execute(context);
            }

            return ExecutionResult.Normal();
        }

        private bool IsTruthy(object value)
        {
            if (value is bool b) return b;
            if (value == null) return false;
            return true;
        }
    }
}
