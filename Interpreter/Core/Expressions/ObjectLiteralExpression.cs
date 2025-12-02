using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class ObjectLiteralExpression : IExpression
    {
        private readonly Dictionary<string, IExpression> _properties;

        public ObjectLiteralExpression(Dictionary<string, IExpression> properties)
        {
            _properties = properties ?? new Dictionary<string, IExpression>();
        }

        public object Evaluate(ScriptContext context)
        {
            var obj = new Dictionary<string, object>();

            foreach (var prop in _properties)
            {
                obj[prop.Key] = prop.Value.Evaluate(context);
            }

            return obj;
        }
    }
}
