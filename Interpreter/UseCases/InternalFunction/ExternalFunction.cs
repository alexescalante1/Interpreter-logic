using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.UseCases.InternalFunction
{
    /// <summary>
    /// Adaptador genérico para exponer métodos externos como funciones del intérprete.
    /// </summary>
    public class ExternalFunction<T> : IInternalFunction
    {
        private readonly T _instance;
        private readonly Func<T, List<object>, ScriptContext, object?> _executor;

        public ExternalFunction(T instance, Func<T, List<object>, ScriptContext, object?> executor)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public object? Execute(List<object> arguments, ScriptContext context)
        {
            return _executor(_instance, arguments, context);
        }

        public static ExternalFunction<T> From(T instance, Func<T, List<object>, ScriptContext, object?> executor)
        {
            return new ExternalFunction<T>(instance, executor);
        }
    }
}
