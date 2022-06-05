using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class InvItemService:GenericService<InvItem>
    {
        public new Response<List<InvItem>> TruncateAllAndInsert(List<InvItem> entity)
        {
            var repository = GetInstance<IInvItem>();
            var result = SafeExecute(() => repository.TruncateAllAndInsert(entity));
            return result;
        }
    }
}
