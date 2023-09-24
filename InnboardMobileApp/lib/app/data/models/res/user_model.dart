// To parse this JSON data, do
//
//     final userModel = userModelFromJson(jsonString);

import 'dart:convert';

UserModel userModelFromJson(String str) => UserModel.fromJson(json.decode(str));

String userModelToJson(UserModel data) => json.encode(data.toJson());

class UserModel {
    int? userInfoId;
    dynamic userId;
    int? userGroupId;
    dynamic groupName;
    dynamic userPassword;
    String? userEmail;
    String? userName;
    String? userPhone;
    String? userAddress;
    dynamic userDesignation;
    int? guestOrMemberId;
    int? empId;
    int? supplierId;
    DateTime? createdDate;
    bool? activeStat;
    dynamic activeStatus;
    bool? isAdminUser;
    dynamic innboardHomePage;
    dynamic userGroupType;
    dynamic userSignature;
    int? isPaymentBillInfoHideInCompanyBillReceive;
    int? isReceiveBillInfoHideInSupplierBillPayment;

    UserModel({
        this.userInfoId,
        this.userId,
        this.userGroupId,
        this.groupName,
        this.userPassword,
        this.userEmail,
        this.userName,
        this.userPhone,
        this.userAddress,
        this.userDesignation,
        this.guestOrMemberId,
        this.empId,
        this.supplierId,
        this.createdDate,
        this.activeStat,
        this.activeStatus,
        this.isAdminUser,
        this.innboardHomePage,
        this.userGroupType,
        this.userSignature,
        this.isPaymentBillInfoHideInCompanyBillReceive,
        this.isReceiveBillInfoHideInSupplierBillPayment,
    });

    factory UserModel.fromJson(Map<String, dynamic> json) => UserModel(
        userInfoId: json["UserInfoId"],
        userId: json["UserId"],
        userGroupId: json["UserGroupId"],
        groupName: json["GroupName"],
        userPassword: json["UserPassword"],
        userEmail: json["UserEmail"],
        userName: json["UserName"],
        userPhone: json["UserPhone"],
        userAddress: json["UserAddress"],
        userDesignation: json["UserDesignation"],
        guestOrMemberId: json["GuestOrMemberId"],
        empId: json["EmpId"],
        supplierId: json["SupplierId"],
        createdDate: json["CreatedDate"] == null ? null : DateTime.parse(json["CreatedDate"]),
        activeStat: json["ActiveStat"],
        activeStatus: json["ActiveStatus"],
        isAdminUser: json["IsAdminUser"],
        innboardHomePage: json["InnboardHomePage"],
        userGroupType: json["UserGroupType"],
        userSignature: json["UserSignature"],
        isPaymentBillInfoHideInCompanyBillReceive: json["IsPaymentBillInfoHideInCompanyBillReceive"],
        isReceiveBillInfoHideInSupplierBillPayment: json["IsReceiveBillInfoHideInSupplierBillPayment"],
    );

    Map<String, dynamic> toJson() => {
        "UserInfoId": userInfoId,
        "UserId": userId,
        "UserGroupId": userGroupId,
        "GroupName": groupName,
        "UserPassword": userPassword,
        "UserEmail": userEmail,
        "UserName": userName,
        "UserPhone": userPhone,
        "UserAddress": userAddress,
        "UserDesignation": userDesignation,
        "GuestOrMemberId": guestOrMemberId,
        "EmpId": empId,
        "SupplierId": supplierId,
        "CreatedDate": createdDate?.toIso8601String(),
        "ActiveStat": activeStat,
        "ActiveStatus": activeStatus,
        "IsAdminUser": isAdminUser,
        "InnboardHomePage": innboardHomePage,
        "UserGroupType": userGroupType,
        "UserSignature": userSignature,
        "IsPaymentBillInfoHideInCompanyBillReceive": isPaymentBillInfoHideInCompanyBillReceive,
        "IsReceiveBillInfoHideInSupplierBillPayment": isReceiveBillInfoHideInSupplierBillPayment,
    };
}
