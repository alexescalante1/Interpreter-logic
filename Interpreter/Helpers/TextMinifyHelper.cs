using System.Text.RegularExpressions;

namespace Interpreter.Helpers
{
    public static class TextMinifyHelper
    {
        /// <summary>
        /// Minifica un script eliminando saltos de línea, espacios innecesarios
        /// y filtrando líneas vacías.
        /// </summary>
        public static string Minify(string script)
        {
            if (string.IsNullOrWhiteSpace(script))
                return string.Empty;

            var cleaned = script
                .Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line));

            var result = string.Join(" ", cleaned);

            result = Regex.Replace(result, @"\s+", " ");

            return result;
        }
    }
}
