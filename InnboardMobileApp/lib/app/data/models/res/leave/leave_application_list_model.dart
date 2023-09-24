// To parse this JSON data, do
//
//     final leaveApplicationListModel = leaveApplicationListModelFromJson(jsonString);

import 'dart:convert';

List<LeaveApplicationListModel> leaveApplicationListModelFromJson(String str) =>
    List<LeaveApplicationListModel>.from(
        json.decode(str).map((x) => LeaveApplicationListModel.fromJson(x)));

String leaveApplicationListModelToJson(List<LeaveApplicationListModel> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class LeaveApplicationListModel {
  int? leaveId;
  int? empId;
  dynamic employeeId;
  String? empCode;
  String? leaveMode;
  String? employeeName;
  int? leaveTypeId;
  dynamic leaveType;
  String? typeName;
  DateTime? fromDate;
  DateTime? toDate;
  dynamic leaveFromDate;
  dynamic leaveToDate;
  int? noOfDays;
  String? reason;
  int? reportingTo;
  int? workHandover;
  String? workHandoverStatus;
  int? designationId;
  dynamic designation;
  dynamic reportingToDesignation;
  String? createdDateString;
  int? createdBy;
  int? lastModifiedBy;
  dynamic transactionType;
  dynamic expireDate;
  int? departmentId;
  dynamic departmentName;
  int? taken;
  int? available;
  dynamic month;
  int? commulativeLeave;
  dynamic showCreatedDate;
  dynamic showFromDate;
  dynamic showToDate;
  dynamic joinDate;
  dynamic yearlyLeave;
  dynamic monthId;
  dynamic leaveMonth;
  dynamic leaveTaken;
  dynamic totalLeaveTaken;
  dynamic leaveBalance;
  double? openingLeave;
  int? leaveModeId;
  dynamic checkedByName;
  dynamic approvedByName;
  dynamic checkedBy;
  dynamic checkedDate;
  dynamic approvedBy;
  dynamic approvedDate;
  String? leaveStatus;
  dynamic cancelReason;
  dynamic pageParams;
  bool? isCanEdit;
  bool? isCanDelete;
  bool? isCanCheck;
  bool? isCanApprove;
  String? profilePictureUrl;

  LeaveApplicationListModel(
      {this.leaveId,
      this.empId,
      this.employeeId,
      this.empCode,
      this.leaveMode,
      this.employeeName,
      this.leaveTypeId,
      this.leaveType,
      this.typeName,
      this.fromDate,
      this.toDate,
      this.leaveFromDate,
      this.leaveToDate,
      this.noOfDays,
      this.reason,
      this.reportingTo,
      this.workHandover,
      this.workHandoverStatus,
      this.designationId,
      this.designation,
      this.reportingToDesignation,
      this.createdDateString,
      this.createdBy,
      this.lastModifiedBy,
      this.transactionType,
      this.expireDate,
      this.departmentId,
      this.departmentName,
      this.taken,
      this.available,
      this.month,
      this.commulativeLeave,
      this.showCreatedDate,
      this.showFromDate,
      this.showToDate,
      this.joinDate,
      this.yearlyLeave,
      this.monthId,
      this.leaveMonth,
      this.leaveTaken,
      this.totalLeaveTaken,
      this.leaveBalance,
      this.openingLeave,
      this.leaveModeId,
      this.checkedByName,
      this.approvedByName,
      this.checkedBy,
      this.checkedDate,
      this.approvedBy,
      this.approvedDate,
      this.leaveStatus,
      this.cancelReason,
      this.pageParams,
      this.isCanEdit,
      this.isCanDelete,
      this.isCanCheck,
      this.isCanApprove,
      this.profilePictureUrl});

  factory LeaveApplicationListModel.fromJson(Map<String, dynamic> json) =>
      LeaveApplicationListModel(
          leaveId: json["LeaveId"],
          empId: json["EmpId"],
          employeeId: json["EmployeeId"],
          empCode: json["EmpCode"],
          leaveMode: json["LeaveMode"],
          employeeName: json["EmployeeName"],
          leaveTypeId: json["LeaveTypeId"],
          leaveType: json["LeaveType"],
          typeName: json["TypeName"],
          fromDate: DateTime.parse(json["FromDate"]),
          toDate: DateTime.parse(json["ToDate"]),
          leaveFromDate: json["LeaveFromDate"],
          leaveToDate: json["LeaveToDate"],
          noOfDays: json["NoOfDays"],
          reason: json["Reason"],
          reportingTo: json["ReportingTo"],
          workHandover: json["WorkHandover"],
          workHandoverStatus: json["WorkHandoverStatus"],
          designationId: json["DesignationId"],
          designation: json["Designation"],
          reportingToDesignation: json["ReportingToDesignation"],
          createdDateString: json["CreatedDateString"],
          createdBy: json["CreatedBy"],
          lastModifiedBy: json["LastModifiedBy"],
          transactionType: json["TransactionType"],
          expireDate: json["ExpireDate"],
          departmentId: json["DepartmentId"],
          departmentName: json["DepartmentName"],
          taken: json["Taken"],
          available: json["Available"],
          month: json["Month"],
          commulativeLeave: json["CommulativeLeave"],
          showCreatedDate: json["ShowCreatedDate"],
          showFromDate: json["ShowFromDate"],
          showToDate: json["ShowToDate"],
          joinDate: json["JoinDate"],
          yearlyLeave: json["YearlyLeave"],
          monthId: json["MonthId"],
          leaveMonth: json["LeaveMonth"],
          leaveTaken: json["LeaveTaken"],
          totalLeaveTaken: json["TotalLeaveTaken"],
          leaveBalance: json["LeaveBalance"],
          openingLeave: json["OpeningLeave"],
          leaveModeId: json["LeaveModeId"],
          checkedByName: json["CheckedByName"],
          approvedByName: json["ApprovedByName"],
          checkedBy: json["CheckedBy"],
          checkedDate: json["CheckedDate"],
          approvedBy: json["ApprovedBy"],
          approvedDate: json["ApprovedDate"],
          leaveStatus: json["LeaveStatus"],
          cancelReason: json["CancelReason"],
          pageParams: json["pageParams"],
          isCanEdit: json["IsCanEdit"],
          isCanDelete: json["IsCanDelete"],
          isCanCheck: json["IsCanCheck"],
          isCanApprove: json["IsCanApprove"],
          profilePictureUrl: json["ProfilePictureUrl"]);

  Map<String, dynamic> toJson() => {
        "LeaveId": leaveId,
        "EmpId": empId,
        "EmployeeId": employeeId,
        "EmpCode": empCode,
        "LeaveMode": leaveMode,
        "EmployeeName": employeeName,
        "LeaveTypeId": leaveTypeId,
        "LeaveType": leaveType,
        "TypeName": typeName,
        "FromDate": fromDate?.toIso8601String(),
        "ToDate": toDate?.toIso8601String(),
        "LeaveFromDate": leaveFromDate,
        "LeaveToDate": leaveToDate,
        "NoOfDays": noOfDays,
        "Reason": reason,
        "ReportingTo": reportingTo,
        "WorkHandover": workHandover,
        "WorkHandoverStatus": workHandoverStatus,
        "DesignationId": designationId,
        "Designation": designation,
        "ReportingToDesignation": reportingToDesignation,
        "CreatedDateString": createdDateString,
        "CreatedBy": createdBy,
        "LastModifiedBy": lastModifiedBy,
        "TransactionType": transactionType,
        "ExpireDate": expireDate,
        "DepartmentId": departmentId,
        "DepartmentName": departmentName,
        "Taken": taken,
        "Available": available,
        "Month": month,
        "CommulativeLeave": commulativeLeave,
        "ShowCreatedDate": showCreatedDate,
        "ShowFromDate": showFromDate,
        "ShowToDate": showToDate,
        "JoinDate": joinDate,
        "YearlyLeave": yearlyLeave,
        "MonthId": monthId,
        "LeaveMonth": leaveMonth,
        "LeaveTaken": leaveTaken,
        "TotalLeaveTaken": totalLeaveTaken,
        "LeaveBalance": leaveBalance,
        "OpeningLeave": openingLeave,
        "LeaveModeId": leaveModeId,
        "CheckedByName": checkedByName,
        "ApprovedByName": approvedByName,
        "CheckedBy": checkedBy,
        "CheckedDate": checkedDate,
        "ApprovedBy": approvedBy,
        "ApprovedDate": approvedDate,
        "LeaveStatus": leaveStatus,
        "CancelReason": cancelReason,
        "pageParams": pageParams,
        "IsCanEdit": isCanEdit,
        "IsCanDelete": isCanDelete,
        "IsCanCheck": isCanCheck,
        "IsCanApprove": isCanApprove,
        "ProfilePictureUrl": profilePictureUrl
      };
}
