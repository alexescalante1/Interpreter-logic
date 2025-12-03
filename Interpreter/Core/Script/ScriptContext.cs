using Interpreter.Entity;
using Interpreter.Enum;
using Interpreter.Interface;

namespace Interpreter.Core.Script
{
    public class ScriptContext
    {
        private readonly List<Dictionary<string, Variable>> _scopeStack = new();
        private readonly Stack<Dictionary<string, Variable>> _scopePool = new();
        private readonly Dictionary<string, IInternalFunction> _internalFunctions;
        private Variable? _lastFoundVariable;
        private string? _lastFoundName;

        public ScriptContext(Dictionary<string, IInternalFunction> internalFunctions)
        {
            _internalFunctions = internalFunctions ?? new Dictionary<string, IInternalFunction>();
            _scopeStack.Add(new Dictionary<string, Variable>());
        }

        public void Reset()
        {
            // Mueve todos los scopes al pool y deja solo uno vacío
            for (int i = _scopeStack.Count - 1; i >= 0; i--)
            {
                var scope = _scopeStack[i];
                scope.Clear();
                _scopePool.Push(scope);
            }

            _scopeStack.Clear();
            _scopeStack.Add(_scopePool.Count > 0 ? _scopePool.Pop() : new Dictionary<string, Variable>());

            _lastFoundName = null;
            _lastFoundVariable = null;
        }

        public void PushScope()
        {
            if (_scopePool.Count > 0)
            {
                var reused = _scopePool.Pop();
                reused.Clear();
                _scopeStack.Add(reused);
            }
            else
            {
                _scopeStack.Add(new Dictionary<string, Variable>());
            }
        }

        public void PopScope()
        {
            if (_scopeStack.Count > 1)
            {
                var last = _scopeStack[_scopeStack.Count - 1];
                _scopeStack.RemoveAt(_scopeStack.Count - 1);
                last.Clear();
                _scopePool.Push(last);
                _lastFoundName = null;
                _lastFoundVariable = null;
            }
        }

        public void DeclareVariable(string name, string type, object? value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la variable no puede estar vacío");

            var currentScope = _scopeStack[_scopeStack.Count - 1];

            if (currentScope.ContainsKey(name))
                throw new Exception($"La variable '{name}' ya está declarada en este ámbito");

            var varType = ParseVariableType(type);
            var variable = new Variable(name, varType, ValidateAndConvertValue(value, varType));
            currentScope[name] = variable;

            _lastFoundName = name;
            _lastFoundVariable = variable;
        }

        public void SetVariable(string name, object? value)
        {
            if (_lastFoundName == name && _lastFoundVariable != null)
            {
                _lastFoundVariable.Value = ValidateAndConvertValue(value, _lastFoundVariable.Type);
                return;
            }

            // Iterar desde el scope más reciente (final) hacia atrás
            for (int i = _scopeStack.Count - 1; i >= 0; i--)
            {
                if (_scopeStack[i].TryGetValue(name, out var variable))
                {
                    variable.Value = ValidateAndConvertValue(value, variable.Type);
                    _lastFoundName = name;
                    _lastFoundVariable = variable;
                    return;
                }
            }

            throw new Exception($"Variable '{name}' no encontrada");
        }

        public object? GetVariable(string name)
        {
            if (_lastFoundName == name && _lastFoundVariable != null)
            {
                return _lastFoundVariable.Value;
            }

            // Iterar desde el scope más reciente (final) hacia atrás
            for (int i = _scopeStack.Count - 1; i >= 0; i--)
            {
                if (_scopeStack[i].TryGetValue(name, out var variable))
                {
                    _lastFoundName = name;
                    _lastFoundVariable = variable;
                    return variable.Value;
                }
            }

            throw new Exception($"Variable '{name}' no encontrada");
        }

        public object? CallInternalFunction(string name, List<object> arguments)
        {
            if (_internalFunctions.TryGetValue(name, out var function))
            {
                return function.Execute(arguments, this);
            }

            throw new Exception($"Función '{name}' no encontrada");
        }

        private VariableTypeEnum ParseVariableType(string type)
        {
            return type switch
            {
                "NUMERIC" => VariableTypeEnum.INT,
                "DECIMAL" => VariableTypeEnum.DECIMAL,
                "STRING" => VariableTypeEnum.STRING,
                "BOOL" => VariableTypeEnum.BOOL,
                "LIST" => VariableTypeEnum.LIST,
                "OBJECT" => VariableTypeEnum.OBJECT,
                _ => throw new Exception($"Tipo de variable desconocido: {type}")
            };
        }

        private object? ValidateAndConvertValue(object? value, VariableTypeEnum type)
        {
            if (value == null)
                return null;

            return type switch
            {
                VariableTypeEnum.INT => value is int ? value : throw new Exception($"El valor debe ser de tipo int"),
                VariableTypeEnum.DECIMAL => value switch
                {
                    int i => (double)i,
                    double d => d,
                    _ => throw new Exception($"El valor debe ser de tipo decimal")
                },
                VariableTypeEnum.STRING => value.ToString(),
                VariableTypeEnum.BOOL => value is bool ? value : throw new Exception($"El valor debe ser de tipo bool"),
                VariableTypeEnum.LIST => value is List<object> ? value : throw new Exception($"El valor debe ser de tipo list"),
                VariableTypeEnum.OBJECT => value is Dictionary<string, object> ? value : throw new Exception($"El valor debe ser de tipo object"),
                _ => value
            };
        }
    }
}
