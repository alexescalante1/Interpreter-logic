using Interpreter.Core.Script;
using Interpreter.Helpers;
using Interpreter.Interface;

namespace Interpreter.UseCases.InternalFunction
{
    public class JsonStringifyFunction : IInternalFunction
    {
        public object Execute(List<object> arguments, ScriptContext context)
        {
            if (arguments == null || arguments.Count != 1)
                throw new Exception("JsonStringify requiere 1 argumento: objeto/lista/primitivo");

            return JsonHelper.Stringify(arguments[0]);
        }
    }
}
