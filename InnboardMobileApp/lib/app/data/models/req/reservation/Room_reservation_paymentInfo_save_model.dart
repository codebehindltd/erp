

import 'dart:convert';

RoomReservationPaymentInfoSaveModel roomReservationPaymentInfoSaveModelFromJson(String str) => RoomReservationPaymentInfoSaveModel.fromJson(json.decode(str));

String roomReservationPaymentInfoSaveModelToJson(RoomReservationPaymentInfoSaveModel data) => json.encode(data.toJson());

class RoomReservationPaymentInfoSaveModel {
    int? reservationId;
    String? transactionType;
    String? transactionId;
    double? transactionAmount;
    String? transactionDetails;
    int? createdBy;

    RoomReservationPaymentInfoSaveModel({
        this.reservationId,
        this.transactionType,
        this.transactionId,
        this.transactionAmount,
        this.transactionDetails,
        this.createdBy,
    });

    factory RoomReservationPaymentInfoSaveModel.fromJson(Map<String, dynamic> json) => RoomReservationPaymentInfoSaveModel(
        reservationId: json["ReservationId"],
        transactionType: json["TransactionType"],
        transactionId: json["TransactionId"],
        transactionAmount: json["TransactionAmount"],
        transactionDetails: json["TransactionDetails"],
        createdBy: json["CreatedBy"],
    );

    Map<String, dynamic> toJson() => {
        "ReservationId": reservationId,
        "TransactionType": transactionType,
        "TransactionId": transactionId,
        "TransactionAmount": transactionAmount,
        "TransactionDetails": transactionDetails,
        "CreatedBy": createdBy,
    };
}
