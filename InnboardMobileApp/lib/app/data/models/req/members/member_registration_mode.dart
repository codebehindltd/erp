// To parse this JSON data, do
//
//     final memberRegistrationModel = memberRegistrationModelFromJson(jsonString);

import 'dart:convert';

MemberRegistrationModel memberRegistrationModelFromJson(String str) => MemberRegistrationModel.fromJson(json.decode(str));

String memberRegistrationModelToJson(MemberRegistrationModel data) => json.encode(data.toJson());

class MemberRegistrationModel {
    int? typeId;
    String? fullName;
    String? mobileNumber;
    String? personalEmail;
    int? createdBy;

    MemberRegistrationModel({
        this.typeId,
        this.fullName,
        this.mobileNumber,
        this.personalEmail,
        this.createdBy,
    });

    factory MemberRegistrationModel.fromJson(Map<String, dynamic> json) => MemberRegistrationModel(
        typeId: json["TypeId"],
        fullName: json["FullName"],
        mobileNumber: json["MobileNumber"],
        personalEmail: json["PersonalEmail"],
        createdBy: json["CreatedBy"],
    );

    Map<String, dynamic> toJson() => {
        "TypeId": typeId,
        "FullName": fullName,
        "MobileNumber": mobileNumber,
        "PersonalEmail": personalEmail,
        "CreatedBy": createdBy,
    };
}
