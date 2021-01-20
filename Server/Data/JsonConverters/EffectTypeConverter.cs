using System;
using Newtonsoft.Json;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Server.Data.JsonConverters
{
    public class EffectTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(EffectType) || t == typeof(EffectType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            return value switch
            {
                "Attack" => EffectType.Attack,
                "Defend" => EffectType.Defend,
                "Modify" => EffectType.Modify,
                "Heal" => EffectType.Heal,
                _ => throw new Exception("Cannot unmarshal type TypeEnum")
            };
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (EffectType)untypedValue;
            switch (value)
            {
                case EffectType.Attack:
                    serializer.Serialize(writer, "Attack");
                    return;
                case EffectType.Defend:
                    serializer.Serialize(writer, "Defend");
                    return;
                case EffectType.Modify:
                    serializer.Serialize(writer, "Modify");
                    return;
                case EffectType.Heal:
                    serializer.Serialize(writer, "Heal");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly EffectTypeConverter Singleton = new EffectTypeConverter();
    }
}
