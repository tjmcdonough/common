using Airslip.Common.Types;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Airslip.Common.Utilities.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<T> DeserializeStream<T>(this Stream requestBody) where T : class
        {
            StreamReader sr = new(requestBody);
            string payload = await sr.ReadToEndAsync();
            return Json.Deserialize<T>(payload);
        }

        public static async Task<T> DeserializeStream<T>(this Stream stream, bool resetStream) where T : class
        {
            StreamReader sr = new(stream);
            string payload = await sr.ReadToEndAsync();
            if (resetStream)
                stream.Position = 0;
            return Json.Deserialize<T>(payload);
        }
        
        public static T DeserializeFunctionStream<T>(this Stream requestBody) where T : class
        {
            StreamReader sr = new(requestBody);
            string payload = sr.ReadToEnd();
            return Json.Deserialize<T>(payload);
        }

        public static async Task<T> DeserializeStream<T>(
            this Stream requestBody,
            Casing casing,
            Formatting formatting = Formatting.None,
            NullValueHandling nullValueHandling = NullValueHandling.Include) where T : class
        {
            StreamReader sr = new(requestBody);
            string payload = await sr.ReadToEndAsync();
            return Json.Deserialize<T>(payload, casing, formatting, nullValueHandling);
        }

        public static async Task<T> DeserializeStream<T>(
            this Stream stream,
            bool resetStream,
            Casing casing,
            Formatting formatting = Formatting.None,
            NullValueHandling nullValueHandling = NullValueHandling.Include) where T : class
        {
            StreamReader sr = new(stream);
            string payload = await sr.ReadToEndAsync();
            if (resetStream)
                stream.Position = 0;
            return Json.Deserialize<T>(payload, casing, formatting, nullValueHandling);
        }

        public static string ReadStream(this Stream requestBody)
        {
            StreamReader sr = new(requestBody);
            return sr.ReadToEnd();
        }

        public static string ReadStream(this Stream stream, bool resetStream)
        {
            StreamReader sr = new(stream);

            string payload = sr.ReadToEnd();

            if (resetStream)
                stream.Position = 0;

            return payload;
        }

        public static TType GetEmbeddedResourceAsType<TType>(
            this Assembly assembly,
            string resourceName,
            Casing casing = Casing.CAMEL_CASE,
            Formatting formatting = Formatting.None)
        {
            string? resourceContent = null;
            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
                if (stream != null)
                {
                    using StreamReader reader = new(stream);
                    resourceContent = reader.ReadToEnd();
                }

            if (resourceContent == null)
                throw new NullReferenceException("Resource not found in assembly");

            return Json.Deserialize<TType>(resourceContent, casing, formatting);
        }
    }
}