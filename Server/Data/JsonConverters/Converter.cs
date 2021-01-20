using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RpgApp.Server.Data.JsonConverters
{
    public static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ClassTypeConverter.Singleton,
                EffectTypeConverter.Singleton,
                RarityConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
