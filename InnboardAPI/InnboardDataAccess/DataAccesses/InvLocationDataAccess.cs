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
    public class InvLocationDataAccess:GenericDataAccess<InvLocation>, IInvLocation
    {
        public InvLocationDataAccess()
        {

        }

        public override InvLocation Insert(InvLocation entity)
        {
            InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT[dbo].[InvLocation] ON");
            base.SaveChanges();
            InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT[dbo].[InvLocation] OFF");
            return entity;
        }
        public List<InvLocation> InsertAll(List<InvLocation> entity)
        {
            InnboardDBContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[InvLocation]");
            InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT[dbo].[InvLocation] ON");
            base.SaveAll(entity);
            base.SaveChanges();
            InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT[dbo].[InvLocation] OFF");
            return entity;
        }
    }
}
