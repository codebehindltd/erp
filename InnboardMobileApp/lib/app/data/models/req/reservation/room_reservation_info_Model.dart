// To parse this JSON data, do
//
//     final roomReservationInfoModel = roomReservationInfoModelFromJson(jsonString);

import 'dart:convert';

import 'hotel_room_reservation_details.dart';

RoomReservationInfoModel roomReservationInfoModelFromJson(String str) => RoomReservationInfoModel.fromJson(json.decode(str));

String roomReservationInfoModelToJson(RoomReservationInfoModel data) => json.encode(data.toJson());

class RoomReservationInfoModel {
    int? reservationId;
    int? guestSourceId;
    String? transactionType;
    int? memberId;
    String? fromDate;
    String? toDate;
    String? guestName;
    String? phoneNumber;
    String? guestRemarks;
    String? transactionId;
    int? transactionAmount;
    String? transactionDetails;
    int? createdBy;
    List<RoomReservationDetail>? roomReservationDetails;

    RoomReservationInfoModel({
        this.reservationId,
        this.guestSourceId,
        this.transactionType,
        this.memberId,
        this.fromDate,
        this.toDate,
        this.guestName,
        this.phoneNumber,
        this.guestRemarks,
        this.transactionId,
        this.transactionAmount,
        this.transactionDetails,
        this.createdBy,
        this.roomReservationDetails,
    });

    factory RoomReservationInfoModel.fromJson(Map<String, dynamic> json) => RoomReservationInfoModel(
        reservationId: json["ReservationId"],
        guestSourceId: json["GuestSourceId"],
        transactionType: json["TransactionType"],
        memberId: json["MemberId"],
        fromDate: json["FromDate"],
        toDate: json["ToDate"],
        guestName: json["GuestName"],
        phoneNumber: json["PhoneNumber"],
        guestRemarks: json["GuestRemarks"],
        transactionId: json["TransactionId"],
        transactionAmount: json["TransactionAmount"],
        transactionDetails: json["TransactionDetails"],
        createdBy: json["CreatedBy"],
        roomReservationDetails: json["HotelRoomReservationDetails"] == null ? [] : List<RoomReservationDetail>.from(json["HotelRoomReservationDetails"]!.map((x) => RoomReservationDetail.fromJson(x))),
    );

    Map<String, dynamic> toJson() => {
        "ReservationId": reservationId,
        "GuestSourceId": guestSourceId,
        "TransactionType": transactionType,
        "MemberId": memberId,
        "FromDate": fromDate,
        "ToDate": toDate,
        "GuestName": guestName,
        "PhoneNumber": phoneNumber,
        "GuestRemarks": guestRemarks,
        "TransactionId": transactionId,
        "TransactionAmount": transactionAmount,
        "TransactionDetails": transactionDetails,
        "CreatedBy": createdBy,
        "HotelRoomReservationDetails": roomReservationDetails == null ? [] : List<dynamic>.from(roomReservationDetails!.map((x) => x.toJson())),
    };
}


