// To parse this JSON data, do
//
//     final roomTypeInfoModel = roomTypeInfoModelFromJson(jsonString);

import 'dart:convert';

List<RoomTypeInfoModel> roomTypeInfoModelFromJson(String str) =>
    List<RoomTypeInfoModel>.from(
        json.decode(str).map((x) => RoomTypeInfoModel.fromJson(x)));

String roomTypeInfoModelToJson(List<RoomTypeInfoModel> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class RoomTypeInfoModel {
  int? roomTypeId;
  String? roomType;
  String? typeCode;
  String? localCurrencyHead;
  double? roomRate;
  double? roomRateUsd;
  double? extrabedRate;
  double? extrabedRateUsd;
  bool? activeStat;
  int? accountsPostingHeadId;
  int? paxQuantity;
  int? childQuantity;
  String? activeStatus;
  int? availableRoomNight;
  double? roomDiscountPercent;
  int? maximumNightAvailPerReservation;

  RoomTypeInfoModel(
      {this.roomTypeId,
      this.roomType,
      this.typeCode,
      this.localCurrencyHead,
      this.roomRate,
      this.roomRateUsd,
      this.extrabedRate,
      this.extrabedRateUsd,
      this.activeStat,
      this.accountsPostingHeadId,
      this.paxQuantity,
      this.childQuantity,
      this.activeStatus,
      this.availableRoomNight,
      this.roomDiscountPercent, 
      this.maximumNightAvailPerReservation});

  factory RoomTypeInfoModel.fromJson(Map<String, dynamic> json) =>
      RoomTypeInfoModel(
          roomTypeId: json["RoomTypeId"],
          roomType: json["RoomType"],
          typeCode: json["TypeCode"],
          localCurrencyHead: json["LocalCurrencyHead"],
          roomRate: json["RoomRate"],
          roomRateUsd: json["RoomRateUSD"]?.toDouble(),
          extrabedRate: json["ExtrabedRate"],
          extrabedRateUsd: json["ExtrabedRateUSD"],
          activeStat: json["ActiveStat"],
          accountsPostingHeadId: json["AccountsPostingHeadId"],
          paxQuantity: json["PaxQuantity"],
          childQuantity: json["ChildQuantity"],
          activeStatus: json["ActiveStatus"],
          availableRoomNight: json["AvailableRoomNight"],
          roomDiscountPercent: json["RoomDiscountPercent"],
          maximumNightAvailPerReservation: json["MaximumNightAvailPerReservation"]);

  Map<String, dynamic> toJson() => {
        "RoomTypeId": roomTypeId,
        "RoomType": roomType,
        "TypeCode": typeCode,
        "LocalCurrencyHead": localCurrencyHead,
        "RoomRate": roomRate,
        "RoomRateUSD": roomRateUsd,
        "ExtrabedRate": extrabedRate,
        "ExtrabedRateUSD": extrabedRateUsd,
        "ActiveStat": activeStat,
        "AccountsPostingHeadId": accountsPostingHeadId,
        "PaxQuantity": paxQuantity,
        "ChildQuantity": childQuantity,
        "ActiveStatus": activeStatus,
      };
}
