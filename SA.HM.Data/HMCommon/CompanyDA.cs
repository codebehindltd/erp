using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Data.HMCommon
{
    public class CompanyDA : BaseService
    {

        public List<CompanyBO> GetCompanyInfo()
        {
            List<CompanyBO> CompanyList = new List<CompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CompanyBO Company = new CompanyBO();

                                Company.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                Company.CompanyCode = reader["CompanyCode"].ToString();
                                Company.CompanyName = reader["CompanyName"].ToString();
                                Company.GroupCompanyName = reader["GroupCompanyName"].ToString();
                                Company.CompanyAddress = reader["CompanyAddress"].ToString();
                                Company.EmailAddress = reader["EmailAddress"].ToString();
                                Company.WebAddress = reader["WebAddress"].ToString();
                                Company.ContactNumber = reader["ContactNumber"].ToString();
                                Company.ContactPerson = reader["ContactPerson"].ToString();
                                Company.VatRegistrationNo = reader["VatRegistrationNo"].ToString();
                                Company.TinNumber = reader["TinNumber"].ToString();
                                Company.Telephone = reader["Telephone"].ToString();
                                Company.HotLineNumber = reader["HotLineNumber"].ToString();
                                Company.Remarks = reader["Remarks"].ToString();
                                Company.ImageName = reader["ImageName"].ToString();
                                Company.ImagePath = reader["ImagePath"].ToString();
                                Company.CompanyType = reader["CompanyType"].ToString();

                                CompanyList.Add(Company);
                            }
                        }
                    }
                }
            }
            return CompanyList;
        }


        public CompanyBO GetCompanyInfoById(int id)
        {
            CompanyBO Company = new CompanyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                               
                                Company.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                Company.CompanyCode = reader["CompanyCode"].ToString();
                                Company.CompanyName = reader["CompanyName"].ToString();
                                Company.GroupCompanyName = reader["GroupCompanyName"].ToString();
                                Company.CompanyAddress = reader["CompanyAddress"].ToString();
                                Company.EmailAddress = reader["EmailAddress"].ToString();
                                Company.WebAddress = reader["WebAddress"].ToString();
                                Company.ContactNumber = reader["ContactNumber"].ToString();
                                Company.ContactPerson = reader["ContactPerson"].ToString();
                                Company.VatRegistrationNo = reader["VatRegistrationNo"].ToString();
                                Company.TinNumber = reader["TinNumber"].ToString();
                                Company.Remarks = reader["Remarks"].ToString();
                                Company.ImageName = reader["ImageName"].ToString();
                                Company.ImagePath = reader["ImagePath"].ToString();
                                Company.CompanyType = reader["CompanyType"].ToString();
                            }
                        }
                    }
                }
            }
            return Company;
        }


        public List<CompanyPaymentBO> GetCompanyListInfoById(int id)
        {
            List<CompanyPaymentBO> CompanyList = new List<CompanyPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CompanyPaymentBO Company = new CompanyPaymentBO();
                                Company.LedgerNumber = reader["LedgerNumber"].ToString();
                                Company.PaymentFor = reader["PaymentFor"].ToString();
                                Company.CompanyName = reader["CompanyName"].ToString();
                                Company.CompanyAddress = reader["CompanyAddress"].ToString();
                                Company.PaymentDateDisplay = reader["PaymentDateDisplay"].ToString();
                                Company.PaymentMode = reader["PaymentMode"].ToString();
                                Company.NodeHead = reader["NodeHead"].ToString();
                                Company.CurrencyType = reader["CurrencyType"].ToString();
                                Company.CurrencyName = reader["CurrencyName"].ToString();
                                Company.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                Company.ConvertionRate = Convert.ToDecimal(reader["ConvertionRate"].ToString());
                                Company.PaymentTotal = Convert.ToDecimal(reader["PaymentTotal"].ToString());
                                Company.ChequeNumber = reader["ChequeNumber"].ToString();
                                Company.ChequeDateDisplay = reader["ChequeDateDisplay"].ToString();
                                Company.Remarks = reader["Remarks"].ToString();
                                Company.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                CompanyList.Add(Company);
                            }
                        }
                    }
                }
            }
            return CompanyList;
        }



        public bool SaveCompanyInfo(CompanyBO companyBO, out int tmpCompanyId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CompanyCode", DbType.String, companyBO.CompanyCode);
                    dbSmartAspects.AddInParameter(command, "@CompanyName", DbType.String, companyBO.CompanyName);
                    dbSmartAspects.AddInParameter(command, "@CompanyAddress", DbType.String, companyBO.CompanyAddress);
                    dbSmartAspects.AddInParameter(command, "@EmailAddress", DbType.String, companyBO.EmailAddress);
                    dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, companyBO.WebAddress);
                    dbSmartAspects.AddInParameter(command, "@ContactNumber", DbType.String, companyBO.ContactNumber);
                    dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, companyBO.ContactPerson);
                    dbSmartAspects.AddInParameter(command, "@VatRegistrationNo", DbType.String, companyBO.VatRegistrationNo);
                    dbSmartAspects.AddInParameter(command, "@TinNumber", DbType.String, companyBO.TinNumber);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, companyBO.Remarks);

                    dbSmartAspects.AddOutParameter(command, "@CompanyId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpCompanyId = Convert.ToInt32(command.Parameters["@CompanyId"].Value);
                }
            }
            return status;
        }

        public bool UpdateCompanyInfo(CompanyBO companyBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, companyBO.CompanyId);
                    dbSmartAspects.AddInParameter(command, "@CompanyCode", DbType.String, companyBO.CompanyCode);
                    dbSmartAspects.AddInParameter(command, "@CompanyName", DbType.String, companyBO.CompanyName);
                    dbSmartAspects.AddInParameter(command, "@CompanyAddress", DbType.String, companyBO.CompanyAddress);
                    dbSmartAspects.AddInParameter(command, "@EmailAddress", DbType.String, companyBO.EmailAddress);
                    dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, companyBO.WebAddress);
                    dbSmartAspects.AddInParameter(command, "@ContactNumber", DbType.String, companyBO.ContactNumber);
                    dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, companyBO.ContactPerson);
                    dbSmartAspects.AddInParameter(command, "@VatRegistrationNo", DbType.String, companyBO.VatRegistrationNo);
                    dbSmartAspects.AddInParameter(command, "@TinNumber", DbType.String, companyBO.TinNumber);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, companyBO.Remarks);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public bool SaveDocumentForCompanyProfile(CompanyBO companyBO)
        {

            Boolean status = false;
            int tmpCompanyId = -1;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDocumentForCompanyProfile_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, companyBO.ImageName);
                    dbSmartAspects.AddInParameter(command, "@ImagePath", DbType.String, companyBO.ImagePath);
                    dbSmartAspects.AddOutParameter(command, "@CompanyId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpCompanyId = Convert.ToInt32(command.Parameters["@CompanyId"].Value);
                }
            }
            return status;
        }

        public bool UpdateDocumentForCompanyProfile(CompanyBO companyBO)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDocumentForCompanyProfile_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, companyBO.ImageName);
                    dbSmartAspects.AddInParameter(command, "@ImagePath", DbType.String, companyBO.ImagePath);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public bool SaveOrUpdateCompanyDocuments(CompanyBO companyBO)
        {
            var CompanyList = GetCompanyInfo();
            bool status;
            if (CompanyList.Count > 0)
            {
                status = UpdateDocumentForCompanyProfile(companyBO);                
            }
            else
            {
                status = SaveDocumentForCompanyProfile(companyBO);           
            }
            return status;
        }

    }
}
