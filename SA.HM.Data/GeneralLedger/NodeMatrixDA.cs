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

namespace HotelManagement.Data.GeneralLedger
{
    public class NodeMatrixDA : BaseService
    {
        public List<NodeMatrixBO> GetNodeMatrixInfo()
        {
            List<NodeMatrixBO> nodeMatrixList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.NodeType = Convert.ToString(reader["NodeType"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                nodeMatrixBO.IsTransactionalHead = Convert.ToBoolean(reader["IsTransactionalHead"]);
                                nodeMatrixList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixList;
        }
        public List<NodeMatrixBO> GetActiveNodeMatrixInfo()
        {
            List<NodeMatrixBO> nodeMatrixList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveNodeMatrixInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.NodeType = Convert.ToString(reader["NodeType"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                nodeMatrixBO.IsTransactionalHead = Convert.ToBoolean(reader["IsTransactionalHead"]);
                                nodeMatrixList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixList;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoForVoucherEntry()
        {
            List<NodeMatrixBO> nodeMatrixList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoForVoucherEntry_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.NodeType = Convert.ToString(reader["NodeType"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                nodeMatrixList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixList;
        }
        public Boolean SaveNodeMatrixInfo(NodeMatrixBO nodeMatrixBO, out int NodeID, List<GLAccountTypeSetupBO> entityBOList)
        {
            bool retVal = false;
            int status = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveNodeMatrixInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, nodeMatrixBO.AncestorId);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeNumber", DbType.String, nodeMatrixBO.NodeNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeHead", DbType.String, nodeMatrixBO.NodeHead);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeMode", DbType.Boolean, nodeMatrixBO.NodeMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@CFHeadId", DbType.Int32, nodeMatrixBO.CFHeadId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PLHeadId", DbType.Int32, nodeMatrixBO.PLHeadId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BSHeadId", DbType.Int32, nodeMatrixBO.BSHeadId);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, nodeMatrixBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@NodeId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));


                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        //int ERR = Convert.ToInt32(commandMaster.Parameters["@Err"].Value);
                        NodeID = Convert.ToInt32(commandMaster.Parameters["@NodeID"].Value);


                        if (status > 0)
                        {
                            int countAccountType = 0;

                            if (entityBOList.Count > 0)
                            {
                                using (DbCommand commandAccountType = dbSmartAspects.GetStoredProcCommand("SaveGLAccountTypeSetup_SP"))
                                {
                                    foreach (GLAccountTypeSetupBO bo in entityBOList)
                                    {
                                        commandAccountType.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandAccountType, "@NodeID", DbType.Int32, NodeID);
                                        dbSmartAspects.AddInParameter(commandAccountType, "@AccountType", DbType.String, bo.AccountType);
                                        countAccountType += dbSmartAspects.ExecuteNonQuery(commandAccountType, transction);
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
                }
            }
            return retVal;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoByAccountHead(string accountHead)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByAccountHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<NodeMatrixSearchBO> GetNodeMatrixInfoByAccountHead(string accountHead, int isVoucherForm)
        {
            List<NodeMatrixSearchBO> result = new List<NodeMatrixSearchBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByAccountHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);
                    dbSmartAspects.AddInParameter(cmd, "@IsVoucherForm", DbType.Int32, isVoucherForm);

                    DataSet VoucherDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, VoucherDS, "Voucher");
                    DataTable Table = VoucherDS.Tables["Voucher"];

                    result = Table.AsEnumerable().Select(r => new NodeMatrixSearchBO
                    {
                        NodeId = r.Field<int>("NodeId"),
                        NodeHead = r.Field<string>("GoogleSearch"),
                        NodeType = r.Field<string>("NodeType")

                    }).ToList();

                }
            }
            return result;
        }
        public List<string> GetNodeMatrixInfoByAccountHead1(string accountHead, int isVoucherForm)
        {
            List<string> result = new List<string>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByAccountHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);
                    dbSmartAspects.AddInParameter(cmd, "@IsVoucherForm", DbType.Int32, isVoucherForm);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                //result.Add(reader["NodeHead"].ToString());
                                result.Add(reader["GoogleSearch"].ToString());
                            }
                        }
                    }
                }
            }
            return result;
        }
        public string GetNodeMatrixInfoByAccountHead2(string accountHead)
        {
            string result = "0";
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoBySpecificAccountHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                result = reader["NodeId"].ToString();
                            }
                        }
                    }
                }
            }
            return result;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoByAccountHeadTest(string accountHead, string groupRIndividualSearch)
        {
            List<NodeMatrixBO> result = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetChartOfAccountsInfoByAccountHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);
                    dbSmartAspects.AddInParameter(cmd, "@GroupRIndividualSearch", DbType.String, groupRIndividualSearch);

                    DataSet VoucherDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, VoucherDS, "Voucher");
                    DataTable Table = VoucherDS.Tables["Voucher"];

                    result = Table.AsEnumerable().Select(r => new NodeMatrixBO
                    {
                        NodeId = r.Field<Int64>("NodeId"),
                        NodeHead = r.Field<string>("GoogleSearch"),
                        Lvl = r.Field<int>("Lvl"),
                        IsTransactionalHead = r.Field<bool?>("IsTransactionalHead")

                    }).ToList();

                    //ArrayList oo = new ArrayList();
                    //oo.Add(result);

                }
            }
            return result;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoByAccountNameLikeSearch(string accountHead)
        {
            List<NodeMatrixBO> result = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByAccountNameLikeSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);

                    DataSet VoucherDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, VoucherDS, "Voucher");
                    DataTable Table = VoucherDS.Tables["Voucher"];

                    result = Table.AsEnumerable().Select(r => new NodeMatrixBO
                    {
                        NodeId = r.Field<Int64>("NodeId"),
                        NodeHead = r.Field<string>("GoogleSearch"),
                        Lvl = r.Field<int>("Lvl"),
                        IsTransactionalHead = r.Field<bool?>("IsTransactionalHead")

                    }).ToList();
                }
            }
            return result;
        }
        //public List<NodeMatrixBO> GetNodeMatrixInfoByAccountNameLikeSearch(string accountHead)
        //{
        //    List<NodeMatrixBO> result = new List<NodeMatrixBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByAccountNameLikeSearch_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);

        //            DataSet VoucherDS = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, VoucherDS, "Voucher");
        //            DataTable Table = VoucherDS.Tables["Voucher"];

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

        //                        nodeMatrixBO.NodeId = Convert.ToInt32(reader["NodeId"]);
        //                        nodeMatrixBO.NodeHead = reader["GoogleSearch"].ToString();
        //                        nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
        //                        nodeMatrixBO.IsTransactionalHead = Convert.ToBoolean(reader["IsTransactionalHead"]);

        //                        result.Add(nodeMatrixBO);

        //                    }
        //                }
        //            }

        //            //result = Table.AsEnumerable().Select(r => new NodeMatrixBO
        //            //{
        //            //    NodeId = r.Field<int>("NodeId"),
        //            //    NodeHead = r.Field<string>("GoogleSearch"),
        //            //    Lvl = r.Field<int>("Lvl"),
        //            //    IsTransactionalHead = r.Field<bool?>("IsTransactionalHead")

        //            //}).ToList();
        //        }
        //    }
        //    return result;
        //}
        public List<NodeMatrixBO> GetNodeMatrixInfoByNameAndTransactionFlag(string accountHead, bool isTransactionalHead)
        {
            List<NodeMatrixBO> result = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByNameAndTransactionFlag_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);
                    dbSmartAspects.AddInParameter(cmd, "@IsTransactionalHead", DbType.Boolean, isTransactionalHead);

                    DataSet VoucherDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, VoucherDS, "Voucher");
                    DataTable Table = VoucherDS.Tables["Voucher"];

                    result = Table.AsEnumerable().Select(r => new NodeMatrixBO
                    {
                        NodeId = r.Field<Int64>("NodeId"),
                        NodeHead = r.Field<string>("GoogleSearch"),
                        Lvl = r.Field<Int32>("Lvl"),
                        IsTransactionalHead = r.Field<bool?>("IsTransactionalHead"),
                        Hierarchy = r.Field<string>("Hierarchy")

                    }).ToList();
                }
            }
            return result;
        }
        public List<string> GetNodeMatrixInfoByNodeIdNSearchNodeHead(int nodeId, string accountHead)
        {
            List<string> result = new List<string>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByNodeIdNSearchNodeHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, nodeId);
                    dbSmartAspects.AddInParameter(cmd, "@SearchNodeHead", DbType.String, accountHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                result.Add(reader["NodeHead"].ToString());
                            }
                        }
                    }
                }
            }
            return result;
        }
        public NodeMatrixBO GetNodeMatrixInfoById(Int64 nodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int64, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.AncestorHead = reader["AncestorHead"].ToString();
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.NotesNumber = reader["NotesNumber"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                if (reader["IsTransactionalHead"].ToString() != string.Empty)
                                    nodeMatrixBO.IsTransactionalHead = Convert.ToBoolean(reader["IsTransactionalHead"]);
                                else
                                    nodeMatrixBO.IsTransactionalHead = false;
                            }
                        }
                    }
                }
            }
            return nodeMatrixBO;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoByAncestorNodeId(Int64 nodeId)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByAncestorNodeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int64, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
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
                                nodeMatrixBO.IsTransactionalHead = Convert.ToBoolean(reader["IsTransactionalHead"]);
                                nodeMatrixBO.NodeType = reader["NodeType"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoByAncestorNodeIdList(String nodeIdList)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByAncestorNodeIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeIdList", DbType.String, nodeIdList);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
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
                                nodeMatrixBO.IsTransactionalHead = Convert.ToBoolean(reader["IsTransactionalHead"]);

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoByCustomString(string customString)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByCustomString_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomString", DbType.String, customString);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                nodeMatrixBO.NotesNumber = reader["NotesNumber"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<NodeMatrixBO> GetNodeMatrixInfoByNodeIdForGroupData(Int64 nodeId)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByNodeIdForGroupData_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int64, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                                nodeMatrixBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
                                nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<string> GetNodeMatrixInfoByProjectIdNCashOrBankOrSearchText(int projectId, string cashOrBank, string accountHead)
        {
            //List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByProjectIdNCashOrBankOrSearchText_SP"))
            //    {
            //        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
            //        dbSmartAspects.AddInParameter(cmd, "@CashOrBank", DbType.String, cashOrBank);
            //        dbSmartAspects.AddInParameter(cmd, "@SearchNodeHead", DbType.String, accountHead);

            //        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
            //        {
            //            if (reader != null)
            //            {
            //                while (reader.Read())
            //                {
            //                    NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

            //                    nodeMatrixBO.NodeId = Convert.ToInt32(reader["NodeId"]);
            //                    nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
            //                    nodeMatrixBO.NodeNumber = reader["NodeNumber"].ToString();
            //                    nodeMatrixBO.NodeHead = reader["NodeHead"].ToString();
            //                    nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
            //                    nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
            //                    nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
            //                    nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
            //                    nodeMatrixBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
            //                    nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

            //                    nodeMatrixBOList.Add(nodeMatrixBO);
            //                }
            //            }
            //        }
            //    }
            //}
            //return nodeMatrixBOList;


            List<string> result = new List<string>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNodeMatrixInfoByProjectIdNCashOrBankOrSearchText_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@CashOrBank", DbType.String, cashOrBank);
                    dbSmartAspects.AddInParameter(cmd, "@SearchNodeHead", DbType.String, accountHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                result.Add(reader["GoogleSearch"].ToString());
                            }
                        }
                    }
                }
            }
            return result;
        }
        public List<GLAccountTypeSetupBO> GetAccountTypeInfoByNodeId(int NodeId)
        {
            List<GLAccountTypeSetupBO> List = new List<GLAccountTypeSetupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountTypeInfoByNodeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, NodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLAccountTypeSetupBO setupBO = new GLAccountTypeSetupBO();

                                setupBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                setupBO.AccountType = reader["AccountType"].ToString();
                                List.Add(setupBO);
                            }
                        }
                    }
                }
            }

            return List;
        }
        public bool UpdateNodeMatrixInfo(NodeMatrixBO nodeMatrixBO, out int NodeId, List<GLAccountTypeSetupBO> entityBOList)
        {
            bool retVal = false;
            NodeId = 0;
            int status = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateNodeMatrixInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeId", DbType.Int64, nodeMatrixBO.NodeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, nodeMatrixBO.AncestorId);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeNumber", DbType.String, nodeMatrixBO.NodeNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeHead", DbType.String, nodeMatrixBO.NodeHead);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeMode", DbType.Boolean, nodeMatrixBO.NodeMode);

                        dbSmartAspects.AddInParameter(commandMaster, "@CFSetupId", DbType.Int32, nodeMatrixBO.CFSetupId);
                        dbSmartAspects.AddInParameter(commandMaster, "@CFHeadId", DbType.Int32, nodeMatrixBO.CFHeadId);

                        dbSmartAspects.AddInParameter(commandMaster, "@PLSetupId", DbType.Int32, nodeMatrixBO.PLSetupId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PLHeadId", DbType.Int32, nodeMatrixBO.PLHeadId);

                        dbSmartAspects.AddInParameter(commandMaster, "@BSSetupId", DbType.Int32, nodeMatrixBO.BSSetupId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BSHeadId", DbType.Int32, nodeMatrixBO.BSHeadId);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, nodeMatrixBO.LastModifiedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        if (status > 0)
                        {
                            int countAccountType = 0;
                            // Delete Block-------------------------
                            using (DbCommand deleteAccountType = dbSmartAspects.GetStoredProcCommand("DeleteGLAccountTypeSetupByNodeId_SP"))
                            {
                                dbSmartAspects.AddInParameter(deleteAccountType, "@NodeID", DbType.Int64, nodeMatrixBO.NodeId);
                                dbSmartAspects.ExecuteNonQuery(deleteAccountType, transction);
                            }

                            if (entityBOList.Count > 0)
                            {
                                // Save Block-------------------------
                                using (DbCommand commandAccountType = dbSmartAspects.GetStoredProcCommand("SaveGLAccountTypeSetup_SP"))
                                {
                                    foreach (GLAccountTypeSetupBO bo in entityBOList)
                                    {
                                        commandAccountType.Parameters.Clear();
                                        //dbSmartAspects.AddInParameter(commandAccountType, "@AccountTypeId", DbType.Int32, nodeMatrixBO.AccountTypeId);
                                        dbSmartAspects.AddInParameter(commandAccountType, "@NodeID", DbType.Int64, nodeMatrixBO.NodeId);
                                        dbSmartAspects.AddInParameter(commandAccountType, "@AccountType", DbType.String, bo.AccountType);
                                        countAccountType += dbSmartAspects.ExecuteNonQuery(commandAccountType, transction);
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
                }
            }
            return retVal;
        }
        public List<GLDealMasterBO> GetVoucherInfoBySearchCriteria(DateTime FormDate, DateTime ToDate, string VoucherNo, int UserInfoId)
        {
            string Where = GenarateWhereConditionstring(FormDate, ToDate, VoucherNo, UserInfoId);
            List<GLDealMasterBO> List = new List<GLDealMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    dbSmartAspects.AddInParameter(cmd, "@LoginUserId", DbType.Int32, UserInfoId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLDealMasterBO dealMasterBO = new GLDealMasterBO();
                                dealMasterBO.DealId = Convert.ToInt32(reader["DealId"]);
                                dealMasterBO.VoucherMode = Convert.ToInt32(reader["VoucherMode"]);
                                dealMasterBO.VoucherNo = reader["VoucherNo"].ToString();
                                dealMasterBO.VoucherDate = Convert.ToDateTime(reader["VoucherDate"].ToString());
                                dealMasterBO.Narration = reader["Narration"].ToString();
                                dealMasterBO.VoucherType = reader["VoucherType"].ToString();
                                dealMasterBO.GLStatus = reader["GLStatus"].ToString();

                                dealMasterBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"]);
                                dealMasterBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);
                                dealMasterBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);

                                List.Add(dealMasterBO);
                            }
                        }
                    }
                }
            }

            return List;
        }
        public string GenarateWhereConditionstring(DateTime FormDate, DateTime ToDate, string VoucherNo, int UserInfoId)
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
        public bool DeleteVoucherInfoById(int DealId)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("DeleteVoucherInfoByDealId_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@DealId", DbType.Int32, DealId);
                        dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        if (status > 0)
                        {
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
        public bool UpdateGLDealMasterStatus(int DealId, string GLStatus, int UserInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLDealMasterStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, DealId);
                    dbSmartAspects.AddInParameter(command, "@GLStatus", DbType.String, GLStatus);
                    dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, UserInfoId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public GLDealMasterBO GetDealMasterInfoByDealId(int DealId)
        {
            GLDealMasterBO masterBO = new GLDealMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealMasterInfoByDealId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int32, DealId);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                masterBO.DealId = Convert.ToInt32(reader["DealId"]);
                                if (!string.IsNullOrEmpty(reader["GLStatus"].ToString()))
                                {
                                    masterBO.GLStatus = reader["GLStatus"].ToString();
                                }
                                //else
                                //{
                                //    masterBO.GLStatus = 0;
                                //}
                            }
                        }
                    }
                }
            }
            return masterBO;
        }
        public Boolean SaveNodeMatrixInfoFromOtherPage(NodeMatrixBO nodeMatrixBO, out int NodeID)
        {
            bool retVal = false;
            int status = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveNodeMatrixInfoForOtherPage_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, nodeMatrixBO.AncestorId);
                        //dbSmartAspects.AddInParameter(commandMaster, "@NodeNumber", DbType.String, nodeMatrixBO.NodeNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeHead", DbType.String, nodeMatrixBO.NodeHead);
                        dbSmartAspects.AddInParameter(commandMaster, "@NodeMode", DbType.Boolean, nodeMatrixBO.NodeMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, nodeMatrixBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@NodeId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));


                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        //int ERR = Convert.ToInt32(commandMaster.Parameters["@Err"].Value);
                        NodeID = Convert.ToInt32(commandMaster.Parameters["@NodeID"].Value);


                        if (status > 0)
                        {
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
        public Boolean UpdateNodeMatrixInfoFromOtherPage(NodeMatrixBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateNodeMatrixInfoFromOtherPage_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, entityBO.NodeId);
                    //dbSmartAspects.AddInParameter(command, "@NodeNumber", DbType.String, entityBO.NodeNumber);
                    dbSmartAspects.AddInParameter(command, "@NodeHead", DbType.String, entityBO.NodeHead);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<GLConfigurableBalanceSheetBO> GetGLReportDynamicallyForReport(DateTime fromDate, DateTime toDate, int projectId, string urlAddress, string reportType)
        {
            List<GLConfigurableBalanceSheetBO> masterBOList = new List<GLConfigurableBalanceSheetBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLReportDynamicallyForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    //dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    //dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, companyAddress);
                    //dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, companyWeb);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, urlAddress);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLConfigurableBalanceSheetBO masterBO = new GLConfigurableBalanceSheetBO();
                                masterBO.RCId = Convert.ToInt32(reader["RCId"]);
                                //masterBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                //masterBO.IsAccountHead = Convert.ToBoolean(reader["IsAccountHead"]);
                                //masterBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                masterBO.NodeNumber = reader["NodeNumber"].ToString();
                                masterBO.NodeHead = reader["NodeHead"].ToString();
                                masterBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                masterBO.GroupName = reader["GroupName"].ToString();
                                masterBO.ReportType = reader["ReportType"].ToString();
                                masterBO.AccountType = reader["AccountType"].ToString();
                                masterBO.CalculatedNodeAmount = Convert.ToDecimal(reader["CalculatedNodeAmount"]);
                                masterBO.CalculationType = reader["CalculationType"].ToString();
                                masterBO.IsActiveLinkUrl = Convert.ToBoolean(reader["IsActiveLinkUrl"]);
                                masterBO.Url = reader["Url"].ToString();

                                masterBOList.Add(masterBO);
                            }
                        }
                    }
                }
            }
            return masterBOList;
        }
        public List<NodeMatrixBO> GetGetChartOfAccounts(string companyName, string companyAddress, string @CompanyWeb)
        {
            List<NodeMatrixBO> chartOfAccountsList = new List<NodeMatrixBO>();

            DataSet chartOfAccountsDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetChartOfAccounts"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, companyAddress);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, CompanyWeb);

                    dbSmartAspects.LoadDataSet(cmd, chartOfAccountsDS, "ChartOfAccounts");
                    DataTable table = chartOfAccountsDS.Tables["ChartOfAccounts"];

                    chartOfAccountsList = table.AsEnumerable().Select(r => new NodeMatrixBO
                    {
                        HMCompanyProfile = r.Field<string>("HMCompanyProfile"),
                        HMCompanyAddress = r.Field<string>("HMCompanyAddress"),
                        HMCompanyWeb = r.Field<string>("HMCompanyWeb"),
                        NodeId = r.Field<Int64>("NodeId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHeadDisplay = r.Field<string>("NodeHeadDisplay"),
                        Nature = r.Field<string>("Nature"),
                        CashFlowNotes = r.Field<string>("CashFlowNotes"),
                        ProfitNLossNotes = r.Field<string>("ProfitNLossNotes"),
                        Lvl = r.Field<int>("Lvl"),
                        NodeMode = r.Field<Boolean>("NodeMode")

                    }).ToList();

                    chartOfAccountsDS.Dispose();
                }
            }

            return chartOfAccountsList;
        }

        public List<NodeMatrixBO> GetGetChartOfAccountsByHierarchy(Int64 nodeId, string hierarchy)
        {
            List<NodeMatrixBO> chartOfAccountsList = new List<NodeMatrixBO>();

            DataSet chartOfAccountsDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetChartOfAccountsByHierarchy_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int64, nodeId);
                    dbSmartAspects.AddInParameter(cmd, "@Hierarchy", DbType.String, hierarchy);

                    dbSmartAspects.LoadDataSet(cmd, chartOfAccountsDS, "ChartOfAccounts");
                    DataTable table = chartOfAccountsDS.Tables["ChartOfAccounts"];

                    chartOfAccountsList = table.AsEnumerable().Select(r => new NodeMatrixBO
                    {
                        NodeId = r.Field<Int64>("NodeId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHeadDisplay = r.Field<string>("NodeHead"),
                        NodeHead = r.Field<string>("GoogleSearch"),
                        Lvl = r.Field<int>("Lvl"),
                        IsTransactionalHead = r.Field<bool?>("IsTransactionalHead"),
                        Hierarchy = r.Field<string>("Hierarchy")

                    }).ToList();

                    chartOfAccountsDS.Dispose();
                }
            }

            return chartOfAccountsList;
        }

        public bool AlreadyHaveOpening(string queryString)
        {
            //string queryAssets = $@"DECLARE @FiscalYearId INT = 0
            //                        SELECT @FiscalYearId = FiscalYearId FROM GLFiscalYear WHERE ('{openingDate}' BETWEEN FromDate AND ToDate)
            //                        SELECT Id FROM GLOpeningBalance WHERE FiscalYearId = @FiscalYearId AND IsApproved = 1";

            int fiscalYearId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(queryString))
                {
                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "assests");
                    DataTable Table = CompanyDS.Tables["assests"];

                    if (Table.Rows.Count > 0)
                        fiscalYearId = Convert.ToInt32(Table.Rows[0][0]);
                }
            }

            return fiscalYearId > 0;
        }

        public List<OpeningBalanceAccountList> GetAccountsForOpeningBalance(long retainedEaringsId)
        {
            List<OpeningBalanceAccountList> assestsAndLiabilities = new List<OpeningBalanceAccountList>();
            List<OpeningBalanceAccounts> assests = new List<OpeningBalanceAccounts>();
            List<OpeningBalanceAccounts> liabilities = new List<OpeningBalanceAccounts>();

            string queryAssets = @"SELECT NodeId, NodeType, NodeHead
                                    FROM GLNodeMatrix
                                    WHERE IsTransactionalHead = 1 AND NodeType = 'Assets'";

            string queryLiabilities = $@"SELECT NodeId, NodeType, NodeHead
                                        FROM GLNodeMatrix
                                        WHERE IsTransactionalHead = 1 AND NodeType = 'Liabilities' AND NodeId != {retainedEaringsId}";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(queryAssets))
                {
                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "assests");
                    DataTable Table = CompanyDS.Tables["assests"];

                    assests = Table.AsEnumerable().Select(r => new OpeningBalanceAccounts
                    {
                        NodeId = r.Field<Int64>("NodeId"),
                        NodeType = r.Field<string>("NodeType"),
                        NodeHead = r.Field<string>("NodeHead")
                    }).ToList();
                }

                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(queryLiabilities))
                {
                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "liabilities");
                    DataTable Table = CompanyDS.Tables["liabilities"];

                    liabilities = Table.AsEnumerable().Select(r => new OpeningBalanceAccounts
                    {
                        NodeId = r.Field<Int64>("NodeId"),
                        NodeType = r.Field<string>("NodeType"),
                        NodeHead = r.Field<string>("NodeHead")
                    }).ToList();
                }
            }

            int assetNodesCount = 0, liabilitiesCount = 0, nodeCount = 0;

            assetNodesCount = assests.Count;
            liabilitiesCount = liabilities.Count;

            OpeningBalanceAccountList openingAccount = new OpeningBalanceAccountList();

            if (assetNodesCount > liabilitiesCount)
            {
                foreach (OpeningBalanceAccounts aset in assests)
                {
                    openingAccount = new OpeningBalanceAccountList()
                    {
                        AssetNodeId = aset.NodeId,
                        AssetNodeType = aset.NodeType,
                        AssetNodeHead = aset.NodeHead
                    };

                    assestsAndLiabilities.Add(openingAccount);
                }

                for (nodeCount = 0; nodeCount < liabilitiesCount; nodeCount++)
                {
                    assestsAndLiabilities[nodeCount].LiabilitiesNodeId = liabilities[nodeCount].NodeId;
                    assestsAndLiabilities[nodeCount].LiabilitiesNodeType = liabilities[nodeCount].NodeType;
                    assestsAndLiabilities[nodeCount].LiabilitiesNodeHead = liabilities[nodeCount].NodeHead;
                }
            }
            else if (liabilitiesCount > assetNodesCount)
            {
                foreach (OpeningBalanceAccounts liable in liabilities)
                {
                    openingAccount = new OpeningBalanceAccountList()
                    {
                        LiabilitiesNodeId = liable.NodeId,
                        LiabilitiesNodeType = liable.NodeType,
                        LiabilitiesNodeHead = liable.NodeHead
                    };

                    assestsAndLiabilities.Add(openingAccount);
                }

                for (nodeCount = 0; nodeCount < assetNodesCount; nodeCount++)
                {
                    assestsAndLiabilities[nodeCount].AssetNodeId = assests[nodeCount].NodeId;
                    assestsAndLiabilities[nodeCount].AssetNodeType = assests[nodeCount].NodeType;
                    assestsAndLiabilities[nodeCount].AssetNodeHead = assests[nodeCount].NodeHead;
                }
            }

            else if (liabilitiesCount == assetNodesCount)
            {
                foreach (OpeningBalanceAccounts liable in liabilities)
                {
                    openingAccount = new OpeningBalanceAccountList()
                    {
                        LiabilitiesNodeId = liable.NodeId,
                        LiabilitiesNodeType = liable.NodeType,
                        LiabilitiesNodeHead = liable.NodeHead
                    };

                    assestsAndLiabilities.Add(openingAccount);
                }

                for (nodeCount = 0; nodeCount < assetNodesCount; nodeCount++)
                {
                    assestsAndLiabilities[nodeCount].AssetNodeId = assests[nodeCount].NodeId;
                    assestsAndLiabilities[nodeCount].AssetNodeType = assests[nodeCount].NodeType;
                    assestsAndLiabilities[nodeCount].AssetNodeHead = assests[nodeCount].NodeHead;
                }
            }

            return assestsAndLiabilities;
        }
    }
}
