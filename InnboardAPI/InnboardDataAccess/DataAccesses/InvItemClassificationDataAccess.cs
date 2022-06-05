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
    public class InvItemClassificationDataAccess : GenericDataAccess<InvItemClassification>, IInvItemClassification
    {
        public new List<InvItemClassification> InsertWithIdentity(List<InvItemClassification> entity)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItemClassification] ON");
                InnboardDBSet.AddRange(entity);
                SaveChanges();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItemClassification] OFF");
                transaction.Commit();
                return entity;
            }
        }
    }
}
