using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.UseCases.InternalFunction
{
    public class ListGetFunction : IInternalFunction
    {
        public object? Execute(List<object> arguments, ScriptContext context)
        {
            if (arguments == null || arguments.Count != 2)
                throw new Exception("ListGet requiere 2 argumentos: lista e índice");

            var list = arguments[0] as List<object>;
            if (list == null)
                throw new Exception("El primer argumento debe ser una lista");

            if (!(arguments[1] is int index))
                throw new Exception("El índice debe ser un entero");

            if (index < 0 || index >= list.Count)
                throw new Exception($"Índice {index} fuera de rango");

            return list[index];
        }
    }
}
