using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMRFQuotationDA : BaseService
    {
        public bool SaveOrUpdateRFQ(RFQuotationBO QuotationInfo, List<RFQuotationSupplierBO> SupplierList, out int RFQIdOut, out int OutRFQItemId)
        {

            int Recentstatus = 0;
            Boolean status = false;
            OutRFQItemId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRFQuotation_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RFQId", DbType.Int32, QuotationInfo.RFQId);
                            dbSmartAspects.AddInParameter(command, "@StoreID", DbType.Int32, QuotationInfo.StoreID);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, QuotationInfo.Description);
                            dbSmartAspects.AddInParameter(command, "@IndentName", DbType.String, QuotationInfo.IndentName);
                            dbSmartAspects.AddInParameter(command, "@PaymentTerm", DbType.String, QuotationInfo.PaymentTerm);
                            dbSmartAspects.AddInParameter(command, "@CreditDays", DbType.Decimal, QuotationInfo.CreditDays);
                            dbSmartAspects.AddInParameter(command, "@DeliveryTerms", DbType.String, QuotationInfo.DeliveryTerms);
                            dbSmartAspects.AddInParameter(command, "@SiteAddress", DbType.String, QuotationInfo.SiteAddress);
                            dbSmartAspects.AddInParameter(command, "@ExpireDateTime", DbType.DateTime, QuotationInfo.ExpireDateTime);
                            dbSmartAspects.AddInParameter(command, "@VAT", DbType.Decimal, QuotationInfo.VAT);
                            dbSmartAspects.AddInParameter(command, "@AIT", DbType.Decimal, QuotationInfo.AIT);
                            dbSmartAspects.AddInParameter(command, "@IndentPurpose", DbType.String, QuotationInfo.IndentPurpose);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, QuotationInfo.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@RFQIdOut", DbType.Int32, sizeof(Int32));
                            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            RFQIdOut = Convert.ToInt32(command.Parameters["@RFQIdOut"].Value);
                        }
                        if (Recentstatus > 0 && QuotationInfo.RFQuotationItems.Count > 0)
                        {
                            foreach (RFQuotationItemBO rfq in QuotationInfo.RFQuotationItems)
                            {
                                int i = 0;
                                using (DbCommand RFQDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRFQuotationItems_SP"))
                                {
                                    RFQDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(RFQDetails, "@RFQItemId", DbType.Int32, rfq.RFQItemId);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@RFQId", DbType.Int32, RFQIdOut);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@ItemId", DbType.Int32, rfq.ItemId);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@StockUnit", DbType.Int32, rfq.StockUnit);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@Quantity", DbType.Decimal, rfq.Quantity);
                                    dbSmartAspects.AddOutParameter(RFQDetails, "@OutRFQItemId", DbType.Int32, sizeof(Int32));

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(RFQDetails, transction);
                                    OutRFQItemId = Convert.ToInt32(RFQDetails.Parameters["@OutRFQItemId"].Value);

                                    if (Recentstatus > 0 && rfq.RFQuotationItemSpecifications.Count > 0)
                                    {
                                        using (DbCommand commandSpecifications = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRFQuotationItemDetails_SP"))
                                        {
                                            foreach (RFQuotationItemDetailBO bs in rfq.RFQuotationItemSpecifications)
                                            {
                                                commandSpecifications.Parameters.Clear();
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@Id", DbType.Int32, bs.Id);
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@Title", DbType.String, bs.Title);
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@Value", DbType.String, bs.Value);
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@RFQItemId", DbType.Int32, OutRFQItemId);

                                                Recentstatus = dbSmartAspects.ExecuteNonQuery(commandSpecifications, transction);
                                            }
                                        }
                                    }

                                }
                                i++;
                            }

                            if (Recentstatus > 0 && SupplierList.Count > 0)
                            {
                                foreach (RFQuotationSupplierBO sl in SupplierList)
                                {
                                    using (DbCommand supplierDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRFQuotationSuppliers_SP"))
                                    {
                                        supplierDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(supplierDetails, "@Id", DbType.Int32, sl.Id);
                                        dbSmartAspects.AddInParameter(supplierDetails, "@RFQId", DbType.Int32, RFQIdOut);
                                        dbSmartAspects.AddInParameter(supplierDetails, "@SupplierId", DbType.Int32, sl.SupplierId);

                                        Recentstatus = dbSmartAspects.ExecuteNonQuery(supplierDetails, transction);

                                    }
                                }
                            }

                            if (Recentstatus > 0)
                            {
                                status = true;
                                transction.Commit();
                            }
                            else
                            {
                                status = false;
                                transction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public List<RFQuotationBO> GetRFQuotationForGridPaging(int supplierId, string quotationType, DateTime? fromDate, DateTime? toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RFQuotationBO> RFQuotationBOList = new List<RFQuotationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRFQuotationForGridPaging_SP"))
                {
                    if (supplierId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);



                    //if (AccountHeadId != 0)
                    //    dbSmartAspects.AddInParameter(cmd, "@AccountHeadId", DbType.Int32, AccountHeadId);
                    //else
                    //    dbSmartAspects.AddInParameter(cmd, "@AccountHeadId", DbType.Int32, DBNull.Value);



                    if (!string.IsNullOrEmpty(quotationType))
                        dbSmartAspects.AddInParameter(cmd, "@quotationType", DbType.String, quotationType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@quotationType", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet RFQDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, RFQDS, "rfq");
                    DataTable Table = RFQDS.Tables["rfq"];

                    RFQuotationBOList = Table.AsEnumerable().Select(r => new RFQuotationBO
                    {
                        RFQId = r.Field<Int32>("RFQId"),
                        IndentName = r.Field<string>("IndentName"),
                        IndentPurpose = r.Field<string>("IndentPurpose"),
                        ExpireDateTime = r.Field<DateTime>("ExpireDateTime")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return RFQuotationBOList;
        }

        public List<RFQuotationItemBO> GetRFQuotationItemsForGridPaging(int quotationId, DateTime? fromDate, DateTime? toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RFQuotationItemBO> RFQuotationItemBOList = new List<RFQuotationItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRFQuotationItemsForGridPaging_SP"))
                {
                    if (quotationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@RFQId", DbType.Int32, quotationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RFQId", DbType.Int32, DBNull.Value);


                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet RFQDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, RFQDS, "rfq");
                    DataTable Table = RFQDS.Tables["rfq"];

                    RFQuotationItemBOList = Table.AsEnumerable().Select(r => new RFQuotationItemBO
                    {
                        RFQId = r.Field<Int32>("RFQId"),
                        RFQItemId = r.Field<Int32>("RFQItemId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        StockUnit = r.Field<Int32>("StockUnit"),
                        Quantity = r.Field<Decimal>("Quantity"),
                        CreditDays = r.Field<Decimal>("CreditDays"),
                        StockUnitName = r.Field<string>("StockUnitName"),
                        ItemName = r.Field<string>("ItemName"),
                        PaymentTerm = r.Field<string>("PaymentTerm"),
                        IndentName = r.Field<string>("IndentName"),
                        DeliveryTerms = r.Field<string>("DeliveryTerms"),
                        SiteAddress = r.Field<string>("SiteAddress"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        CreatedByName = r.Field<string>("CreatedByName")



                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return RFQuotationItemBOList;
        }

        public RFQuotationBO GetItemsForQuotation(int RFQId)
        {
            RFQuotationBO rFQuotationBO = new RFQuotationBO();
            List<RFQuotationItemBO> RFQuotationItems = new List<RFQuotationItemBO>();
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            string IndentBy= "";
            if (files[0].CompanyId > 0)
            {
                IndentBy = files[0].CompanyName;
            }
            

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRFQuotationDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RFQId", DbType.Int32, RFQId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                rFQuotationBO.RFQId = Convert.ToInt32(reader["RFQId"]);
                                rFQuotationBO.StoreID = Convert.ToInt32(reader["StoreID"]);
                                rFQuotationBO.Description = reader["Description"].ToString();
                                rFQuotationBO.IndentName = reader["IndentName"].ToString();
                                rFQuotationBO.PaymentTerm = reader["PaymentTerm"].ToString();
                                rFQuotationBO.CreditDays = Convert.ToDecimal(reader["CreditDays"]);
                                rFQuotationBO.DeliveryTerms = (reader["DeliveryTerms"]).ToString();
                                rFQuotationBO.SiteAddress = reader["SiteAddress"].ToString();
                                rFQuotationBO.ExpireDateTime = Convert.ToDateTime(reader["ExpireDateTime"]);
                                rFQuotationBO.VAT = Convert.ToDecimal(reader["VAT"]);
                                rFQuotationBO.AIT = Convert.ToDecimal(reader["AIT"]);
                                rFQuotationBO.IndentPurpose = reader["IndentPurpose"].ToString();
                                rFQuotationBO.IndentBy = IndentBy;





                            }
                        }
                    }

                }

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRFQuotationItems_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RFQId", DbType.Int32, RFQId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                RFQuotationItemBO Item = new RFQuotationItemBO();


                                Item.RFQId = Convert.ToInt32(reader["RFQId"]);
                                Item.RFQItemId = Convert.ToInt32(reader["RFQItemId"]);
                                Item.ItemId = Convert.ToInt32(reader["ItemId"]);
                                Item.StockUnit = Convert.ToInt32(reader["StockUnit"]);
                                Item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                Item.StockUnitName = (reader["StockUnitName"]).ToString();
                                Item.ItemName = (reader["ItemName"]).ToString();

                                List<RFQuotationItemDetailBO> itemDetailsList = new List<RFQuotationItemDetailBO>();

                                using (DbCommand cmd2 = dbSmartAspects.GetStoredProcCommand("GetRFQuotationItemSpecifications_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmd2, "@RFQItemId", DbType.Int32, Item.RFQItemId);

                                    using (IDataReader reader2 = dbSmartAspects.ExecuteReader(cmd2))
                                    {
                                        if (reader2 != null)
                                        {
                                            while (reader2.Read())
                                            {
                                                RFQuotationItemDetailBO itemDetails = new RFQuotationItemDetailBO();

                                                itemDetails.Id = Convert.ToInt32(reader2["Id"]);
                                                itemDetails.Title = (reader2["Title"]).ToString();
                                                itemDetails.Value = (reader2["Value"]).ToString();
                                                itemDetails.RFQItemId = Convert.ToInt32(reader2["RFQItemId"]);

                                                itemDetailsList.Add(itemDetails);

                                            }
                                        }
                                    }
                                }

                                Item.RFQuotationItemSpecifications = itemDetailsList;

                                RFQuotationItems.Add(Item);


                            }
                        }
                    }



                }
            }
            rFQuotationBO.RFQuotationItems = RFQuotationItems;
            return rFQuotationBO;
        }

        public List<RFQuotationItemsFeedbackBO> GetRFQuotationItemFeedbackByRFQItemId(int RFQItemId)
        {
            List<RFQuotationItemsFeedbackBO> RFQuotationItemsFeedbackList = new List<RFQuotationItemsFeedbackBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRFQuotationItemFeedbackByRFQItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RFQItemId", DbType.Int32, RFQItemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                RFQuotationItemsFeedbackBO Item = new RFQuotationItemsFeedbackBO();


                                Item.RFQSupplierItemId = Convert.ToInt32(reader["RFQSupplierItemId"]);
                                Item.RFQSupplierId = Convert.ToInt32(reader["RFQSupplierId"]);
                                Item.ItemId = Convert.ToInt32(reader["ItemId"]);
                                Item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                Item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                Item.Discount = Convert.ToDecimal(reader["Discount"]);
                                Item.OfferedUnitPrice = Convert.ToDecimal(reader["OfferedUnitPrice"]);
                                Item.OfferedUnitPriceWithVatAit = Convert.ToDecimal(reader["OfferedUnitPriceWithVatAit"]);
                                Item.BillingAmount = Convert.ToDecimal(reader["BillingAmount"]);
                                Item.AdvanceAmount = Convert.ToDecimal(reader["AdvanceAmount"]);
                                Item.OfferValidation = Convert.ToInt32(reader["OfferValidation"]);
                                Item.StockUnit = Convert.ToInt32(reader["StockUnit"]);
                                Item.DeliveryDuration = Convert.ToInt32(reader["DeliveryDuration"]);
                                Item.RFQItemId = Convert.ToInt32(reader["RFQItemId"]);
                                Item.QuoteDate = Convert.ToDateTime(reader["QuoteDate"]);

                                Item.ItemRemarks = (reader["ItemRemarks"]).ToString();
                                Item.ItemName = (reader["ItemName"]).ToString();
                                Item.StockUnitName = (reader["StockUnitName"]).ToString();
                                Item.SupplierName = (reader["SupplierName"]).ToString();

                                List<RFQuotationItemDetailsFeedbackBO> itemDetailsList = new List<RFQuotationItemDetailsFeedbackBO>();

                                using (DbCommand cmd2 = dbSmartAspects.GetStoredProcCommand("GetRFQuotationItemSpecificationsFeedback_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmd2, "@RFQSupplierItemId", DbType.Int32, Item.RFQSupplierItemId);

                                    using (IDataReader reader2 = dbSmartAspects.ExecuteReader(cmd2))
                                    {
                                        if (reader2 != null)
                                        {
                                            while (reader2.Read())
                                            {
                                                RFQuotationItemDetailsFeedbackBO itemDetails = new RFQuotationItemDetailsFeedbackBO();

                                                itemDetails.FeedbackId = Convert.ToInt32(reader2["FeedbackId"]);
                                                itemDetails.Title = (reader2["Title"]).ToString();
                                                itemDetails.Value = (reader2["Value"]).ToString();
                                                itemDetails.Feedback = (reader2["Feedback"]).ToString();

                                                itemDetailsList.Add(itemDetails);

                                            }
                                        }
                                    }
                                }

                                Item.RFQuotationItemSpecificationsFeedback = itemDetailsList;

                                RFQuotationItemsFeedbackList.Add(Item);


                            }
                        }
                    }



                }
            }
            return RFQuotationItemsFeedbackList;
        }


        public List<RFQuotationBO> GetAllIndentInfo()
        {
            List<RFQuotationBO> RFQuotationBOList = new List<RFQuotationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllIndentInfo_SP"))
                {

                    DataSet RFQDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, RFQDS, "rfq");
                    DataTable Table = RFQDS.Tables["rfq"];

                    RFQuotationBOList = Table.AsEnumerable().Select(r => new RFQuotationBO
                    {
                        RFQId = r.Field<Int32>("RFQId"),
                        IndentName = r.Field<string>("IndentName"),
                        IndentPurpose = r.Field<string>("IndentPurpose"),
                        ExpireDateTime = r.Field<DateTime>("ExpireDateTime")

                    }).ToList();

                }
            }
            return RFQuotationBOList;
        }

        public List<RFQuotationItemBO> GetItemsByIndentId(int RFQid)
        {

            List<RFQuotationItemBO> RFQuotationItemBOList = new List<RFQuotationItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRFQuotationItems_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RFQId", DbType.Int32, RFQid);

                    DataSet RFQDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, RFQDS, "rfq");
                    DataTable Table = RFQDS.Tables["rfq"];

                    RFQuotationItemBOList = Table.AsEnumerable().Select(r => new RFQuotationItemBO
                    {
                        RFQId = r.Field<Int32>("RFQId"),
                        RFQItemId = r.Field<Int32>("RFQItemId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        StockUnit = r.Field<Int32>("StockUnit"),
                        Quantity = r.Field<Decimal>("Quantity"),
                        StockUnitName = r.Field<string>("StockUnitName"),
                        ItemName = r.Field<string>("ItemName")

                    }).ToList();

                }
            }
            return RFQuotationItemBOList;

        }

        public bool SaveOrUpdateRFQFeedback(RFQuotationFeedbackBO QuotationFeedbackInfo, out int RFQSupplierIdOut, out int OutRFQSupplierItemId)
        {

            int Recentstatus = 0;
            Boolean status = false;
            OutRFQSupplierItemId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRFQuotationFeedback_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RFQSupplierId", DbType.Int32, QuotationFeedbackInfo.RFQSupplierId);
                            dbSmartAspects.AddInParameter(command, "@TotalItemQuoted", DbType.Int32, QuotationFeedbackInfo.TotalItemQuoted);
                            dbSmartAspects.AddInParameter(command, "@QuotedAmount", DbType.Decimal, QuotationFeedbackInfo.QuotedAmount);
                            dbSmartAspects.AddInParameter(command, "@ApplicableVatAit", DbType.Decimal, QuotationFeedbackInfo.ApplicableVatAit);
                            dbSmartAspects.AddInParameter(command, "@DeliveryCost", DbType.Decimal, QuotationFeedbackInfo.DeliveryCost);
                            dbSmartAspects.AddInParameter(command, "@TotalBillingAmount", DbType.Decimal, QuotationFeedbackInfo.TotalBillingAmount);
                            dbSmartAspects.AddInParameter(command, "@AdditionalInformation", DbType.String, QuotationFeedbackInfo.AdditionalInformation);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, QuotationFeedbackInfo.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@RFQId", DbType.Int32, QuotationFeedbackInfo.RFQId);

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, QuotationFeedbackInfo.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@RFQSupplierIdOut", DbType.Int32, sizeof(Int32));
                            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            RFQSupplierIdOut = Convert.ToInt32(command.Parameters["@RFQSupplierIdOut"].Value);
                        }
                        if (Recentstatus > 0 && QuotationFeedbackInfo.RFQuotationItemsFeedback.Count > 0)
                        {
                            foreach (RFQuotationItemsFeedbackBO rfq in QuotationFeedbackInfo.RFQuotationItemsFeedback)
                            {
                                int i = 0;
                                using (DbCommand RFQDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRFQuotationItemsFeedback_SP"))
                                {
                                    RFQDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(RFQDetails, "@RFQSupplierItemId", DbType.Int32, rfq.RFQSupplierItemId);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@RFQSupplierId", DbType.Int32, RFQSupplierIdOut);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@ItemId", DbType.Int32, rfq.ItemId);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@UnitPrice", DbType.Decimal, rfq.UnitPrice);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@Discount", DbType.Decimal, rfq.Discount);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@OfferedUnitPrice", DbType.Decimal, rfq.OfferedUnitPrice);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@OfferedUnitPriceWithVatAit", DbType.Decimal, rfq.OfferedUnitPriceWithVatAit);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@BillingAmount", DbType.Decimal, rfq.BillingAmount);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@AdvanceAmount", DbType.Decimal, rfq.AdvanceAmount);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@OfferValidation", DbType.Int32, rfq.OfferValidation);

                                    dbSmartAspects.AddInParameter(RFQDetails, "@Quantity", DbType.Decimal, rfq.Quantity);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@StockUnit", DbType.Int32, rfq.StockUnit);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@RFQItemId", DbType.Int32, rfq.RFQItemId);
                                    dbSmartAspects.AddInParameter(RFQDetails, "@ItemRemarks", DbType.String, rfq.ItemRemarks);

                                    dbSmartAspects.AddInParameter(RFQDetails, "@DeliveryDuration", DbType.Int32, rfq.DeliveryDuration);
                                    dbSmartAspects.AddOutParameter(RFQDetails, "@OutRFQSupplierItemId", DbType.Int32, sizeof(Int32));

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(RFQDetails, transction);
                                    OutRFQSupplierItemId = Convert.ToInt32(RFQDetails.Parameters["@OutRFQSupplierItemId"].Value);

                                    if (Recentstatus > 0 && rfq.RFQuotationItemSpecificationsFeedback.Count > 0)
                                    {
                                        using (DbCommand commandSpecifications = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRFQuotationItemDetailsFeedback_SP"))
                                        {
                                            foreach (RFQuotationItemDetailsFeedbackBO bs in rfq.RFQuotationItemSpecificationsFeedback)
                                            {
                                                commandSpecifications.Parameters.Clear();
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@FeedbackId", DbType.Int32, bs.FeedbackId);
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@RFQSupplierItemId", DbType.Int32, OutRFQSupplierItemId);
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@RFQuotationItemDetailsId", DbType.Int32, bs.RFQuotationItemDetailsId);
                                                dbSmartAspects.AddInParameter(commandSpecifications, "@Feedback", DbType.String, bs.Feedback);

                                                Recentstatus = dbSmartAspects.ExecuteNonQuery(commandSpecifications, transction);
                                            }
                                        }
                                    }

                                }
                                i++;
                            }

                            if (Recentstatus > 0)
                            {
                                status = true;
                                transction.Commit();
                            }
                            else
                            {
                                status = false;
                                transction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;
        }
    }
}
