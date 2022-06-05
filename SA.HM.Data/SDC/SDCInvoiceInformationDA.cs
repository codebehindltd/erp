using HotelManagement.Entity.SDC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SDC
{
    public class SDCInvoiceInformationDA : BaseService
    {
        public bool SaveSDCInvoiceInformation(SDCInvoiceInformationBO sdcBo, out int SDCInvoiceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SDCInvoiceInformation_Insert_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, sdcBo.BillId);
                    dbSmartAspects.AddInParameter(command, "@SDCInvoiceNumber", DbType.String, sdcBo.SDCInvoiceNumber);
                    dbSmartAspects.AddInParameter(command, "@QRCode", DbType.String, sdcBo.QRCode);
                    dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, sdcBo.BillType);
                    dbSmartAspects.AddOutParameter(command, "@SDCInvoiceId", DbType.Int32, sizeof(Int32));
                    
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    SDCInvoiceId = Convert.ToInt32(command.Parameters["@SDCInvoiceId"].Value);
                    
                }
            }
            return status;
        }
    }
}
