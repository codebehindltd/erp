using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;
using HotelManagement.Data.HMCommon;
using System.Collections;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
    public class PaymentModeDA : BaseService
    {
        public PaymentModeBO GetPaymentModeInfoById(int paymentModeId)
        {
            PaymentModeBO nodeMatrixBO = new PaymentModeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaymentModeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentModeId", DbType.Int32, paymentModeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                nodeMatrixBO.PaymentModeId = Convert.ToInt32(reader["PaymentModeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.AncestorHead = reader["AncestorHead"].ToString();
                                nodeMatrixBO.PaymentMode = reader["PaymentMode"].ToString();
                                nodeMatrixBO.DisplayName = reader["DisplayName"].ToString();
                                nodeMatrixBO.PaymentCode = reader["PaymentCode"].ToString();
                                //nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return nodeMatrixBO;
        }
        public List<PaymentModeBO> GetPaymentModeInfoByAncestorId(int ancestorId)
        {
            List<PaymentModeBO> nodeMatrixBOList = new List<PaymentModeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaymentModeInfoByAncestorId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AncestorId", DbType.Int32, ancestorId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PaymentModeBO nodeMatrixBO = new PaymentModeBO();
                                nodeMatrixBO.PaymentModeId = Convert.ToInt32(reader["PaymentModeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.AncestorHead = reader["AncestorHead"].ToString();
                                nodeMatrixBO.PaymentMode = reader["PaymentMode"].ToString();
                                nodeMatrixBO.DisplayName = reader["DisplayName"].ToString();
                                nodeMatrixBO.PaymentCode = reader["PaymentCode"].ToString();
                                //nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<PaymentModeBO> GetPaymentModeInfoByCustomString(string customString)
        {
            List<PaymentModeBO> nodeMatrixBOList = new List<PaymentModeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaymentModeInfoByCustomString_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomString", DbType.String, customString);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PaymentModeBO nodeMatrixBO = new PaymentModeBO();
                                nodeMatrixBO.PaymentModeId = Convert.ToInt32(reader["PaymentModeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.AncestorHead = reader["AncestorHead"].ToString();
                                nodeMatrixBO.PaymentMode = reader["PaymentMode"].ToString();
                                nodeMatrixBO.DisplayName = reader["DisplayName"].ToString();
                                nodeMatrixBO.PaymentCode = reader["PaymentCode"].ToString();
                                //nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
    }
}
