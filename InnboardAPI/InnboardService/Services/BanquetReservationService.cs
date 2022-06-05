using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.Utility;
using InnboardDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class BanquetReservationService: GenericService<BanquetReservation>
    {
        public new Response<BanquetReservation> Save(BanquetReservation entity)
        {
            var repository = GetInstance<IBanquetReservation>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
        public Response<BanquetBillDataSync> Sync(BanquetBillDataSync entity)
        {
            var repository = GetInstance<IBanquetReservation>();
            var result = SafeExecute(() => repository.Sync(entity));
            return result;
        }

        //public Response<BanquetBillDataSync> ConvertDateTimeUTCtoLocalTime(BanquetBillDataSync entity)
        //{
        //    var result = SafeExecute(() => ConvertAllDateTimePropertiesUTCtoLocalTime(entity));
        //    return result;
        //}

        //public BanquetBillDataSync ConvertAllDateTimePropertiesUTCtoLocalTime(BanquetBillDataSync banquet)
        //{
        //    if (banquet.BanquetReservation != null)
        //        CommonHelper.ConvertDateTimePropertiesUTCtoLocalTime(banquet.BanquetReservation);            

        //    if (banquet.BanquetReservationDetails.Count > 0)
        //        CommonHelper.ConvertListDateTimePropertiesUTCtoLocalTime(banquet.BanquetReservationDetails);

        //    if (banquet.GuestBillPayments != null)
        //        CommonHelper.ConvertDateTimePropertiesUTCtoLocalTime(banquet.GuestBillPayments);

        //    if (banquet.CompanyPayments != null)
        //        CommonHelper.ConvertDateTimePropertiesUTCtoLocalTime(banquet.CompanyPayments);

        //    return banquet;
        //}
    }

    
}
