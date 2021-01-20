using System;
using Newtonsoft.Json;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Server.Data.JsonConverters
{
    public class ClassTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ClassType) || t == typeof(ClassType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            return value switch
            {
                "Mage" => ClassType.Mage,
                "Ranger" => ClassType.Ranger,
                "Warrior" => ClassType.Warrior,
                _ => throw new Exception("Cannot unmarshal type ClassType")
            };
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ClassType)untypedValue;
            switch (value)
            {
                case ClassType.Mage:
                    serializer.Serialize(writer, "Mage");
                    return;
                case ClassType.Ranger:
                    serializer.Serialize(writer, "Ranger");
                    return;
                case ClassType.Warrior:
                    serializer.Serialize(writer, "Warrior");
                    return;
            }
            throw new Exception("Cannot marshal type ClassType");
        }

        public static readonly ClassTypeConverter Singleton = new ClassTypeConverter();
    }
}
