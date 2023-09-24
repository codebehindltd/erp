import 'dart:convert';

List<MemberShipType> memberShipTypeFromJson(String str) =>
    List<MemberShipType>.from(
        json.decode(str).map((x) => MemberShipType.fromJson(x)));

String memberShipTypeToJson(List<MemberShipType> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class MemberShipType {
  int? typeId;
  String? name;
  String? code;
  double? subscriptionFee;
  double? minimumInstallmentSubscriptionFee;
  dynamic discountPercent;
  int? costCenterId;
  String? benefitsType;
  String? benefitsName;
  dynamic benefitsDetails;
  double? benefitsValue;
  String? benefitsTransactionType;
  String? tncType;
  dynamic tncName;
  String? tncDetails;
  int? displaySequence;
  List<MemberShipType>? benefits;
  List<MemberShipType>? termTitles;
  List<MemberShipType>? terms;
  List<MemberShipType>? termSubTitiles;

  MemberShipType(
      {this.typeId,
      this.name,
      this.code,
      this.subscriptionFee,
      this.minimumInstallmentSubscriptionFee,
      this.discountPercent,
      this.costCenterId,
      this.benefitsType,
      this.benefitsName,
      this.benefitsDetails,
      this.benefitsValue,
      this.benefitsTransactionType,
      this.tncType,
      this.tncName,
      this.tncDetails,
      this.displaySequence,
      this.benefits,
      this.terms,
      this.termTitles,
      this.termSubTitiles});

  factory MemberShipType.fromJson(Map<String, dynamic> json) => MemberShipType(
      typeId: json["TypeId"],
      name: json["Name"],
      code: json["Code"],
      subscriptionFee: json["SubscriptionFee"],
      minimumInstallmentSubscriptionFee:
          json["MinimumInstallmentSubscriptionFee"],
      discountPercent: json["DiscountPercent"],
      costCenterId: json["CostCenterId"],
      benefitsType: json["BenefitsType"],
      benefitsName: json["BenefitsName"],
      benefitsDetails: json["BenefitsDetails"],
      benefitsValue: json["BenefitsValue"],
      benefitsTransactionType: json["BenefitsTransactionType"],
      tncType: json["TNCType"],
      tncName: json["TNCName"],
      tncDetails: json["TNCDetails"],
      displaySequence: json["DisplaySequence"],
      benefits: [],
      terms: [],
      termTitles:[], 
      termSubTitiles: []);

  Map<String, dynamic> toJson() => {
        "TypeId": typeId,
        "Name": name,
        "Code": code,
        "SubscriptionFee": subscriptionFee,
        "MinimumInstallmentSubscriptionFee": minimumInstallmentSubscriptionFee,
        "DiscountPercent": discountPercent,
        "CostCenterId": costCenterId,
        "BenefitsType": benefitsType,
        "BenefitsName": benefitsName,
        "BenefitsDetails": benefitsDetails,
        "BenefitsValue": benefitsValue,
        "BenefitsTransactionType": benefitsTransactionType,
        "TNCType": tncType,
        "TNCName": tncName,
        "TNCDetails": tncDetails,
        "DisplaySequence": displaySequence,
      };
}
