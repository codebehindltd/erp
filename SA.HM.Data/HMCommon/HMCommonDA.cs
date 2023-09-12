using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using HotelManagement.Entity;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using QRCoder;
using System.Drawing;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Data.HMCommon
{
    public class HMCommonDA : BaseService
    {
        public RoomStatisticsInfoBO IsModuleWisePreviousDayTransaction(DateTime transactionDateTime, string moduleName)
        {
            RoomStatisticsInfoBO customFieldObject = new RoomStatisticsInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("IsModuleWisePreviousDayTransaction_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionDateTime", DbType.DateTime, transactionDateTime);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                            }
                        }
                    }
                }
            }

            return customFieldObject;
        }
        public RoomStatisticsInfoBO GetRoomStatisticsInfo()
        {
            RoomStatisticsInfoBO customFieldObject = new RoomStatisticsInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomStatisticsInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.ExpectedArrival = reader["ExpectedArrival"].ToString();
                                customFieldObject.ExpectedDeparture = reader["ExpectedDeparture"].ToString();
                                customFieldObject.CheckInRoom = reader["CheckInRoom"].ToString();
                                customFieldObject.WalkInRoom = reader["WalkInRoom"].ToString();
                                customFieldObject.RoomToSell = reader["RoomToSell"].ToString();
                                customFieldObject.RegisterComplaint = reader["RegisterComplaint"].ToString();
                                customFieldObject.InhouseRoomOrGuest = reader["InhouseRoomOrGuest"].ToString();
                                customFieldObject.ExtraAdultsOrChild = reader["ExtraAdultsOrChild"].ToString();
                                customFieldObject.InhouseForeigners = reader["InhouseForeigners"].ToString();
                                customFieldObject.GuestBlock = reader["GuestBlock"].ToString();
                                customFieldObject.AirportPickupDrop = reader["AirportPickupDrop"].ToString();
                                customFieldObject.Occupency = reader["Occupency"].ToString();
                            }
                        }
                    }
                }
            }

            return customFieldObject;
        }
        public Boolean UpdateTableInfoForNodeId(string tableName, string tablePKField, int pkId, int nodeId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTableInfoForNodeId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, tablePKField);
                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, pkId);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.String, nodeId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateVoucherPostTableForDealId(string tableName, string tablePKField, int pkId, int dealId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateVoucherPostTableForDealId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, tablePKField);
                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, pkId);
                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.String, dealId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateGLLedgerForVoucherPostingByDealId(int dealId, int fieldId, decimal currencyAmount, decimal ledgerAmount)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLLedgerForVoucherPostingByDealId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, dealId);
                    dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, fieldId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, currencyAmount);
                    dbSmartAspects.AddInParameter(command, "@LedgerAmount", DbType.Decimal, ledgerAmount);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteInfoById(string tableName, string tablePKField, long pkId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                        dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, tablePKField);
                        dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, pkId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean DeleteInfoByAnyIdAndDate(string tableName, string tableField1, string field1Value, string tableField2, string field2Value)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamicallyByAnyIdAndDate_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@TableField1", DbType.String, tableField1);
                    dbSmartAspects.AddInParameter(command, "@Field1Value", DbType.String, field1Value);
                    dbSmartAspects.AddInParameter(command, "@TableField2", DbType.String, tableField2);
                    dbSmartAspects.AddInParameter(command, "@Field2Value", DbType.String, field2Value);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public int DuplicateDataCountDynamicaly(string tableName, string fieldName, string fieldValue)
        {
            Boolean status = false;
            int IsDuplicate = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DuplicateDataCountDynamicaly_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, fieldName);
                    dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, fieldValue);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                IsDuplicate = Convert.ToInt32(reader["IsDuplicate"]);
                            }
                        }
                    }
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return IsDuplicate;
        }
        public int DuplicateCheckDynamicaly(string tableName, string fieldName, string fieldValue, int isUpdate, string pkFieldName, string pkFieldValue)
        {
            Boolean status = false;
            int IsDuplicate = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DuplicateCheckDynamicaly_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, fieldName);
                    dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, fieldValue);
                    dbSmartAspects.AddInParameter(command, "@IsUpdate", DbType.Int32, isUpdate);
                    dbSmartAspects.AddInParameter(command, "@PKFieldName", DbType.String, pkFieldName);
                    dbSmartAspects.AddInParameter(command, "@PKFieldValue", DbType.String, pkFieldValue);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                IsDuplicate = Convert.ToInt32(reader["IsDuplicate"]);
                            }
                        }
                    }
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return IsDuplicate;
        }
        public int DuplicateCheckDynamicaly(string tableName, string fieldName1, string fieldValue1, string fieldName2, string fieldValue2, int isUpdate, string pkFieldName, string pkFieldValue)
        {
            Boolean status = false;
            int IsDuplicate = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DuplicateCheckDynamicalyWithTwoColumn_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@FieldName1", DbType.String, fieldName1);
                    dbSmartAspects.AddInParameter(command, "@FieldValue1", DbType.String, fieldValue1);
                    dbSmartAspects.AddInParameter(command, "@FieldName2", DbType.String, fieldName2);
                    dbSmartAspects.AddInParameter(command, "@FieldValue2", DbType.String, fieldValue2);
                    dbSmartAspects.AddInParameter(command, "@IsUpdate", DbType.Int32, isUpdate);
                    dbSmartAspects.AddInParameter(command, "@PKFieldName", DbType.String, pkFieldName);
                    dbSmartAspects.AddInParameter(command, "@PKFieldValue", DbType.String, pkFieldValue);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                IsDuplicate = Convert.ToInt32(reader["IsDuplicate"]);
                            }
                        }
                    }
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return IsDuplicate;
        }
        public int DuplicateDataCheckDynamicalyWithCustomCondition(string tableName, string fieldName, string fieldValue, int isUpdate, string pkFieldName, string pkFieldValue, string customCondition)
        {
            Boolean status = false;
            int IsDuplicate = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DuplicateDataCheckDynamicalyWithCustomCondition_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, fieldName);
                    dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, fieldValue);
                    dbSmartAspects.AddInParameter(command, "@IsUpdate", DbType.Int32, isUpdate);
                    dbSmartAspects.AddInParameter(command, "@PKFieldName", DbType.String, pkFieldName);
                    dbSmartAspects.AddInParameter(command, "@PKFieldValue", DbType.String, pkFieldValue);
                    dbSmartAspects.AddInParameter(command, "@CustomCondition", DbType.String, customCondition);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                IsDuplicate = Convert.ToInt32(reader["IsDuplicate"]);
                            }
                        }
                    }
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return IsDuplicate;
        }
        public List<RoomStatusPossiblePathViewBO> GetRoomStatusPossiblePath(int userGroupId, string possiblePathType)
        {
            List<RoomStatusPossiblePathViewBO> menuGroup = new List<RoomStatusPossiblePathViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomStatusPossiblepath_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@PossiblePathType", DbType.String, possiblePathType);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "RoomStatusPossiblePath");
                    DataTable Table = SaleServiceDS.Tables["RoomStatusPossiblePath"];

                    menuGroup = Table.AsEnumerable().Select(r => new RoomStatusPossiblePathViewBO
                    {
                        PathId = r.Field<int>("PathId"),
                        UserGroupId = r.Field<int>("UserGroupId"),
                        PossiblePath = r.Field<string>("PossiblePath"),
                        DisplayText = r.Field<string>("DisplayText"),
                        PossiblePathType = r.Field<string>("PossiblePathType"),
                        DisplayOrder = r.Field<int>("DisplayOrder")

                    }).ToList();
                }
            }
            return menuGroup;
        }
        public List<CustomFieldBO> GetCustomField(string FieldName)
        {
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCustomField_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FieldName", DbType.String, FieldName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CustomFieldBO customFieldObject = new CustomFieldBO();
                                customFieldObject.FieldId = Convert.ToInt32(reader["FieldId"]);
                                customFieldObject.FieldType = reader["FieldType"].ToString();
                                customFieldObject.FieldValue = reader["FieldValue"].ToString();
                                customFieldObject.Description = reader["Description"].ToString();
                                customFieldObject.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                fields.Add(customFieldObject);
                            }
                        }
                    }
                }
            }

            return fields;
        }
        public List<CustomFieldBO> GetRevenueDivisionInformation()
        {
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRevenueDivisionInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CustomFieldBO customFieldObject = new CustomFieldBO();
                                //customFieldObject.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                customFieldObject.ServiceName = reader["ServiceName"].ToString();
                                fields.Add(customFieldObject);
                            }
                        }
                    }
                }
            }

            return fields;
        }
        public List<CustomFieldBO> GetCustomField(string FieldName, string dropDownFirstValue = "")
        {
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            if (!string.IsNullOrEmpty(dropDownFirstValue))
            {
                CustomFieldBO customNoneObject = new CustomFieldBO();
                customNoneObject.FieldValue = dropDownFirstValue;
                fields.Add(customNoneObject);
            }

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCustomField_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FieldName", DbType.String, FieldName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CustomFieldBO customFieldObject = new CustomFieldBO();
                                customFieldObject.FieldId = Convert.ToInt32(reader["FieldId"]);
                                customFieldObject.FieldType = reader["FieldType"].ToString();
                                customFieldObject.FieldValue = reader["FieldValue"].ToString();
                                customFieldObject.Description = reader["Description"].ToString();
                                customFieldObject.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                fields.Add(customFieldObject);
                            }
                        }
                    }
                }
            }

            return fields;
        }
        public string CurrencyMask(string value)
        {
            string fraction = "";
            StringBuilder tmpNumber = new StringBuilder();

            // Handle after (.)
            int dotIndex = value.IndexOf('.');

            if (dotIndex != -1)
                fraction = value.Substring(dotIndex);

            if (fraction.Length == 0)
            {
                fraction = ".00";
            }
            else
                fraction = fraction.PadRight(3, '0');


            // Handle before (.)
            if (dotIndex == -1 && value.Length > 0)
                tmpNumber = new StringBuilder(value);
            else if (dotIndex != -1)
                tmpNumber = new StringBuilder(value.Substring(0, dotIndex));
            else
                tmpNumber = new StringBuilder("0");

            if (tmpNumber.Length > 3)
            {
                for (int i = (tmpNumber.Length - 3), j = 1; i >= 0; i -= 2, j++)
                {
                    if (j == 4) break;
                    tmpNumber.Insert(i, ',');
                }
            }

            if (tmpNumber[0] == ',') tmpNumber.Remove(0, 1);

            return tmpNumber.Append(fraction).ToString();
        }
        public CustomFieldBO GetCustomFieldByFieldName(string FieldName)
        {
            CustomFieldBO customFieldObject = new CustomFieldBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCustomField_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FieldName", DbType.String, FieldName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.FieldId = Convert.ToInt32(reader["FieldId"]);
                                customFieldObject.FieldType = reader["FieldType"].ToString();
                                customFieldObject.FieldValue = reader["FieldValue"].ToString();
                                customFieldObject.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                            }
                        }
                    }
                }
            }

            return customFieldObject;
        }
        public CustomFieldBO GetCustomFieldByTypeAndValue(string fieldType, string fieldValue)
        {
            CustomFieldBO customFieldObject = new CustomFieldBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCustomFieldByTypeAndValue_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FieldType", DbType.String, fieldType);
                    dbSmartAspects.AddInParameter(cmd, "@FieldValue", DbType.String, fieldValue);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.FieldId = Convert.ToInt32(reader["FieldId"]);
                                customFieldObject.FieldType = reader["FieldType"].ToString();
                                customFieldObject.FieldValue = reader["FieldValue"].ToString();
                                customFieldObject.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                            }
                        }
                    }
                }
            }

            return customFieldObject;
        }
        public CommonNodeMatrixBO GetCommonNodeMatrixInfoById(int nodeId)
        {
            CommonNodeMatrixBO nodeMatrixBO = new CommonNodeMatrixBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                nodeMatrixBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.AncestorHead = reader["AncestorHead"].ToString();
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return nodeMatrixBO;
        }
        public List<CountriesBO> GetAllCountries()
        {
            List<CountriesBO> countryList = new List<CountriesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCountryList_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CountriesBO countryBO = new CountriesBO();
                                countryBO.CountryId = Convert.ToInt32(reader["CountryId"]);
                                countryBO.CountryName = reader["CountryName"].ToString().Trim();
                                countryBO.Nationality = reader["Nationality"].ToString().Trim();
                                countryBO.Code2Digit = reader["Code2Digit"].ToString();
                                countryBO.Code3Digit = reader["Code3Digit"].ToString();
                                countryBO.CodeNumeric = reader["CodeNumeric"].ToString();

                                countryList.Add(countryBO);
                            }
                        }
                    }
                }
            }

            return countryList;
        }
        public List<CountriesBO> GetCountriesBySearch(string searchTerm)
        {
            List<CountriesBO> countryList = new List<CountriesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCountryListBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CountriesBO countryBO = new CountriesBO();
                                countryBO.CountryId = Convert.ToInt32(reader["CountryId"]);
                                countryBO.CountryName = reader["CountryName"].ToString().Trim();
                                countryBO.Nationality = reader["Nationality"].ToString().Trim();
                                countryBO.Code2Digit = reader["Code2Digit"].ToString();
                                countryBO.Code3Digit = reader["Code3Digit"].ToString();
                                countryBO.CodeNumeric = reader["CodeNumeric"].ToString();

                                countryList.Add(countryBO);
                            }
                        }
                    }
                }
            }

            return countryList;
        }
        public List<StateBO> GetStateForAutoSearchByCountry(string searchTerm, int countryId)
        {
            List<StateBO> StateList = new List<StateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetStateListForAutoSearchByCountry_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);
                    dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, countryId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                StateBO stateBO = new StateBO();
                                stateBO.Id = Convert.ToInt64(reader["Id"]);
                                stateBO.CountryId = Convert.ToInt32(reader["CountryId"]);
                                stateBO.StateName = (reader["StateName"]).ToString();

                                StateList.Add(stateBO);
                            }
                        }
                    }
                }
            }

            return StateList;
        }
        public CountriesBO GetCountriesById(int countryId)
        {
            CountriesBO country = new CountriesBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCountryById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, countryId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                country.CountryId = Convert.ToInt32(reader["CountryId"]);
                                country.CountryName = reader["CountryName"].ToString().Trim();
                                country.Nationality = reader["Nationality"].ToString().Trim();
                                country.Code2Digit = reader["Code2Digit"].ToString();
                                country.Code3Digit = reader["Code3Digit"].ToString();
                                country.CodeNumeric = reader["CodeNumeric"].ToString();
                            }
                        }
                    }
                }
            }

            return country;
        }
        public string GetHTMLGuestGridView(List<GuestInformationBO> list)
        {
            string strTable = "";

            if (list.Count() <= 0)
            {
                strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation_Guest' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Country Name</th><th align='left' scope='col'>Phone</th><th align='left' scope='col'>Email</th><th align='left' scope='col'>Room No.</th></tr>";
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
                strTable += "</table>";

                return strTable;
            }

            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation_Guest' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Country Name</th><th align='left' scope='col'>Phone</th><th align='left' scope='col'>Email</th><th align='left' scope='col'>Room No.</th></tr>";
            int counter = 0;
            foreach (GuestInformationBO dr in list)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestName + "</td>";
                }
                else
                {
                    //  It's odd
                    strTable += "<tr style='background-color:White;'><td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestName + "</td>";
                }

                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.CountryName + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestPhone + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.RoomNumber + "</td>";


                strTable += "</td></tr>";


            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;

        }
        public string GetHTMLGuestDetailInfoGridView(List<GuestInformationBO> list)
        {
            string strTable = "";

            if (list.Count() <= 0)
            {
                strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation_Guest' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Country Name</th><th align='left' scope='col'>Phone</th><th align='left' scope='col'>Email</th><th align='left' scope='col'>Room No.</th></tr>";
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
                strTable += "</table>";

                return strTable;
            }

            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation_Guest' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Country Name</th><th align='left' scope='col'>Phone</th><th align='left' scope='col'>Email</th><th align='left' scope='col'>Room No.</th></tr>";
            int counter = 0;
            foreach (GuestInformationBO dr in list)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestForDetailInformation(" + dr.GuestId + ")'>" + dr.GuestName + "</td>";
                }
                else
                {
                    //  It's odd
                    strTable += "<tr style='background-color:White;'><td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestForDetailInformation(" + dr.GuestId + ")'>" + dr.GuestName + "</td>";
                }

                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestForDetailInformation(" + dr.GuestId + ")'>" + dr.CountryName + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestForDetailInformation(" + dr.GuestId + ")'>" + dr.GuestPhone + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestForDetailInformation(" + dr.GuestId + ")'>" + dr.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestForDetailInformation(" + dr.GuestId + ")'>" + dr.RoomNumber + "</td>";
                strTable += "</td></tr>";
            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;

        }
        public GuestInformationBO GetGuestDetailInformation(string GuestId)
        {
            GuestInformationBO guestBO = new GuestInformationBO();
            if (!String.IsNullOrEmpty(GuestId))
            {
                GuestDocumentsDA documentDA = new GuestDocumentsDA();
                GuestInformationDA guestDA = new GuestInformationDA();
                guestBO = guestDA.GetGuestInformationByGuestId(Convert.ToInt32(GuestId));
                guestBO.Path = @"Image/" + documentDA.GetImageName(GuestId, "");
            }
            return guestBO;
        }
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] info = type.GetMember(en.ToString());

            if (info != null && info.Length > 0)
            {
                object[] attrs = info[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
        public string GetCustomFieldValueByFieldName(string FieldName)
        {
            CustomFieldBO customFieldObject = new CustomFieldBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCustomField_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FieldName", DbType.String, FieldName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.FieldValue = reader["FieldValue"].ToString();
                            }
                        }
                    }
                }
            }

            return customFieldObject.FieldValue;
        }
        public string GetOutletImageNameByCostCenterId(int costCenterId)
        {
            CustomFieldBO customFieldObject = new CustomFieldBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOutletImageNameByCostCenterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.FieldValue = reader["CostCenterLogo"].ToString();
                            }
                        }
                    }
                }
            }

            return customFieldObject.FieldValue;
        }
        public DayCloseBO GetHotelDayCloseInformation(DateTime dayClossingDate)
        {
            DayCloseBO dayCloseBO = new DayCloseBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelDayCloseInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DayClossingDate", DbType.DateTime, dayClossingDate);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    dayCloseBO.DayCloseId = Convert.ToInt32(reader["DayCloseId"]);
                                    dayCloseBO.DayCloseDate = reader["DayCloseDate"].ToString();
                                    dayCloseBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                    dayCloseBO.CreatedDate = reader["CreatedDate"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dayCloseBO;
        }
        public DayCloseBO GetPayrollMonthlyAttendanceProcessLog(DateTime processFromDate, DateTime processToDate)
        {
            DayCloseBO dayCloseBO = new DayCloseBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollMonthlyAttendanceProcessLog_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProcessFromDate", DbType.DateTime, processFromDate);
                        dbSmartAspects.AddInParameter(cmd, "@ProcessToDate", DbType.DateTime, processToDate);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    dayCloseBO.Id = Convert.ToInt64(reader["Id"]);
                                    dayCloseBO.ProcessFromDate = Convert.ToDateTime(reader["ProcessFromDate"]);
                                    dayCloseBO.ProcessToDate = Convert.ToDateTime(reader["ProcessToDate"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dayCloseBO;
        }
        public List<CustomFieldBO> GetCustomFieldList(string FieldName)
        {
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCustomField_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FieldName", DbType.String, FieldName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CustomFieldBO customFieldObject = new CustomFieldBO();
                                customFieldObject.FieldId = Convert.ToInt32(reader["FieldId"]);
                                customFieldObject.FieldType = reader["FieldType"].ToString();
                                customFieldObject.FieldValue = reader["FieldValue"].ToString();
                                customFieldObject.Description = reader["Description"].ToString();
                                customFieldObject.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                fields.Add(customFieldObject);
                            }
                        }
                    }
                }
            }


            return fields;

        }
        public Boolean UpdateUploadedDocumentsInformationByOwnerId(long ownerId, long randomId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateUploadedDocumentsInformationByOwnerId_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@OwnerId", DbType.Int64, ownerId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.Int64, randomId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }

        public Boolean UpdateItemAverageCostByItemId(long itemId, decimal itemAverageCost)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateItemAverageCostByItemId_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@ItemId", DbType.Int64, itemId);
                    dbSmartAspects.AddInParameter(commandDocument, "@AverageCost", DbType.Decimal, itemAverageCost);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }
        public bool UpdateUploadedDocumentsInformation(string docType, string docPath, int OwnerId)
        {
            bool stauts = true;
            string strDocName = string.Empty;
            string strNewDocName = string.Empty;
            string strDocPath = string.Empty;
            string strDocExtension = string.Empty;
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count == 0)
            {
                return true;
            }
            if (docList.Count > 0)
            {
                var Image = docList[0];

                strDocPath = docPath;
                strDocName = Image.Name;
                strDocExtension = Image.Extention;
                strNewDocName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                string newPath = Path.Combine(strDocPath, strNewDocName);
                try
                {
                    File.Move(strDocPath + strDocName, newPath + strDocExtension);
                }
                catch (Exception e)
                {

                }

                DocumentsBO docBO = new DocumentsBO();
                docBO.DocumentId = docList[0].DocumentId;
                docBO.OwnerId = docList[0].OwnerId;
                docBO.DocumentCategory = docList[0].DocumentCategory;
                docBO.DocumentType = docList[0].DocumentType;
                docBO.Extention = docList[0].Extention;
                docBO.Name = strNewDocName + strDocExtension;
                docBO.Path = docList[0].Path;
                docBO.LastModified = docList[0].LastModified;
                Boolean updateStatus = docDA.UpdateDocumentsInfoByOwnerId(docBO);
                if (!updateStatus)
                {
                    stauts = false;
                }
            }

            return stauts;
        }
        public string GetRegistrationIdList(string registrationIdList)
        {
            CustomFieldBO customFieldObject = new CustomFieldBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationIdList", DbType.String, registrationIdList);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.FieldValue = reader["RegistrationIdList"].ToString();
                            }
                        }
                    }
                }
            }

            return customFieldObject.FieldValue;
        }
        public string GetRegistrationIdListWithoutCDateCheckOut(string registrationIdList, DateTime ledgerDate)
        {
            CustomFieldBO customFieldObject = new CustomFieldBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationIdListWithoutCDateCheckOut_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationIdList", DbType.String, registrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@LedgerDate", DbType.DateTime, ledgerDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.FieldValue = reader["RegistrationIdList"].ToString();
                            }
                        }
                    }
                }
            }

            return customFieldObject.FieldValue;
        }
        public string GetPaidByDetailsInformationForRestaurantInvoice(string transactionType, string transactionId)
        {
            HMCommonSetupBO customFieldObject = new HMCommonSetupBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaidByDetailsInformationForRestaurantInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.String, transactionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                customFieldObject.PayByDetails = reader["PayByDetails"].ToString();
                            }
                        }
                    }
                }
            }

            return customFieldObject.PayByDetails;
        }
        public HMCommonSetupBO GetFeatureWisePermission(string TypeName, string SetupName, int UserInfoId, string Feature, int CreatedBy)
        {
            HMCommonSetupBO commonBO = new HMCommonSetupBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFeatureWisePermission"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, TypeName);
                    dbSmartAspects.AddInParameter(cmd, "@SetupName", DbType.String, SetupName);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.String, UserInfoId);
                    dbSmartAspects.AddInParameter(cmd, "@Feature", DbType.String, Feature);
                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.String, CreatedBy);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                commonBO.IsCanCheck = Convert.ToInt32(reader["IsCanCheck"]);
                                commonBO.IsCanApprove = Convert.ToInt32(reader["IsCanApprove"]);
                            }
                        }
                    }
                }
            }

            return commonBO;
        }
        public static List<DateTime> GetDateArrayBetweenTwoDates(DateTime StartDate, DateTime EndDate)
        {
            var dates = new List<DateTime>();

            for (var dt = StartDate; dt <= EndDate; dt = dt.AddDays(1))
            {
                //dates.Add(dt.AddDays(1).AddSeconds(-1));
                dates.Add(dt);
            }
            return dates;
        }
        public static List<RoomCalenderBO> GetRoomCalenderList(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(StartDate, EndDate);
            return calenderList;
        }
        public List<RoomAvailableStatusBO> GetRoomWithAvailableStatusByDate(DateTime startDate, DateTime endDate, string reportType)
        {
            List<DateTime> dateList = new List<DateTime>();
            dateList = GetDateArrayBetweenTwoDates(startDate, endDate);
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            calenderList = GetRoomCalenderList(startDate, endDate);

            foreach (RoomCalenderBO rrow in calenderList)
            {
                if (rrow.TransectionStatus == "OutOfOrder")
                {
                    rrow.RoomType = "Out Of Order";
                }

                if (rrow.TransectionStatus == "OutOfService")
                {
                    rrow.RoomType = "Out Of Service";
                }

                if (rrow.CheckIn.Date < rrow.CheckOut.Date)
                {
                    rrow.CheckOut = rrow.CheckOut.AddDays(-1);
                }
            }


            List<RoomAvailableStatusBO> availableList = new List<RoomAvailableStatusBO>();

            List<RoomTypeBO> typeList = new List<RoomTypeBO>();
            RoomTypeDA typeDA = new RoomTypeDA();
            typeList = typeDA.GetRoomTypeInfoWithRoomCount();

            int hotelTotalRoomQty = 0;
            //HMUtility hmUtility = new HMUtility();

            foreach (DateTime dt in dateList)
            {
                foreach (RoomTypeBO rt in typeList)
                {
                    int OutOfOrderAndOutOfServiceRoomCount = 0;
                    RoomAvailableStatusBO availableBO = new RoomAvailableStatusBO();
                    availableBO.TransectionDate = dt;
                    //availableBO.TransectionDateDisplay = hmUtility.GetStringFromDateTime(dt);
                    availableBO.TransectionOrderId = 1;
                    //var rs = (from c in calenderList where c.RoomType == rt.RoomType && (dt.Date >= c.CheckIn.Date && dt.Date <= c.CheckOut.Date) select c).ToList();
                    var rs = (from c in calenderList where c.RoomType == rt.RoomType && (dt.Date >= c.CheckIn.Date && dt.Date < c.OriginalCheckOutDate.Date) select c).ToList();

                    if (rs.Count() != 0)
                    {
                        if (rs[0].RoomType == "Out Of Order")
                        {
                            OutOfOrderAndOutOfServiceRoomCount = OutOfOrderAndOutOfServiceRoomCount + 1;
                            availableBO.TransectionOrderId = 2;
                            availableBO.RoomType = rs[0].RoomType;
                            availableBO.TotalAvailable = rs.Count();
                        }
                        else if (rs[0].RoomType == "Out Of Service")
                        {
                            OutOfOrderAndOutOfServiceRoomCount = OutOfOrderAndOutOfServiceRoomCount + 1;
                            availableBO.TransectionOrderId = 3;
                            availableBO.RoomType = rs[0].RoomType;
                            availableBO.TotalAvailable = rs.Count();
                        }
                        else
                        {
                            availableBO.RoomType = rs[0].RoomType + " (" + rt.TotalRoom.ToString() + ")";
                            if (reportType == "Availability")
                            {
                                availableBO.TotalAvailable = (rt.TotalRoom * rt.PaxQuantity) - rs.Count();
                            }
                            else
                            {
                                availableBO.TotalAvailable = rs.Count();
                            }
                        }
                    }
                    else
                    {
                        if (rt.RoomType == "Out Of Order")
                        {
                            OutOfOrderAndOutOfServiceRoomCount = OutOfOrderAndOutOfServiceRoomCount + 1;
                            availableBO.TransectionOrderId = 2;
                            availableBO.RoomType = rt.RoomType;
                            availableBO.TotalAvailable = rs.Count();
                        }
                        else if (rt.RoomType == "Out Of Service")
                        {
                            OutOfOrderAndOutOfServiceRoomCount = OutOfOrderAndOutOfServiceRoomCount + 1;
                            availableBO.TransectionOrderId = 3;
                            availableBO.RoomType = rt.RoomType;
                            availableBO.TotalAvailable = rs.Count();
                        }
                        else
                        {
                            availableBO.RoomType = rt.RoomType + " (" + rt.TotalRoom.ToString() + ")";
                            if (reportType == "Availability")
                            {
                                availableBO.TotalAvailable = rt.TotalRoom;
                            }
                            else
                            {
                                availableBO.TotalAvailable = rs.Count();
                            }
                        }
                    }

                    hotelTotalRoomQty = hotelTotalRoomQty + rt.TotalRoom;
                    availableBO.TotalRoomNumber = rt.TotalRoom;
                    availableBO.RoomTypeId = rt.RoomTypeId;
                    availableBO.OutOfOrderAndOutOfServiceRoomCount = OutOfOrderAndOutOfServiceRoomCount;
                    availableList.Add(availableBO);
                }
            }

            return availableList;
        }
        public int GetRoomAvailableQuantity(Int64 reservationId, DateTime startDate, DateTime endDate, string roomTypeId, string roomQuantityEntered)
        {
            //Get all room available room types
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> roomTypeList = roomTypeDA.GetRoomTypeInfoWithRoomCount();

            int totalRoomCount = 1;
            totalRoomCount = roomTypeList.Sum(item => item.TotalRoom);

            RoomNumberDA entityDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberInfoBO = entityDA.GetRoomNumberInfo().Where(x => x.IsPMDummyRoom == false).ToList();

            if (roomNumberInfoBO != null)
            {
                totalRoomCount = roomNumberInfoBO.Count();
            }

            //Getting all active registration information
            RoomRegistrationDA rRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> activeRegistrations = rRegistrationDA.GetActiveRoomRegistrationWithRoomType();

            DateTime fromDate = DateTime.Today;
            List<DateTime> DateList = new List<DateTime>();
            DateList = GetDateArrayBetweenTwoDates(fromDate, endDate);

            //Getting reservation information from today to end of current month
            RoomReservationDA rReservationDA = new RoomReservationDA();
            List<RoomReservationBO> reservationList = rReservationDA.GetRoomReservationRoomInfoByDateRange(fromDate.AddSeconds(-1), endDate);

            //Calculating Inhouse Guest counts for each room type
            foreach (RoomTypeBO rType in roomTypeList)
            {
                rType.OccupiedRoomCount = activeRegistrations.Where(rr => rr.RoomTypeId == rType.RoomTypeId).Count();
            }

            int todaysInhouse = activeRegistrations.Count;

            //Getting out of order information
            List<RoomLogFileBO> outOfOrderOutOfServiceLog = entityDA.GetRoomLogFileInfoByDateRange(fromDate, endDate);
            List<RoomLogFileBO> outOfOrderLog;
            List<RoomLogFileBO> outOfServiceLog;

            List<RoomCalenderBO> calenderReportListBO = new List<RoomCalenderBO>();

            foreach (DateTime dateRow in DateList)
            {
                // // ------------Available Rooms
                RoomCalenderBO trCountBO = new RoomCalenderBO();
                trCountBO.TransactionDate = dateRow;
                trCountBO.ServiceType = "Available Rooms";
                trCountBO.ServiceQuantity = Convert.ToDecimal(totalRoomCount);
                trCountBO.DisplayQuantity = Convert.ToInt64(totalRoomCount).ToString();
                calenderReportListBO.Add(trCountBO);

                // // ------------Expected Arrivals
                int eciCount = reservationList.Where(x => x.DateIn.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

                RoomCalenderBO eciCountBO = new RoomCalenderBO();
                eciCountBO.TransactionDate = dateRow;
                eciCountBO.ServiceType = "Expected Arrivals";
                eciCountBO.ServiceQuantity = eciCount;
                eciCountBO.DisplayQuantity = Convert.ToInt64(eciCount).ToString();
                calenderReportListBO.Add(eciCountBO);

                // // ------------In House Guest
                RoomCalenderBO ihCountBO = new RoomCalenderBO();
                ihCountBO.TransactionDate = dateRow;
                ihCountBO.ServiceType = "Stay On (In House)";
                ihCountBO.ServiceQuantity = todaysInhouse;
                ihCountBO.DisplayQuantity = Convert.ToInt64(todaysInhouse).ToString();
                calenderReportListBO.Add(ihCountBO);

                int ecoCount = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date).Sum(i => i.TotalRoomNumber);

                // // ------------Expected Departure
                RoomCalenderBO ecoCountBO = new RoomCalenderBO();
                ecoCountBO.TransactionDate = dateRow;
                ecoCountBO.ServiceType = "Expected Departure";
                ecoCountBO.ServiceQuantity = ecoCount;
                ecoCountBO.DisplayQuantity = Convert.ToInt64(ecoCount).ToString();
                calenderReportListBO.Add(ecoCountBO);

                // // ------------Occupency Count
                int ocCount = todaysInhouse + eciCount - ecoCount;

                RoomCalenderBO ocCountBO = new RoomCalenderBO();
                ocCountBO.TransactionDate = dateRow;
                ocCountBO.ServiceType = "Occupency";
                ocCountBO.ServiceQuantity = ocCount;
                ocCountBO.DisplayQuantity = Convert.ToInt64(ocCount).ToString();
                calenderReportListBO.Add(ocCountBO);

                ///-----Out Of Order
                if (dateRow.Date == DateTime.Now.Date)
                    outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Order").ToList();
                else
                    outOfOrderLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Order").ToList();
                int outOfOrderCount = outOfOrderLog.Select(r => r.RoomId).Distinct().Count();
                RoomCalenderBO oooBO = new RoomCalenderBO();
                oooBO.TransactionDate = dateRow;
                oooBO.ServiceType = "Out Of Order";
                oooBO.ServiceQuantity = outOfOrderCount;
                oooBO.DisplayQuantity = outOfOrderCount.ToString();
                calenderReportListBO.Add(oooBO);

                ///-----Out Of Service
                if (dateRow.Date == DateTime.Now.Date)
                    outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ToDate && x.StatusName == "Out of Service").ToList();
                else
                    outOfServiceLog = outOfOrderOutOfServiceLog.Where(x => x.FromDate.Date <= dateRow.Date && dateRow.Date <= x.ToDate.Date && x.StatusName == "Out of Service").ToList();
                int outOfServiceCount = outOfServiceLog.Select(r => r.RoomId).Distinct().Count();
                RoomCalenderBO oocBO = new RoomCalenderBO();
                oocBO.TransactionDate = dateRow;
                oocBO.ServiceType = "Out Of Service";
                oocBO.ServiceQuantity = outOfServiceCount;
                oocBO.DisplayQuantity = outOfServiceCount.ToString();
                calenderReportListBO.Add(oocBO);

                ///Total Room Position
                int positionCount = totalRoomCount - ocCount - outOfOrderCount - outOfServiceCount;
                RoomCalenderBO positionBO = new RoomCalenderBO();
                positionBO.TransactionDate = dateRow;
                positionBO.ServiceType = "Position";
                positionBO.ServiceQuantity = positionCount;
                positionBO.DisplayQuantity = positionCount.ToString();
                calenderReportListBO.Add(positionBO);

                // // ------------Occupency Percentage (%)
                decimal ocpCount = Convert.ToDecimal((ocCount * 100) / totalRoomCount);

                RoomCalenderBO ocpCountBO = new RoomCalenderBO();
                ocpCountBO.TransactionDate = dateRow;
                ocpCountBO.ServiceType = "Occupency %";
                ocpCountBO.ServiceQuantity = ocpCount;
                ocpCountBO.DisplayQuantity = ocpCount.ToString() + "%";
                calenderReportListBO.Add(ocpCountBO);

                RoomCalenderBO blankCountBO = new RoomCalenderBO();
                blankCountBO.TransactionDate = dateRow;
                blankCountBO.ServiceType = "";
                blankCountBO.ServiceQuantity = -99999;
                blankCountBO.DisplayQuantity = "-99999";
                calenderReportListBO.Add(blankCountBO);

                RoomCalenderBO roomTypeBO = new RoomCalenderBO();
                roomTypeBO.TransactionDate = dateRow;
                roomTypeBO.ServiceType = "Room Type";
                roomTypeBO.ServiceQuantity = 0;// dateRow.Day;
                roomTypeBO.DisplayQuantity = ""; //dateRow.Day.ToString();
                calenderReportListBO.Add(roomTypeBO);

                foreach (RoomTypeBO rType in roomTypeList)
                {
                    int eciCountRT = reservationList.Where(x => x.DateIn.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);

                    int ecoCountRT = activeRegistrations.Where(x => x.ExpectedCheckOutDate.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Count() + reservationList.Where(x => x.DateOut.Date == dateRow.Date && x.RoomTypeId == rType.RoomTypeId).Sum(i => i.TotalRoomNumber);
                    rType.OccupiedRoomCount += eciCountRT - ecoCountRT;

                    outOfOrderCount = outOfOrderLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();
                    outOfServiceCount = outOfServiceLog.Where(x => x.RoomTypeId == rType.RoomTypeId).Select(r => r.RoomId).Distinct().Count();

                    rType.AvailableRoomCount = rType.TotalRoom - rType.OccupiedRoomCount - outOfOrderCount - outOfServiceCount;

                    RoomCalenderBO ocpCountRoomTypeBO = new RoomCalenderBO();
                    ocpCountRoomTypeBO.TransactionDate = dateRow;
                    ocpCountRoomTypeBO.RoomTypeId = rType.RoomTypeId;
                    ocpCountRoomTypeBO.ServiceType = rType.RoomType;
                    ocpCountRoomTypeBO.ServiceQuantity = rType.AvailableRoomCount;
                    ocpCountRoomTypeBO.DisplayQuantity = rType.AvailableRoomCount.ToString();
                    calenderReportListBO.Add(ocpCountRoomTypeBO);
                }

                RoomCalenderBO vacantBO = new RoomCalenderBO();
                vacantBO.TransactionDate = dateRow;
                vacantBO.ServiceType = "Total Vacant";
                vacantBO.ServiceQuantity = positionCount;
                vacantBO.DisplayQuantity = positionCount.ToString();
                calenderReportListBO.Add(vacantBO);

                todaysInhouse = ocCount;
            }

            foreach (var item in calenderReportListBO.Where(w => w.TransactionDate < DateTime.Today))
            {
                item.ServiceQuantity = 0;
                item.DisplayQuantity = "";
            }

            calenderReportListBO = calenderReportListBO.Where(x => x.TransactionDate.Date >= startDate.Date).ToList().Where(x => x.RoomTypeId.ToString() == roomTypeId).ToList();

            int roomQuantity = 0;
            int enteredRoomQuantity = 0;
            enteredRoomQuantity = Convert.ToInt16(roomQuantityEntered);
            Boolean isReservationWillContinue = true;

            foreach (RoomCalenderBO rcRow in calenderReportListBO)
            {
                if (isReservationWillContinue)
                {
                    if (reservationId > 0)
                    {
                        RoomReservationDA roomReservationDA = new RoomReservationDA();
                        RoomReservationBO GetRoomReservationSummaryInfoBO = roomReservationDA.GetRoomReservationSummaryInfoById(reservationId);
                        if (GetRoomReservationSummaryInfoBO != null)
                        {
                            if (GetRoomReservationSummaryInfoBO.ReservationId > 0)
                            {
                                rcRow.ServiceQuantity = rcRow.ServiceQuantity + Convert.ToDecimal(GetRoomReservationSummaryInfoBO.RoomQuantity);
                            }
                        }
                    }

                    if (enteredRoomQuantity <= Convert.ToInt32(rcRow.ServiceQuantity))
                    {
                        roomQuantity = roomQuantity + Convert.ToInt32(rcRow.ServiceQuantity);
                    }
                    else
                    {
                        isReservationWillContinue = false;
                        roomQuantity = 0;
                    }
                }
            }

            return roomQuantity;
        }
        public List<RoomCalenderBO> GetHotelDetailedRoomPositionStatusInfo(DateTime positionDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDetailedHotelPosition_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PositionDate", DbType.DateTime, positionDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomCalenderBO calenderBO = new RoomCalenderBO();
                                calenderBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                calenderBO.RoomType = reader["RoomType"].ToString();
                                calenderBO.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);
                                calenderBO.ServiceType = reader["ServiceType"].ToString();
                                calenderBO.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"].ToString());
                                calenderList.Add(calenderBO);
                            }
                        }
                    }
                }
            }
            return calenderList;

        }
        public List<RoomCalenderBO> GetHotelForecastReportInfo(DateTime fromDate, DateTime toDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelForecastReportInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomCalenderBO calenderBO = new RoomCalenderBO();
                                calenderBO.TransactionDate = Convert.ToDateTime(reader["TransactionDate"].ToString());
                                calenderBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                calenderBO.RoomType = reader["RoomType"].ToString();
                                calenderBO.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);
                                calenderBO.ServiceType = reader["ServiceType"].ToString();
                                calenderBO.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"].ToString());
                                calenderBO.DisplayQuantity = reader["DisplayQuantity"].ToString();
                                calenderList.Add(calenderBO);
                            }
                        }
                    }
                }
            }
            return calenderList;

        }
        public string GetReservationGridView(List<RoomReservationInfoByDateRangeReportBO> list)
        {
            string strTable = "";

            if (list.Count() <= 0)
            {
                strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservationInformation' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Reservation No</th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Room Type</th><th align='left' scope='col'>Room No</th><th align='left' scope='col'>Check-In</th><th align='left' scope='col'>Check-Out</th></tr>";
                strTable = "<tr><td colspan='5' align='center'>No Record Available!</td></tr>";
                strTable += "</table>";

                return strTable;
            }

            strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservationInformation' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Reservation No</th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Room Type</th><th align='left' scope='col'>Room No</th><th align='left' scope='col'>Check-In</th><th align='left' scope='col'>Check-Out</th></tr>";
            int counter = 0;
            foreach (RoomReservationInfoByDateRangeReportBO dr in list)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    if (dr.PartialRegistration == 1)
                    {
                        strTable += "<tr style='background-color:#cbf5e2'>";
                    }
                    else
                    {
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }
                }
                else
                {
                    //  It's odd
                    if (dr.PartialRegistration == 1)
                    {
                        strTable += "<tr style='background-color:#cbf5e2'>";
                    }
                    else
                    {
                        strTable += "<tr style='background-color:White;'>";
                    }

                }

                strTable += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + dr.ReservationId + "," + dr.ReservationDetailId + "," + dr.RoomId + "," + 0 + ")'>" + dr.ReservationNumber + "</td>";
                strTable += "<td align='left' style='width: 30%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + dr.ReservationId + "," + dr.ReservationDetailId + "," + dr.RoomId + "," + 0 + ")'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + dr.ReservationId + "," + dr.ReservationDetailId + "," + dr.RoomId + "," + 0 + ")'>" + dr.RoomType + "</td>";
                strTable += "<td align='left' style='width: 30%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + dr.ReservationId + "," + dr.ReservationDetailId + "," + dr.RoomId + "," + 0 + ")'>" + dr.RoomNumber + "</td>";
                strTable += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + dr.ReservationId + "," + dr.ReservationDetailId + "," + dr.RoomId + "," + 0 + ")'>" + dr.CheckIn + "</td>";
                strTable += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + dr.ReservationId + "," + dr.ReservationDetailId + "," + dr.RoomId + "," + 0 + ")'>" + dr.DateOut + "</td>";

                strTable += "</tr>";


            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        public string GetReservationGridInfo(List<RoomReservationInfoByDateRangeReportBO> list)
        {
            string strTable = "";

            if (list.Count() <= 0)
            {
                strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservationInformation' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Reservation No</th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Room No</th><th align='left' scope='col'>Check-In</th><th align='left' scope='col'>Check-Out</th></tr>";
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
                strTable += "</table>";

                return strTable;
            }

            strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservationInformation' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Reservation No</th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Room No</th><th align='left' scope='col'>Check-In</th><th align='left' scope='col'>Check-Out</th></tr>";
            int counter = 0;
            foreach (RoomReservationInfoByDateRangeReportBO dr in list)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    //  It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return AssignReservationNumber(\"" + dr.ReservationNumber + "\")'>" + dr.ReservationNumber + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return AssignReservationNumber(\"" + dr.ReservationNumber + "\")'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return AssignReservationNumber(\"" + dr.ReservationNumber + "\")'>" + dr.RoomNumber + "</td>";
                strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return AssignReservationNumber(\"" + dr.ReservationNumber + "\")'>" + dr.CheckIn + "</td>";
                strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return AssignReservationNumber(\"" + dr.ReservationNumber + "\")'>" + dr.DateOut + "</td>";

                strTable += "</tr>";


            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        public string GetPrevGuestForReservation(List<GuestInformationBO> list)
        {
            string strTable = "";

            if (list.Count() <= 0)
            {
                strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservationInformation' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Reservation No</th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Room No</th><th align='left' scope='col'>Check-In</th><th align='left' scope='col'>Check-Out</th></tr>";
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
                strTable += "</table>";

                return strTable;
            }

            strTable += "<table class='table table-bordered table-condensed table-responsive' id='ReservationInformation' width= '100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Reservation No</th><th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Room No</th><th align='left' scope='col'>Check-In</th><th align='left' scope='col'>Check-Out</th></tr>";
            int counter = 0;
            foreach (GuestInformationBO dr in list)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    //  It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='left' style='width: 35%;cursor:pointer' onClick='javascript:return LoadPrevGuestInfo(" + dr.GuestId + ")'>" + dr.RegistrationNumber + "</td>";
                strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return LoadPrevGuestInfo(" + dr.GuestId + ")'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return LoadPrevGuestInfo(" + dr.GuestId + ")'>" + dr.RoomNumber + "</td>";
                strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return LoadPrevGuestInfo(" + dr.GuestId + ")'>" + dr.ShowCheckInDate + "</td>";
                strTable += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return LoadPrevGuestInfo(" + dr.GuestId + ")'>" + dr.ShowCheckOutDate + "</td>";

                strTable += "</tr>";


            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        public string GetPreferenceListView(List<GuestPreferenceBO> List)
        {
            string strTable = "";

            strTable += "<ul>";
            foreach (GuestPreferenceBO item in List)
            {
                strTable += "<li>" + item.PreferenceName + "</li>";
            }
            strTable += "</ul>";

            if (List.Count == 0)
            {
                strTable = "No Preferences Available !";
            }

            return strTable;
        }
        public DateTime GetModuleWisePreviousDayTransaction(string moduleName)
        {
            //string query = "SELECT dbo.FnIsModuleWisePreviousDayTransaction(GETDATE(), 'Restaurant') TransactionDate";
            string query = "SELECT dbo.FnIsModuleWisePreviousDayTransaction(GETDATE(), '" + moduleName + "') TransactionDate";
            DateTime TransactionDate = DateTime.Now;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TransactionDate = Convert.ToDateTime(reader["TransactionDate"].ToString());
                            }
                        }
                    }
                }
            }
            return TransactionDate;
        }
        public List<CommonPaymentModeBO> GetCommonPaymentModeInfo(string paymentMode)
        {
            List<CommonPaymentModeBO> fields = new List<CommonPaymentModeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonPaymentModeInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, paymentMode);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonPaymentModeBO customFieldObject = new CommonPaymentModeBO();
                                customFieldObject.PaymentModeId = Convert.ToInt32(reader["PaymentModeId"]);
                                customFieldObject.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                customFieldObject.PaymentMode = reader["PaymentMode"].ToString();
                                customFieldObject.PaymentCode = reader["PaymentCode"].ToString();
                                customFieldObject.Lvl = Convert.ToInt32(reader["Lvl"]);
                                customFieldObject.PaymentAccountsPostingId = Convert.ToInt32(reader["PaymentAccountsPostingId"]);
                                customFieldObject.ReceiveAccountsPostingId = Convert.ToInt32(reader["ReceiveAccountsPostingId"]);
                                customFieldObject.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                fields.Add(customFieldObject);
                            }
                        }
                    }
                }
            }

            return fields;
        }
        public Boolean SaveUpdateForRestaurantEmployeeAttendance(int bearerId, int createdOrUpdatedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUpdateForRestaurantEmployeeAttendance_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, bearerId);
                    dbSmartAspects.AddInParameter(command, "@CreatedOrUpdatedBy", DbType.Int32, createdOrUpdatedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool SaveOrUpdateCommonCustomFieldData(CustomFieldBO fieldBO, out int tmpId)
        {
            bool status = false;
            tmpId = 0;

            if (fieldBO.FieldId == 0 || fieldBO.FieldId.ToString() == "")
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCommonCustomFieldData_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FieldType", DbType.String, fieldBO.FieldType);
                        dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, fieldBO.FieldValue);

                        dbSmartAspects.AddOutParameter(command, "@FieldId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpId = Convert.ToInt32(command.Parameters["@FieldId"].Value);
                    }
                }
            }
            else
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCommonCustomFieldData_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, fieldBO.FieldId);
                        dbSmartAspects.AddInParameter(command, "@FieldType", DbType.String, fieldBO.FieldType);
                        dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, fieldBO.FieldValue);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public List<DayCloseBO> GetHotelDayCloseCheckingInformation(DateTime dayClossingDate)
        {
            List<DayCloseBO> dayCloseBOList = new List<DayCloseBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelDayCloseCheckingInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DayClossingDate", DbType.DateTime, dayClossingDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DayCloseBO dayCloseBO = new DayCloseBO();
                                dayCloseBO.DayCloseDate = reader["DayCloseDate"].ToString();
                                dayCloseBO.DataCount = Convert.ToInt32(reader["DataCount"]);
                                dayCloseBO.DayClosedDescription = reader["DayClosedDescription"].ToString();

                                dayCloseBOList.Add(dayCloseBO);
                            }
                        }
                    }
                }
            }

            return dayCloseBOList;
        }
        public List<DocumentsBO> GetDocumentListWithIcon(List<DocumentsBO> docList)
        {
            for (int i = 0; i < docList.Count; i++)
            {
                if (docList[i].Extention.ToLower() == ".doc" || docList[i].Extention.ToLower() == ".docx")
                {
                    docList[i].IconImage = "/Images/FileType/doc.png";
                }
                else if (docList[i].Extention.ToLower() == ".flv")
                {
                    docList[i].IconImage = "/Images/FileType/flv.png";
                }
                else if (docList[i].Extention.ToLower() == ".html")
                {
                    docList[i].IconImage = "/Images/FileType/html.png";
                }
                else if (docList[i].Extention.ToLower() == ".pdf")
                {
                    docList[i].IconImage = "/Images/FileType/pdf.png";
                }
                else if (docList[i].Extention.ToLower() == ".xls")
                {
                    docList[i].IconImage = "/Images/FileType/xls.png";
                }
                else if (docList[i].Extention.ToLower() == ".xlsx")
                {
                    docList[i].IconImage = "/Images/FileType/xlsx.png";
                }
                else if (docList[i].Extention.ToLower() == ".zip")
                {
                    docList[i].IconImage = "/Images/FileType/zip.png";
                }
                else if (docList[i].Extention.ToLower() == ".xml")
                {
                    docList[i].IconImage = "/Images/FileType/xml.png";
                }
                else
                {
                    docList[i].IconImage = "/Images/FileType/Unknown.png";
                }
            }
            return docList;
        }
        public List<CommonCheckByApproveByUserBO> GetCommonCheckByApproveByUserByPrimaryKey(string tableName, string primaryKey, string primaryKeyValue)
        {
            List<CommonCheckByApproveByUserBO> EmployeeList = new List<CommonCheckByApproveByUserBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonCheckByApproveByUserList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(cmd, "@PrimaryKey", DbType.String, primaryKey);
                    dbSmartAspects.AddInParameter(cmd, "@PrimaryKeyValue", DbType.String, primaryKeyValue);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonCheckByApproveByUserBO BO = new CommonCheckByApproveByUserBO();
                                BO.DisplayName = reader["DisplayName"].ToString();
                                BO.ApproveType = reader["ApproveType"].ToString();
                                EmployeeList.Add(BO);
                            }
                        }
                    }
                }
            }

            return EmployeeList;
        }
        public List<int> GetCommonCheckByApproveByListForSMS(string tableName, string primaryKeyName, string primaryKeyValue,
                                                            string featuresValue, string statusColumnName)
        {
            List<int> CheckByApproveByList = new List<int>();
            int CheckByApproveBy;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetCommonCheckByApproveByListForSMS_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(command, "@PKName", DbType.String, primaryKeyName);
                    dbSmartAspects.AddInParameter(command, "@PKValue", DbType.String, primaryKeyValue);
                    dbSmartAspects.AddInParameter(command, "@FeaturesValue", DbType.String, featuresValue);
                    dbSmartAspects.AddInParameter(command, "@StatusColumnName", DbType.String, statusColumnName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                CheckByApproveBy = Convert.ToInt32(reader["UserInfoId"]);
                                CheckByApproveByList.Add(CheckByApproveBy);
                            }
                        }
                    }
                }
            }
            return CheckByApproveByList;
        }
        public byte[] GenerateQrCode(string qrmsg)
        {
            QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
            QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrmsg, QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qRCode = new QRCoder.QRCode(qRCodeData);
            Bitmap bmp = qRCode.GetGraphic(5);
            // bmp.SetPixel(150,150, Color.Black);
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 150;
            //imgBarCode.Width = 150;
            using (Bitmap bitMap = qRCode.GetGraphic(5))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    return byteImage;
                }
            }
        }
    }
}
