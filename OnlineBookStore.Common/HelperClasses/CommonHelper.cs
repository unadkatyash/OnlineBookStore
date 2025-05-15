using System.Dynamic;
using System.Reflection;

namespace OnlineBookStore.Common.HelperClasses
{
    /// <summary>
    /// Helper class for common date-time related operations.
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// Gets the current UTC date and time.
        /// </summary>
        /// <returns>Current UTC date and time.</returns>
        public static DateTime GetCurrentDateTimeUTC()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the current local date and time of the server.
        /// </summary>
        /// <returns>Current local date and time.</returns>
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Converts a given DateTime to UTC.
        /// </summary>
        /// <param name="localDateTime">The local DateTime to convert.</param>
        /// <returns>Converted UTC DateTime.</returns>
        public static DateTime ConvertToUTC(DateTime localDateTime)
        {
            return localDateTime.ToUniversalTime();
        }

        /// <summary>
        /// Converts a given UTC DateTime to local time based on the server's time zone.
        /// </summary>
        /// <param name="utcDateTime">The UTC DateTime to convert.</param>
        /// <returns>Converted local DateTime.</returns>
        public static DateTime ConvertToLocalTime(DateTime utcDateTime)
        {
            return utcDateTime.ToLocalTime();
        }
        public static List<T> ConvertToList<T>(List<Dictionary<string, object>> dataList) where T : new()
        {
            var result = new List<T>();

            foreach (var dict in dataList)
            {
                T obj = new T();
                foreach (var key in dict.Keys)
                {
                    PropertyInfo prop = typeof(T).GetProperty(key);
                    if (prop != null && dict[key] != null)
                    {
                        prop.SetValue(obj, Convert.ChangeType(dict[key], prop.PropertyType));
                    }
                }
                result.Add(obj);
            }

            return result;
        }

        public static List<ExpandoObject> ConvertToDynamicModel(List<Dictionary<string, object>> data)
        {
            var list = new List<ExpandoObject>();
            foreach (var dict in data)
            {
                IDictionary<string, object> expando = new ExpandoObject();
                foreach (var kvp in dict)
                {
                    expando[kvp.Key] = kvp.Value;
                }
                list.Add((ExpandoObject)expando);
            }
            return list;
        }
    }
}
