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
    public class HotelRoomNumberDataAccess : GenericDataAccess<HotelRoomNumber>, IHotelRoomNumber
    {
        public HotelRoomNumberDataAccess() 
        {
        }
        public int UpdateRoomStatus(HotelRoomNumber room)
        {
            InnboardDBSet.Attach(room);
            InnboardDBContext.Entry(room).Property(p => p.StatusId).IsModified = true;
            InnboardDBContext.Entry(room).Property(p => p.HKRoomStatusId).IsModified = true;
            InnboardDBContext.Entry(room).Property(p => p.CleanupStatus).IsModified = true;
            return base.SaveChanges();
        }

        public new List<HotelRoomNumber> TruncateAllAndInsert(List<HotelRoomNumber> entity)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                TruncateAll();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[HotelRoomNumber] ON");
                InnboardDBSet.AddRange(entity);
                SaveChanges();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[HotelRoomNumber] OFF");
                transaction.Commit();
                return entity;
            }
        }
    }
}
