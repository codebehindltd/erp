using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class DynamicReportDA : BaseService
    {
        public List<DynamicReportBO> GetFieldsFromTable(string segment)
        {
            List<DynamicReportBO> reportBOs = new List<DynamicReportBO>();
            try
            {
                //string query = string.Format("select * from {0} ", segment);
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                   
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFieldsFromTable_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Segment", DbType.String, segment);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    
                                    {
                                        DynamicReportBO bO = new DynamicReportBO();
                                        bO.ColumnName = reader["COLUMN_NAME"].ToString();
                                        bO.DataType = reader["DATA_TYPE"].ToString();
                                        
                                        reportBOs.Add(bO);
                                    }

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

            return reportBOs;
        }
        public List<DynamicReportBO> GetDataForReport( string sqlComm)
        {
            List<DynamicReportBO> reportBOs = new List<DynamicReportBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSampleForDynamicReport"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SQLComm", DbType.String, sqlComm);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    DynamicReportBO bO = new DynamicReportBO();
                                    bO.ColumnId = Convert.ToInt32(reader["ColumnId"]);
                                    bO.RowId = Convert.ToInt32(reader["RowId"]);
                                    bO.ColumnName = reader["ColumnName"].ToString();
                                    bO.ItemValue = reader["ItemValue"].ToString();

                                    reportBOs.Add(bO);
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
            return reportBOs;
        }

        public int MyProperty { get; set; }
    }
}
