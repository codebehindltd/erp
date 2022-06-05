using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class InvLocationService:GenericService<InvLocation>
    {
        public List<InvLocation> InsertAll(List<InvLocation> entity)
        {
            var repository = GetInstance<IInvLocation>();
            var result = SafeExecute(() => repository.InsertAll(entity));
            return result.Data;
        }
    }
}
