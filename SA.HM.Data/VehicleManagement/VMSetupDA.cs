using HotelManagement.Entity.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.VehicleManagement
{
    public class VMSetupDA : BaseService
    {
        public bool SaveDriver(VMDriverInformationBO informationBO, out int outId)
        {
            Boolean status = false;
            outId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateDriverInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, informationBO.Id);
                        dbSmartAspects.AddInParameter(command, "@DriverName", DbType.String, informationBO.DriverName);
                        dbSmartAspects.AddInParameter(command, "@DrivingLicenceNumber", DbType.String, informationBO.DrivingLicenceNumber);
                        dbSmartAspects.AddInParameter(command, "@DateOfBirth", DbType.DateTime, informationBO.DateOfBirth);
                        dbSmartAspects.AddInParameter(command, "@NID", DbType.String, informationBO.NID);
                        dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, informationBO.Phone);
                        dbSmartAspects.AddInParameter(command, "@Email", DbType.String, informationBO.Email);
                        dbSmartAspects.AddInParameter(command, "@EmergancyContactNumber", DbType.String, informationBO.EmergancyContactNumber);
                        dbSmartAspects.AddInParameter(command, "@EmergancyContactPerson", DbType.String, informationBO.EmergancyContactPerson);
                        dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, informationBO.EmployeeId);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, informationBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        outId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }
            }
            return status;
        }

        public VMDriverInformationBO GetDriverInformationById(long id)
        {
            VMDriverInformationBO infoBO = new VMDriverInformationBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDriverInformationById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    infoBO.Id = Convert.ToInt32(reader["Id"]);
                                    if ((reader["DateOfBirth"]) != DBNull.Value)
                                    {
                                        infoBO.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                                    }

                                    infoBO.DriverName = reader["DriverName"].ToString();
                                    infoBO.DrivingLicenceNumber = Convert.ToString(reader["DrivingLicenceNumber"]);
                                    infoBO.NID = Convert.ToString(reader["NID"]);
                                    infoBO.Phone = Convert.ToString(reader["Phone"]);
                                    infoBO.Email = reader["Email"].ToString();
                                    infoBO.EmergancyContactNumber = Convert.ToString(reader["EmergancyContactNumber"]);
                                    if ((reader["EmployeeId"]) != DBNull.Value)
                                    {
                                        infoBO.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                                    }
                                    infoBO.EmergancyContactPerson = reader["EmergancyContactPerson"].ToString();

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
            return infoBO;
        }

        public List<VMDriverInformationBO> GetDriverInformationGridding(string driverName, string licenceNumber, string phone, int empId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<VMDriverInformationBO> infoBOs = new List<VMDriverInformationBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDriverInformationBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrWhiteSpace(driverName))
                            dbSmartAspects.AddInParameter(cmd, "@DriverName", DbType.String, driverName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DriverName", DbType.String, DBNull.Value);

                        if (!string.IsNullOrWhiteSpace(licenceNumber))
                            dbSmartAspects.AddInParameter(cmd, "@DrivingLicenceNumber", DbType.String, licenceNumber);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DrivingLicenceNumber", DbType.String, DBNull.Value);


                        if (empId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);
                        }

                        if (!string.IsNullOrWhiteSpace(phone))
                            dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, phone);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    VMDriverInformationBO bO = new VMDriverInformationBO();
                                    bO.Id = Convert.ToInt64(reader["Id"]);
                                    bO.DriverName = Convert.ToString(reader["DriverName"]);
                                    bO.DrivingLicenceNumber = Convert.ToString(reader["DrivingLicenceNumber"]);
                                    bO.EmergancyContactNumber = Convert.ToString(reader["EmergancyContactNumber"]);
                                    bO.Phone = Convert.ToString(reader["Phone"]);

                                    infoBOs.Add(bO);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return infoBOs;
        }

        // Vehicle Infos
        public bool SaveUpdateVehicle(VMVehicleInformationBO informationBO, out int outId)
        {
            Boolean status = false;
            outId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveVehicle_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, informationBO.Id);
                        dbSmartAspects.AddInParameter(command, "@VehicleName", DbType.String, informationBO.VehicleName);
                        dbSmartAspects.AddInParameter(command, "@ModelName", DbType.String, informationBO.ModelName);
                        dbSmartAspects.AddInParameter(command, "@VehicleTypeId", DbType.String, informationBO.VehicleTypeId);
                        dbSmartAspects.AddInParameter(command, "@ModelYear", DbType.Int32, informationBO.ModelYear);
                        dbSmartAspects.AddInParameter(command, "@TaxValidationYear", DbType.Int32, informationBO.TaxValidationYear);
                        dbSmartAspects.AddInParameter(command, "@Fare", DbType.Decimal, informationBO.Fare);
                        dbSmartAspects.AddInParameter(command, "@PassengerCapacity", DbType.Int32, informationBO.PassengerCapacity);
                        dbSmartAspects.AddInParameter(command, "@NumberPlate", DbType.String, informationBO.NumberPlate);
                        dbSmartAspects.AddInParameter(command, "@TaxNumber", DbType.String, informationBO.TaxNumber);
                        dbSmartAspects.AddInParameter(command, "@InsuranceNumber", DbType.String, informationBO.InsuranceNumber);
                        dbSmartAspects.AddInParameter(command, "@AirConditioningType", DbType.String, informationBO.AirConditioningType);
                        dbSmartAspects.AddInParameter(command, "@FuelType", DbType.String, informationBO.FuelType);
                        dbSmartAspects.AddInParameter(command, "@EngineType", DbType.String, informationBO.EngineType);
                        dbSmartAspects.AddInParameter(command, "@BodyType", DbType.String, informationBO.BodyType);
                        dbSmartAspects.AddInParameter(command, "@IsABSEnable", DbType.Boolean, informationBO.IsABSEnable);
                        dbSmartAspects.AddInParameter(command, "@IsAirBagAvailable", DbType.Boolean, informationBO.IsAirBagAvailable);
                        dbSmartAspects.AddInParameter(command, "@Status", DbType.Boolean, informationBO.Status);
                        dbSmartAspects.AddInParameter(command, "@AccountHeadId", DbType.Int32, informationBO.AccountHeadId);
                        dbSmartAspects.AddInParameter(command, "@ManufacturerId", DbType.Int32, informationBO.ManufacturerId);
                        dbSmartAspects.AddInParameter(command, "@Mileage", DbType.Decimal, informationBO.Mileage);
                        dbSmartAspects.AddInParameter(command, "@FuelTankCapacity", DbType.Decimal, informationBO.FuelTankCapacity);
                        dbSmartAspects.AddInParameter(command, "@EngineCapacity", DbType.Decimal, informationBO.EngineCapacity);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, informationBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        outId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }
            }
            return status;
        }

        public VMVehicleInformationBO GetVehicleInformationById(long id)
        {
            VMVehicleInformationBO infoBO = new VMVehicleInformationBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVehicleInformationById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    infoBO.Id = Convert.ToInt32(reader["Id"]);

                                    infoBO.VehicleName = reader["VehicleName"].ToString();
                                    infoBO.ModelName = Convert.ToString(reader["ModelName"]);
                                    if ((reader["AccountHeadId"]) != DBNull.Value)
                                    {
                                        infoBO.AccountHeadId = Convert.ToInt32(reader["AccountHeadId"]);
                                    }
                                    if ((reader["ManufacturerId"]) != DBNull.Value)
                                    {
                                        infoBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                    }
                                    if ((reader["ModelYear"]) != DBNull.Value)
                                    {
                                        infoBO.ModelYear = Convert.ToInt32(reader["ModelYear"]);
                                    }
                                    if ((reader["TaxValidationYear"]) != DBNull.Value)
                                    {
                                        infoBO.TaxValidationYear = Convert.ToInt32(reader["TaxValidationYear"]);
                                    }
                                    if ((reader["Fare"]) != DBNull.Value)
                                    {
                                        infoBO.Fare = Convert.ToDecimal(reader["Fare"]);
                                    }
                                    if ((reader["Mileage"]) != DBNull.Value)
                                    {
                                        infoBO.Mileage = Convert.ToDecimal(reader["Mileage"]);
                                    }
                                    if ((reader["FuelTankCapacity"]) != DBNull.Value)
                                    {
                                        infoBO.FuelTankCapacity = Convert.ToDecimal(reader["FuelTankCapacity"]);
                                    }
                                    if ((reader["EngineCapacity"]) != DBNull.Value)
                                    {
                                        infoBO.EngineCapacity = Convert.ToDecimal(reader["EngineCapacity"]);
                                    }
                                    if ((reader["PassengerCapacity"]) != DBNull.Value)
                                    {
                                        infoBO.PassengerCapacity = Convert.ToInt32(reader["PassengerCapacity"]);
                                    }
                                    infoBO.Status = Convert.ToBoolean(reader["Status"]);
                                    infoBO.IsABSEnable = Convert.ToBoolean(reader["IsABSEnable"]);
                                    infoBO.IsAirBagAvailable = Convert.ToBoolean(reader["IsAirBagAvailable"]);
                                    infoBO.VehicleTypeId = Convert.ToInt32(reader["VehicleTypeId"]);
                                    infoBO.NumberPlate = reader["NumberPlate"].ToString();
                                    infoBO.TaxNumber = reader["TaxNumber"].ToString();
                                    infoBO.InsuranceNumber = reader["InsuranceNumber"].ToString();
                                    infoBO.AirConditioningType = reader["AirConditioningType"].ToString();
                                    infoBO.FuelType = reader["FuelType"].ToString();
                                    infoBO.EngineType = reader["EngineType"].ToString();
                                    infoBO.BodyType = reader["BodyType"].ToString();
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
            return infoBO;
        }

        public List<VMVehicleInformationBO> GetVehicleInformationGridding(string vehicleName, string model, string airConditioning, int manufacturerId, int accountHeadId, int vehicleTypeId, string status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<VMVehicleInformationBO> infoBOs = new List<VMVehicleInformationBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVehicleInformationBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrWhiteSpace(vehicleName))
                            dbSmartAspects.AddInParameter(cmd, "@VehicleName", DbType.String, vehicleName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@VehicleName", DbType.String, DBNull.Value);

                        if (!string.IsNullOrWhiteSpace(model))
                            dbSmartAspects.AddInParameter(cmd, "@ModelName", DbType.String, model);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ModelName", DbType.String, DBNull.Value);


                        if (manufacturerId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, manufacturerId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, DBNull.Value);
                        }

                        if (vehicleTypeId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@VehicleTypeId", DbType.Int32, vehicleTypeId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@VehicleTypeId", DbType.Int32, DBNull.Value);
                        }

                        if (accountHeadId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@AccountHeadId", DbType.Int32, accountHeadId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@AccountHeadId", DbType.Int32, DBNull.Value);
                        }

                        if (airConditioning != "0")
                            dbSmartAspects.AddInParameter(cmd, "@AirConditioningType", DbType.String, airConditioning);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@AirConditioningType", DbType.String, DBNull.Value);

                        if (status != "0")
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, status);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    VMVehicleInformationBO bO = new VMVehicleInformationBO();
                                    bO.Id = Convert.ToInt64(reader["Id"]);
                                    bO.VehicleName = Convert.ToString(reader["VehicleName"]);
                                    bO.ModelName = Convert.ToString(reader["ModelName"]);
                                    bO.ManufacturerName = Convert.ToString(reader["ManufacturerName"]);
                                    bO.NumberPlate = Convert.ToString(reader["NumberPlate"]);

                                    infoBOs.Add(bO);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return infoBOs;
        }

        public List<VMVehicleInformationBO> GetAllVehicleInformation()
        {
            List<VMVehicleInformationBO> infoBOs = new List<VMVehicleInformationBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllVehicleInformation_SP"))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    VMVehicleInformationBO bO = new VMVehicleInformationBO();
                                    bO.Id = Convert.ToInt64(reader["Id"]);
                                    bO.VehicleName = Convert.ToString(reader["VehicleName"]);
                                    bO.ModelName = Convert.ToString(reader["ModelName"]);
                                    bO.NumberPlate = Convert.ToString(reader["NumberPlate"]);

                                    infoBOs.Add(bO);
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

            return infoBOs;
        }

    }
}
