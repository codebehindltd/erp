import 'dart:convert';

CompanyModel companyModelFromJson(String str) => CompanyModel.fromJson(json.decode(str));

String companyModelToJson(CompanyModel data) => json.encode(data.toJson());

class CompanyModel {
    CompanyModel({
        this.id,
        this.name,
        this.code,
    });

    int? id;
    String? name;
    String? code;

    factory CompanyModel.fromJson(Map<String, dynamic> json) => CompanyModel(
        id: json["Id"],
        name: json["Name"],
        code: json["Code"],
    );

    Map<String, dynamic> toJson() => {
        "Id": id,
        "Name": name,
        "Code": code,
    };
}
