using Interpreter.Enum;

namespace Interpreter.Entity
{
    public class Variable
    {
        public string Name { get; }
        public VariableTypeEnum Type { get; }
        public string TypeName { get; }
        public object Value { get; set; }

        public Variable(string name, VariableTypeEnum type, object value)
        {
            Name = name;
            Type = type;
            TypeName = type.ToString().ToLower();
            Value = value;
        }

        public override string ToString()
        {
            return $"{TypeName} {Name} = {Value}";
        }
    }
}
