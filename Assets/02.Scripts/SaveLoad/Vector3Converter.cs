using UnityEngine;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var x = (float)jObject["x"];
        var y = (float)jObject["y"];
        var z = (float)jObject["z"];
        return new Vector3(x, y, z);
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.Formatting = Formatting.Indented;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }
}