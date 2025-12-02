using Interpreter.Core.Script;
using Interpreter.Interface;
using Interpreter.Models;

namespace Interpreter.Core.Statements
{
    public class PropertyAssignmentStatement : IStatement
    {
        private readonly string _objectName;
        private readonly string _propertyName;
        private readonly IExpression _value;

        public PropertyAssignmentStatement(string objectName, string propertyName, IExpression value)
        {
            _objectName = objectName;
            _propertyName = propertyName;
            _value = value;
        }

        public ExecutionResult Execute(ScriptContext context)
        {
            var obj = context.GetVariable(_objectName);

            if (obj is Dictionary<string, object> dict)
            {
                var value = _value.Evaluate(context);
                dict[_propertyName] = value;
                return ExecutionResult.Normal();
            }

            throw new Exception($"'{_objectName}' no es un objeto");
        }
    }
}
