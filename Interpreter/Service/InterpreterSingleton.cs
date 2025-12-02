using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Service
{
    public static class InterpreterSingleton
    {
        private static readonly object _lock = new();
        private static ScriptInterpreter? _instance;

        // Obtiene una instancia única del intérprete. Si cambias el script se vuelve a preparar (lexer + parser).
        public static ScriptInterpreter GetInstance(string script, Dictionary<string, IInternalFunction> internalFunctions, bool warmup = true)
        {
            if (internalFunctions == null) throw new ArgumentNullException(nameof(internalFunctions));

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new ScriptInterpreter(script, internalFunctions);
                }
                else
                {
                    _instance.UpdateScript(script);
                }

                if (warmup)
                {
                    _instance.PrepareScript(withMetrics: true);
                }

                return _instance;
            }
        }
    }
}
