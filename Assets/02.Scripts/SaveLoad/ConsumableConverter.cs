using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ConsumableConverter : JsonConverter<PlayerInventory.Consumable>
{
    public override PlayerInventory.Consumable ReadJson(JsonReader reader, Type objectType, PlayerInventory.Consumable existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var id = (string)jObject["id"];
        var count = (int)jObject["count"];
        PlayerInventory.Consumable consumable;
        consumable.id = id;
        consumable.count = count;
        return consumable;
    }

    public override void WriteJson(JsonWriter writer, PlayerInventory.Consumable value, JsonSerializer serializer)
    {
        writer.Formatting = Formatting.Indented;
        writer.WriteStartObject();
        writer.WritePropertyName("id");
        writer.WriteValue(value.id);
        writer.WritePropertyName("count");
        writer.WriteValue(value.count);
        writer.WriteEndObject();
    }
}
