using Interpreter.Core.Script;
using Interpreter.Interface;

namespace Interpreter.Core.Expressions
{
    public class ListAccessExpression : IExpression
    {
        private readonly string _listName;
        private readonly IExpression _index;

        public ListAccessExpression(string listName, IExpression index)
        {
            _listName = listName;
            _index = index;
        }

        public object? Evaluate(ScriptContext context)
        {
            var list = context.GetVariable(_listName) as List<object>;
            if (list == null)
                throw new Exception($"'{_listName}' no es una lista");

            var index = _index.Evaluate(context);
            if (!(index is int idx))
                throw new Exception("El índice debe ser un entero");

            if (idx < 0 || idx >= list.Count)
                throw new Exception($"Índice {idx} fuera de rango (0-{list.Count - 1})");

            return list[idx];
        }
    }
}
