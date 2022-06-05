using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.VehicleManagement
{
    public class VMVehicleInformationBO
    {
        public long Id { get; set; }
        public string VehicleName { get; set; }
        public long? AccountHeadId { get; set; }
        public long? ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public Nullable<int> ModelYear { get; set; }
        public Nullable<int> TaxValidationYear { get; set; }
        public Nullable<decimal> Fare { get; set; }
        public Nullable<int> PassengerCapacity { get; set; }
        public Nullable<bool> Status { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleType { get; set; }
        public string NumberPlate { get; set; }
        public Nullable<bool> IsABSEnable { get; set; }
        public Nullable<bool> IsAirBagAvailable { get; set; }
        public string TaxNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string AirConditioningType { get; set; }
        public decimal? Mileage { get; set; }
        public string FuelType { get; set; }
        public decimal? FuelTankCapacity { get; set; }
        public decimal? EngineCapacity { get; set; }
        public string EngineType { get; set; }
        public string BodyType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
