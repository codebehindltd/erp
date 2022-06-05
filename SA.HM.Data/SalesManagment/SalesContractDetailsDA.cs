using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.SalesManagment;

namespace HotelManagement.Data.SalesManagment
{
   public class SalesContractDetailsDA :BaseService
    {

       public bool SaveSalesContractDetailsInfo(SalesContractDetailsBO contactBO, out int tmpOrderId)
       {
           bool retVal = false;
           int status = 0;
           int tmpContractDetailsId;
           //int tmpRegistrationId = 0;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveSalesContractDetailsInfo_SP"))
               {
                   conn.Open();
                   using (DbTransaction transction = conn.BeginTransaction())
                   {

                       dbSmartAspects.AddInParameter(commandMaster, "@CustomerId", DbType.String, contactBO.CustomerId);
                       dbSmartAspects.AddInParameter(commandMaster, "@SigningDate", DbType.DateTime, contactBO.SigningDate);
                       dbSmartAspects.AddInParameter(commandMaster, "@ExpiryDate", DbType.DateTime, contactBO.ExpiryDate);
                       dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.String, contactBO.CreatedBy);

                       dbSmartAspects.AddOutParameter(commandMaster, "@ContractDetailsId", DbType.Int32, sizeof(Int32));
                       status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                       tmpContractDetailsId = Convert.ToInt32(commandMaster.Parameters["@ContractDetailsId"].Value);
                       tmpOrderId = tmpContractDetailsId;
                       if (status > 0)
                       {
                           using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeDocumentAndSignature_SP"))
                           {
                               commandDocument.Parameters.Clear();
                               dbSmartAspects.AddInParameter(commandDocument, "@EmpId", DbType.Int32, tmpContractDetailsId);
                               dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.String, contactBO.tempContractDetailsId);
                               status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                           }

                           transction.Commit();
                           retVal = true;

                       }
                       else
                       {
                           retVal = false;
                       }
                   }
               }
           }
           return retVal;
       }
        public bool UpdateSalesContractDetailsInfo(Entity.SalesManagment.SalesContractDetailsBO contactBO)
        {
            throw new NotImplementedException();
        }
    }
}
