class RoomReservationDetail {
  int? reservationId;
  int? roomQuantity;
  int? roomTypeId;
  String? roomType;
  int? paxQuantity;
  int? childQuantity;
  int? extraBedQuantity;
  String? guestNotes;
  double? totalPrice;
  double? discountPercent;
  double? regularPrice;
  double? discountAmount;
  double? extraBedAmount;

  RoomReservationDetail({
    this.reservationId,
    this.roomQuantity,
    this.roomTypeId,
    this.roomType,
    this.paxQuantity,
    this.childQuantity,
    this.extraBedQuantity,
    this.guestNotes,
    this.totalPrice,
    this.discountPercent,
    this.regularPrice,
    this.discountAmount,
    this.extraBedAmount
  });

  factory RoomReservationDetail.fromJson(Map<String, dynamic> json) =>
      RoomReservationDetail(
        reservationId: json["ReservationId"],
        roomQuantity: json["RoomQuantity"],
        roomTypeId: json["RoomTypeId"],
        roomType: json["roomType"],
        paxQuantity: json["PaxQuantity"],
        childQuantity: json["ChildQuantity"],
        extraBedQuantity: json["ExtraBedQuantity"],
        guestNotes: json["GuestNotes"],
        totalPrice: json["TotalPrice"]?.toDouble(),
      );

  Map<String, dynamic> toJson() => {
        "ReservationId": reservationId,
        "RoomQuantity": roomQuantity,
        "RoomTypeId": roomTypeId,
        "roomType": roomType,
        "PaxQuantity": paxQuantity,
        "ChildQuantity": childQuantity,
        "ExtraBedQuantity": extraBedQuantity,
        "GuestNotes": guestNotes,
        "TotalPrice": totalPrice,
      };
}
