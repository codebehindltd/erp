// To parse this JSON data, do
//
//     final leaveApplicationModel = leaveApplicationModelFromJson(jsonString);

import 'dart:convert';

LeaveApplicationModel leaveApplicationModelFromJson(String str) => LeaveApplicationModel.fromJson(json.decode(str));

String leaveApplicationModelToJson(LeaveApplicationModel data) => json.encode(data.toJson());

class LeaveApplicationModel {
    int? leaveId;
    int? empId;
    String? leaveMode;
    int? leaveTypeId;
    DateTime? fromDate;
    DateTime? toDate;
    int? noOfDays;
    String? reason;
    int? workHandover;
    int? createdBy;
    String? transactionType;
    String? expireDate;
    int? reportingTo;
    String? leaveStatus;

    LeaveApplicationModel({
        this.leaveId,
        this.empId,
        this.leaveMode,
        this.leaveTypeId,
        this.fromDate,
        this.toDate,
        this.noOfDays,
        this.reason,
        this.workHandover,
        this.createdBy,
        this.transactionType,
        this.expireDate,
        this.reportingTo,
        this.leaveStatus,
    });

    factory LeaveApplicationModel.fromJson(Map<String, dynamic> json) => LeaveApplicationModel(
        leaveId: json["LeaveId"],
        empId: json["EmpId"],
        leaveMode: json["LeaveMode"],
        leaveTypeId: json["LeaveTypeId"],
        fromDate: DateTime.parse(json["FromDate"]),
        toDate: DateTime.parse(json["ToDate"]),
        noOfDays: json["NoOfDays"],
        reason: json["Reason"],
        workHandover: json["WorkHandover"],
        createdBy: json["CreatedBy"],
        transactionType: json["TransactionType"],
        expireDate: json["ExpireDate"],
        reportingTo: json["ReportingTo"],
        leaveStatus: json["LeaveStatus"],
    );

    Map<String, dynamic> toJson() => {
        "LeaveId": leaveId,
        "EmpId": empId,
        "LeaveMode": leaveMode,
        "LeaveTypeId": leaveTypeId,
        "FromDate": fromDate?.toIso8601String(),
        "ToDate": toDate?.toIso8601String(),
        "NoOfDays": noOfDays,
        "Reason": reason,
        "WorkHandover": workHandover,
        "CreatedBy": createdBy,
        "TransactionType": transactionType,
        "ExpireDate": expireDate,
        "ReportingTo": reportingTo,
        "LeaveStatus": leaveStatus,
    };
}
