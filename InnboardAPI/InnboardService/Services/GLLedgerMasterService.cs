using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class GLLedgerMasterService : GenericService<GLLedgerMaster>
    {
        public new Response<GLLedgerMaster> Save(GLLedgerMaster entity)
        {
            var repository = GetInstance<IGLLedgerMaster>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
    }
}
