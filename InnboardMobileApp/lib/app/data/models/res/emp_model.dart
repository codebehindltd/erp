
import 'dart:convert';

List<EmpModel> empListModelFromJson(String str) => List<EmpModel>.from(json.decode(str).map((x) => EmpModel.fromJson(x)));

EmpModel empModelFromJson(String str) => EmpModel.fromJson(json.decode(str));

String empListModelToJson(List<EmpModel> data) => json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class EmpModel {
    EmpModel({
        this.empId,
        this.empCode,
        this.imageUrl,
        this.firstName,
        this.lastName,
        this.displayName,
        this.permanentAddress,
        this.presentAddress,
        this.presentPhone,
        this.departmentName,
        this.designationName,
        this.latitude,
        this.longitude,
        this.trackingAttDateTime,
        this.trackingCreatedDate,
        this.deviceInfo,
        this.googleMapUrl,
    });

    int? empId;
    String? empCode;
    String? imageUrl;
    String? firstName;
    String? lastName;
    String? displayName;
    String? permanentAddress;
    String? presentAddress;
    String? presentPhone;
    String? departmentName;
    String? designationName;
    double? latitude;
    double? longitude;
    String? trackingAttDateTime;
    String? trackingCreatedDate;
    String? deviceInfo;
    String? googleMapUrl;

    factory EmpModel.fromJson(Map<String, dynamic> json) => EmpModel(
        empId: json["EmpId"],
        empCode: json["EmpCode"],
        imageUrl: json["ImageUrl"],
        firstName: json["FirstName"],
        lastName: json["LastName"],
        displayName: json["DisplayName"],
        permanentAddress: json["PermanentAddress"],
        presentAddress: json["PresentAddress"],
        presentPhone: json["PresentPhone"],
        departmentName: json["DepartmentName"],
        designationName: json["DesignationName"],
        latitude: json["Latitude"]?.toDouble(),
        longitude: json["Longitude"]?.toDouble(),
        trackingAttDateTime: json["TrackingAttDateTime"],
        trackingCreatedDate: json["TrackingCreatedDate"],
        deviceInfo: json["DeviceInfo"],
        googleMapUrl: json["GoogleMapUrl"],
    );

    Map<String, dynamic> toJson() => {
        "EmpId": empId,
        "EmpCode": empCode,
        "ImageUrl": imageUrl,
        "FirstName": firstName,
        "LastName": lastName,
        "DisplayName": displayName,
        "PermanentAddress": permanentAddress,
        "PresentAddress": presentAddress,
        "PresentPhone": presentPhone,
        "DepartmentName": departmentName,
        "DesignationName": designationName,
        "Latitude": latitude,
        "Longitude": longitude,
        "TrackingAttDateTime": trackingAttDateTime,
        "TrackingCreatedDate": trackingCreatedDate,
        "DeviceInfo": deviceInfo,
        "GoogleMapUrl": googleMapUrl,
    };
}
