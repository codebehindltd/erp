import 'dart:convert';

ProfileUpdateModel profileUpdateModelFromJson(String str) =>
    ProfileUpdateModel.fromJson(json.decode(str));

String profileUpdateModelToJson(ProfileUpdateModel data) =>
    json.encode(data.toJson());

class ProfileUpdateModel {
  String? transactionType;
  String? memberId;
  String? fullName;
  String? passportNumber;
  String? personalEmail;
  String? memberAddress;
  String? memberAppsProfilePicture;
  String? memberAppsProfilePictureByte;

  ProfileUpdateModel(
      {this.transactionType,
      this.memberId,
      this.fullName,
      this.passportNumber,
      this.personalEmail,
      this.memberAddress,
      this.memberAppsProfilePicture,
      this.memberAppsProfilePictureByte});

  factory ProfileUpdateModel.fromJson(Map<String, dynamic> json) =>
      ProfileUpdateModel(
          transactionType: json["TransactionType"],
          memberId: json["MemberId"],
          fullName: json["FullName"],
          passportNumber: json["PassportNumber"],
          personalEmail: json["PersonalEmail"],
          memberAddress: json["MemberAddress"],
          memberAppsProfilePicture: json["MemberAppsProfilePicture"],
          memberAppsProfilePictureByte: json["MemberAppsProfilePictureByte"]);

  Map<String, dynamic> toJson() => {
        "TransactionType": transactionType,
        "MemberId": memberId,
        "FullName": fullName,
        "PassportNumber": passportNumber,
        "PersonalEmail": personalEmail,
        "MemberAddress": memberAddress,
        "MemberAppsProfilePicture": memberAppsProfilePicture,
        "MemberAppsProfilePictureByte": memberAppsProfilePictureByte
      };
}
