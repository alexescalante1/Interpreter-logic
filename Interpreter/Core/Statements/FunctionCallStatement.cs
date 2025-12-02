using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class FunctionCallStatement : IStatement
    {
        private readonly string _name;
        private readonly List<IExpression> _arguments;
        private readonly List<object> _argBuffer;

        public FunctionCallStatement(string name, List<IExpression> arguments)
        {
            _name = name;
            _arguments = arguments ?? new List<IExpression>();
            _argBuffer = new List<object>(_arguments.Count);
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            _argBuffer.Clear();
            for (int i = 0; i < _arguments.Count; i++)
            {
                _argBuffer.Add(_arguments[i].Evaluate(context));
            }

            context.CallInternalFunction(_name, _argBuffer);
            return ExecutionResult.Normal();
        }
    }
}
