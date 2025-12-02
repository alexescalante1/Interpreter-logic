using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Block
{
    public class CaseBlock
    {
        public IExpression Value { get; }
        public IStatement Statement { get; }

        public CaseBlock(IExpression value, IStatement statement)
        {
            Value = value;
            Statement = statement;
        }

        public bool Matches(object testValue, ScriptContext context)
        {
            var caseValue = Value.Evaluate(context);
            if (caseValue == null && testValue == null) return true;
            if (caseValue == null || testValue == null) return false;
            return caseValue.Equals(testValue);
        }
    }
}
