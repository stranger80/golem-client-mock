using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class PropertyMappers
    {
        public static string MapToJsonString(IDictionary<string, JToken> props)
        {
            return JsonConvert.SerializeObject(props);
        }

        public static IDictionary<string, JToken> MapFromJsonString(string propertiesJson)
        {
            var deserialized = JsonConvert.DeserializeObject(propertiesJson) as JObject;
            var result = new Dictionary<string, JToken>();

            foreach(var child in deserialized.Children())
            {
                var property = child as JProperty;

                if (property != null)
                {
                    ProcessJProperty(property.Name, property, result);
                }

            }

            return result;
        }

        /// <summary>
        /// Recursive traversal of json structure to extract properties.
        /// Accumulates the collected properties in the dictionary.
        /// </summary>
        /// <param name="parentPath"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected static void ProcessJProperty(string parentPath, JProperty property, IDictionary<string, JToken> properties)
        {
            if(property.Value is JObject)
            {
                foreach (var child in property.Value.Children())
                {
                    var childProp = child as JProperty;

                    if (childProp != null)
                    {
                        ProcessJProperty(parentPath + "." + childProp.Name, childProp, properties);
                    }
                }

            }
            else
            {
                properties.Add(parentPath, property.Value);
            }
        }
    }
}
