import 'dart:convert';

ReservationResModel reservationResModelFromJson(String str) =>
    ReservationResModel.fromJson(json.decode(str));

String reservationResModelToJson(ReservationResModel data) =>
    json.encode(data.toJson());

class ReservationResModel {
  int? guestId;
  int? reservationId;

  ReservationResModel({
    this.guestId,
    this.reservationId,
  });

  factory ReservationResModel.fromJson(Map<String, dynamic> json) =>
      ReservationResModel(
        guestId: json["GuestId"],
        reservationId: json["ReservationId"],
      );

  Map<String, dynamic> toJson() => {
        "GuestId": guestId,
        "ReservationId": reservationId,
      };
}
