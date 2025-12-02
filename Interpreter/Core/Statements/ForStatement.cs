using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class ForStatement : IStatement
    {
        private readonly IStatement? _initializer;
        private readonly IExpression _condition;
        private readonly IStatement? _increment;
        private readonly IStatement _body;

        public ForStatement(IStatement? initializer, IExpression condition, IStatement? increment, IStatement body)
        {
            _initializer = initializer;
            _condition = condition;
            _increment = increment;
            _body = body;
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            context.PushScope(); // Scope para el FOR completo (inicializador)

            try
            {
                if (_initializer != null)
                {
                    _initializer.Execute(context);
                }

                while (IsTruthy(_condition.Evaluate(context)))
                {
                    // CREAR SCOPE PARA CADA ITERACIÓN
                    context.PushScope();

                    try
                    {
                        var result = _body.Execute(context);

                        if (result.ShouldBreak)
                        {
                            break;
                        }

                        if (result.ShouldContinue)
                        {
                            goto Increment;
                        }

                        if (result.ShouldReturn)
                        {
                            return result;
                        }
                    }
                    finally
                    {
                        context.PopScope(); // Limpiar scope de la iteración
                    }

                Increment:
                    if (_increment != null)
                    {
                        _increment.Execute(context);
                    }
                }

                return ExecutionResult.Normal();
            }
            finally
            {
                context.PopScope(); // Limpiar scope del FOR
            }
        }

        private bool IsTruthy(object value)
        {
            if (value is bool b) return b;
            if (value == null) return false;
            return true;
        }
    }
}
