using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class TableManagementBO
    {
        public int TableManagementId { get; set; }
        public int CostCenterId { get; set; }
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int StatusId { get; set; }

        public decimal XCoordinate { get; set; }
        public decimal YCoordinate { get; set; }
        public decimal TableWidth { get; set; }
        public decimal TableHeight { get; set; }
        public string DivTransition { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        //---------------- --------- ------- -------- ------ ----- ----- ------ -----
        public int KotId { get; set; }
        public int BearerId { get; set; }
        public int DetailId { get; set; }
        public int BillId { get; set; }
        public int KotCreatedBy { get; set; }
        public string KotCreatedByName { get; set; }
        public string Remarks { get; set; }

        public string VacantImagePath { get; set; }
        public string OccupiedImagePath { get; set; }
    }
}
