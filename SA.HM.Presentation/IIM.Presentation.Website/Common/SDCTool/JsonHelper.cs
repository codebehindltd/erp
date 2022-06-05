using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InnBoardSDC.SDCTool
{
    /// <summary>
    /// Json Helper
    /// The open source project library must be referenced before use : Newtonsoft.Json.dll
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// Serialize the object to json format
        /// </summary>
        /// <param name="obj">Serialized object</param>
        /// <returns>json string</returns>
        public static string SerializeObjct(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Parse JSON string to generate object entities
        /// </summary>
        /// <typeparam name="T">Entity class</typeparam>
        /// <param name="json">String</param>
        /// <returns></returns>
        public static T JsonConvertObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Parse the JSON array to generate a collection of object entities
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="json">Array</param>
        /// <returns>Object entity collection</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object obj = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = obj as List<T>;
            return list;
        }

        /// <summary>
        /// Convert JSON to array
        /// usage:jsonArr[0]["xxxx"]
        /// </summary>
        /// <param name="json">json String</param>
        /// <returns></returns>
        public static JArray GetToJsonList(string json)
        {
            JArray jsonArr = (JArray)JsonConvert.DeserializeObject(json);
            return jsonArr;
        }

        /// <summary>
        /// Convert DataTable into entity class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DtConvertToModel<T>(DataTable dt) where T : new()
        {
            List<T> ts = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo pi in t.GetType().GetProperties())
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        var value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            switch (pi.PropertyType.FullName)
                            {
                                case "System.Decimal":
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                    break;
                                case "System.String":
                                    pi.SetValue(t, value.ToString(), null);
                                    break;
                                case "System.Int32":
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                    break;
                                default:
                                    pi.SetValue(t, value, null);
                                    break;
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    }
}
