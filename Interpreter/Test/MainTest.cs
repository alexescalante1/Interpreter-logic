using Interpreter.Interface;
using Interpreter.UseCases.ExternalFunction;
using Interpreter.UseCases.InternalFunction;

namespace Interpreter.Test
{
    static class MainTest
    {
        private static readonly DiscountService DiscountService = new();
        private static readonly RecommendationService RecommendationService = new();
        private static readonly ExternalFunction<DiscountService> AplicarDescuentoFunction =
            ExternalFunction<DiscountService>.From(
                DiscountService,
                (svc, args, ctx) =>
                {
                    if (args.Count < 1)
                        throw new Exception("AplicarDescuento requiere un objeto { monto, factor }");

                    return svc.AplicarDescuento(args[0]);
                });
        private static readonly ExternalFunction<RecommendationService> TopCategoriasFunction =
            ExternalFunction<RecommendationService>.From(
                RecommendationService,
                (svc, args, ctx) =>
                {
                    var payload = args.Count > 0 ? args[0] : new Dictionary<string, object?>();
                    return svc.ObtenerTopCategorias(payload);
                });

        private static readonly Dictionary<string, IInternalFunction> InternalFunctions = new()
        {
            ["Mostrar"] = new MostrarFunction(),
            ["ListAdd"] = new ListAddFunction(),
            ["ListGet"] = new ListGetFunction(),
            ["ListCount"] = new ListCountFunction(),
            ["ListClear"] = new ListClearFunction(),
            ["ListRemoveAt"] = new ListRemoveAtFunction(),
            ["JsonParse"] = new JsonParseFunction(),
            ["JsonStringify"] = new JsonStringifyFunction(),
            ["AplicarDescuento"] = AplicarDescuentoFunction,
            ["TopCategorias"] = TopCategoriasFunction
        };

        public static void Examples()
        {
            try
            {
                //InterpreterTest.RunTestSuiteCompleta(InternalFunctions);
                //InterpreterTest.RunTestMutacionBasica(InternalFunctions);
                //InterpreterTest.RunTestControlFlow(InternalFunctions);
                //InterpreterTest.RunTestColecciones(InternalFunctions);
                //InterpreterTest.RunTestJson(InternalFunctions);
                //InterpreterTest.RunTestJsonExterno(InternalFunctions);
                //InterpreterTest.RunTestTopCategorias(InternalFunctions);
                InterpreterTest.RunTestJsonExternoSinHidratacion(InternalFunctions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nMensaje: {ex.Message}");
                Console.WriteLine($"\nStack Trace:\n{ex.StackTrace}");
            }

            Console.WriteLine("\n--- Presiona cualquier tecla para salir ---");
            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }
        }
    }
}
