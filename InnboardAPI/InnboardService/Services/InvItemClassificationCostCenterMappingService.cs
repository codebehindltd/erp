using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class InvItemClassificationCostCenterMappingService:GenericService<InvItemClassificationCostCenterMapping>
    {
        public new Response<List<InvItemClassificationCostCenterMapping>> InsertWithIdentity(List<InvItemClassificationCostCenterMapping> entity)
        {
            var repository = GetInstance<IInvItemClassificationCostCenterMapping>();
            var result = SafeExecute(() => repository.InsertWithIdentity(entity));
            return result;
        }
    }
}
