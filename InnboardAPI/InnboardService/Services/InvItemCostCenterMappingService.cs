using InnboardDomain.Models;
using InnboardDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class InvItemCostCenterMappingService:GenericService<InvItemCostCenterMapping>
    {
        public new Response<List<InvItemCostCenterMapping>> TruncateAllAndInsert(List<InvItemCostCenterMapping> entity)
        {
            var repository = GetInstance<IInvItemCostCenterMapping>();
            var result = SafeExecute(() => repository.TruncateAllAndInsert(entity));
            return result;
        }
    }
}
