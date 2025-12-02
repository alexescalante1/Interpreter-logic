using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class BlockStatement : IStatement
    {
        private readonly IStatement[] _statements;

        public BlockStatement(List<IStatement> statements)
        {
            _statements = statements?.ToArray() ?? Array.Empty<IStatement>();
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            for (int i = 0; i < _statements.Length; i++)
            {
                var result = _statements[i].Execute(context);

                if (result.ShouldStopExecution())
                {
                    return result;
                }
            }

            return ExecutionResult.Normal();
        }
    }
}
