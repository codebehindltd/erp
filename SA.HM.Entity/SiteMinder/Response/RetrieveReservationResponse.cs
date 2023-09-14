using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Xml;

namespace HotelManagement.Entity.SiteMinder.Response
{
    internal class RetrieveReservationResponse
    {
    }

    [XmlRoot(ElementName = "RequestorID")]
    public class RequestorID
    {

        [XmlAttribute(AttributeName = "Type")]
        public int Type { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "CompanyName")]
    public class CompanyName
    {

        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "BookingChannel")]
    public class BookingChannel
    {

        [XmlElement(ElementName = "CompanyName")]
        public CompanyName CompanyName { get; set; }

        [XmlAttribute(AttributeName = "Primary")]
        public bool Primary { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public int Type { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Source")]
    public class Source
    {

        [XmlElement(ElementName = "RequestorID")]
        public RequestorID RequestorID { get; set; }

        [XmlElement(ElementName = "BookingChannel")]
        public BookingChannel BookingChannel { get; set; }
    }

    [XmlRoot(ElementName = "POS")]
    public class POS
    {

        [XmlElement(ElementName = "Source")]
        public Source Source { get; set; }
    }

    [XmlRoot(ElementName = "UniqueID")]
    public class UniqueID
    {

        [XmlAttribute(AttributeName = "Type")]
        public int Type { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "ID_Context")]
        public string IDContext { get; set; }
    }

    [XmlRoot(ElementName = "RoomType")]
    public class RoomType
    {

        [XmlAttribute(AttributeName = "RoomType")]
        public string RoomTypeName { get; set; }

        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public string RoomTypeCode { get; set; }

        [XmlAttribute(AttributeName = "NonSmoking")]
        public bool NonSmoking { get; set; }

        [XmlAttribute(AttributeName = "Configuration")]
        public string Configuration { get; set; }
    }

    [XmlRoot(ElementName = "RoomTypes")]
    public class RoomTypes
    {

        [XmlElement(ElementName = "RoomType")]
        public RoomType RoomType { get; set; }
    }

    [XmlRoot(ElementName = "RatePlan")]
    public class RatePlan
    {

        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }

        [XmlAttribute(AttributeName = "RatePlanName")]
        public string RatePlanName { get; set; }

        [XmlAttribute(AttributeName = "EffectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [XmlAttribute(AttributeName = "ExpireDate")]
        public DateTime ExpireDate { get; set; }
    }

    [XmlRoot(ElementName = "RatePlans")]
    public class RatePlans
    {

        [XmlElement(ElementName = "RatePlan")]
        public RatePlan RatePlan { get; set; }
    }

    [XmlRoot(ElementName = "Taxes")]
    public class Taxes
    {

        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }
    }

    [XmlRoot(ElementName = "Total")]
    public class Total
    {

        [XmlElement(ElementName = "Taxes")]
        public Taxes Taxes { get; set; }

        [XmlAttribute(AttributeName = "AmountAfterTax")]
        public double AmountAfterTax { get; set; }

        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "Rate")]
    public class Rate
    {

        [XmlElement(ElementName = "Total")]
        public Total Total { get; set; }

        [XmlAttribute(AttributeName = "UnitMultiplier")]
        public int UnitMultiplier { get; set; }

        [XmlAttribute(AttributeName = "RateTimeUnit")]
        public string RateTimeUnit { get; set; }

        [XmlAttribute(AttributeName = "EffectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [XmlAttribute(AttributeName = "ExpireDate")]
        public DateTime ExpireDate { get; set; }
    }

    [XmlRoot(ElementName = "Rates")]
    public class Rates
    {

        [XmlElement(ElementName = "Rate")]
        public Rate Rate { get; set; }
    }

    [XmlRoot(ElementName = "RoomRate")]
    public class RoomRate
    {

        [XmlElement(ElementName = "Rates")]
        public Rates Rates { get; set; }

        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public string RoomTypeCode { get; set; }

        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }

        [XmlAttribute(AttributeName = "NumberOfUnits")]
        public int NumberOfUnits { get; set; }
    }

    [XmlRoot(ElementName = "RoomRates")]
    public class RoomRates
    {

        [XmlElement(ElementName = "RoomRate")]
        public RoomRate RoomRate { get; set; }
    }

    [XmlRoot(ElementName = "GuestCount")]
    public class GuestCount
    {

        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public int AgeQualifyingCode { get; set; }

        [XmlAttribute(AttributeName = "Count")]
        public int Count { get; set; }
    }

    [XmlRoot(ElementName = "GuestCounts")]
    public class GuestCounts
    {

        [XmlElement(ElementName = "GuestCount")]
        public List<GuestCount> GuestCount { get; set; }
    }

    [XmlRoot(ElementName = "TimeSpan")]
    public class TimeSpan
    {

        [XmlAttribute(AttributeName = "Start")]
        public DateTime Start { get; set; }

        [XmlAttribute(AttributeName = "End")]
        public DateTime End { get; set; }
    }

    [XmlRoot(ElementName = "BasicPropertyInfo")]
    public class BasicPropertyInfo
    {

        [XmlAttribute(AttributeName = "HotelCode")]
        public string HotelCode { get; set; }
    }

    [XmlRoot(ElementName = "ServiceRPH")]
    public class ServiceRPH
    {

        [XmlAttribute(AttributeName = "RPH")]
        public int RPH { get; set; }
    }

    [XmlRoot(ElementName = "ServiceRPHs")]
    public class ServiceRPHs
    {

        [XmlElement(ElementName = "ServiceRPH")]
        public List<ServiceRPH> ServiceRPH { get; set; }
    }

    [XmlRoot(ElementName = "RoomStay")]
    public class RoomStay
    {

        [XmlElement(ElementName = "RoomTypes")]
        public RoomTypes RoomTypes { get; set; }

        [XmlElement(ElementName = "RatePlans")]
        public RatePlans RatePlans { get; set; }

        [XmlElement(ElementName = "RoomRates")]
        public RoomRates RoomRates { get; set; }

        [XmlElement(ElementName = "GuestCounts")]
        public GuestCounts GuestCounts { get; set; }

        [XmlElement(ElementName = "TimeSpan")]
        public TimeSpan TimeSpan { get; set; }

        [XmlElement(ElementName = "Total")]
        public Total Total { get; set; }

        [XmlElement(ElementName = "BasicPropertyInfo")]
        public BasicPropertyInfo BasicPropertyInfo { get; set; }

        [XmlElement(ElementName = "ServiceRPHs")]
        public ServiceRPHs ServiceRPHs { get; set; }
    }

    [XmlRoot(ElementName = "RoomStays")]
    public class RoomStays
    {

        [XmlElement(ElementName = "RoomStay")]
        public RoomStay RoomStay { get; set; }
    }

    [XmlRoot(ElementName = "RateDescription")]
    public class RateDescription
    {

        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Price")]
    public class Price
    {

        [XmlElement(ElementName = "Total")]
        public Total Total { get; set; }

        [XmlElement(ElementName = "RateDescription")]
        public RateDescription RateDescription { get; set; }
    }

    [XmlRoot(ElementName = "Service")]
    public class Service
    {

        [XmlElement(ElementName = "Price")]
        public Price Price { get; set; }

        [XmlAttribute(AttributeName = "ServiceInventoryCode")]
        public string ServiceInventoryCode { get; set; }

        [XmlAttribute(AttributeName = "Inclusive")]
        public bool Inclusive { get; set; }

        [XmlAttribute(AttributeName = "Quantity")]
        public int Quantity { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public int Type { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "ServiceRPH")]
        public int ServiceRPH { get; set; }
    }

    [XmlRoot(ElementName = "Services")]
    public class Services
    {

        [XmlElement(ElementName = "Service")]
        public List<Service> Service { get; set; }
    }

    [XmlRoot(ElementName = "PersonName")]
    public class PersonName
    {

        [XmlElement(ElementName = "NamePrefix")]
        public string NamePrefix { get; set; }

        [XmlElement(ElementName = "GivenName")]
        public string GivenName { get; set; }

        [XmlElement(ElementName = "Surname")]
        public string Surname { get; set; }
    }

    [XmlRoot(ElementName = "Telephone")]
    public class Telephone
    {

        [XmlAttribute(AttributeName = "PhoneNumber")]
        public double PhoneNumber { get; set; }
    }

    [XmlRoot(ElementName = "Address")]
    public class Address
    {

        [XmlElement(ElementName = "AddressLine")]
        public string AddressLine { get; set; }

        [XmlElement(ElementName = "CityName")]
        public string CityName { get; set; }

        [XmlElement(ElementName = "PostalCode")]
        public int PostalCode { get; set; }

        [XmlElement(ElementName = "StateProv")]
        public string StateProv { get; set; }

        [XmlElement(ElementName = "CountryName")]
        public string CountryName { get; set; }
    }

    [XmlRoot(ElementName = "Customer")]
    public class Customer
    {

        [XmlElement(ElementName = "PersonName")]
        public PersonName PersonName { get; set; }

        [XmlElement(ElementName = "Telephone")]
        public Telephone Telephone { get; set; }

        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }

        [XmlElement(ElementName = "Address")]
        public Address Address { get; set; }
    }

    [XmlRoot(ElementName = "Profile")]
    public class Profile
    {

        [XmlElement(ElementName = "Customer")]
        public Customer Customer { get; set; }

        [XmlAttribute(AttributeName = "ProfileType")]
        public int ProfileType { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ProfileInfo")]
    public class ProfileInfo
    {

        [XmlElement(ElementName = "Profile")]
        public Profile Profile { get; set; }
    }

    [XmlRoot(ElementName = "Profiles")]
    public class Profiles
    {

        [XmlElement(ElementName = "ProfileInfo")]
        public ProfileInfo ProfileInfo { get; set; }
    }

    [XmlRoot(ElementName = "ResGuest")]
    public class ResGuest
    {

        [XmlElement(ElementName = "Profiles")]
        public Profiles Profiles { get; set; }

        [XmlAttribute(AttributeName = "ResGuestRPH")]
        public int ResGuestRPH { get; set; }

        [XmlAttribute(AttributeName = "ArrivalTime")]
        public DateTime ArrivalTime { get; set; }

        [XmlAttribute(AttributeName = "PrimaryIndicator")]
        public bool PrimaryIndicator { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ResGuests")]
    public class ResGuests
    {

        [XmlElement(ElementName = "ResGuest")]
        public ResGuest ResGuest { get; set; }
    }

    [XmlRoot(ElementName = "AmountPercent")]
    public class AmountPercent
    {

        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }

        [XmlAttribute(AttributeName = "TaxInclusive")]
        public bool TaxInclusive { get; set; }
    }

    [XmlRoot(ElementName = "GuaranteePayment")]
    public class GuaranteePayment
    {

        [XmlElement(ElementName = "AmountPercent")]
        public AmountPercent AmountPercent { get; set; }
    }

    [XmlRoot(ElementName = "DepositPayments")]
    public class DepositPayments
    {

        [XmlElement(ElementName = "GuaranteePayment")]
        public GuaranteePayment GuaranteePayment { get; set; }
    }

    [XmlRoot(ElementName = "HotelReservationID")]
    public class HotelReservationID
    {

        [XmlAttribute(AttributeName = "ResID_Type")]
        public int ResIDType { get; set; }

        [XmlAttribute(AttributeName = "ResID_Value")]
        public string ResIDValue { get; set; }
    }

    [XmlRoot(ElementName = "HotelReservationIDs")]
    public class HotelReservationIDs
    {

        [XmlElement(ElementName = "HotelReservationID")]
        public HotelReservationID HotelReservationID { get; set; }
    }

    [XmlRoot(ElementName = "Comment")]
    public class Comment
    {

        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Comments")]
    public class Comments
    {

        [XmlElement(ElementName = "Comment")]
        public Comment Comment { get; set; }
    }

    [XmlRoot(ElementName = "ResGlobalInfo")]
    public class ResGlobalInfo
    {

        [XmlElement(ElementName = "DepositPayments")]
        public DepositPayments DepositPayments { get; set; }

        [XmlElement(ElementName = "Total")]
        public Total Total { get; set; }

        [XmlElement(ElementName = "HotelReservationIDs")]
        public HotelReservationIDs HotelReservationIDs { get; set; }

        [XmlElement(ElementName = "Profiles")]
        public Profiles Profiles { get; set; }

        [XmlElement(ElementName = "Comments")]
        public Comments Comments { get; set; }

        [XmlElement(ElementName = "BasicPropertyInfo")]
        public BasicPropertyInfo BasicPropertyInfo { get; set; }
    }

    [XmlRoot(ElementName = "HotelReservation")]
    public class HotelReservation
    {

        [XmlElement(ElementName = "POS")]
        public POS POS { get; set; }

        [XmlElement(ElementName = "UniqueID")]
        public List<UniqueID> UniqueID { get; set; }

        [XmlElement(ElementName = "RoomStays")]
        public RoomStays RoomStays { get; set; }

        [XmlElement(ElementName = "Services")]
        public Services Services { get; set; }

        [XmlElement(ElementName = "ResGuests")]
        public ResGuests ResGuests { get; set; }

        [XmlElement(ElementName = "ResGlobalInfo")]
        public ResGlobalInfo ResGlobalInfo { get; set; }

        [XmlAttribute(AttributeName = "CreateDateTime")]
        public DateTime CreateDateTime { get; set; }

        [XmlAttribute(AttributeName = "ResStatus")]
        public string ResStatus { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ReservationsList")]
    public class ReservationsList
    {

        [XmlElement(ElementName = "HotelReservation")]
        public HotelReservation HotelReservation { get; set; }
    }

    [XmlRoot(ElementName = "OTA_ResRetrieveRS")]
    public class OTAResRetrieveRS
    {

        [XmlElement(ElementName = "Success")]
        public object Success { get; set; }

        [XmlElement(ElementName = "ReservationsList")]
        public ReservationsList ReservationsList { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public double Version { get; set; }

        [XmlAttribute(AttributeName = "TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [XmlAttribute(AttributeName = "EchoToken")]
        public string EchoToken { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Body")]
    public class Body
    {

        [XmlElement(ElementName = "OTA_ResRetrieveRS")]
        public OTAResRetrieveRS OTAResRetrieveRS { get; set; }
    }
}
