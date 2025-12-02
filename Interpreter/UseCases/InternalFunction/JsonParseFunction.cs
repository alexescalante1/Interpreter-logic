using Interpreter.Core.Script;
using Interpreter.Helpers;
using Interpreter.Interface;

namespace Interpreter.UseCases.InternalFunction
{
    public class JsonParseFunction : IInternalFunction
    {
        public object? Execute(List<object> arguments, ScriptContext context)
        {
            if (arguments == null || arguments.Count != 1)
                throw new Exception("JsonParse requiere 1 argumento: string json");

            if (arguments[0] is not string json)
                throw new Exception("JsonParse requiere un string con el contenido JSON");

            return JsonHelper.Parse(json);
        }
    }
}
