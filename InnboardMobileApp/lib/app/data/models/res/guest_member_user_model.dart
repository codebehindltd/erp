import 'dart:convert';

GuestMemberUserModel guestMemberUserModelFromJson(String str) => GuestMemberUserModel.fromJson(json.decode(str));

String guestMemberUserModelToJson(GuestMemberUserModel data) => json.encode(data.toJson());

class GuestMemberUserModel {
    int? profileId;
    String? profileNumber;
    String? guestName;
    String? guestPhone;
    String? guestNationality;
    String? nationalId;
    String? passportNumber;
    String? guestEmail;
    String? guestAddress;
    String? countryName;
    String? imagePath;
    String? imageName;

    GuestMemberUserModel({
        this.profileId,
        this.profileNumber,
        this.guestName,
        this.guestPhone,
        this.guestNationality,
        this.nationalId,
        this.passportNumber,
        this.guestEmail,
        this.guestAddress,
        this.countryName,
        this.imagePath,
        this.imageName,
    });

    factory GuestMemberUserModel.fromJson(Map<String, dynamic> json) => GuestMemberUserModel(
        profileId: json["ProfileId"],
        profileNumber: json["ProfileNumber"],
        guestName: json["GuestName"],
        guestPhone: json["GuestPhone"],
        guestNationality: json["GuestNationality"],
        nationalId: json["NationalId"],
        passportNumber: json["PassportNumber"],
        guestEmail: json["GuestEmail"],
        guestAddress: json["GuestAddress"],
        countryName: json["CountryName"],
        imagePath: json["ImagePath"],
        imageName: json["ImageName"],
    );

    Map<String, dynamic> toJson() => {
        "ProfileId": profileId,
        "ProfileNumber": profileNumber,
        "GuestName": guestName,
        "GuestPhone": guestPhone,
        "GuestNationality": guestNationality,
        "NationalId": nationalId,
        "PassportNumber": passportNumber,
        "GuestEmail": guestEmail,
        "GuestAddress": guestAddress,
        "CountryName": countryName,
        "ImagePath": imagePath,
        "ImageName": imageName,
    };
}