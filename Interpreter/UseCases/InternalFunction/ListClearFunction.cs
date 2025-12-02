using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.UseCases.InternalFunction
{
    public class ListClearFunction : IInternalFunction
    {
        public object? Execute(List<object> arguments, ScriptContext context)
        {
            if (arguments == null || arguments.Count != 1)
                throw new Exception("ListClear requiere 1 argumento: lista");

            var list = arguments[0] as List<object>;
            if (list == null)
                throw new Exception("El argumento debe ser una lista");

            list.Clear();
            return null;
        }
    }
}
