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
    public class InvItemClassificationCostCenterMappingDataAccess:GenericDataAccess<InvItemClassificationCostCenterMapping>, IInvItemClassificationCostCenterMapping
    {
        public InvItemClassificationCostCenterMappingDataAccess()
        {

        }

        public new List<InvItemClassificationCostCenterMapping> InsertWithIdentity(List<InvItemClassificationCostCenterMapping> entity)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItemClassificationCostCenterMapping] ON");
                InnboardDBSet.AddRange(entity);
                SaveChanges();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItemClassificationCostCenterMapping] OFF");
                transaction.Commit();
                return entity;
            }
        }
    }
}
