using System;
using Newtonsoft.Json;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Server.Data.JsonConverters
{
    public class RarityConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rarity) || t == typeof(Rarity?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            return value switch
            {
                "Moderate" => Rarity.Moderate,
                "VeryCommon" => Rarity.VeryCommon,
                "Common" => Rarity.Common,
                "Rare" => Rarity.Rare,
                "VeryRare" => Rarity.VeryRare,
                _ => throw new Exception("Cannot unmarshal type Rarity")
            };
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rarity)untypedValue;
            switch (value)
            {
                case Rarity.Moderate:
                    serializer.Serialize(writer, "Moderate");
                    return;
                case Rarity.VeryCommon:
                    serializer.Serialize(writer, "VeryCommon");
                    return;
                case Rarity.Rare:
                    serializer.Serialize(writer, "Rare");
                    return;
                case Rarity.Common:
                    serializer.Serialize(writer, "Common");
                    return;
                case Rarity.VeryRare:
                    serializer.Serialize(writer, "VeryRare");
                    return;
            }
            throw new Exception("Cannot marshal type Rarity");
        }

        public static readonly RarityConverter Singleton = new RarityConverter();
    }
}
