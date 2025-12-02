using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Service
{
    public class InterpreterBuilderService
    {
        private string _script;
        private readonly Dictionary<string, IInternalFunction> _internalFunctions = new();

        public InterpreterBuilderService WithScript(string script)
        {
            _script = script ?? throw new ArgumentNullException(nameof(script));
            return this;
        }

        public InterpreterBuilderService AddInternalFunction(string name, IInternalFunction function)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la función no puede estar vacío", nameof(name));

            if (function == null)
                throw new ArgumentNullException(nameof(function));

            _internalFunctions[name] = function;
            return this;
        }

        public ScriptInterpreter Build()
        {
            if (string.IsNullOrWhiteSpace(_script))
                throw new InvalidOperationException("Debe proporcionar un script antes de construir el intérprete");

            return new ScriptInterpreter(_script, _internalFunctions);
        }
    }
}
