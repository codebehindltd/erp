using HotelManagement.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.Inventory
{
    public class InvServiceFrequencyDA : BaseService
    {
        public Boolean SaveOrUpdateFrequency(InvServiceFrequency frequency, out int id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateFrequency_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, frequency.Id);
                        dbSmartAspects.AddInParameter(command, "@Frequency", DbType.String, frequency.Frequency);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);

                        id = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<InvServiceFrequency> GetFrequencyBySearchCriteria(string frequency, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<InvServiceFrequency> frequencies = new List<InvServiceFrequency>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFreequencyBySearchCriteria_SP"))
                    {
                        if (!string.IsNullOrWhiteSpace(frequency))
                            dbSmartAspects.AddInParameter(cmd, "@Frequency", DbType.Int32, frequency);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Frequency", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    InvServiceFrequency frequencyBO = new InvServiceFrequency();

                                    frequencyBO.Id = Convert.ToInt32(reader["Id"]);
                                    frequencyBO.Frequency = Convert.ToInt32(reader["Frequency"]);

                                    frequencies.Add(frequencyBO);
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
            return frequencies;
        }
    }
}
