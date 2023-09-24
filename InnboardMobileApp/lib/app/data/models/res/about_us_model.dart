

import 'dart:convert';

List<AppInfoModel> appInfoModelFromJson(String str) => List<AppInfoModel>.from(json.decode(str).map((x) => AppInfoModel.fromJson(x)));

String appInfoModelToJson(List<AppInfoModel> data) => json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class AppInfoModel {
    int? companyId;
    String? companyCode;
    String? companyName;
    String? groupCompanyName;
    String? companyAddress;
    String? emailAddress;
    String? webAddress;
    String? contactNumber;
    String? contactPerson;
    String? vatRegistrationNo;
    String? tinNumber;
    String? remarks;
    String? imageName;
    String? imagePath;
    String? companyType;
    dynamic telephone;
    dynamic hotLineNumber;
    dynamic fax;
    String? aboutUsDetail;

    AppInfoModel({
        this.companyId,
        this.companyCode,
        this.companyName,
        this.groupCompanyName,
        this.companyAddress,
        this.emailAddress,
        this.webAddress,
        this.contactNumber,
        this.contactPerson,
        this.vatRegistrationNo,
        this.tinNumber,
        this.remarks,
        this.imageName,
        this.imagePath,
        this.companyType,
        this.telephone,
        this.hotLineNumber,
        this.fax,
        this.aboutUsDetail,
    });

    factory AppInfoModel.fromJson(Map<String, dynamic> json) => AppInfoModel(
        companyId: json["CompanyId"],
        companyCode: json["CompanyCode"],
        companyName: json["CompanyName"],
        groupCompanyName: json["GroupCompanyName"],
        companyAddress: json["CompanyAddress"],
        emailAddress: json["EmailAddress"],
        webAddress: json["WebAddress"],
        contactNumber: json["ContactNumber"],
        contactPerson: json["ContactPerson"],
        vatRegistrationNo: json["VatRegistrationNo"],
        tinNumber: json["TinNumber"],
        remarks: json["Remarks"],
        imageName: json["ImageName"],
        imagePath: json["ImagePath"],
        companyType: json["CompanyType"],
        telephone: json["Telephone"],
        hotLineNumber: json["HotLineNumber"],
        fax: json["Fax"],
        aboutUsDetail: json["AboutUsDetail"],
    );

    Map<String, dynamic> toJson() => {
        "CompanyId": companyId,
        "CompanyCode": companyCode,
        "CompanyName": companyName,
        "GroupCompanyName": groupCompanyName,
        "CompanyAddress": companyAddress,
        "EmailAddress": emailAddress,
        "WebAddress": webAddress,
        "ContactNumber": contactNumber,
        "ContactPerson": contactPerson,
        "VatRegistrationNo": vatRegistrationNo,
        "TinNumber": tinNumber,
        "Remarks": remarks,
        "ImageName": imageName,
        "ImagePath": imagePath,
        "CompanyType": companyType,
        "Telephone": telephone,
        "HotLineNumber": hotLineNumber,
        "Fax": fax,
        "AboutUsDetail": aboutUsDetail,
    };
}
