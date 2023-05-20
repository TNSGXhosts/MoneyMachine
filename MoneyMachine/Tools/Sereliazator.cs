using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace MoneyMachine.Tools
{
	public class Serializator
	{
        public static string SerializeObject<T>(T obj)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var jsonString = System.Text.Json.JsonSerializer.Serialize(obj, serializeOptions);
            return jsonString;
        }

        public static T DeserializeObject<T>(string? jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                throw new Exception("null response content.");
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            T obj = System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, serializeOptions);//JsonConvert.DeserializeObject<T>(jsonString);
            if (obj == null)
                throw new Exception($"Cannot deserialize response: {jsonString}");

            return obj;
        }

        //todo: update deserialization
        public static List<T> DeserializeListFieldByName<T>(string? jsonString, string fieldName)
        {
            if (string.IsNullOrEmpty(jsonString))
                throw new Exception("null response content");
            if (string.IsNullOrEmpty(fieldName))
                throw new Exception($"wrong field name: {jsonString} - {fieldName}");

            var parsedObj = JObject.Parse(jsonString);
            var fieldJson = parsedObj?[fieldName].ToString();
            return DeserializeObject<List<T>>(fieldJson);
        }

        public static T DeserializeFieldByName<T>(string? jsonString, string fieldName)
        {
            if (string.IsNullOrEmpty(jsonString))
                throw new Exception("null response content");
            if (string.IsNullOrEmpty(fieldName))
                throw new Exception($"wrong field name: {jsonString} - {fieldName}");

            var parsedObj = JObject.Parse(jsonString);
            var fieldJson = parsedObj?[fieldName].ToString();
            return DeserializeObject<T>(fieldJson);
        }
    }
}

