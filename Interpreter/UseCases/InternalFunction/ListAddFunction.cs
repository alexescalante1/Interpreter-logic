using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.UseCases.InternalFunction
{
    public class ListAddFunction : IInternalFunction
    {
        public object? Execute(List<object> arguments, ScriptContext context)
        {
            if (arguments == null || arguments.Count != 2)
                throw new Exception("ListAdd requiere 2 argumentos: lista y valor");

            var list = arguments[0] as List<object>;
            if (list == null)
                throw new Exception("El primer argumento debe ser una lista");

            list.Add(arguments[1]);
            return null;
        }
    }
}
