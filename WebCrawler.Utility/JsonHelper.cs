using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCrawler.Utility
{
    public class JsonHelper
    {
        private static JsonSerializerSettings _jsonSettings;

        static JsonHelper()
        {
            IsoDateTimeConverterContent datetimeConverter = new IsoDateTimeConverterContent();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);
        }

        public static string SerializeObject(object obj)
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSettings);
        }


        public static T DeserializeObject<T>(string json)
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
        }
    }

    public class IsoDateTimeConverterContent : IsoDateTimeConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                if (dateTime == default(DateTime)
                    || dateTime == DateTime.MinValue
                    || dateTime.ToString("yyyy-MM-dd") == "1970-01-01"
                    || dateTime.ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    writer.WriteValue("");
                    return;
                }
            }
            base.WriteJson(writer, value, serializer);
        }
    }
}