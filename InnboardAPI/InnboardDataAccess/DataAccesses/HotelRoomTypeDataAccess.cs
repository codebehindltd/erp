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
    public class HotelRoomTypeDataAccess:GenericDataAccess<HotelRoomType>, IHotelRoomType
    {
        public HotelRoomTypeDataAccess()
        {

        }
        public new List<HotelRoomType> TruncateAllAndInsert(List<HotelRoomType> entity)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                TruncateAll();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[HotelRoomType] ON");
                InnboardDBSet.AddRange(entity);
                SaveChanges();
                InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[HotelRoomType] OFF");
                transaction.Commit();
                return entity;
            }
        }
    }
}
