using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class DailyConsolidatedRevenueCenterSalesDetailViewBO
    {
        public List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO> CostcenterWisesalesDetails = new List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO>();
        public List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO> SalesDetails = new List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO>();
        public List<DailyConsolidatedRevenueSalesPaymentDetailsBO> PaymentDetails = new List<DailyConsolidatedRevenueSalesPaymentDetailsBO>();


        public List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO> CostcenterWisesalesDetailsLunch = new List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO>();
        public List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO> SalesDetailsLunch = new List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO>();
        public List<DailyConsolidatedRevenueSalesPaymentDetailsBO> PaymentDetailsLunch = new List<DailyConsolidatedRevenueSalesPaymentDetailsBO>();

        public List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO> CostcenterWisesalesDetailsSnacks = new List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO>();
        public List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO> SalesDetailsSnacks = new List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO>();
        public List<DailyConsolidatedRevenueSalesPaymentDetailsBO> PaymentDetailsSnacks = new List<DailyConsolidatedRevenueSalesPaymentDetailsBO>();

        public List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO> CostcenterWisesalesDetailsDinner = new List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO>();
        public List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO> SalesDetailsDinner = new List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO>();
        public List<DailyConsolidatedRevenueSalesPaymentDetailsBO> PaymentDetailsDinner = new List<DailyConsolidatedRevenueSalesPaymentDetailsBO>();
    }
}
