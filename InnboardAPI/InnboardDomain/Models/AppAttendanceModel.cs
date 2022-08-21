using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InnboardDomain.Models
{
    public class AppAttendanceModel
    {
        public int EmpId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime AttDateTime { get; set; }
        public double IntAttDateTime { get; set; }
        public virtual byte[] ImageByte { get; set; }
        public virtual string ImageName { get; set; }
        public string Image { get; set; }
        public string GoogleMapUrl { get; set; }
        //public  HttpPostedFileBase ImageFile { get; set; }

    }
}
