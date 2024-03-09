using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ServiceLayer.Helpers
{
    internal static class SecretsHelper
    {
        /// <summary>
        /// Retrieves a secret value from the specified configuration section and key using user secrets.
        /// </summary>
        /// <typeparam name="T">The type of the class whose user secrets are being accessed.</typeparam>
        /// <param name="section">The name of the configuration section containing the secret.</param>
        /// <param name="key">The key of the secret within the specified section.</param>
        /// <exception cref="InvalidOperationException">Thrown when the specified section or key is not found in the configuration.</exception>
        /// <returns>The secret value associated with the specified section and key.</returns>
        public static string GetSecret<T>(string section, string key) where T : class
        {
            var assembly = Assembly.GetExecutingAssembly();

            using Stream stream = assembly.GetManifestResourceStream("ServiceLayer.secrets.json");
            if (stream == null)
            {
                Console.WriteLine("Ресурс не найден.");
            }

            var serializer = new JsonSerializer();

            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);

            var jsObj = (JObject)serializer.Deserialize(jsonTextReader);
            var sectionValue = jsObj.GetValue(section);

            if (sectionValue is null)
            {
                throw new InvalidOperationException($"Section '{section}' not found in secrets");
            }

            var value = sectionValue[key];
            if (value is null)
            {
                throw new InvalidOperationException($"Key '{key}' not found in Section {section}");
            }

            return value.ToString();
        }
    }
}
