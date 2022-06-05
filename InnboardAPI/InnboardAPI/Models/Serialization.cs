using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountingDomain.Common
{
    public class Serialization
    {
        public static object GetObjectWithoutRefLooping<T>(T Object)
        {
            var result = JsonConvert.SerializeObject(Object, Formatting.Indented,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               }
               );
            return JsonConvert.DeserializeObject(result);
        }

        public static T GetObjectFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        //public static object ToResponseModelWithoutRefLooping<T>(T model)
        //{
        //    var responseModel = new Response<T>() { Data = model };
        //    return GetObjectWithoutRefLooping(responseModel);
        //}
 
    }
}