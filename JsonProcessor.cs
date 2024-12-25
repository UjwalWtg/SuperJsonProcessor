
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace SuperJsonProcessor
{
    public class SuperJsonProcessor
    {
        /// <summary>
        /// Flattens a nested JSON object into key-value pairs.
        /// </summary>
        public static Dictionary<string, object> FlattenJson(string json)
        {
            var result = new Dictionary<string, object>();
            var jObject = JObject.Parse(json);

            void Recurse(JToken token, string prefix)
            {
                if (token is JValue value)
                {
                    result[prefix] = value.Value;
                }
                else if (token is JObject obj)
                {
                    foreach (var property in obj.Properties())
                    {
                        Recurse(property.Value, string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}");
                    }
                }
                else if (token is JArray array)
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        Recurse(array[i], $"{prefix}[{i}]");
                    }
                }
            }

            Recurse(jObject, string.Empty);
            return result;
        }

        /// <summary>
        /// Unflattens a flattened JSON dictionary back into its nested structure.
        /// </summary>
        public static string UnflattenJson(Dictionary<string, object> flatJson)
        {
            var result = new JObject();

            foreach (var pair in flatJson)
            {
                var keys = pair.Key.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                JObject current = result;

                for (int i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    var isLast = i == keys.Length - 1;

                    if (int.TryParse(key, out int index))
                    {
                        if (!(current.Last is JArray array))
                        {
                            array = new JArray();
                            current[keys[i - 1]] = array;
                        }

                        while (array.Count <= index)
                        {
                            array.Add(null);
                        }

                        if (isLast)
                        {
                            array[index] = JToken.FromObject(pair.Value);
                        }
                        else
                        {
                            if (!(array[index] is JObject nestedObject))
                            {
                                nestedObject = new JObject();
                                array[index] = nestedObject;
                            }

                            current = nestedObject;
                        }
                    }
                    else
                    {
                        if (!current.ContainsKey(key))
                        {
                            current[key] = isLast ? JToken.FromObject(pair.Value) : new JObject();
                        }

                        if (!isLast)
                        {
                            current = (JObject)current[key];
                        }
                    }
                }
            }

            return result.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Validates JSON against a schema (not implemented in detail, replace with an actual schema validation library if needed).
        /// </summary>
        public static bool ValidateJson(string json, string schema)
        {
            // Load the JSON schema
            JSchema schemaObject = JSchema.Parse(schema);

            // Load the JSON document
            JObject jsonObject = JObject.Parse(json);

            // Validate the JSON against the schema
            bool isValid = jsonObject.IsValid(schemaObject);

            // Return the validation result
            return isValid;
        }

        /// <summary>
        /// Streams large JSON payloads.
        /// </summary>
        public static void ProcessLargeJson(string jsonFilePath, Action<JToken> processAction)
        {
            using (var reader = File.OpenText(jsonFilePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.StartObject)
                    {
                        var token = JToken.Load(jsonReader);
                        processAction(token);
                    }
                }
            }
        }
    }

}
