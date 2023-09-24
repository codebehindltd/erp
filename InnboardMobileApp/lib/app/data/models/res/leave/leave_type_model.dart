// To parse this JSON data, do
//
//     final leaveTypeModel = leaveTypeModelFromJson(jsonString);

import 'dart:convert';

List<LeaveTypeModel> leaveTypeModelFromJson(String str) =>
    List<LeaveTypeModel>.from(
        json.decode(str).map((x) => LeaveTypeModel.fromJson(x)));

String leaveTypeModelToJson(List<LeaveTypeModel> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class LeaveTypeModel {
  int? empId;
  dynamic empCode;
  dynamic employeeFullName;
  int? leaveTypeId;
  String? leaveTypeName;
  dynamic totalLeave;
  int? totalTakenLeave;
  int? remainingLeave;

  LeaveTypeModel({
    this.empId,
    this.empCode,
    this.employeeFullName,
    this.leaveTypeId,
    this.leaveTypeName,
    this.totalLeave,
    this.totalTakenLeave,
    this.remainingLeave,
  });

  factory LeaveTypeModel.fromJson(Map<String, dynamic> json) => LeaveTypeModel(
        empId: json["EmpId"],
        empCode: json["EmpCode"],
        employeeFullName: json["EmployeeFullName"],
        leaveTypeId: json["LeaveTypeID"],
        leaveTypeName: json["LeaveTypeName"],
        totalLeave: json["TotalLeave"],
        totalTakenLeave: json["TotalTakenLeave"],
        remainingLeave: json["RemainingLeave"],
      );

  Map<String, dynamic> toJson() => {
        "EmpId": empId,
        "EmpCode": empCode,
        "EmployeeFullName": employeeFullName,
        "LeaveTypeID": leaveTypeId,
        "LeaveTypeName": leaveTypeName,
        "TotalLeave": totalLeave,
        "TotalTakenLeave": totalTakenLeave,
        "RemainingLeave": remainingLeave,
      };
}
