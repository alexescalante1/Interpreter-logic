using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class WhileStatement : IStatement
    {
        private readonly IExpression _condition;
        private readonly IStatement _body;

        public WhileStatement(IExpression condition, IStatement body)
        {
            _condition = condition;
            _body = body;
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            while (IsTruthy(_condition.Evaluate(context)))
            {
                // CREAR SCOPE PARA CADA ITERACIÓN
                context.PushScope();

                try
                {
                    var result = _body.Execute(context);

                    if (result.ShouldBreak)
                    {
                        break;
                    }

                    if (result.ShouldContinue)
                    {
                        continue;
                    }

                    if (result.ShouldReturn)
                    {
                        return result;
                    }

                    if (result.ShouldContinue)
                    {
                        continue;
                    }
                }
                finally
                {
                    context.PopScope();
                }
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
