using InnboardAPI.DataAccesses;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using InnboardDomain.ViewModel;
using InnboardDomain.Utility;
using InnboardDomain.CriteriaDtoModel;
using InnboardDomain.Models.Payroll;
using InnboardDomain.Models.Membership;

namespace InnboardDataAccess.DataAccesses
{
    public class MemberDataAccess : GenericDataAccess<GetMembershipSetupDataBO>
    {
        public async Task<List<GetMembershipSetupDataBO>> GetMembershipSetupData()
        {
            var result = await InnboardDBContext.Database.SqlQuery<GetMembershipSetupDataBO>("EXEC [dbo].[GetMembershipSetupData_SP]").ToListAsync();
            return result;
        }
    }
}
