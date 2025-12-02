using Interpreter.Core.Script;
using Interpreter.Models;

namespace Interpreter.Interface
{
    public interface IStatement
    {
        ExecutionResult Execute(ScriptContext context);
    }
}
