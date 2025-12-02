namespace Interpreter.UseCases.ExternalFunction
{
    /// <summary>
    /// Servicio externo de ejemplo que aplica un factor de descuento y retorna una estructura.
    /// </summary>
    public class DiscountService
    {
        public Dictionary<string, object?> AplicarDescuento(object request)
        {
            // Soporta request como diccionario { monto, factor } o como lista/args posicionales.
            double monto;
            double factor;

            if (request is Dictionary<string, object?> dict)
            {
                monto = Convert.ToDouble(dict["monto"]);
                factor = Convert.ToDouble(dict["factor"]);
            }
            else if (request is List<object> list && list.Count >= 2)
            {
                monto = Convert.ToDouble(list[0]);
                factor = Convert.ToDouble(list[1]);
            }
            else
            {
                throw new ArgumentException("Request inválido para AplicarDescuento. Se espera { monto, factor }.");
            }

            var resultado = monto * factor;

            return new Dictionary<string, object?>
            {
                ["monto"] = monto,
                ["factor"] = factor,
                ["resultado"] = resultado,
                ["aplicadoEn"] = DateTime.UtcNow.ToString("O")
            };
        }
    }
}
