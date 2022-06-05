using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Utility
{
    public static class CommonHelper
    {
        public static T ConvertDateTimePropertiesUTCtoLocalTime<T>(T entity) where T : class
        {
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var entityobject = property.GetValue(entity);

                if (entityobject == null || entityobject.GetType().IsPrimitive)
                    continue;
                else if (property.PropertyType == typeof(DateTime) || (property.PropertyType == typeof(DateTime?)
                                && property.GetValue(entity, null) != null) && property.CanWrite)
                {

                    property.SetValue(entity, ((DateTime)property.GetValue(entity, null)).ToLocalTime(), null);

                }
                else if (entityobject.GetType().Name == typeof(List<>).Name)
                {
                    var nestedList = property.GetValue(entity);

                    foreach (var item in (IList)nestedList)
                    {
                        var nestedProperties = item.GetType().GetProperties();

                        foreach (var prop in nestedProperties)
                        {
                            if (prop.PropertyType == typeof(DateTime) || (prop.PropertyType == typeof(DateTime?)
                                            && prop.GetValue(item, null) != null) && prop.CanWrite)
                                prop.SetValue(item, ((DateTime)prop.GetValue(item, null)).ToLocalTime(), null);
                        }
                    }
                }
                else
                {
                    var nestedProperties = entityobject.GetType().GetProperties();
                    foreach (var item in nestedProperties)
                    {
                        if (item.PropertyType == typeof(DateTime) || (item.PropertyType == typeof(DateTime?)
                                    && item.GetValue(entityobject, null) != null) && property.CanWrite)
                            item.SetValue(entityobject, ((DateTime)item.GetValue(entityobject, null)).ToLocalTime(), null);
                    }
                }
            }
            return entity;
        }
    }
}
