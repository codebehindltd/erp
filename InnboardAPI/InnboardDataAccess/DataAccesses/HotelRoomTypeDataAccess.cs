using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.Models.Membership;
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
        public async Task<List<HotelRoomTypeBO>> GetRoomTypeInfo()
        {
            var result = await InnboardDBContext.Database.SqlQuery<HotelRoomTypeBO>("EXEC [dbo].[GetRoomTypeInfo_SP]").ToListAsync();
            return result;
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
