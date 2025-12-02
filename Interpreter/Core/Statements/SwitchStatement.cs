using Interpreter.Interface;
using Interpreter.Models;
using Interpreter.Core.Block;
using Interpreter.Core.Script;

namespace Interpreter.Core.Statements
{
    public class SwitchStatement : IStatement
    {
        private readonly IExpression _expression;
        private readonly CaseBlock[] _cases;
        private readonly BlockStatement? _defaultCase;

        public SwitchStatement(IExpression expression, List<CaseBlock> cases, BlockStatement? defaultCase = null)
        {
            _expression = expression;
            _cases = cases?.ToArray() ?? Array.Empty<CaseBlock>();
            _defaultCase = defaultCase;
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            var value = _expression.Evaluate(context);

            for (int i = 0; i < _cases.Length; i++)
            {
                if (_cases[i].Matches(value, context))
                {
                    var result = _cases[i].Statement.Execute(context);
                    if (result.ShouldBreak)
                    {
                        return ExecutionResult.Normal();
                    }
                    if (result.ShouldContinue || result.ShouldReturn)
                    {
                        return result;
                    }
                    return ExecutionResult.Normal();
                }
            }

            if (_defaultCase != null)
            {
                var result = _defaultCase.Execute(context);
                if (result.ShouldBreak)
                {
                    return ExecutionResult.Normal();
                }
                return result;
            }

            return ExecutionResult.Normal();
        }
    }
}
