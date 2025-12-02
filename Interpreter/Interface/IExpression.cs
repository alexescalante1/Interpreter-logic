using Interpreter.Core.Script;

namespace Interpreter.Interface
{
    public interface IExpression
    {
        object? Evaluate(ScriptContext context);
    }
}
