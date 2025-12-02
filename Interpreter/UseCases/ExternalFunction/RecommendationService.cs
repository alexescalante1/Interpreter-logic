namespace Interpreter.UseCases.ExternalFunction
{
    /// <summary>
    /// Servicio externo de ejemplo que retorna una lista de recomendaciones.
    /// </summary>
    public class RecommendationService
    {
        public List<object> ObtenerTopCategorias(object request)
        {
            // Acepta request como diccionario opcional con preferencia del cliente.
            string preferencia = "general";
            if (request is Dictionary<string, object?> dict && dict.TryGetValue("preferencia", out var pref) && pref != null)
            {
                preferencia = pref.ToString() ?? "general";
            }

            var lista = new List<object>
            {
                new Dictionary<string, object?> { ["categoria"] = "tecnologia", ["score"] = 95, ["preferencia"] = preferencia },
                new Dictionary<string, object?> { ["categoria"] = "hogar", ["score"] = 88, ["preferencia"] = preferencia },
                new Dictionary<string, object?> { ["categoria"] = "deportes", ["score"] = 82, ["preferencia"] = preferencia }
            };

            return lista;
        }
    }
}
