using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class BreakStatement : IStatement
    {
        public ExecutionResult Execute(ScriptContext context)
        {
            return ExecutionResult.Break();
        }
    }
}
