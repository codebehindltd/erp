using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLLedgerDA : BaseService
    {
        public List<GLLedgerBO> GetGLLedgerByLedgerId(int ledgerId)
        {
            List<GLLedgerBO> glLedgerBOList = new List<GLLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLLedgerByLedgerId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LedgerId", DbType.Int32, ledgerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLLedgerBO Detail = new GLLedgerBO();

                                Detail.LedgerId = Convert.ToInt32(reader["LedgerId"]);
                                Detail.GLMasterId = Convert.ToInt32(reader["GLMasterId"]);
                                Detail.NodeId = Convert.ToInt32(reader["NodeId"]);
                                Detail.NodeNumber = reader["NodeNumber"].ToString();
                                Detail.NodeHead = reader["NodeHead"].ToString();
                                Detail.LedgerMode = Convert.ToInt32(reader["LedgerMode"]);
                                Detail.BankAccountId = Convert.ToInt32(reader["BankAccountId"]);
                                Detail.ChequeNumber = reader["ChequeNumber"].ToString();
                                Detail.LedgerAmount = Convert.ToDecimal(reader["LedgerAmount"].ToString());
                                Detail.NodeNarration = reader["NodeNarration"].ToString();
                                Detail.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                Detail.CostCenter = reader["CostCenter"].ToString();

                                glLedgerBOList.Add(Detail);
                            }
                        }
                    }
                }
            }
            return glLedgerBOList;
        }

        public GLLedgerBO IsChequeNumberExistByNodeId(int NodeId, string ChequeNumber)
        {
            GLLedgerBO ledgerBO = new GLLedgerBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("IsChequeNumberExistByNodeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, NodeId);
                    dbSmartAspects.AddInParameter(cmd, "@ChequeNumber", DbType.String, ChequeNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                ledgerBO.LedgerId = Convert.ToInt32(reader["LedgerId"]);
                                //ledgerBO.GLMasterId = Convert.ToInt32(reader["GLMasterId"]);
                                ledgerBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                ledgerBO.NodeNumber = reader["NodeNumber"].ToString();
                                ledgerBO.NodeHead = reader["NodeHead"].ToString();
                                ledgerBO.LedgerMode = Convert.ToInt32(reader["LedgerMode"]);
                                ledgerBO.BankAccountId = Convert.ToInt32(reader["BankAccountId"]);
                                ledgerBO.ChequeNumber = reader["ChequeNumber"].ToString();
                                ledgerBO.LedgerAmount = Convert.ToDecimal(reader["LedgerAmount"].ToString());
                                ledgerBO.NodeNarration = reader["NodeNarration"].ToString();
                                ledgerBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                ledgerBO.CostCenter = reader["CostCenter"].ToString();


                            }
                        }
                    }
                }
            }
            return ledgerBO;
        }
    }
}
