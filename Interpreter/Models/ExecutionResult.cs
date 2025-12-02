namespace Interpreter.Models
{
    public class ExecutionResult
    {
        private static readonly ExecutionResult _normal = new ExecutionResult();
        private static readonly ExecutionResult _break = new ExecutionResult { ShouldBreak = true };
        private static readonly ExecutionResult _continue = new ExecutionResult { ShouldContinue = true };

        public bool ShouldBreak { get; init; }
        public bool ShouldContinue { get; init; }
        public bool ShouldReturn { get; init; }
        public object? ReturnValue { get; init; }

        public static ExecutionResult Normal() => _normal;
        public static ExecutionResult Break() => _break;
        public static ExecutionResult Continue() => _continue;
        public static ExecutionResult Return(object value) => new ExecutionResult { ShouldReturn = true, ReturnValue = value };

        public bool ShouldStopExecution()
        {
            return ShouldBreak || ShouldContinue || ShouldReturn;
        }
    }
}
