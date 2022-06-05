using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HotelManagement.Entity.HMCommon
{
    public class AutoSearch<T> where T : class
    {
        public List<AutoSearchBO> AutoSearchPrepare(List<T> data, string displayFieldColumnName, string valueFieldColumnName)
        {
            List<AutoSearchBO> autoSearchList = new List<AutoSearchBO>();
            AutoSearchBO searchBo;

            PropertyInfo displayFiledInfo, dataFiledInfo;

            foreach (T obj in data)
            {
                displayFiledInfo = obj.GetType().GetProperty(displayFieldColumnName);
                dataFiledInfo = obj.GetType().GetProperty(valueFieldColumnName);

                searchBo = new AutoSearchBO();
                searchBo.DisplayField = displayFiledInfo.GetValue(obj, null) as string;
                searchBo.ValueField = Convert.ToString(dataFiledInfo.GetValue(obj, null) as int?);

                autoSearchList.Add(searchBo);
            }

            return autoSearchList;
        }
    }
}
