using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.PurchaseManagment;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMSupplierDA : BaseService
    {
        public bool SavePMSupplierInfo(PMSupplierBO supplierBO, List<PMSupplierBO> contactInformationAdded, List<PMSupplierBO> contactInformationDeleted, out int tmpSupplierId)
        {
            int Recentstatus = 0;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMSupplierInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, supplierBO.Name);
                            dbSmartAspects.AddInParameter(command, "@SupplierTypeId", DbType.String, supplierBO.SupplierTypeId);
                            dbSmartAspects.AddInParameter(command, "@Code", DbType.String, supplierBO.Code);
                            dbSmartAspects.AddInParameter(command, "@Email", DbType.String, supplierBO.Email);
                            dbSmartAspects.AddInParameter(command, "@Fax", DbType.String, supplierBO.Fax);
                            dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, supplierBO.Phone);
                            dbSmartAspects.AddInParameter(command, "@Address", DbType.String, supplierBO.Address);
                            dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, supplierBO.WebAddress);
                            dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, supplierBO.ContactPerson);
                            dbSmartAspects.AddInParameter(command, "@ContactEmail", DbType.String, supplierBO.ContactEmail);
                            dbSmartAspects.AddInParameter(command, "@ContactPhone", DbType.String, supplierBO.ContactPhone);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, supplierBO.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, supplierBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@SupplierId", DbType.Int32, sizeof(Int32));
                            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            tmpSupplierId = Convert.ToInt32(command.Parameters["@SupplierId"].Value);
                        }

                        if (Recentstatus > 0 && contactInformationAdded.Count > 0)
                        {
                            foreach (PMSupplierBO bs in contactInformationAdded)
                            {
                                using (DbCommand PMDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdatePMSupplierDetails_SP"))
                                {
                                    PMDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(PMDetails, "@SupplierDetailsId", DbType.Int32, bs.SupplierDetailsId);
                                    dbSmartAspects.AddInParameter(PMDetails, "@SupplierId", DbType.Int32, tmpSupplierId);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactPerson", DbType.String, bs.ContactPerson);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactEmail", DbType.String, bs.ContactEmail);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactPhone", DbType.String, bs.ContactPhone);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactType", DbType.String, bs.ContactType);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactAddress", DbType.String, bs.ContactAddress);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(PMDetails, transction);
                                }
                            }
                        }
                        if (Recentstatus > 0 && contactInformationDeleted.Count > 0)
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeletePMSupplierDetails_SP"))
                            {
                                foreach (PMSupplierBO bs in contactInformationDeleted)
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@SupplierDetailsId", DbType.Int32, bs.SupplierDetailsId);
                                    dbSmartAspects.AddInParameter(commandDelete, "@SupplierId", DbType.Int32, tmpSupplierId);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactPerson", DbType.String, bs.ContactPerson);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactEmail", DbType.String, bs.ContactEmail);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactPhone", DbType.String, bs.ContactPhone);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactType", DbType.String, bs.ContactType);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactAddress", DbType.String, bs.ContactAddress);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
                                }
                            }
                        }

                        if(Recentstatus > 0 && !string.IsNullOrEmpty(supplierBO.CompanyCommaSeperatedIds))
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveCompanySeparation_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@SupplierId", DbType.Int32, tmpSupplierId);
                                dbSmartAspects.AddInParameter(commandDetails, "@CreatedBy", DbType.Int32, supplierBO.CreatedBy);
                                dbSmartAspects.AddInParameter(commandDetails, "@CompanyIdsCommaSeperated", DbType.String, supplierBO.CompanyCommaSeperatedIds);

                                dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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

        //public bool SavePMSupplierInfo(PMSupplierBO supplierBO, List<PMSupplierBO> contactInformationAdded, List<PMSupplierBO> contactInformationDeleted, out int tmpSupplierId)
        //{

        //    int Recentstatus = 0;
        //    Boolean status = false;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();
        //        using (DbTransaction transction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMSupplierInfo_SP"))
        //                {
        //                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, supplierBO.Name);
        //                    dbSmartAspects.AddInParameter(command, "@SupplierTypeId", DbType.String, supplierBO.SupplierTypeId);
        //                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, supplierBO.Code);
        //                    dbSmartAspects.AddInParameter(command, "@Email", DbType.String, supplierBO.Email);
        //                    dbSmartAspects.AddInParameter(command, "@Fax", DbType.String, supplierBO.Fax);
        //                    dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, supplierBO.Phone);
        //                    dbSmartAspects.AddInParameter(command, "@Address", DbType.String, supplierBO.Address);
        //                    dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, supplierBO.WebAddress);
        //                    dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, supplierBO.ContactPerson);
        //                    dbSmartAspects.AddInParameter(command, "@ContactEmail", DbType.String, supplierBO.ContactEmail);
        //                    dbSmartAspects.AddInParameter(command, "@ContactPhone", DbType.String, supplierBO.ContactPhone);
        //                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, supplierBO.Remarks);
        //                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, supplierBO.CreatedBy);
        //                    dbSmartAspects.AddOutParameter(command, "@SupplierId", DbType.Int32, sizeof(Int32));
        //                    //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //                    Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
        //                    tmpSupplierId = Convert.ToInt32(command.Parameters["@SupplierId"].Value);
        //                }

        //                if (Recentstatus > 0 && contactInformationAdded.Count > 0)
        //                {
        //                    foreach (PMSupplierBO bs in contactInformationAdded)
        //                    {
        //                        using (DbCommand PMDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdatePMSupplierDetails_SP"))
        //                        {
        //                            PMDetails.Parameters.Clear();

        //                            dbSmartAspects.AddInParameter(PMDetails, "@SupplierDetailsId", DbType.Int32, bs.SupplierDetailsId);
        //                            dbSmartAspects.AddInParameter(PMDetails, "@SupplierId", DbType.Int32, tmpSupplierId);
        //                            dbSmartAspects.AddInParameter(PMDetails, "@ContactPerson", DbType.String, bs.ContactPerson);
        //                            dbSmartAspects.AddInParameter(PMDetails, "@ContactEmail", DbType.String, bs.ContactEmail);
        //                            dbSmartAspects.AddInParameter(PMDetails, "@ContactPhone", DbType.String, bs.ContactPhone);
        //                            dbSmartAspects.AddInParameter(PMDetails, "@ContactType", DbType.String, bs.ContactType);
        //                            dbSmartAspects.AddInParameter(PMDetails, "@ContactAddress", DbType.String, bs.ContactAddress);

        //                            Recentstatus = dbSmartAspects.ExecuteNonQuery(PMDetails, transction);
        //                        }
        //                    }
        //                }
        //                if (Recentstatus > 0 && contactInformationDeleted.Count > 0)
        //                {
        //                    using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeletePMSupplierDetails_SP"))
        //                    {
        //                        foreach (PMSupplierBO bs in contactInformationDeleted)
        //                        {
        //                            commandDelete.Parameters.Clear();
        //                            dbSmartAspects.AddInParameter(commandDelete, "@SupplierDetailsId", DbType.Int32, bs.SupplierDetailsId);
        //                            dbSmartAspects.AddInParameter(commandDelete, "@SupplierId", DbType.Int32, tmpSupplierId);
        //                            dbSmartAspects.AddInParameter(commandDelete, "@ContactPerson", DbType.String, bs.ContactPerson);
        //                            dbSmartAspects.AddInParameter(commandDelete, "@ContactEmail", DbType.String, bs.ContactEmail);
        //                            dbSmartAspects.AddInParameter(commandDelete, "@ContactPhone", DbType.String, bs.ContactPhone);
        //                            dbSmartAspects.AddInParameter(commandDelete, "@ContactType", DbType.String, bs.ContactType);
        //                            dbSmartAspects.AddInParameter(commandDelete, "@ContactAddress", DbType.String, bs.ContactAddress);

        //                            Recentstatus = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
        //                        }
        //                    }
        //                }
        //                if (Recentstatus > 0)
        //                {
        //                    status = true;
        //                    transction.Commit();
        //                }
        //                else
        //                {
        //                    status = false;
        //                    transction.Rollback();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                status = false;
        //                transction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //    return status;
        //}
        public bool UpdatePMSupplierInfo(PMSupplierBO supplierBO, List<PMSupplierBO> contactInformationAdded, List<PMSupplierBO> contactInformationDeleted, int isNewChartOfAccountsHeadCreateForSupplier)
        {
            int Recentstatus = 0;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMSupplierInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, supplierBO.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, supplierBO.Name);
                            dbSmartAspects.AddInParameter(command, "@SupplierTypeId", DbType.String, supplierBO.SupplierTypeId);
                            dbSmartAspects.AddInParameter(command, "@Code", DbType.String, supplierBO.Code);
                            dbSmartAspects.AddInParameter(command, "@Email", DbType.String, supplierBO.Email);
                            dbSmartAspects.AddInParameter(command, "@Fax", DbType.String, supplierBO.Fax);
                            dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, supplierBO.Phone);
                            dbSmartAspects.AddInParameter(command, "@Address", DbType.String, supplierBO.Address);
                            dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, supplierBO.WebAddress);
                            dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, supplierBO.ContactPerson);
                            dbSmartAspects.AddInParameter(command, "@ContactEmail", DbType.String, supplierBO.ContactEmail);
                            dbSmartAspects.AddInParameter(command, "@ContactPhone", DbType.String, supplierBO.ContactPhone);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, supplierBO.Remarks);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, supplierBO.LastModifiedBy);
                            dbSmartAspects.AddInParameter(command, "@isNewCOACreateForSupplier", DbType.Int32, isNewChartOfAccountsHeadCreateForSupplier);
                            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);


                        }
                        if (Recentstatus > 0 && contactInformationAdded.Count > 0)
                        {
                            foreach (PMSupplierBO bs in contactInformationAdded)
                            {
                                using (DbCommand PMDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdatePMSupplierDetails_SP"))
                                {
                                    PMDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(PMDetails, "@SupplierDetailsId", DbType.Int32, bs.SupplierDetailsId);
                                    dbSmartAspects.AddInParameter(PMDetails, "@SupplierId", DbType.Int32, supplierBO.SupplierId);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactPerson", DbType.String, bs.ContactPerson);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactEmail", DbType.String, bs.ContactEmail);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactPhone", DbType.String, bs.ContactPhone);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactType", DbType.String, bs.ContactType);
                                    dbSmartAspects.AddInParameter(PMDetails, "@ContactAddress", DbType.String, bs.ContactAddress);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(PMDetails, transction);
                                }
                            }
                        }

                        if (Recentstatus > 0 && !string.IsNullOrEmpty(supplierBO.CompanyCommaSeperatedIds))
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveCompanySeparation_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@SupplierId", DbType.Int32, supplierBO.SupplierId);
                                dbSmartAspects.AddInParameter(commandDetails, "@CreatedBy", DbType.Int32, supplierBO.CreatedBy);
                                dbSmartAspects.AddInParameter(commandDetails, "@CompanyIdsCommaSeperated", DbType.String, supplierBO.CompanyCommaSeperatedIds);

                                dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }
                        else
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveCompanySeparation_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@SupplierId", DbType.Int32, supplierBO.SupplierId);
                                dbSmartAspects.AddInParameter(commandDetails, "@CreatedBy", DbType.Int32, supplierBO.CreatedBy);
                                dbSmartAspects.AddInParameter(commandDetails, "@CompanyIdsCommaSeperated", DbType.String, supplierBO.CompanyCommaSeperatedIds);

                                dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }
                        if (Recentstatus > 0 && contactInformationDeleted.Count > 0)
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeletePMSupplierDetails_SP"))
                            {
                                foreach (PMSupplierBO bs in contactInformationDeleted)
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@SupplierDetailsId", DbType.Int32, bs.SupplierDetailsId);
                                    dbSmartAspects.AddInParameter(commandDelete, "@SupplierId", DbType.Int32, bs.SupplierId);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactPerson", DbType.String, bs.ContactPerson);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactEmail", DbType.String, bs.ContactEmail);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactPhone", DbType.String, bs.ContactPhone);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactType", DbType.String, bs.ContactType);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ContactAddress", DbType.String, bs.ContactAddress);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
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

        
        public List<PMSupplierBO> GetSupplierInfoBySearchCriteria(string Code, string ContactPerson, string Email, string Name, string Phone, string supplierTypeId)
        {
            List<PMSupplierBO> supplierList = new List<PMSupplierBO>();
            string SearchCriteria = GenerateWhereCondition(Code, ContactPerson, Email, Name, Phone);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, SearchCriteria);
                    //dbSmartAspects.AddInParameter(cmd, "@SupplierTypeId", DbType.String, supplierTypeId);

                    DataSet SupplierDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupplierDS, "Supplier");
                    DataTable table = SupplierDS.Tables["Supplier"];

                    supplierList = table.AsEnumerable().Select(r => new PMSupplierBO
                    {

                        SupplierId = r.Field<int>("SupplierId"),
                        Name = r.Field<string>("Name"),
                        SupplierTypeId = r.Field<string>("SupplierTypeId"),
                        Code = r.Field<string>("Code"),
                        Fax = r.Field<string>("Fax"),
                        Phone = r.Field<string>("Phone"),
                        Email = r.Field<string>("Email"),
                        Address = r.Field<string>("Address"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        WebAddress = r.Field<string>("WebAddress"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return supplierList;
        }
        public List<PMSupplierBO> GetSupplierInfoBySearchCriteria(string searchText)
        {
            List<PMSupplierBO> supplierList = new List<PMSupplierBO>();
            string query = "SELECT * FROM PMSupplier WHERE Name LIKE '%" + searchText + "%'";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet SupplierDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupplierDS, "Supplier");
                    DataTable table = SupplierDS.Tables["Supplier"];

                    supplierList = table.AsEnumerable().Select(r => new PMSupplierBO
                    {
                        SupplierId = r.Field<int>("SupplierId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code")

                    }).ToList();
                }
            }
            return supplierList;
        }
        public List<PMSupplierBO> GetPMSupplierInfoDetailsById(long id)
        {
            string query = string.Format("SELECT * FROM PMSupplierDetails WHERE SupplierId = {0}", id);
            List<PMSupplierBO> PMSupplierBOList = new List<PMSupplierBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet PMDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, PMDS, "Details");
                    DataTable Table = PMDS.Tables["Details"];

                    PMSupplierBOList = Table.AsEnumerable().Select(r => new PMSupplierBO
                    {
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactAddress = r.Field<string>("ContactAddress"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        ContactType = r.Field<string>("ContactType"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        SupplierDetailsId = r.Field<Int32>("SupplierDetailsId")
                    }).ToList();

                }
            }
            return PMSupplierBOList;
        }
        public PMSupplierBO GetPMSupplierInfoById(int SupplierId)
        {
            PMSupplierBO supplierBO = new PMSupplierBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, SupplierId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                supplierBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                supplierBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                supplierBO.Name = reader["Name"].ToString();
                                supplierBO.SupplierTypeId = reader["SupplierTypeId"].ToString();
                                supplierBO.Code = reader["Code"].ToString();
                                supplierBO.Fax = reader["Fax"].ToString();
                                supplierBO.Phone = reader["Phone"].ToString();
                                supplierBO.Email = reader["Email"].ToString();
                                supplierBO.Address = reader["Address"].ToString();
                                supplierBO.ContactPerson = reader["ContactPerson"].ToString();
                                supplierBO.WebAddress = reader["WebAddress"].ToString();
                                supplierBO.ContactEmail = reader["ContactEmail"].ToString();
                                supplierBO.ContactPhone = reader["ContactPhone"].ToString();
                                supplierBO.Remarks = reader["Remarks"].ToString();
                                supplierBO.Balance = Convert.ToDecimal(reader["Balance"].ToString());
                            }
                        }
                    }
                }
            }
            return supplierBO;
        }
        public List<PMSupplierBO> GetPMSupplierInfoByOrderType(string supplierType)
        {
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfoByOrderType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OrderType", DbType.String, supplierType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSupplierBO supplierBO = new PMSupplierBO();
                                supplierBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                supplierBO.Name = reader["Name"].ToString();
                                supplierBO.SupplierTypeId = reader["SupplierTypeId"].ToString();
                                supplierBO.Code = reader["Code"].ToString();
                                supplierBO.Fax = reader["Fax"].ToString();
                                supplierBO.Phone = reader["Phone"].ToString();
                                supplierBO.Email = reader["Email"].ToString();
                                supplierBO.Address = reader["Address"].ToString();
                                supplierBO.ContactPerson = reader["ContactPerson"].ToString();
                                supplierBO.WebAddress = reader["WebAddress"].ToString();
                                supplierBO.ContactEmail = reader["ContactEmail"].ToString();
                                supplierBO.ContactPhone = reader["ContactPhone"].ToString();
                                supplierBO.Remarks = reader["Remarks"].ToString();
                                supplierBO.IsAdhocSupplier = Convert.ToInt32(reader["IsAdhocSupplier"]);
                                supplierBOList.Add(supplierBO);
                            }
                        }
                    }
                }
            }
            return supplierBOList;
        }
        public List<PMSupplierBO> GetPMSupplierInfo()
        {
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSupplierBO supplierBO = new PMSupplierBO();
                                supplierBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                supplierBO.Name = reader["Name"].ToString();
                                supplierBO.Code = reader["Code"].ToString();
                                supplierBO.Fax = reader["Fax"].ToString();
                                supplierBO.Phone = reader["Phone"].ToString();
                                supplierBO.Email = reader["Email"].ToString();
                                supplierBO.Address = reader["Address"].ToString();
                                supplierBO.ContactPerson = reader["ContactPerson"].ToString();
                                supplierBO.WebAddress = reader["WebAddress"].ToString();
                                supplierBO.ContactEmail = reader["ContactEmail"].ToString();
                                supplierBO.ContactPhone = reader["ContactPhone"].ToString();
                                supplierBO.Remarks = reader["Remarks"].ToString();
                                supplierBO.IsAdhocSupplier = Convert.ToInt32(reader["IsAdhocSupplier"]);
                                supplierBOList.Add(supplierBO);
                            }
                        }
                    }
                }
            }
            return supplierBOList;
        }
        public List<PMSupplierBO> GetPMSupplierInfoByUserInfoId(int userInfoId)
        {
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfoByUserInfoId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSupplierBO supplierBO = new PMSupplierBO();
                                supplierBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                supplierBO.Name = reader["Name"].ToString();
                                supplierBO.Code = reader["Code"].ToString();
                                supplierBO.Fax = reader["Fax"].ToString();
                                supplierBO.Phone = reader["Phone"].ToString();
                                supplierBO.Email = reader["Email"].ToString();
                                supplierBO.Address = reader["Address"].ToString();
                                supplierBO.ContactPerson = reader["ContactPerson"].ToString();
                                supplierBO.WebAddress = reader["WebAddress"].ToString();
                                supplierBO.ContactEmail = reader["ContactEmail"].ToString();
                                supplierBO.ContactPhone = reader["ContactPhone"].ToString();
                                supplierBO.Remarks = reader["Remarks"].ToString();
                                supplierBO.IsAdhocSupplier = Convert.ToInt32(reader["IsAdhocSupplier"]);
                                supplierBOList.Add(supplierBO);
                            }
                        }
                    }
                }
            }
            return supplierBOList;
        }
        public List<PMSupplierBO> GetPMSupplierInfoUsingItemList(string ItemIdList)
        {
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfoUsingItemList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemList", DbType.String, ItemIdList);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSupplierBO supplierBO = new PMSupplierBO();
                                supplierBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                supplierBO.Name = reader["Name"].ToString();
                                supplierBO.Code = reader["Code"].ToString();
                                supplierBO.Fax = reader["Fax"].ToString();
                                supplierBO.Phone = reader["Phone"].ToString();
                                supplierBO.Email = reader["Email"].ToString();
                                supplierBO.Address = reader["Address"].ToString();
                                supplierBO.ContactPerson = reader["ContactPerson"].ToString();
                                supplierBO.WebAddress = reader["WebAddress"].ToString();
                                supplierBO.ContactEmail = reader["ContactEmail"].ToString();
                                supplierBO.ContactPhone = reader["ContactPhone"].ToString();
                                supplierBO.Remarks = reader["Remarks"].ToString();
                                supplierBO.IsAdhocSupplier = Convert.ToInt32(reader["IsAdhocSupplier"]);
                                supplierBOList.Add(supplierBO);
                            }
                        }
                    }
                }
            }
            return supplierBOList;
        }
        public List<PMSupplierBO> GetPMSupplierInfoForAutoSearch(string supplierName)
        {
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfoForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierName", DbType.String, supplierName);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSupplierBO supplierBO = new PMSupplierBO();
                                supplierBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                supplierBO.Name = reader["Name"].ToString();
                                supplierBO.Code = reader["Code"].ToString();
                                supplierBO.NameWithCode = reader["NameWithCode"].ToString();
                                supplierBOList.Add(supplierBO);
                            }
                        }
                    }
                }
            }
            return supplierBOList;
        }
        private string GenerateWhereCondition(string Code, string ContactPerson, string Email, string Name, string Phone)
        {
            string Where = string.Empty, Condition = string.Empty;

            if (!string.IsNullOrWhiteSpace(Name))
            {
                Condition = "Name LIKE '%" + Name + "%'";
            }

            if (!string.IsNullOrWhiteSpace(Code))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND Code LIKE '%" + Code + "%'";
                }
                else
                {
                    Condition = "Code LIKE '%" + Code + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND Phone LIKE '%" + Phone + "%'";
                }
                else
                {
                    Condition = "Phone LIKE '%" + Phone + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND Email LIKE '%" + Email + "%'";
                }
                else
                {
                    Condition = "Email LIKE '%" + Email + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(ContactPerson))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ContactPerson LIKE '%" + ContactPerson + "%'";
                }
                else
                {
                    Condition = "ContactPerson LIKE '%" + ContactPerson + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Condition))
                Where += " WHERE " + Condition;

            return Where;
        }
        public List<PMSupplierBO> GetSupllierInfoById(int? supplierId, int companyId)
        {
            List<PMSupplierBO> supplierInfo = new List<PMSupplierBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfoForReport_SP"))
                {
                    if (supplierId != null)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new PMSupplierBO
                    {
                        SupplierId = r.Field<Int32>("SupplierId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        Address = r.Field<string>("Address"),
                        Phone = r.Field<string>("Phone"),
                        Fax = r.Field<string>("Fax"),
                        Email = r.Field<string>("Email"),
                        ContactPerson = r.Field<string>("ContactPerson")

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public Boolean UpdateSupplierNAccountsInfo(int tmpSupplierId, int tmpNodeId, int isNewChartOfAccountsHeadCreateForSupplier)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSupplierNAccountsInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, tmpSupplierId);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.String, tmpNodeId);
                    dbSmartAspects.AddInParameter(command, "@IsCOAEnable", DbType.Int32, isNewChartOfAccountsHeadCreateForSupplier);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public PMSupplierBO GetSupplierByCode(string suppliercode)
        {
            PMSupplierBO bo = new PMSupplierBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSupplierInfoBySupplierCode_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, suppliercode);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SupplierInfo");
                    DataTable Table = ds.Tables["SupplierInfo"];

                    bo = Table.AsEnumerable().Select(r => new PMSupplierBO
                    {
                        SupplierId = r.Field<int>("SupplierId"),
                        NodeId = r.Field<int>("NodeId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        Address = r.Field<string>("Address"),
                        Phone = r.Field<string>("Phone"),
                        Fax = r.Field<string>("Fax"),
                        Email = r.Field<string>("Email"),
                        WebAddress = r.Field<string>("WebAddress"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        Remarks = r.Field<string>("Remarks")
                    }).FirstOrDefault();
                }
            }
            return bo;
        }
        public Boolean SaveSupplierPaymentLedger(PMSupplierPaymentLedgerBO supplierPaymentLedgerBO, out long tmpSupplierPaymentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMSupplierPaymentLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, supplierPaymentLedgerBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, supplierPaymentLedgerBO.BillNumber);
                    dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, supplierPaymentLedgerBO.PaymentDate);
                    dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, supplierPaymentLedgerBO.SupplierId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, supplierPaymentLedgerBO.CurrencyId);
                    dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, supplierPaymentLedgerBO.ConvertionRate);
                    dbSmartAspects.AddInParameter(command, "@DRAmount", DbType.Decimal, supplierPaymentLedgerBO.DRAmount);
                    dbSmartAspects.AddInParameter(command, "@CRAmount", DbType.Decimal, supplierPaymentLedgerBO.CRAmount);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, supplierPaymentLedgerBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, supplierPaymentLedgerBO.AccountsPostingHeadId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, supplierPaymentLedgerBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, supplierPaymentLedgerBO.ChequeNumber);
                    dbSmartAspects.AddInParameter(command, "@PaymentStatus", DbType.String, supplierPaymentLedgerBO.PaymentStatus);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, supplierPaymentLedgerBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@SupplierPaymentId", DbType.Int64, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpSupplierPaymentId = Convert.ToInt64(command.Parameters["@SupplierPaymentId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateSupplierPaymentLedger(PMSupplierPaymentLedgerBO supplierPaymentLedgerBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMSupplierPaymentLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SupplierPaymentId", DbType.Int64, supplierPaymentLedgerBO.SupplierPaymentId);
                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, supplierPaymentLedgerBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, supplierPaymentLedgerBO.BillNumber);
                    dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, supplierPaymentLedgerBO.PaymentDate);
                    dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, supplierPaymentLedgerBO.SupplierId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, supplierPaymentLedgerBO.CurrencyId);
                    dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, supplierPaymentLedgerBO.ConvertionRate);
                    dbSmartAspects.AddInParameter(command, "@DRAmount", DbType.Decimal, supplierPaymentLedgerBO.DRAmount);
                    dbSmartAspects.AddInParameter(command, "@CRAmount", DbType.Decimal, supplierPaymentLedgerBO.CRAmount);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, supplierPaymentLedgerBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, supplierPaymentLedgerBO.AccountsPostingHeadId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, supplierPaymentLedgerBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, supplierPaymentLedgerBO.ChequeNumber);
                    dbSmartAspects.AddInParameter(command, "@PaymentStatus", DbType.String, supplierPaymentLedgerBO.PaymentStatus);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, supplierPaymentLedgerBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SupplierPaymentLedgerVwBO> GetSupllierLedger(int userInfoId, int companyId, int supplierId, DateTime dateFrom, DateTime dateTo, string paymentStatus, string reportType)
        {
            List<SupplierPaymentLedgerVwBO> supplierInfo = new List<SupplierPaymentLedgerVwBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierLedgerReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (paymentStatus != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, paymentStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new SupplierPaymentLedgerVwBO
                    {
                        SupplierId = r.Field<Int32?>("SupplierId"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        PaymentDateDisplay = r.Field<string>("PaymentDateDisplay"),
                        Narration = r.Field<string>("Narration"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        BalanceCommulative = r.Field<decimal?>("BalanceCommulative"),
                        SupplierName = r.Field<string>("SupplierName")

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<PMSupplierPaymentLedgerBO> GetSupplierPaymentLedger(DateTime? dateFrom, DateTime? dateTo, string paymentId, bool isInvoice)
        {
            List<PMSupplierPaymentLedgerBO> paymentInfo = new List<PMSupplierPaymentLedgerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPaymentLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.String, paymentId);
                    dbSmartAspects.AddInParameter(cmd, "@IsInvoice", DbType.Boolean, isInvoice);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new PMSupplierPaymentLedgerBO
                    {
                        SupplierPaymentId = r.Field<Int64>("SupplierPaymentId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentType = r.Field<string>("PaymentType"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate")

                    }).ToList();
                }
            }

            return paymentInfo;
        }
        public PMSupplierPaymentLedgerBO GetSupplierPaymentLedgerById(long id)
        {
            PMSupplierPaymentLedgerBO paymentInfo = new PMSupplierPaymentLedgerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPaymentLedgerById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierPaymentId", DbType.Int64, id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new PMSupplierPaymentLedgerBO
                    {
                        SupplierPaymentId = r.Field<Int64>("SupplierPaymentId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentType = r.Field<string>("PaymentType"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        //CurrencyName = r.Field<string>("CurrencyName"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        Remarks = r.Field<string>("Remarks"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        SupplierId = r.Field<int>("SupplierId"),
                        AccountsPostingHeadId = r.Field<Int64>("AccountsPostingHeadId")

                    }).SingleOrDefault();
                }
            }

            return paymentInfo;
        }
        //---------Bill Payment
        public List<PMSupplierPaymentLedgerBO> SupplierBillBySearch(int supplierId)
        {
            List<PMSupplierPaymentLedgerBO> supplierInfo = new List<PMSupplierPaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierBillBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new PMSupplierPaymentLedgerBO
                    {
                        SupplierPaymentId = r.Field<Int64>("SupplierPaymentId"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        IsBillGenerated = r.Field<bool>("IsBillGenerated"),
                        PaymentDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<PMSupplierPaymentLedgerBO> SupplierBillInfoByPaymentId(Int64 paymentId)
        {
            List<PMSupplierPaymentLedgerBO> supplierInfo = new List<PMSupplierPaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierBillInfoByPaymentId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int32, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new PMSupplierPaymentLedgerBO
                    {
                        BillNumber = r.Field<string>("BillNumber"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        BillDateDisplay = r.Field<string>("BillDateDisplay"),
                        BillAmount = r.Field<decimal>("BillAmount")

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<PMSupplierPaymentLedgerBO> SupplierBillAdvanceBySearch(int supplierId)
        {
            List<PMSupplierPaymentLedgerBO> supplierInfo = new List<PMSupplierPaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierAdvanceBillBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new PMSupplierPaymentLedgerBO
                    {
                        SupplierPaymentId = r.Field<Int64>("SupplierPaymentId"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        AdvanceAmountRemaining = r.Field<decimal>("AdvanceAmountRemaining"),
                        IsBillGenerated = r.Field<bool>("IsBillGenerated"),
                        PaymentDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public Boolean UpdateSupplierPaymentLedgerForBillGeneration(List<PMSupplierPaymentLedgerBO> supplierPaymentLedger)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSupplierPaymentLedgerForBillGeneration_SP"))
                {
                    foreach (PMSupplierPaymentLedgerBO cpl in supplierPaymentLedger)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@SupplierPaymentId", DbType.Int64, cpl.SupplierPaymentId);
                        dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                        dbSmartAspects.AddInParameter(command, "@IsBillGenerated", DbType.Boolean, cpl.IsBillGenerated);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public bool SaveSupplierBillPayment(SupplierPaymentBO supplierPayment, int userInfoId, List<SupplierPaymentDetailsBO> supplierPaymentDetails)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSupplierPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, supplierPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, supplierPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@SupplierPaymentAdvanceId", DbType.String, supplierPayment.SupplierPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.String, supplierPayment.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, supplierPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, supplierPayment.ChecqueDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, supplierPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, supplierPayment.AdjustmentAmount);
                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, supplierPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, supplierPayment.AccountingPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, supplierPayment.ApprovedStatus);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);

                            if (!string.IsNullOrEmpty(supplierPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, supplierPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(supplierPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, supplierPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (supplierPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, supplierPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (supplierPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, supplierPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (supplierPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, supplierPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (supplierPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, supplierPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            supplierIdPaymentId = Convert.ToInt64(command.Parameters["@PaymentId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSupplierPaymentDetails_SP"))
                        {
                            foreach (SupplierPaymentDetailsBO cpl in supplierPaymentDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, supplierIdPaymentId);
                                dbSmartAspects.AddInParameter(command, "@SupplierPaymentId", DbType.Int64, cpl.SupplierPaymentId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }



                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public bool UpdateSupplierBillPayment(SupplierPaymentBO supplierPayment, int userInfoId, List<SupplierPaymentDetailsBO> supplierPaymentDetails,
                                              List<SupplierPaymentDetailsBO> SupplierPaymentDetailsEdited, List<SupplierPaymentDetailsBO> SupplierPaymentDetailsDeleted)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSupplierPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, supplierPayment.PaymentId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, supplierPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, supplierPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@SupplierPaymentAdvanceId", DbType.String, supplierPayment.SupplierPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.String, supplierPayment.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, supplierPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, supplierPayment.ChecqueDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.String, supplierPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, supplierPayment.AdjustmentAmount);
                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, supplierPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, supplierPayment.AccountingPostingHeadId);

                            if (!string.IsNullOrEmpty(supplierPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, supplierPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(supplierPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, supplierPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (supplierPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, supplierPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (supplierPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, supplierPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (supplierPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, supplierPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (supplierPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, supplierPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0 && supplierPaymentDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSupplierPaymentDetails_SP"))
                            {
                                foreach (SupplierPaymentDetailsBO cpl in supplierPaymentDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, supplierPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@SupplierPaymentId", DbType.Int64, cpl.SupplierPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && SupplierPaymentDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSupplierPaymentDetails_SP"))
                            {
                                foreach (SupplierPaymentDetailsBO cpl in SupplierPaymentDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentDetailsId", DbType.Int64, cpl.PaymentDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, supplierPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@SupplierPaymentId", DbType.Int64, cpl.SupplierPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && SupplierPaymentDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (SupplierPaymentDetailsBO cpl in SupplierPaymentDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PMSupplierPaymentDetails");
                                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "PaymentDetailsId");
                                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, cpl.PaymentDetailsId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public bool CheckedPayment(Int64 paymentId, int checkedBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckedSupplierPaymentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.String, checkedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public bool ApprovedPayment(Int64 paymentId, int createdBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedSupplierPaymentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public bool DeletePaymentInfo(Int64 paymentId, int createdBy)
        {
            int status = 0;
            string query = string.Format(@"
                                DELETE FROM PMSupplierPaymentDetails
                                WHERE PaymentId = {0}
                                DELETE FROM PMSupplierPayment
                                WHERE PaymentId = {0}
                            ", paymentId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public List<SupplierPaymentBO> GetSupplierPaymentBySearch(int supplierId, DateTime? dateFrom, DateTime? dateTo, string paymentFor)
        {
            List<SupplierPaymentBO> paymentInfo = new List<SupplierPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPaymentBySearch_SP"))
                {
                    if (supplierId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, paymentFor);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new SupplierPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        AdjustmentType = r.Field<string>("AdjustmentType")

                    }).ToList();
                }
            }

            return paymentInfo;
        }
        public SupplierPaymentBO GetSupplierPayment(Int64 paymentId)
        {
            SupplierPaymentBO paymentInfo = new SupplierPaymentBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPayment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new SupplierPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        ChecqueDate = r.Field<DateTime>("ChecqueDate"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        SupplierId = r.Field<int>("SupplierId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        CurrencyId = r.Field<int?>("CurrencyId"),
                        ConvertionRate = r.Field<decimal?>("ConvertionRate"),
                        PaymentType = r.Field<string>("PaymentType"),
                        PaymentFor = r.Field<string>("PaymentFor"),
                        AccountingPostingHeadId = r.Field<int>("AccountingPostingHeadId"),
                        AdjustmentType = r.Field<string>("AdjustmentType"),
                        SupplierPaymentAdvanceId = r.Field<Int64>("SupplierPaymentAdvanceId"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        AdjustmentAmount = r.Field<decimal>("AdjustmentAmount"),
                        AdjustmentAccountHeadId = r.Field<int?>("AdjustmentAccountHeadId"),
                        PaymentAdjustmentAmount = r.Field<decimal?>("PaymentAdjustmentAmount")

                    }).FirstOrDefault();
                }
            }

            return paymentInfo;
        }
        public List<SupplierPaymentDetailsBO> GetSupplierPaymentDetails(Int64 paymentId)
        {
            List<SupplierPaymentDetailsBO> paymentInfo = new List<SupplierPaymentDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPaymentLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new SupplierPaymentDetailsBO
                    {
                        SupplierPaymentId = r.Field<Int64>("SupplierPaymentId"),
                        PaymentDetailsId = r.Field<Int64>("PaymentDetailsId"),
                        PaymentId = r.Field<Int64>("PaymentId"),
                        BillId = r.Field<int>("BillId"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount")

                    }).ToList();
                }
            }

            return paymentInfo;
        }
        public List<PMSupplierPaymentLedgerBO> GetSupplierPaymentDetailsByPaymentAndLedger(Int64 paymentId)
        {

            List<PMSupplierPaymentLedgerBO> paymentInfo = new List<PMSupplierPaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPaymentDetailsByPaymentAndLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new PMSupplierPaymentLedgerBO
                    {
                        SupplierPaymentId = r.Field<Int64>("SupplierPaymentId"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        IsBillGenerated = r.Field<bool>("IsBillGenerated"),
                        PaymentDetailsId = r.Field<Int64>("PaymentDetailsId"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount")

                    }).ToList();
                }
            }

            return paymentInfo;
        }
        public bool DeletePayment(Int64 paymentId, int createdBy)
        {
            int status = 0;
            string query = string.Format(@"
                                DELETE FROM HotelCompanyPaymentDetails
                                WHERE PaymentId = {0}
                                DELETE FROM HotelCompanyPayment
                                WHERE PaymentId = {0}
                            ", paymentId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public List<SupplierPaymentLedgerVwBO> GetCNFLedger(int supplierId, DateTime dateFrom, DateTime dateTo, string paymentStatus, string reportType)
        {
            List<SupplierPaymentLedgerVwBO> supplierInfo = new List<SupplierPaymentLedgerVwBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCnfLedgerReport_SP"))
                {
                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (paymentStatus != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, paymentStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new SupplierPaymentLedgerVwBO
                    {
                        SupplierId = r.Field<Int32?>("SupplierId"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        ShowPaymentDate = r.Field<string>("ShowPaymentDate"),
                        Narration = r.Field<string>("Narration"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        BalanceCommulative = r.Field<decimal?>("BalanceCommulative"),
                        SupplierName = r.Field<string>("SupplierName")

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<PMSupplierBO> GetCNFInfoBySearchCriteria(string searchText)
        {
            List<PMSupplierBO> supplierList = new List<PMSupplierBO>();
            string query = "SELECT * FROM LCCnfInfo WHERE Name LIKE '%" + searchText + "%'";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet SupplierDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupplierDS, "Supplier");
                    DataTable table = SupplierDS.Tables["Supplier"];

                    supplierList = table.AsEnumerable().Select(r => new PMSupplierBO
                    {
                        SupplierId = r.Field<int>("SupplierId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code")

                    }).ToList();
                }
            }
            return supplierList;
        }
        public List<SupplierPaymentBO> GetSupplierPaymentBySearch(int userInfoId, int supplierId, DateTime? dateFrom, DateTime? dateTo, string paymentFor, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SupplierPaymentBO> paymentInfo = new List<SupplierPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPaymentBySearchForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    if (supplierId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (paymentFor != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, paymentFor);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new SupplierPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentFor = r.Field<string>("PaymentFor"),
                        SupplierId = r.Field<int>("SupplierId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        AdjustmentType = r.Field<string>("AdjustmentType"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }

            return paymentInfo;
        }
        public List<SupplierPaymentBO> GetSupplierPaymentInfoByPaymentId(int paymentId)
        {
            List<SupplierPaymentBO> paymentInfo = new List<SupplierPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierPaymentInfoByPaymentId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int32, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new SupplierPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        PaymentFor = r.Field<string>("PaymentFor"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentDateDisplay = r.Field<string>("PaymentDateDisplay"),
                        SupplierId = r.Field<int>("SupplierId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        ChecqueDateDisplay = r.Field<string>("ChecqueDateDisplay"),
                        PaymentAdjustmentAmount = r.Field<decimal>("PaymentAdjustmentAmount"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        PaymentType = r.Field<string>("PaymentType"),
                        AccountingPostingHead = r.Field<string>("AccountingPostingHead")
                    }).ToList();
                }
            }

            return paymentInfo;
        }
        public List<CompanyPaymentLedgerReportVwBo> GetSupplierAPAging(string reportType, int supplierId, DateTime asOfDate, int intervalBands, string intervalType)
        {
            List<CompanyPaymentLedgerReportVwBo> supplierInfo = new List<CompanyPaymentLedgerReportVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierAPAging_SP"))
                {
                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@AsOfDate", DbType.Date, asOfDate);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalBands", DbType.Int32, intervalBands);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalType", DbType.String, intervalType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerReportVwBo
                    {
                        CompanyId = r.Field<Int32>("CompanyId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ColumnOrderDisplay = r.Field<Int32>("ColumnOrderDisplay"),
                        ColumnAgingTitle = r.Field<string>("ColumnAgingTitle"),
                        ColumnAgingBalance = r.Field<decimal>("ColumnAgingBalance")
                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<CompanyPaymentLedgerReportVwBo> GetSupplierAPAgingDetail(string reportType, int supplierId, DateTime asOfDate, int intervalBands, string intervalType)
        {
            List<CompanyPaymentLedgerReportVwBo> supplierInfo = new List<CompanyPaymentLedgerReportVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierAPAgingDetail_SP"))
                {
                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@AsOfDate", DbType.Date, asOfDate);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalBands", DbType.Int32, intervalBands);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalType", DbType.String, intervalType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerReportVwBo
                    {
                        CompanyId = r.Field<Int32>("CompanyId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentDateDisplay = r.Field<string>("PaymentDateDisplay"),
                        BillNumber = r.Field<string>("BillNumber"),
                        TransactionAge = r.Field<Int32>("TransactionAge"),
                        DueAmount = r.Field<decimal>("DueAmount")
                    }).ToList();
                }
            }

            return supplierInfo;
        }
    }
}