// To parse this JSON data, do
//
//     final LeaveCriteriaModel = LeaveCriteriaModelFromJson(jsonString);

import 'dart:convert';

LeaveCriteriaModel leaveCriteriaModelFromJson(String str) =>
    LeaveCriteriaModel.fromJson(json.decode(str));

String leaveCriteriaModelToJson(LeaveCriteriaModel data) =>
    json.encode(data.toJson());

class LeaveCriteriaModel {
  LeaveCriteriaModel(
      {required this.pageNumber,
      required this.pageSize,
      this.userInfoId,
      this.fromDate,
      this.toDate,
      this.employeeId,
      this.leaveStatus});

  int pageNumber;
  int pageSize;
  String? fromDate;
  String? toDate;
  String? userInfoId;
  String? employeeId;
  String? leaveStatus;

  factory LeaveCriteriaModel.fromJson(Map<String, dynamic> json) =>
      LeaveCriteriaModel(
        pageNumber: json["PageNumber"],
        pageSize: json["PageSize"],
        userInfoId: json["userInfoId"],
        fromDate: json["fromDate"],
        toDate: json["toDate"],
        employeeId: json["employeeId"],
        leaveStatus: json["leaveStatus"],
      );

  Map<String, dynamic> toJson() => {
        "PageNumber": pageNumber,
        "PageSize": pageSize,
        "userInfoId": userInfoId,
        "fromDate": fromDate,
        "toDate": toDate,
        "employeeId": employeeId,
        "leaveStatus": leaveStatus
      };
}
