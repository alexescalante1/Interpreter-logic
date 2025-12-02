using System.Diagnostics;
using Interpreter.Interface;

namespace Interpreter.Core.Script
{
    public class ScriptInterpreter
    {
        private string _script;
        private readonly ScriptContext _context;
        private readonly Lexer.Lexer _lexer;
        private readonly Parser.Parser _parser;
        private List<Entity.Token>? _cachedTokens;
        private IStatement[]? _cachedStatements;
        private long _lexerTime;
        private long _parserTime;
        private readonly Dictionary<string, (string Type, object? Value)> _initialGlobals = new();
        private readonly object _stateLock = new();

        public ScriptContext Context => _context;

        public ScriptInterpreter(string script, Dictionary<string, IInternalFunction> internalFunctions)
        {
            _script = script;
            _context = new ScriptContext(internalFunctions);
            _lexer = new Lexer.Lexer();
            _parser = new Parser.Parser();
        }

        public void Execute()
        {
            lock (_stateLock)
            {
                _context.Reset();
                ApplyInitialGlobals();
                EnsureTokensAndStatements(withMetrics: false);

                var statements = _cachedStatements!;
                for (int i = 0; i < statements.Length; i++)
                {
                    var result = statements[i].Execute(_context);
                    if (result.ShouldStopExecution())
                    {
                        break;
                    }
                }
            }
        }

        public void ExecuteWithMetrics()
        {
            lock (_stateLock)
            {
                _context.Reset();
                ApplyInitialGlobals();
                var shouldMeasurePrep = _cachedTokens == null || _cachedStatements == null || (_lexerTime == 0 && _parserTime == 0);
                EnsureTokensAndStatements(withMetrics: shouldMeasurePrep);

                var sw = Stopwatch.StartNew();
                var statements = _cachedStatements!;
                for (int i = 0; i < statements.Length; i++)
                {
                    var result = statements[i].Execute(_context);
                    if (result.ShouldStopExecution())
                    {
                        break;
                    }
                }
                var executionTime = sw.ElapsedMilliseconds;

                Console.WriteLine($"\n=== EJECUCION: {executionTime} ms");
            }
        }

        public void SetInitialGlobal(string name, string type, object? value)
        {
            lock (_stateLock)
            {
                _initialGlobals[name] = (type, value);
            }
        }

        // Permite hidratar un nuevo script y forzar recalcular lexer/parser en la siguiente ejecucion.
        public void UpdateScript(string script)
        {
            lock (_stateLock)
            {
                _script = script ?? throw new ArgumentNullException(nameof(script));
                _cachedTokens = null;
                _cachedStatements = null;
                _lexerTime = 0;
                _parserTime = 0;
            }
        }

        // Prepara el script (lexer + parser) sin ejecutar.
        public void PrepareScript(bool withMetrics = false)
        {
            lock (_stateLock)
            {
                EnsureTokensAndStatements(withMetrics);
            }
        }

        public (long LexerTime, long ParserTime) GetPreparationMetrics()
        {
            lock (_stateLock)
            {
                return (_lexerTime, _parserTime);
            }
        }

        private void EnsureTokensAndStatements(bool withMetrics = false)
        {
            var sw = Stopwatch.StartNew();
            var tokensCached = _cachedTokens != null;
            if (!tokensCached)
            {
                _cachedTokens = _lexer.Tokenize(_script);
            }
            if (withMetrics && !tokensCached)
            {
                _lexerTime = sw.ElapsedMilliseconds;
            }

            sw.Restart();
            var statementsCached = _cachedStatements != null;
            if (!statementsCached)
            {
                _cachedStatements = _parser.Parse(_cachedTokens!).ToArray();
            }
            if (withMetrics && !statementsCached)
            {
                _parserTime = sw.ElapsedMilliseconds;
            }
        }

        private void ApplyInitialGlobals()
        {
            foreach (var kvp in _initialGlobals)
            {
                _context.DeclareVariable(kvp.Key, kvp.Value.Type, kvp.Value.Value);
            }
        }
    }
}
