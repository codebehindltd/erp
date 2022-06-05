using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.SalesManagment;
using System.Collections;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Data.SalesManagment
{
    public class PMSalesDetailsDA :BaseService
    {
        public int SavePMSalesInfo(PMSalesBO salesBO, out int tmpSalesId, List<PMSalesDetailBO> detailBO ,List<PMSalesBillPaymentBO> guestPaymentDetailList, List<PMProductOutBO> pmProductOutList)
        {
            bool retVal = false;
            int status = 0;
            decimal dueOrAdvanceAmount = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SavePMSalesInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@CustomerId", DbType.Int32, salesBO.CustomerId);
                        dbSmartAspects.AddInParameter(commandMaster, "@SalesDate", DbType.DateTime, salesBO.SalesDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@BillExpireDate", DbType.DateTime, salesBO.BillExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Frequency", DbType.String, salesBO.Frequency);
                        dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, salesBO.VatAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@SalesAmount", DbType.Decimal, salesBO.SalesAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@GrandTotal", DbType.Decimal, salesBO.GrandTotal);
                        dbSmartAspects.AddInParameter(commandMaster, "@FieldId", DbType.Int32, salesBO.FieldId);

                        if (guestPaymentDetailList != null)
                        {                            
                            decimal guestPaymentAmount = 0;
                            foreach (PMSalesBillPaymentBO guestBillPaymentBOForDueAmount in guestPaymentDetailList)
                            {
                                guestPaymentAmount = guestPaymentAmount + guestBillPaymentBOForDueAmount.PaymentAmout;
                            }

                            dueOrAdvanceAmount = salesBO.GrandTotal - guestPaymentAmount;
                        }

                        dbSmartAspects.AddInParameter(commandMaster, "@DueOrAdvanceAmount", DbType.Decimal, dueOrAdvanceAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@SiteInfoId", DbType.Int32, salesBO.SiteInfoId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BillingInfoId", DbType.Int32, salesBO.BillingInfoId);
                        dbSmartAspects.AddInParameter(commandMaster, "@TechnicalInfoId", DbType.Int32, salesBO.TechnicalInfoId);


                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, salesBO.Remarks);             
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, salesBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster,"@SalesId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpSalesId = Convert.ToInt32(commandMaster.Parameters["@SalesId"].Value);

                        if (status > 0)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveSalesDetailsInfo_SP"))
                            {
                                foreach (PMSalesDetailBO salesDetailBO in detailBO)
                                {
                                    if (salesDetailBO.SalesId == 0)
                                    {
                                        commandDetails.Parameters.Clear();
                                        
                                        dbSmartAspects.AddInParameter(commandDetails, "@SalesId", DbType.Int32, tmpSalesId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ServiceType", DbType.String, salesDetailBO.ServiceType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, salesDetailBO.ItemId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, salesDetailBO.ItemUnit);
                                        dbSmartAspects.AddInParameter(commandDetails, "@SellingLocalCurrencyId", DbType.Int32, salesDetailBO.SellingLocalCurrencyId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPriceLocal", DbType.Decimal, salesDetailBO.UnitPriceLocal);
                                        dbSmartAspects.AddInParameter(commandDetails, "@SellingUsdCurrencyId", DbType.Int32, salesDetailBO.SellingUsdCurrencyId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPriceUsd", DbType.Decimal, salesDetailBO.UnitPriceUsd);


                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (guestPaymentDetailList != null)
                            {
                                int countGuestBillPaymentList = 0;
                                //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SavePMSalesBillPaymentInfo_SP"))
                                {
                                    foreach (PMSalesBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CustomerId", DbType.Int32, salesBO.CustomerId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmout", DbType.Decimal, guestBillPaymentBO.PaymentAmout);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);  
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@NodeId", DbType.String, guestBillPaymentBO.NodeId);

                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        countGuestBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);

                                        //int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                                    }
                                }
                            }

                            //------------------------------------Product Out Information------------------------------------------
                            if (pmProductOutList != null)
                            {
                                int countPMProductOutBO = 0;
                                //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                using (DbCommand commandPMProductOutBO = dbSmartAspects.GetStoredProcCommand("SaveAndUpdateProductOutInfo_SP"))
                                {
                                    foreach (PMProductOutBO pmProductOutBO in pmProductOutList)
                                    {
                                        commandPMProductOutBO.Parameters.Clear();

                                        //dbSmartAspects.AddInParameter(commandPMProductOutBO, "@ProductId", DbType.Int32, pmProductOutBO.ProductId);
                                        //dbSmartAspects.AddInParameter(commandPMProductOutBO, "@SerialNumber", DbType.String, pmProductOutBO.SerialNumber);
                                        //dbSmartAspects.AddInParameter(commandPMProductOutBO, "@Quantity", DbType.String, pmProductOutBO.Quantity);
                                        dbSmartAspects.AddInParameter(commandPMProductOutBO, "@SalesId", DbType.Int32, tmpSalesId);
                                        dbSmartAspects.AddInParameter(commandPMProductOutBO, "@OutFor", DbType.Int32, pmProductOutBO.OutFor);
                                        dbSmartAspects.AddInParameter(commandPMProductOutBO, "@Remarks", DbType.String, pmProductOutBO.Remarks);
                                        dbSmartAspects.AddInParameter(commandPMProductOutBO, "@CreatedBy", DbType.Int32, pmProductOutBO.CreatedBy);

                                        countPMProductOutBO += dbSmartAspects.ExecuteNonQuery(commandPMProductOutBO, transction);

                                        //int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                                    }
                                }
                            }
                            //----------------------------End Product Out Information----------------------------------------
                            //new end
                            if (count == detailBO.Count)
                            {
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return tmpSalesId;
        }
        public bool UpdatePMSalesInfo(PMSalesBO salesBO, List<PMSalesDetailBO> detailBO, List<PMSalesDetailBO> deleteDetailBO, ArrayList arrayDelete)
        {
            bool retVal = false;
            int status = 0;
            int tmpSalesId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdatePMSalesInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@SalesId", DbType.Int32, salesBO.SalesId);
                        dbSmartAspects.AddInParameter(commandMaster, "@CustomerId", DbType.Int32, salesBO.CustomerId);
                        dbSmartAspects.AddInParameter(commandMaster, "@SalesDate", DbType.DateTime, salesBO.SalesDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Frequency", DbType.String, salesBO.Frequency);
                        dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, salesBO.VatAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@SalesAmount", DbType.Decimal, salesBO.SalesAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@GrandTotal", DbType.Decimal, salesBO.GrandTotal);
                        dbSmartAspects.AddInParameter(commandMaster, "@FieldId", DbType.Int32, salesBO.FieldId);

                        dbSmartAspects.AddInParameter(commandMaster, "@SiteInfoId", DbType.Int32, salesBO.SiteInfoId);
                        dbSmartAspects.AddInParameter(commandMaster, "@TechnicalInfoId", DbType.Int32, salesBO.TechnicalInfoId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BillingInfoId", DbType.Int32, salesBO.BillingInfoId);



                        //dbSmartAspects.AddInParameter(commandMaster, "@SiteName", DbType.String, salesBO.SiteName);
                        //dbSmartAspects.AddInParameter(commandMaster, "@SiteAddress", DbType.String, salesBO.SiteAddress);
                        //dbSmartAspects.AddInParameter(commandMaster, "@SiteContactPerson", DbType.String, salesBO.SiteContactPerson);
                        //dbSmartAspects.AddInParameter(commandMaster, "@SitePhoneNumber", DbType.String, salesBO.SitePhoneNumber);
                        //dbSmartAspects.AddInParameter(commandMaster, "@SiteEmail", DbType.String, salesBO.SiteEmail);

                        //dbSmartAspects.AddInParameter(commandMaster, "@BillingContactPerson", DbType.String, salesBO.BillingContactPerson);
                        //dbSmartAspects.AddInParameter(commandMaster, "@BillingPersonDepartment", DbType.String, salesBO.BillingPersonDepartment);
                        //dbSmartAspects.AddInParameter(commandMaster, "@BillingPersonDesignation", DbType.String, salesBO.BillingPersonDesignation);
                        //dbSmartAspects.AddInParameter(commandMaster, "@BillingPersonPhone", DbType.String, salesBO.BillingPersonPhone);
                        //dbSmartAspects.AddInParameter(commandMaster, "@BillingPersonEmail", DbType.String, salesBO.BillingPersonEmail);

                        //dbSmartAspects.AddInParameter(commandMaster, "@TechnicalContactPerson", DbType.String, salesBO.TechnicalContactPerson);
                        //dbSmartAspects.AddInParameter(commandMaster, "@TechnicalPersonDepartment", DbType.String, salesBO.TechnicalPersonDepartment);
                        //dbSmartAspects.AddInParameter(commandMaster, "@TechnicalPersonDesignation", DbType.String, salesBO.TechnicalPersonDesignation);
                        //dbSmartAspects.AddInParameter(commandMaster, "@TechnicalPersonPhone", DbType.String, salesBO.TechnicalPersonPhone);
                        //dbSmartAspects.AddInParameter(commandMaster, "@TechnicalPersonEmail", DbType.String, salesBO.TechnicalPersonEmail);

                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, salesBO.Remarks);

                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, salesBO.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpSalesId = salesBO.SalesId;
                        if (status > 0)
                        {
                            int count = 0;
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveSalesDetailsInfo_SP"))
                            {
                                foreach (PMSalesDetailBO saleseDetailBO in detailBO)
                                {
                                    if (saleseDetailBO.DetailId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@SalesId", DbType.Int32, tmpSalesId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ServiceType", DbType.String, saleseDetailBO.ServiceType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, saleseDetailBO.ItemId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, saleseDetailBO.ItemUnit);
                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateSalesDetailsInfo_SP"))
                            {

                                foreach (PMSalesDetailBO saleseDetailBO in detailBO)
                                {
                                    if (saleseDetailBO.SalesId != 0)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@DetailId", DbType.Int32, saleseDetailBO.DetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ServiceType", DbType.String, saleseDetailBO.ServiceType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, saleseDetailBO.ItemId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, saleseDetailBO.ItemUnit);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPriceLocal", DbType.Decimal, saleseDetailBO.UnitPriceLocal);
                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                            if (count == detailBO.Count)
                            {
                                if (arrayDelete.Count > 0)
                                {
                                    foreach (int delId in arrayDelete)
                                    {
                                        using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                        {
                                            dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "PMSalesDetail");
                                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "DetailId");
                                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);
                                            status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                        }
                                    }
                                }
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
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
        public PMSalesBO GetSalesInformationBySalesId(int salesId)
        {
            PMSalesBO salesBO = new PMSalesBO();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesInformationBySalesId_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@SalesId", DbType.Int32, salesId);
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               salesBO.SalesId = Convert.ToInt32(reader["SalesId"]);
                               salesBO.CustomerId = Convert.ToInt32(reader["CustomerId"].ToString());
                               salesBO.SalesDate = Convert.ToDateTime(reader["SalesDate"]);
                               salesBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                               salesBO.SalesAmount = Convert.ToDecimal(reader["SalesAmount"]);   
                               salesBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);

                               salesBO.SiteInfoId =Int32.Parse( reader["SiteInfoId"].ToString());
                               salesBO.TechnicalInfoId =Int32.Parse(  reader["TechnicalInfoId"].ToString());
                               salesBO.BillingInfoId =Int32.Parse(  reader["BillingInfoId"].ToString());

                               salesBO.Remarks = reader["Remarks"].ToString();
                               salesBO.Frequency = (reader["Frequency"].ToString());
                           }
                       }
                   }
               }
           }
           return salesBO;
        }
        public List<PMSalesBO> GetAllSalesInformation()
        {

            List<PMSalesBO> salesList = new List<PMSalesBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllSalesInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesBO salesBO = new PMSalesBO();
                                salesBO.SalesId = Convert.ToInt32(reader["SalesId"]);
                                salesBO.BillNumber = reader["BillNumber"].ToString();
                                salesBO.CustomerId =Convert.ToInt32( reader["CustomerId"].ToString());
                                salesBO.SalesDate = Convert.ToDateTime(reader["SalesDate"]);
                                salesBO.CustomerName = reader["CustomerName"].ToString();
                               // salesBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                              //  salesBO.SalesAmount = Convert.ToDecimal(reader["SalesAmount"]);
                               // salesBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                salesList.Add(salesBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
        public List<PMSalesDetailBO> GetPMSalesDetailsBySalesId(int salesId)
        {
            List<PMSalesDetailBO> detailList = new List<PMSalesDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSalesDetailsBySalesId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalesId", DbType.Int32, salesId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesDetailBO detail = new PMSalesDetailBO();

                                detail.ServiceType = reader["ServiceType"].ToString();
                                detail.ItemId = Int32.Parse(reader["ItemId"].ToString());
                                detail.ItemName = reader["ItemName"].ToString();
                                detail.ItemUnit = Convert.ToDecimal(reader["ItemUnit"].ToString());
                                detail.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"].ToString());
                                detail.TotalPrice = detail.ItemUnit * detail.UnitPriceLocal;
                                
                                detail.SalesId = Int32.Parse(reader["SalesId"].ToString());
                                detail.DetailId = Int32.Parse(reader["DetailId"].ToString());   
                                detailList.Add(detail);
                            }
                        }
                    }
                }
            }
            return detailList;
        }
        public bool DeletePMSalesDetailsInfoBySalesId(int salesId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeletePMSalesDetailsInfoBySalesId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SalesId", DbType.Int32, salesId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }        
        public PMSalesDetailBO GetSalesCurrencyInformation(string TableName, int ItemId)
        {
            PMSalesDetailBO detail = new PMSalesDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCurrencyInformationByTable_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, TableName);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.String, ItemId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                detail.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                detail.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                detail.UnitPriceLocal = Decimal.Parse(reader["UnitPriceLocal"].ToString());
                                detail.UnitPriceUsd = Decimal.Parse(reader["UnitPriceUsd"].ToString());
                            }
                        }
                    }
                }
            }
            return detail;
        }
        public List<PMSalesBO> GetAllSalesInformationBySearchCriteria(DateTime FromDate, DateTime ToDate, int CustomerName, string BillNo)
        {
          string  Where = GenarateWhereConditionstring(FromDate, ToDate, CustomerName, BillNo);
            List<PMSalesBO> salesList = new List<PMSalesBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllSalesInformationBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesBO salesBO = new PMSalesBO();
                                salesBO.SalesId = Convert.ToInt32(reader["SalesId"]);
                                salesBO.CustomerId = Convert.ToInt32(reader["CustomerId"].ToString());
                                salesBO.SalesDate = Convert.ToDateTime(reader["SalesDate"]);
                                salesBO.CustomerName = reader["CustomerName"].ToString();
                                //salesBO.CustomerCode = reader["CustomerCode"].ToString();
                                salesBO.InvoiceNumber = reader["InvoiceNumber"].ToString();
                                
                                salesList.Add(salesBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
        public string GenarateWhereConditionstring(DateTime FromDate, DateTime ToDate, int CustomerName, string BillNo)
        {

            string Where=string.Empty;
            if (CustomerName>0)
            {
                    Where += "  SC.CustomerId = '" + CustomerName + "'";
            }

            if (!string.IsNullOrEmpty(BillNo))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += "  AND PMS.BillNumber = '" + BillNo + "'";
                }
                else
                {
                    Where += "  PMS.BillNumber = '" + BillNo + "'";
                }
            }




            if (!string.IsNullOrEmpty(FromDate.ToString()) && !string.IsNullOrEmpty(ToDate.ToString()))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND ( dbo.FnDate(PMS.SalesDate) >= dbo.FnDate('" + FromDate + "')  AND dbo.FnDate(PMS.SalesDate) <= dbo.FnDate('" + ToDate + "') )";
                }
                else
                {
                    Where += " ( dbo.FnDate(PMS.SalesDate) >= dbo.FnDate('" + FromDate + "')  AND dbo.FnDate(PMS.SalesDate) <= dbo.FnDate('" + ToDate + "') )";
                }
                 
            }


            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = "  WHERE " + Where;
            }
            return Where;
        }
        public string GenarateWhereConditionstring(DateTime FormDate, DateTime ToDate, string VoucherNo)
        {
            string Where = string.Empty;
            if (!string.IsNullOrEmpty(FormDate.ToString()) && !string.IsNullOrEmpty(ToDate.ToString()))
            {
                Where += "dbo.FnDate(gdm.VoucherDate) >= dbo.FnDate('" + FormDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate(gdm.VoucherDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "')";
            }
            if (!string.IsNullOrEmpty(VoucherNo))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND gdm.VoucherNo = '" + VoucherNo + "'";
                }
                else
                {
                    Where += "  gdm.VoucherNo = '" + VoucherNo + "'";
                }
            }
            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }
            return Where;
        }
        public List<PMSalesBillPreviewBO> GetPMSalesBillPreview()
        {
            List<PMSalesBillPreviewBO> salesList = new List<PMSalesBillPreviewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSalesBillPreview_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesBillPreviewBO salesBO = new PMSalesBillPreviewBO();
                                salesBO.SalesId = Convert.ToInt32(reader["SalesId"]);
                                salesBO.BillNumber = reader["BillNumber"].ToString();
                                salesBO.SalesDate = Convert.ToDateTime(reader["SalesDate"]);
                                salesBO.CustomerId = Convert.ToInt32(reader["CustomerId"].ToString());
                                salesBO.CustomerName = reader["CustomerName"].ToString();
                                salesBO.CustomerCode = reader["CustomerCode"].ToString();  
                                salesBO.SalesAmount = Convert.ToDecimal(reader["SalesAmount"]);
                                salesBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                salesBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                                salesBO.Frequency = reader["Frequency"].ToString();
                                salesBO.DueAmount = Convert.ToDecimal(reader["DueAmount"]);
                                salesBO.BillExpireDate = Convert.ToDateTime(reader["BillExpireDate"]);
                                //salesBO.DetailId = Int32.Parse(reader["DetailId"].ToString());
                                //salesBO.ServiceType = reader["ServiceType"].ToString();
                                //salesBO.ItemId = Int32.Parse(reader["ItemId"].ToString());
                                //salesBO.ItemName = reader["ItemName"].ToString();
                                //salesBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"].ToString());
                                //salesBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                salesBO.BillAmount = Convert.ToDecimal(reader["BillAmount"].ToString());
                                salesBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"].ToString());
                                

                                salesList.Add(salesBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
        public List<PMSalesBillPreviewBO> GetPMIndividualSalesBillPreview(int customerId, DateTime toDate)
        {
            List<PMSalesBillPreviewBO> salesList = new List<PMSalesBillPreviewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMIndividualSalesBillPreview_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomerId", DbType.Int32, customerId);
                    dbSmartAspects.AddInParameter(cmd, "@ToBillExpireDate", DbType.DateTime, toDate);                    

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesBillPreviewBO salesBO = new PMSalesBillPreviewBO();
                                salesBO.SalesId = Convert.ToInt32(reader["SalesId"]);
                                salesBO.BillNumber = reader["BillNumber"].ToString();
                                salesBO.SalesDate = Convert.ToDateTime(reader["SalesDate"]);
                                salesBO.CustomerId = Convert.ToInt32(reader["CustomerId"].ToString());
                                salesBO.CustomerName = reader["CustomerName"].ToString();
                                salesBO.CustomerCode = reader["CustomerCode"].ToString();
                                salesBO.SalesAmount = Convert.ToDecimal(reader["SalesAmount"]);
                                salesBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                salesBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                                salesBO.Frequency = reader["Frequency"].ToString();
                                salesBO.DueAmount = Convert.ToDecimal(reader["DueAmount"]);
                                salesBO.BillExpireDate = Convert.ToDateTime(reader["BillExpireDate"]);
                                //salesBO.DetailId = Int32.Parse(reader["DetailId"].ToString());
                                //salesBO.ServiceType = reader["ServiceType"].ToString();
                                //salesBO.ItemId = Int32.Parse(reader["ItemId"].ToString());
                                //salesBO.ItemName = reader["ItemName"].ToString();
                                //salesBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"].ToString());
                                //salesBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                salesBO.BillAmount = Convert.ToDecimal(reader["BillAmount"].ToString());
                                salesBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"].ToString());


                                salesList.Add(salesBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
        public Boolean SavePMSalesInvoice(PMSalesInvoiceBO entityBO, out int tmpPKId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMSalesInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@InvoiceId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpPKId = Convert.ToInt32(command.Parameters["@InvoiceId"].Value);
                }
            }
            return status;
        }
        public Boolean SavePMIndividualSalesInvoice(PMSalesInvoiceBO entityBO, out int tmpPKId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMIndividualSalesInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, entityBO.CustomerId);
                    dbSmartAspects.AddInParameter(command, "@ToBillExpireDate", DbType.DateTime, entityBO.BillFromDate);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@InvoiceId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpPKId = Convert.ToInt32(command.Parameters["@InvoiceId"].Value);
                }
            }
            return status;
        }
        public List<PMSalesBO> GetPMSalesByInvoiceNumberAndCustomerId(string InvoiceNumber, string CustomerCode)
        {
            List<PMSalesBO> salesList = new List<PMSalesBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSalesByInvoiceNumberAndCustomerId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@InvoiceNumber", DbType.String, InvoiceNumber);
                    dbSmartAspects.AddInParameter(cmd, "@CustomerCode", DbType.String, CustomerCode);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesBO salesBO = new PMSalesBO();
                                salesBO.SalesId = Convert.ToInt32(reader["SalesId"]);
                                salesBO.InvoiceId = Convert.ToInt32(reader["InvoiceId"]); 
                                salesBO.CustomerId = Convert.ToInt32(reader["CustomerId"].ToString());
                                salesBO.SalesDate = Convert.ToDateTime(reader["SalesDate"]);
                                salesBO.CustomerName = reader["CustomerName"].ToString();
                                salesBO.CustomerCode = reader["Code"].ToString();
                                salesBO.InvoiceNumber = reader["InvoiceNumber"].ToString();
                                salesBO.GrandTotal = Convert.ToDecimal( reader["GrandTotal"].ToString());
                    
                                // salesBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                                //  salesBO.SalesAmount = Convert.ToDecimal(reader["SalesAmount"]);
                                // salesBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                salesList.Add(salesBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
        public List<PMSalesInvoiceViewBO> GetPMSalesByInvoiceNumberAndCustomerId(int salesId, string salesType, int invoiceId, DateTime toBillExpireDate)
        {
            List<PMSalesInvoiceViewBO> salesList = new List<PMSalesInvoiceViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSalesInvoiceInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalesId", DbType.Int32, salesId);
                    dbSmartAspects.AddInParameter(cmd, "@SalesType", DbType.String, salesType);
                    dbSmartAspects.AddInParameter(cmd, "@InvoiceId", DbType.Int32, invoiceId);
                    dbSmartAspects.AddInParameter(cmd, "@ToBillExpireDate", DbType.DateTime, toBillExpireDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesInvoiceViewBO salesInvoiceViewBO = new PMSalesInvoiceViewBO();

                                salesInvoiceViewBO.AccountName = reader["AccountName"].ToString();
                                salesInvoiceViewBO.AccountNoLocal = reader["AccountNoLocal"].ToString();
                                salesInvoiceViewBO.AccountNoUSD = reader["AccountNoUSD"].ToString();
                                salesInvoiceViewBO.AdvanceOrDueAmount = Convert.ToDecimal(reader["AdvanceOrDueAmount"].ToString());
                                salesInvoiceViewBO.BankName = reader["BankName"].ToString();
                                salesInvoiceViewBO.BillFromDate = Convert.ToDateTime(reader["BillFromDate"]);
                                salesInvoiceViewBO.BillToDate = Convert.ToDateTime(reader["BillToDate"]);
                                salesInvoiceViewBO.BranchName = reader["BranchName"].ToString();
                                salesInvoiceViewBO.CompanyAddress = reader["CompanyAddress"].ToString();
                                salesInvoiceViewBO.CompanyName = reader["CompanyName"].ToString();
                                salesInvoiceViewBO.ContactNumber = reader["ContactNumber"].ToString();
                                salesInvoiceViewBO.Currency = reader["Currency"].ToString();
                                salesInvoiceViewBO.CustomerAddress = reader["CustomerAddress"].ToString();
                                salesInvoiceViewBO.CustomerCode = reader["CustomerCode"].ToString();
                                salesInvoiceViewBO.CustomerContactNumber = reader["CustomerContactNumber"].ToString();
                                salesInvoiceViewBO.CustomerEmailAddress = reader["CustomerEmailAddress"].ToString();
                                salesInvoiceViewBO.CustomerId = Convert.ToInt32(reader["CustomerId"].ToString());
                                salesInvoiceViewBO.CustomerName = reader["CustomerName"].ToString();
                                salesInvoiceViewBO.CustomerWebAddress = reader["CustomerWebAddress"].ToString();
                                salesInvoiceViewBO.DueDate = Convert.ToDateTime(reader["DueDate"]);
                                salesInvoiceViewBO.EmailAddress = reader["EmailAddress"].ToString();
                                salesInvoiceViewBO.FieldId = Convert.ToInt32(reader["FieldId"].ToString());
                                salesInvoiceViewBO.InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]);
                                salesInvoiceViewBO.InvoiceFor = reader["InvoiceFor"].ToString();
                                salesInvoiceViewBO.InvoiceNo = reader["InvoiceNo"].ToString();
                                salesInvoiceViewBO.SwiftCode = reader["SwiftCode"].ToString();
                                salesInvoiceViewBO.WebAddress = reader["WebAddress"].ToString();
                                salesInvoiceViewBO.ItemId = Convert.ToInt32(reader["ItemId"].ToString());
                                salesInvoiceViewBO.ItemName = reader["ItemName"].ToString();
                                salesInvoiceViewBO.ItemQuantity = Convert.ToDecimal(reader["ItemQuantity"].ToString());
                                salesInvoiceViewBO.TotalPrice = Convert.ToDecimal(reader["TotalPrice"].ToString());
                                salesInvoiceViewBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                salesList.Add(salesInvoiceViewBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
    }
}
