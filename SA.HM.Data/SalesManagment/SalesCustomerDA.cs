using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesManagment
{
    public class SalesCustomerDA : BaseService
    {
        public SalesCustomerBO GetSalesCustomerInfoByCustomerId(int CustomerId)
        {
            SalesCustomerBO customerBO = new SalesCustomerBO();
            DataSet customerDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCustomerInfoByCustomerId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomerId", DbType.Int32, CustomerId);
                    dbSmartAspects.LoadDataSet(cmd, customerDS, "SalesCustomer");

                    DataTable table = customerDS.Tables["SalesCustomer"];
                    customerBO = table.AsEnumerable().Select(r =>
                                new SalesCustomerBO
                                {
                                    CustomerId = r.Field<int>("CustomerId"),
                                    CustomerType = r.Field<string>("CustomerType"),
                                    Name = r.Field<string>("Name"),
                                    Code = r.Field<string>("Code"),
                                    Email = r.Field<string>("Email"),
                                    Phone = r.Field<string>("Phone"),
                                    Address = r.Field<string>("Address"),
                                    WebAddress = r.Field<string>("WebAddress"),
                                    ContactPerson = r.Field<string>("ContactPerson"),
                                    ContactDesignation = r.Field<string>("ContactDesignation"),
                                    Department = r.Field<string>("Department"),
                                    ContactEmail = r.Field<string>("ContactEmail"),
                                    ContactPhone = r.Field<string>("ContactPhone"),
                                    ContactFax = r.Field<string>("ContactFax"),
                                    ContactPerson2 = r.Field<string>("ContactPerson2"),
                                    ContactDesignation2 = r.Field<string>("ContactDesignation2"),
                                    Department2 = r.Field<string>("Department2"),
                                    ContactEmail2 = r.Field<string>("ContactEmail2"),
                                    ContactPhone2 = r.Field<string>("ContactPhone2"),
                                    ContactFax2 = r.Field<string>("ContactFax2")

                                }).FirstOrDefault();

                    customerDS.Dispose();

                    //using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    //{
                    //    if (reader != null)
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            customerBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                    //            customerBO.CustomerType = reader["CustomerType"].ToString();
                    //            customerBO.Name = reader["Name"].ToString();
                    //            customerBO.Code = reader["Code"].ToString();
                    //            customerBO.Email = reader["Email"].ToString();
                    //            customerBO.Phone = reader["Phone"].ToString();
                    //            customerBO.Address = reader["Address"].ToString();
                    //            customerBO.WebAddress = reader["WebAddress"].ToString();

                    //            customerBO.ContactPerson = reader["ContactPerson"].ToString();
                    //            customerBO.ContactDesignation = reader["ContactDesignation"].ToString();
                    //            customerBO.ContactEmail = reader["ContactEmail"].ToString();
                    //            customerBO.ContactPhone = reader["ContactPhone"].ToString();
                    //            customerBO.ContactFax = reader["ContactFax"].ToString();
                    //        }
                    //    }
                    //}
                }
            }
            return customerBO;
        }

        public List<SalesCustomerBO> GetSalesCustomersInfoBySearchCriteria(string Name, string Code, string Phone)
        {
            string searchCriteria = string.Empty;
            searchCriteria = GenerteWereCondition(Name, Code, Phone);


            List<SalesCustomerBO> customerList = new List<SalesCustomerBO>();
            DataSet customerDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCustomersInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    dbSmartAspects.LoadDataSet(cmd, customerDS, "CustomerSearch");

                    DataTable table = customerDS.Tables["CustomerSearch"];
                    customerList = table.AsEnumerable().Select(r =>
                                new SalesCustomerBO
                                {
                                    CustomerId = r.Field<int>("CustomerId"),
                                    CustomerType = r.Field<string>("CustomerType"),
                                    Name = r.Field<string>("Name"),
                                    Code = r.Field<string>("Code"),
                                    Email = r.Field<string>("Email"),
                                    Phone = r.Field<string>("Phone"),
                                    Address = r.Field<string>("Address"),
                                    WebAddress = r.Field<string>("WebAddress"),
                                    ContactPerson = r.Field<string>("ContactPerson"),
                                    ContactDesignation = r.Field<string>("ContactDesignation"),
                                    Department = r.Field<string>("Department"),
                                    ContactEmail = r.Field<string>("ContactEmail"),
                                    ContactPhone = r.Field<string>("ContactPhone"),
                                    ContactFax = r.Field<string>("ContactFax"),
                                    ContactPerson2 = r.Field<string>("ContactPerson2"),
                                    ContactDesignation2 = r.Field<string>("ContactDesignation2"),
                                    Department2 = r.Field<string>("Department2"),
                                    ContactEmail2 = r.Field<string>("ContactEmail2"),
                                    ContactPhone2 = r.Field<string>("ContactPhone2"),
                                    ContactFax2 = r.Field<string>("ContactFax2")

                                }).ToList();

                    customerDS.Dispose();
                }
            }
            return customerList;
        }

        public bool SaveSalesCustomerInfo(SalesCustomerBO customarBO, out int tmpCustomerId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalesCustomerInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CustomerType", DbType.String, customarBO.CustomerType);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, customarBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Address", DbType.String, customarBO.Address);
                    dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, customarBO.Phone);
                    dbSmartAspects.AddInParameter(command, "@Email", DbType.String, customarBO.Email);
                    dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, customarBO.WebAddress);

                    dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, customarBO.ContactPerson);
                    dbSmartAspects.AddInParameter(command, "@ContactDesignation", DbType.String, customarBO.ContactDesignation);
                    dbSmartAspects.AddInParameter(command, "@Department", DbType.String, customarBO.Department);
                    dbSmartAspects.AddInParameter(command, "@ContactEmail", DbType.String, customarBO.ContactEmail);
                    dbSmartAspects.AddInParameter(command, "@ContactPhone", DbType.String, customarBO.ContactPhone);
                    dbSmartAspects.AddInParameter(command, "@ContactFax", DbType.String, customarBO.ContactFax);

                    dbSmartAspects.AddInParameter(command, "@ContactPerson2", DbType.String, customarBO.ContactPerson2);
                    dbSmartAspects.AddInParameter(command, "@ContactDesignation2", DbType.String, customarBO.ContactDesignation2);
                    dbSmartAspects.AddInParameter(command, "@Department2", DbType.String, customarBO.Department2);
                    dbSmartAspects.AddInParameter(command, "@ContactEmail2", DbType.String, customarBO.ContactEmail2);
                    dbSmartAspects.AddInParameter(command, "@ContactPhone2", DbType.String, customarBO.ContactPhone2);
                    dbSmartAspects.AddInParameter(command, "@ContactFax2", DbType.String, customarBO.ContactFax2);

                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, customarBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@CustomerId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpCustomerId = Convert.ToInt32(command.Parameters["@CustomerId"].Value);
                }
            }
            return status;
        }

        public bool UpdateSalesCustomerInfo(SalesCustomerBO customarBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalesCustomerInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, customarBO.CustomerId);
                    dbSmartAspects.AddInParameter(command, "@CustomerType", DbType.String, customarBO.CustomerType);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, customarBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Address", DbType.String, customarBO.Address);
                    dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, customarBO.Phone);
                    dbSmartAspects.AddInParameter(command, "@Email", DbType.String, customarBO.Email);
                    dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, customarBO.WebAddress);

                    dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, customarBO.ContactPerson);
                    dbSmartAspects.AddInParameter(command, "@ContactDesignation", DbType.String, customarBO.ContactDesignation);
                    dbSmartAspects.AddInParameter(command, "@Department", DbType.String, customarBO.Department);
                    dbSmartAspects.AddInParameter(command, "@ContactEmail", DbType.String, customarBO.ContactEmail);
                    dbSmartAspects.AddInParameter(command, "@ContactPhone", DbType.String, customarBO.ContactPhone);
                    dbSmartAspects.AddInParameter(command, "@ContactFax", DbType.String, customarBO.ContactFax);

                    dbSmartAspects.AddInParameter(command, "@ContactPerson2", DbType.String, customarBO.ContactPerson2);
                    dbSmartAspects.AddInParameter(command, "@ContactDesignation2", DbType.String, customarBO.ContactDesignation2);
                    dbSmartAspects.AddInParameter(command, "@Department2", DbType.String, customarBO.Department2);
                    dbSmartAspects.AddInParameter(command, "@ContactEmail2", DbType.String, customarBO.ContactEmail2);
                    dbSmartAspects.AddInParameter(command, "@ContactPhone2", DbType.String, customarBO.ContactPhone2);
                    dbSmartAspects.AddInParameter(command, "@ContactFax2", DbType.String, customarBO.ContactFax2);

                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, customarBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<SalesCustomerBO> GetAllSalesCustomerInfo()
        {
            List<SalesCustomerBO> customerList = new List<SalesCustomerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllSalesCustomerInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesCustomerBO customerBO = new SalesCustomerBO();
                                customerBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                customerBO.CustomerType = reader["CustomerType"].ToString();
                                customerBO.Name = reader["Name"].ToString();
                                customerBO.Code = reader["Code"].ToString();
                                customerBO.Email = reader["Email"].ToString();
                                customerBO.Phone = reader["Phone"].ToString();
                                customerBO.Address = reader["Address"].ToString();
                                customerBO.WebAddress = reader["WebAddress"].ToString();
                                customerBO.ContactPerson = reader["ContactPerson"].ToString();
                                customerBO.ContactDesignation = reader["ContactDesignation"].ToString();
                                customerBO.ContactEmail = reader["ContactEmail"].ToString();
                                customerBO.ContactPhone = reader["ContactPhone"].ToString();
                                customerBO.ContactFax = reader["ContactFax"].ToString();
                                customerBO.DisplayName = reader["DisplayName"].ToString();
                                customerList.Add(customerBO);

                            }
                        }
                    }
                }
            }
            return customerList;
        }

        public List<SalesCustomerBO> GetDistinctSalesCustomerInfo()
        {
            List<SalesCustomerBO> customerList = new List<SalesCustomerBO>();
            List<SalesCustomerBO> returnList = new List<SalesCustomerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDistinctSalesCustomerInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesCustomerBO customerBO = new SalesCustomerBO();
                                customerBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);

                                customerBO.Name = reader["Name"].ToString();

                                customerList.Add(customerBO);

                            }
                        }
                    }
                }
            }


            returnList = customerList.GroupBy(cust => cust.CustomerId).Select(grp => grp.First()).ToList();


            return returnList;
        }

        private string GenerteWereCondition(string Name, string Code, string Phone)
        {
            string Where = string.Empty, Condition = string.Empty;

            if (!string.IsNullOrEmpty(Name))
            {
                Condition += "Name LIKE '%" + Name + "%'";
            }

            if (!string.IsNullOrEmpty(Code))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND Code LIKE '% " + Code + " %'";
                }
                else
                {
                    Condition += "Code LIKE '%" + Code + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND Phone LIKE '%" + Phone + "%'";
                }
                else
                {
                    Condition += "Phone LIKE '%" + Phone + "%'";
                }
            }

            Where = !string.IsNullOrEmpty(Condition) ? ("WHERE " + Condition) : Condition;

            return Where;
        }

        public List<SalesCustomerBO> GetSalesCustomerInfoForReport(int CustomerId)
        {
            List<SalesCustomerBO> customerList = new List<SalesCustomerBO>();
            //DataSet customerDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCustomerInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomerId", DbType.Int32, CustomerId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesCustomer");
                    DataTable table = ds.Tables["SalesCustomer"];
                    customerList = table.AsEnumerable().Select(r =>
                                new SalesCustomerBO
                                {
                                    CustomerId = r.Field<int>("CustomerId"),
                                    CustomerType = r.Field<string>("CustomerType"),
                                    Name = r.Field<string>("Name"),
                                    Code = r.Field<string>("Code"),
                                    Email = r.Field<string>("Email"),
                                    Phone = r.Field<string>("Phone"),
                                    Address = r.Field<string>("Address"),                                    
                                    ContactPerson = r.Field<string>("ContactPerson"),
                                    ContactFax = r.Field<string>("Fax")                                    
                                }).ToList();                   
                }
            }
            return customerList;
        }

    }
}
