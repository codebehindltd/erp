using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Interfaces
{
    public interface IInvLocation: IGenericRepository<InvLocation>
    {
        List<InvLocation> InsertAll(List<InvLocation> entity);
    }
}
