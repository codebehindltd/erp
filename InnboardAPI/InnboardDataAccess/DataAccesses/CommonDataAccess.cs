using InnboardAPI.DataAccesses;
using InnboardDomain.Models;
using InnboardDomain.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDataAccess.DataAccesses
{
    public class CommonDataAccess : GenericDataAccess<CustomFieldBO>
    {
        public async Task<List<CustomFieldBO>> GetCustomField(string fieldType)
        {
            SqlParameter param1 = new SqlParameter("@FieldName", fieldType);
            var result = await InnboardDBContext.Database.SqlQuery<CustomFieldBO>("EXEC [dbo].[GetCustomField_SP] @FieldName", param1).ToListAsync();
            return result;
        }
    }
}
