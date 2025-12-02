using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class FunctionCallExpression : IExpression
    {
        private readonly string _name;
        private readonly List<IExpression> _arguments;
        private readonly List<object> _argBuffer;

        public FunctionCallExpression(string name, List<IExpression> arguments)
        {
            _name = name;
            _arguments = arguments ?? new List<IExpression>();
            _argBuffer = new List<object>(_arguments.Count);
        }

        public object? Evaluate(ScriptContext context)
        {
            _argBuffer.Clear();

            for (int i = 0; i < _arguments.Count; i++)
            {
                _argBuffer.Add(_arguments[i].Evaluate(context));
            }

            return context.CallInternalFunction(_name, _argBuffer);
        }
    }
}
