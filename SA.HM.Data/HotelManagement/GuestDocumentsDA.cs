using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
   public class GuestDocumentsDA :BaseService
   {


       public string GetImageName(string GuestId , string OwnerType)
       {
           string Path="";
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDocumentPathByGuestId"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.Int32, GuestId);

                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               Path = reader["Path"].ToString();

                           }
                       }
                   }
               }
               return Path;
           }
       }
   }
}
