// To parse this JSON data, do
//
//     final promotionalOfferModel = promotionalOfferModelFromJson(jsonString);

import 'dart:convert';

List<PromotionalOfferModel> promotionalOfferModelFromJson(String str) =>
    List<PromotionalOfferModel>.from(
        json.decode(str).map((x) => PromotionalOfferModel.fromJson(x)));

String promotionalOfferModelToJson(List<PromotionalOfferModel> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class PromotionalOfferModel {
  int? costCenterId;
  String? benefitsType;
  String? benefitsName;
  String? benefitsDetails;
  double? benefitsValue;
  double? enjoyedBenefitsValue;
  double? remainingBenefitsValue;
  String? benefitsTransactionType;
  int? displaySequence;
  String? propertyName;
  int? roomNights;
  int? availNights;
  int? balanceNight;

  PromotionalOfferModel({
    this.costCenterId,
    this.benefitsType,
    this.benefitsName,
    this.benefitsDetails,
    this.benefitsValue,
    this.enjoyedBenefitsValue,
    this.remainingBenefitsValue,
    this.benefitsTransactionType,
    this.displaySequence,
    this.propertyName,
    this.roomNights,
    this.availNights,
    this.balanceNight,
  });

  factory PromotionalOfferModel.fromJson(Map<String, dynamic> json) =>
      PromotionalOfferModel(
        costCenterId: json["CostCenterId"],
        benefitsType: json["BenefitsType"],
        benefitsName: json["BenefitsName"],
        benefitsDetails: json["BenefitsDetails"],
        benefitsValue: json["BenefitsValue"],
        enjoyedBenefitsValue: json["EnjoyedBenefitsValue"],
        remainingBenefitsValue: json["RemainingBenefitsValue"],
        benefitsTransactionType: json["BenefitsTransactionType"],
        displaySequence: json["DisplaySequence"],
        propertyName: json["PropertyName"],
        roomNights: json["RoomNights"],
        availNights: json["AvailNights"],
        balanceNight: json["BalanceNight"],
      );

  Map<String, dynamic> toJson() => {
        "CostCenterId": costCenterId,
        "BenefitsType": benefitsType,
        "BenefitsName": benefitsName,
        "BenefitsDetails": benefitsDetails,
        "BenefitsValue": benefitsValue,
        "EnjoyedBenefitsValue": enjoyedBenefitsValue,
        "RemainingBenefitsValue": remainingBenefitsValue,
        "BenefitsTransactionType": benefitsTransactionType,
        "DisplaySequence": displaySequence,
        "PropertyName": propertyName,
        "RoomNights": roomNights,
        "AvailNights": availNights,
        "BalanceNight": balanceNight,
      };
}
