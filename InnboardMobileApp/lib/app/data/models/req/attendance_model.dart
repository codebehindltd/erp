import 'dart:convert';

AttendanceModel attendanceModelFromJson(String str) =>
    AttendanceModel.fromJson(json.decode(str));

String attendanceModelToJson(AttendanceModel data) =>
    json.encode(data.toJson());

class AttendanceModel {
  AttendanceModel({
    this.id,
    this.empId,
    this.imageByte,
    this.imageName,
    this.latitude,
    this.longitude,
    this.attDateTime,
    this.address,
    this.attendanceDate,
    this.entryTime,
    this.exitTime,
    this.userInfoId,
    this.remark,
  });

  int? id;
  int? empId;
  String? imageByte;
  String? imageName;
  double? latitude;
  double? longitude;
  String? attDateTime;
  String? address;
  DateTime? attendanceDate;
  DateTime? entryTime;
  DateTime? exitTime;
  int? userInfoId;
  String? remark;

  factory AttendanceModel.fromJson(Map<String, dynamic> json) =>
      AttendanceModel(
        id: json["Id"],
        empId: json["EmpId"],
        imageByte: json["ImageByte"],
        imageName: json["ImageName"],
        latitude: json["Latitude"].toDouble(),
        longitude: json["Longitude"].toDouble(),
        attDateTime: json["AttDateTime"],
        address: json["Address"],
        attendanceDate: DateTime.parse(json["AttendanceDate"]),
        entryTime: DateTime.parse(json["EntryTime"]),
        exitTime: DateTime.parse(json["ExitTime"]),
        userInfoId: json["UserInfoId"],
        remark: json["Remark"],
      );

  Map<String, dynamic> toJson() => {
        "Id": id,
        "EmpId": empId,
        "ImageByte": imageByte,
        "ImageName": imageName,
        "Latitude": latitude,
        "Longitude": longitude,
        "AttDateTime": attDateTime,
        "Address": address,
        "AttendanceDate":
            attendanceDate == null ? null : attendanceDate!.toIso8601String(),
        "EntryTime": entryTime == null ? null : entryTime!.toIso8601String(),
        "ExitTime": exitTime == null ? null : exitTime!.toIso8601String(),
        "UserInfoId": userInfoId,
        "Remark": remark,
      };
}
