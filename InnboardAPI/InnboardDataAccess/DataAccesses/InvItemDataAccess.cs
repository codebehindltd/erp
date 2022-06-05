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
    public class InvItemDataAccess : GenericDataAccess<InvItem>, IInvItem
    {
        public new List<InvItem> TruncateAllAndInsert(List<InvItem> entity)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                TruncateAll();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItem] ON");
                InnboardDBSet.AddRange(entity);
                SaveChanges();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[InvItem] OFF");
                transaction.Commit();
                return entity;
            }
        }
    }
}
