using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class BenefitDA : BaseService
    {
        public Boolean SaveBenefitInfo(BenefitHeadBO bo, out long tmpBenefitId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBenefitInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@BenefitName", DbType.String, bo.BenefitName);
                        dbSmartAspects.AddOutParameter(command, "@BenefitId", DbType.Int64, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpBenefitId = Convert.ToInt32(command.Parameters["@BenefitId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateBenefitInfo(BenefitHeadBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBenefitInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@BenefitId", DbType.Int64, bo.BenefitHeadId);
                        dbSmartAspects.AddInParameter(command, "@BenefitName", DbType.String, bo.BenefitName);

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
        public BenefitHeadBO GetBenefitInfoById(long benefitId)
        {
            BenefitHeadBO bo = new BenefitHeadBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBenefitInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BenefitId", DbType.Int64, benefitId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BenefitInfo");
                    DataTable Table = ds.Tables["BenefitInfo"];
                    bo = Table.AsEnumerable().Select(r =>
                                new BenefitHeadBO
                                {
                                    BenefitHeadId = r.Field<long>("BenefitHeadId"),
                                    BenefitName = r.Field<string>("BenefitName")
                                }).FirstOrDefault();
                }
            }
            return bo;
        }
        public List<BenefitHeadBO> GetAllBenefit()
        {
            List<BenefitHeadBO> benefitList = new List<BenefitHeadBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBenefitInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Benefits");
                    DataTable Table = ds.Tables["Benefits"];
                    benefitList = Table.AsEnumerable().Select(r =>
                                new BenefitHeadBO
                                {
                                    BenefitHeadId = r.Field<long>("BenefitHeadId"),
                                    BenefitName = r.Field<string>("BenefitName")
                                }).ToList();
                }
            }
            return benefitList;
        }

        //public List<BenefitHeadBO> GetBenefitList()
        //{
        //    List<BenefitHeadBO> benList = new List<BenefitHeadBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllEmpBenefit_SP"))
        //        {
        //            DataSet ds = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, ds, "EmpBenefit");
        //            DataTable Table = ds.Tables["EmpBenefit"];
        //            benList = Table.AsEnumerable().Select(r =>
        //                        new BenefitHeadBO
        //                        {
        //                            BenefitHeadId = r.Field<long>("BenefitHeadId"),
        //                            BenefitName = r.Field<string>("BenefitName")
        //                        }).ToList();
        //        }
        //    }
        //    return benList;
        //}
        public List<PayrollEmpBenefitBO> GetEmpBenefitListByEmpId(int empId)
        {
            List<PayrollEmpBenefitBO> benList = new List<PayrollEmpBenefitBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpBenefitListByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpBenefit");
                    DataTable Table = ds.Tables["EmpBenefit"];
                    benList = Table.AsEnumerable().Select(r =>
                                new PayrollEmpBenefitBO
                                {
                                    EmpBenefitMappingId = r.Field<long>("EmpBenefitMappingId"),
                                    BenefitHeadId = r.Field<long>("BenefitHeadId"),
                                    EmpId = r.Field<Int32>("EmpId"),
                                    BenefitName = r.Field<string>("BenefitName"),
                                    ShowEffectiveDate = r.Field<string>("ShowEffectiveDate")
                                }).ToList();
                }
            }
            return benList;
        }
        public List<PayrollEmpBenefitBO> GetEmpEffectiveBenefitInfo(int empId)
        {
            List<PayrollEmpBenefitBO> benList = new List<PayrollEmpBenefitBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpEffectiveBenefitInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpBenefit");
                    DataTable Table = ds.Tables["EmpBenefit"];
                    benList = Table.AsEnumerable().Select(r =>
                                new PayrollEmpBenefitBO
                                {
                                    EmpBenefitMappingId = r.Field<long>("EmpBenefitMappingId"),
                                    BenefitHeadId = r.Field<long>("BenefitHeadId"),
                                    EmpId = r.Field<Int32>("EmpId"),
                                    BenefitName = r.Field<string>("BenefitName"),
                                    //ShowEffectiveDate = r.Field<string>("ShowEffectiveDate"),
                                    EmpName = r.Field<string>("EmpName")
                                }).ToList();
                }
            }
            return benList;
        }
        public List<PayrollEmpBenefitBO> GetEmpBenefitForReport(int? empId, int? departmentId, DateTime? effectiveDate)
        {
            List<PayrollEmpBenefitBO> benList = new List<PayrollEmpBenefitBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpBenefitForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    dbSmartAspects.AddInParameter(cmd, "@EffectiveDate", DbType.DateTime, effectiveDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpBenefit");
                    DataTable Table = ds.Tables["EmpBenefit"];
                    benList = Table.AsEnumerable().Select(r =>
                                new PayrollEmpBenefitBO
                                {
                                    EmpBenefitMappingId = r.Field<long>("EmpBenefitMappingId"),
                                    BenefitHeadId = r.Field<long>("BenefitHeadId"),
                                    EmpId = r.Field<Int32>("EmpId"),
                                    BenefitName = r.Field<string>("BenefitName"),
                                    ShowEffectiveDate = r.Field<string>("ShowEffectiveDate"),
                                    EmpName = r.Field<string>("EmpName"),
                                    Designation = r.Field<string>("Designation")
                                }).ToList();
                }
            }
            return benList;
        }
    }
}
