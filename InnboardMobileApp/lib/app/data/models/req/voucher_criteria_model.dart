// To parse this JSON data, do
//
//     final voucherCriteriaModel = voucherCriteriaModelFromJson(jsonString);

import 'dart:convert';

VoucherCriteriaModel voucherCriteriaModelFromJson(String str) =>
    VoucherCriteriaModel.fromJson(json.decode(str));

String voucherCriteriaModelToJson(VoucherCriteriaModel data) =>
    json.encode(data.toJson());

class VoucherCriteriaModel {
  VoucherCriteriaModel({
    required this.pageNumber,
    required this.pageSize,
    required this.companyId,
    required this.projectId,
    required this.userGroupId,
    required this.userInfoId,
    this.fromDate,
    this.toDate,
    this.voucherNo,
    this.referenceNo,
    this.referenceVoucherNo,
    this.narration,
    this.voucherType,
    this.voucherStatus,
  });

  int pageNumber;
  int pageSize;
  String companyId;
  String projectId;
  String userGroupId;
  String userInfoId;
  String? fromDate;
  String? toDate;
  String? voucherNo;
  String? referenceNo;
  String? referenceVoucherNo;
  String? narration;
  String? voucherType;
  String? voucherStatus;

  factory VoucherCriteriaModel.fromJson(Map<String, dynamic> json) =>
      VoucherCriteriaModel(
        pageNumber: json["PageNumber"],
        pageSize: json["PageSize"],
        companyId: json["companyId"],
        projectId: json["projectId"],
        userGroupId: json["userGroupId"],
        userInfoId: json["userInfoId"],
        fromDate: json["fromDate"],
        toDate: json["toDate"],
        voucherNo: json["voucherNo"],
        referenceNo: json["referenceNo"],
        referenceVoucherNo: json["referenceVoucherNo"],
        narration: json["narration"],
        voucherType: json["voucherType"],
        voucherStatus: json["voucherStatus"],
      );

  Map<String, dynamic> toJson() => {
        "PageNumber": pageNumber,
        "PageSize": pageSize,
        "companyId": companyId,
        "projectId": projectId,
        "userGroupId": userGroupId,
        "userInfoId": userInfoId,
        "fromDate": fromDate,
        "toDate": toDate,
        "voucherNo": voucherNo,
        "referenceNo": referenceNo,
        "referenceVoucherNo": referenceVoucherNo,
        "narration": narration,
        "voucherType": voucherType,
        "voucherStatus": voucherStatus,
      };
}
