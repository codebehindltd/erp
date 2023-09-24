
import 'dart:convert';

UserLoginModel userLoginModelFromJson(String str) => UserLoginModel.fromJson(json.decode(str));

String userLoginModelToJson(UserLoginModel data) => json.encode(data.toJson());

class UserLoginModel {
    UserLoginModel({
        this.userName,
        this.password,
    });

    String? userName;
    String? password;

    factory UserLoginModel.fromJson(Map<String, dynamic> json) => UserLoginModel(
        userName: json["UserName"],
        password: json["Password"],
    );

    Map<String, dynamic> toJson() => {
        "UserName": userName,
        "Password": password,
    };
}