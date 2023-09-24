import 'dart:convert';

List<ReservationListModel> reservationListModelFromJson(String str) =>
    List<ReservationListModel>.from(
        json.decode(str).map((x) => ReservationListModel.fromJson(x)));

// String reservationListModelToJson(List<ReservationListModel> data) => json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class ReservationListModel {
  int? reservationId;
  String? reservationNumber;
  String? reservationDate;
  String? reservationDateDisplay;
  String? dateIn;
  String? dateInDisplay;
  String? dateOut;
  String? dateOutDisplay;
  String? confirmationDate;
  String? reservedCompany;
  int? guestId;
  String? guestName;
  String? contactAddress;
  String? contactPerson;
  String? contactNumber;
  String? mobileNumber;
  String? faxNumber;
  String? contactEmail;
  int? totalRoomNumber;
  String? reservedMode;
  String? reservationType;
  String? reservationMode;
  String? pendingDeadline;
  bool? isListedCompany;
  int? companyId;
  int? businessPromotionId;
  int? referenceId;
  String? paymentMode;
  int? payFor;
  int? currencyType;
  double? conversionRate;
  String? reason;
  String? remarks;
  int? createdBy;
  String? createdDate;
  int? lastModifiedBy;
  String? lastModifiedDate;
  int? numberOfPersonAdult;
  int? numberOfPersonChild;
  bool? isFamilyOrCouple;
  String? airportPickUp;
  String? airportDrop;
  String? roomInfo;
  int? marketSegmentId;
  int? guestSourceId;
  bool? isRoomRateShowInPreRegistrationCard;
  int? mealPlanId;
  int? classificationId;
  bool? isVipGuest;
  int? vipGuestTypeId;
  String? bookersName;
  String? guestRemarks;
  String? posRemarks;
  String? logoUrl;

  ReservationListModel({
    this.reservationId,
    this.reservationNumber,
    this.reservationDate,
    this.reservationDateDisplay,
    this.dateIn,
    this.dateInDisplay,
    this.dateOut,
    this.dateOutDisplay,
    this.confirmationDate,
    this.reservedCompany,
    this.guestId,
    this.guestName,
    this.contactAddress,
    this.contactPerson,
    this.contactNumber,
    this.mobileNumber,
    this.faxNumber,
    this.contactEmail,
    this.totalRoomNumber,
    this.reservedMode,
    this.reservationType,
    this.reservationMode,
    this.pendingDeadline,
    this.isListedCompany,
    this.companyId,
    this.businessPromotionId,
    this.referenceId,
    this.paymentMode,
    this.payFor,
    this.currencyType,
    this.conversionRate,
    this.reason,
    this.remarks,
    this.createdBy,
    this.createdDate,
    this.lastModifiedBy,
    this.lastModifiedDate,
    this.numberOfPersonAdult,
    this.numberOfPersonChild,
    this.isFamilyOrCouple,
    this.airportPickUp,
    this.airportDrop,
    this.roomInfo,
    this.marketSegmentId,
    this.guestSourceId,
    this.isRoomRateShowInPreRegistrationCard,
    this.mealPlanId,
    this.classificationId,
    this.isVipGuest,
    this.vipGuestTypeId,
    this.bookersName,
    this.guestRemarks,
    this.posRemarks,
    this.logoUrl
  });

  factory ReservationListModel.fromJson(Map<String, dynamic> json) =>
      ReservationListModel(
        reservationId: json["ReservationId"],
        reservationNumber: json["ReservationNumber"],
        reservationDate: json["ReservationDate"],
        reservationDateDisplay: json["ReservationDateDisplay"],
        dateIn: json["DateIn"],
        dateInDisplay: json["DateInDisplay"],
        dateOut: json["DateOut"],
        dateOutDisplay: json["DateOutDisplay"],
        confirmationDate: json["ConfirmationDate"],
        reservedCompany: json["ReservedCompany"],
        guestId: json["GuestId"],
        guestName: json["GuestName"],
        contactAddress: json["ContactAddress"],
        contactPerson: json["ContactPerson"],
        contactNumber: json["ContactNumber"],
        mobileNumber: json["MobileNumber"],
        faxNumber: json["FaxNumber"],
        contactEmail: json["ContactEmail"],
        totalRoomNumber: json["TotalRoomNumber"],
        reservedMode: json["ReservedMode"],
        reservationType: json["ReservationType"],
        reservationMode: json["ReservationMode"],
        pendingDeadline: json["PendingDeadline"],
        isListedCompany: json["IsListedCompany"],
        companyId: json["CompanyId"],
        businessPromotionId: json["BusinessPromotionId"],
        referenceId: json["ReferenceId"],
        paymentMode: json["PaymentMode"],
        payFor: json["PayFor"],
        currencyType: json["CurrencyType"],
        conversionRate: json["ConversionRate"]?.toDouble(),
        reason: json["Reason"],
        remarks: json["Remarks"],
        createdBy: json["CreatedBy"],
        createdDate: json["CreatedDate"],
        lastModifiedBy: json["LastModifiedBy"],
        lastModifiedDate: json["LastModifiedDate"],
        numberOfPersonAdult: json["NumberOfPersonAdult"],
        numberOfPersonChild: json["NumberOfPersonChild"],
        isFamilyOrCouple: json["IsFamilyOrCouple"],
        airportPickUp: json["AirportPickUp"],
        airportDrop: json["AirportDrop"],
        roomInfo: json["RoomInfo"],
        marketSegmentId: json["MarketSegmentId"],
        guestSourceId: json["GuestSourceId"],
        isRoomRateShowInPreRegistrationCard:
            json["IsRoomRateShowInPreRegistrationCard"],
        mealPlanId: json["MealPlanId"],
        classificationId: json["ClassificationId"],
        isVipGuest: json["IsVIPGuest"],
        vipGuestTypeId: json["VipGuestTypeId"],
        bookersName: json["BookersName"],
        guestRemarks: json["GuestRemarks"],
        posRemarks: json["POSRemarks"],
        logoUrl: json["LogoUrl"]
      );
}
