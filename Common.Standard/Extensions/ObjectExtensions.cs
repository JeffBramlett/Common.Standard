using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Common.Standard.Extensions
{
    /// <summary>
    /// General collection of extension methods useful when working with
    /// any objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns version of the assembly where the object class is defined
        /// </summary>
        public static string GetAssemblyVersion(this object obj)
        {
            var objType = obj.GetType();
            var assembly = objType.Assembly;

            var version = assembly.GetName().Version;

            return version.ToString();
        }

        /// <summary>
        /// Do implicit cast and return if null or not
        /// </summary>
        /// <typeparam name="ToType">the type to implicitly cast to</typeparam>
        /// <param name="inObject">the class to extend</param>
        /// <param name="output">the resulting output (null if unsuccessful)</param>
        /// <returns>true if implicit cast is successful, false otherwise</returns>
        public static bool CastAs<ToType>(this object inObject, out ToType output) where ToType : class
        {
            output = inObject as ToType; // Using implicit cast (see method criteria disallowing data types) for efficiency
            return output != null;
        }

        /// <summary>
        /// Throw ArmumentNullException if the object is null
        /// </summary>
        /// <param name="obj">the object for the null check</param>
        /// <param name="argName">the argument name</param>
        public static void ThrowIfNull(this object obj, string argName)
        {
            if (obj == null)
                throw new ArgumentNullException(string.Format("{0} is null.", argName));
        }

        /// <summary>
        /// Throw ArgumentNullException if the object is null
        /// </summary>
        /// <param name="obj">the object to check for null</param>
        public static void ThrowIfNull(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Get the json serialization of this object
        /// </summary>
        /// <param name="obj">the object itself</param>
        /// <param name="settings">(optional) serialization settings</param>
        /// <returns>the object serialized to Json</returns>
        public static string ToJson(this object obj, JsonSerializerSettings settings = null)
        {
            if (obj == null)
                throw new ArgumentNullException();
            if (settings != null)
            {
                return JsonConvert.SerializeObject(obj);
            }
            else
            {
                return JsonConvert.SerializeObject(obj, settings);
            }
        }

        /// <summary>
        /// Get the json serialization of this object
        /// </summary>
        /// <param name="obj">the object itself</param>
        /// <param name="converters">Collection of converters</param>
        /// <returns>the object serialized to Json</returns>
        public static string ToJson(this object obj, params JsonConverter[] converters)
        {
            if (obj == null)
                throw new ArgumentNullException();

            return JsonConvert.SerializeObject(obj, converters);
        }

        /// <summary>
        /// Get the xml serialization of this object
        /// </summary>
        /// <param name="obj">the object itself</param>
        /// <returns>the object serialized to Xml</returns>
        public static string ToXml(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            XmlSerializer xs = new XmlSerializer(obj.GetType());
            using (StringWriter sw = new StringWriter())
            {
                xs.Serialize(sw, obj);
                sw.Flush();
                return sw.ToString();

            }
        }
    }
}
