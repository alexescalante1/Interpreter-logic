using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class ContinueStatement : IStatement
    {
        public ExecutionResult Execute(ScriptContext context)
        {
            return ExecutionResult.Continue();
        }
    }
}
