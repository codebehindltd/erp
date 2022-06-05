using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class InvItemClassificationService:GenericService<InvItemClassification>
    {
        public new Response<List<InvItemClassification>> InsertWithIdentity(List<InvItemClassification> entity)
        {
            var repository = GetInstance<IInvItemClassification>();
            var result = SafeExecute(() => repository.InsertWithIdentity(entity));
            return result;
        }
    }
}
