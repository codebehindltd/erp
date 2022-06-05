using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
   public class GLFixedAssetsDA:BaseService
    {

        public GLFixedAssetsBO GetGLFixedAssetsByNodeId(int NodeId)
        {
            GLFixedAssetsBO assetsBO = new GLFixedAssetsBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLFixedAssetsByNodeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, NodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                assetsBO.FixedAssetsId = Convert.ToInt32(reader["FixedAssetsId"]);
                                assetsBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                assetsBO.BlockB =Convert.ToDecimal( reader["BlockB"].ToString());
                                assetsBO.BlockC =Convert.ToDecimal( reader["BlockC"].ToString());
                                assetsBO.BlockD =Convert.ToDecimal( reader["BlockD"].ToString());
                                assetsBO.BlockE =Convert.ToDecimal( reader["BlockE"].ToString());
                                assetsBO.BlockF =Convert.ToDecimal( reader["BlockF"].ToString());
                                assetsBO.BlockG =Convert.ToDecimal( reader["BlockG"].ToString());
                                assetsBO.BlockH =Convert.ToDecimal( reader["BlockH"].ToString());
                                assetsBO.BlockI =Convert.ToDecimal( reader["BlockI"].ToString());
                            }
                        }
                    }
                }
            }
            return assetsBO;
        }



        public bool SaveGLFixedAssetsInfo(GLFixedAssetsBO fixedAssetBO, out int tmpFixedAssetsId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLFixedAssetsInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, fixedAssetBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@BlockB", DbType.Decimal, fixedAssetBO.BlockB);
                    dbSmartAspects.AddInParameter(command, "@BlockE", DbType.Decimal, fixedAssetBO.BlockE);
                    dbSmartAspects.AddInParameter(command, "@BlockF", DbType.Decimal, fixedAssetBO.BlockF);
                    dbSmartAspects.AddOutParameter(command, "@FixedAssetsId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpFixedAssetsId = Convert.ToInt32(command.Parameters["@FixedAssetsId"].Value);
                }
            }
            return status;
        }

        public bool UpdateGLFixedAssetsInfo(GLFixedAssetsBO fixedAssetBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLFixedAssetsInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FixedAssetsId", DbType.Int32, fixedAssetBO.FixedAssetsId);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, fixedAssetBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@BlockB", DbType.Decimal, fixedAssetBO.BlockB);
                    dbSmartAspects.AddInParameter(command, "@BlockE", DbType.Decimal, fixedAssetBO.BlockE);
                    dbSmartAspects.AddInParameter(command, "@BlockF", DbType.Decimal, fixedAssetBO.BlockF);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
