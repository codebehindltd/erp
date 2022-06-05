using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public static class Extensions
    {
        public static List<ClassPropertyVeriance> CompareTwoCalssPropertyFindDifference<T>(this T class1, T class2)
        {
            List<ClassPropertyVeriance> variances = new List<ClassPropertyVeriance>();
            PropertyInfo[] fi = class1.GetType().GetProperties();

            foreach (PropertyInfo f in fi)
            {
                ClassPropertyVeriance v = new ClassPropertyVeriance();
                v.PropertyName = f.Name;
                v.Value1 = f.GetValue(class1, null);
                v.Value2 = f.GetValue(class2, null);

                if (v.Value1 != null)
                {
                    if (v.Value1.ToString() == "0")
                        v.Value1 = null;
                }

                if (v.Value2 != null)
                {
                    if (v.Value2.ToString() == "0")
                        v.Value2 = null;
                }

                if (v.Value1 != null && v.Value2 != null)
                {
                    if (!v.Value1.Equals(v.Value2))
                    {
                        variances.Add(v);
                    }
                }
                else if ((v.Value1 == null && v.Value2 != null) || (v.Value1 != null && v.Value2 == null))
                {
                    variances.Add(v);
                }
            }

            return variances;
        }
    }

}