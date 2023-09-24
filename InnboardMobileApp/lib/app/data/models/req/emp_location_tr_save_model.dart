import 'dart:convert';

EmpLocationTrSaveModel empLocationTrSaveModelFromJson(String str) =>
    EmpLocationTrSaveModel.fromJson(json.decode(str));

String empLocationTrSaveModelToJson(EmpLocationTrSaveModel data) =>
    json.encode(data.toJson());

class EmpLocationTrSaveModel {
  EmpLocationTrSaveModel({
    this.empId,
    this.latitude,
    this.longitude,
    this.attDateTime,
    this.deviceInfo,
    this.googleMapUrl,
    this.createdBy,
  });

  int? empId;
  double? latitude;
  double? longitude;
  DateTime? attDateTime;
  String? deviceInfo;
  String? googleMapUrl;
  int? createdBy;

  factory EmpLocationTrSaveModel.fromJson(Map<String, dynamic> json) =>
      EmpLocationTrSaveModel(
        empId: json["EmpId"],
        latitude: json["Latitude"].toDouble(),
        longitude: json["Longitude"].toDouble(),
        attDateTime: DateTime.parse(json["AttDateTime"]),
        deviceInfo: json["DeviceInfo"],
        googleMapUrl: json["GoogleMapUrl"],
        createdBy: json["CreatedBy"],
      );

  Map<String, dynamic> toJson() => {
        "EmpId": empId,
        "Latitude": latitude,
        "Longitude": longitude,
        "AttDateTime": attDateTime!.toIso8601String(),
        "DeviceInfo": deviceInfo,
        "GoogleMapUrl": googleMapUrl,
        "CreatedBy": createdBy,
      };
}
