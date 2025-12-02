using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class VariableDeclarationStatement : IStatement
    {
        private readonly string _type;
        private readonly string _name;
        private readonly IExpression _initializer;

        public VariableDeclarationStatement(string type, string name, IExpression initializer)
        {
            _type = type;
            _name = name;
            _initializer = initializer;
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            var value = _initializer.Evaluate(context);
            context.DeclareVariable(_name, _type, value);
            return ExecutionResult.Normal();
        }
    }
}
