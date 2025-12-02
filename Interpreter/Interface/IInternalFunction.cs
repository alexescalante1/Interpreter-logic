using Interpreter.Core.Script;

namespace Interpreter.Interface
{
    public interface IInternalFunction
    {
        object? Execute(List<object> arguments, ScriptContext context);
    }
}
