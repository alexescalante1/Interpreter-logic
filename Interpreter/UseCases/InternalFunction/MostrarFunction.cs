using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.UseCases.InternalFunction
{
    public class MostrarFunction : IInternalFunction
    {
        public object? Execute(List<object> arguments, ScriptContext context)
        {
            #if DEBUG
                if (arguments == null || arguments.Count == 0)
                {
                    Console.WriteLine();
                    return null;
                }

                var message = string.Join(" ", arguments.Select(arg => FormatValue(arg)));
                Console.WriteLine($">> {message}");
            #endif

            return null;
        }

        private string FormatValue(object value)
        {
            if (value == null)
                return "null";

            if (value is string str)
                return str;

            if (value is bool b)
                return b ? "true" : "false";

            return value.ToString();
        }
    }
}
