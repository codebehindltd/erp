using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDataAccess.DataAccesses
{
    public class InvItemCostCenterMappingDataAccess:GenericDataAccess<InvItemCostCenterMapping>, IInvItemCostCenterMapping
    {
        public new List<InvItemCostCenterMapping> TruncateAllAndInsert(List<InvItemCostCenterMapping> entity)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                TruncateAll();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItemCostCenterMapping] ON");
                InnboardDBSet.AddRange(entity);
                SaveChanges();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItemCostCenterMapping] OFF");
                transaction.Commit();
                return entity;
            }
        }
    }
}
