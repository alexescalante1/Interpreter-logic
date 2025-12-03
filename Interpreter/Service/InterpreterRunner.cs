using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Helpers;

namespace Interpreter.Service
{
    public static class InterpreterRunner
    {
        public static ScriptInterpreter HydrateScript(string script, Dictionary<string, IInternalFunction> internalFunctions)
        {
            var interpreter = InterpreterSingleton.GetInstance(script, internalFunctions, warmup: true);
            var (lexerTime, parserTime) = interpreter.GetPreparationMetrics();

            #if DEBUG
                Console.WriteLine($"=== PREPARACION: -> Lexer: {lexerTime} ms | Parser: {parserTime} ms");
            #endif

            return interpreter;
        }
        
        public static object RunExecution(ScriptInterpreter interpreter, Dictionary<string, object?> externalData)
        {
            interpreter.SetInitialGlobal("input", "OBJECT", externalData);

            #if DEBUG
                interpreter.ExecuteWithMetrics();
            #else
                interpreter.Execute();
            #endif

            return ShowResult(interpreter);
        }

        private static object ShowResult(ScriptInterpreter interpreter)
        {
            try
            {
                return interpreter.Context.GetVariable("output");
            }
            catch
            {
                return interpreter.Context.GetVariable("input");
            }
        }
    }
}
