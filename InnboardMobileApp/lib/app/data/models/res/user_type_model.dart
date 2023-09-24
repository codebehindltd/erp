// To parse this JSON data, do
//
//     final userTypeModel = userTypeModelFromJson(jsonString);

import 'dart:convert';

List<UserTypeModel> userTypeModelFromJson(String str) => List<UserTypeModel>.from(json.decode(str).map((x) => UserTypeModel.fromJson(x)));

String userTypeModelToJson(List<UserTypeModel> data) => json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class UserTypeModel {
    int? fieldId;
    String? fieldValue;
    String? fieldType;
    String? description;
    bool? activeStat;
    DateTime? transactionDate;

    UserTypeModel({
        this.fieldId,
        this.fieldValue,
        this.fieldType,
        this.description,
        this.activeStat,
        this.transactionDate,
    });

    factory UserTypeModel.fromJson(Map<String, dynamic> json) => UserTypeModel(
        fieldId: json["FieldId"],
        fieldValue: json["FieldValue"],
        fieldType: json["FieldType"],
        description: json["Description"],
        activeStat: json["ActiveStat"],
        transactionDate: json["TransactionDate"] == null ? null : DateTime.parse(json["TransactionDate"]),
    );

    Map<String, dynamic> toJson() => {
        "FieldId": fieldId,
        "FieldValue": fieldValue,
        "FieldType": fieldType,
        "Description": description,
        "ActiveStat": activeStat,
        "TransactionDate": transactionDate?.toIso8601String(),
    };
}
