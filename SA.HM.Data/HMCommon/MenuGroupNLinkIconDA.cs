using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class MenuGroupNLinkIconDA : BaseService
    {
        public List<MenuGroupNLinkIconBO> GetIconList()
        {
            List<MenuGroupNLinkIconBO> iconList = new List<MenuGroupNLinkIconBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIconList_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "IconList");
                    DataTable Table = SaleServiceDS.Tables["IconList"];

                    iconList = Table.AsEnumerable().Select(r => new MenuGroupNLinkIconBO
                    {
                        IconId = r.Field<Int64>("IconId"),
                        Name = r.Field<string>("Name"),
                        Class = r.Field<string>("Class"),
                        Code = r.Field<string>("Code")

                    }).ToList();
                }
            }
            return iconList;
        }
    }
}
