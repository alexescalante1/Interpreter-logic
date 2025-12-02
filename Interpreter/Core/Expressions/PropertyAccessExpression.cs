using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class PropertyAccessExpression : IExpression
    {
        private readonly string _objectName;
        private readonly string _propertyName;

        public PropertyAccessExpression(string objectName, string propertyName)
        {
            _objectName = objectName;
            _propertyName = propertyName;
        }

        public object? Evaluate(ScriptContext context)
        {
            var obj = context.GetVariable(_objectName);

            if (obj is Dictionary<string, object> dict)
            {
                if (dict.TryGetValue(_propertyName, out var value))
                {
                    return value;
                }
                throw new Exception($"La propiedad '{_propertyName}' no existe en el objeto");
            }

            throw new Exception($"'{_objectName}' no es un objeto");
        }
    }
}
