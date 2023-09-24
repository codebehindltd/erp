import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:innboard/app/core/environment/environment.dart';
import 'package:innboard/app/core/utils/api_end_point.dart';
import 'package:innboard/app/data/models/req/attendance_model.dart';
import 'package:innboard/app/data/models/req/user_login_model.dart';
import 'package:innboard/app/data/models/res/user_model.dart';

class AccountService {
  static Future<UserModel> loginUser(UserLoginModel login) async {
    var client = http.Client();
    try {
      var apiUrl = Uri.parse(Env.rootApiUrl + ApiEntPoint.appLogin);
      final response = await client.post(
        apiUrl,
        body: jsonEncode(login.toJson()),
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json"
        },
      );
      if (response.statusCode == 200 || response.statusCode == 201) {
        var data = jsonDecode(response.body);
        return UserModel.fromJson(data);
      } else {
        var body = jsonDecode(response.body);
        String errorMsg = body['Message'];
        throw errorMsg;
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> attendancePost(AttendanceModel model) async {
    var client = http.Client();
    try {
      var apiUrl =
          Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.attendance);
      final response = await client.post(
        apiUrl,
        body: jsonEncode(model.toJson()),
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json"
        },
      );
      if (response.statusCode == 200 || response.statusCode == 201) {
        return true;
      } else {
        return false;
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> attendanceApplicationPost(AttendanceModel model) async {
    var client = http.Client();
    try {
      var apiUrl = Uri.parse(
          await Env.propertryApiUrl() + ApiEntPoint.attendanceApplication);
      final response = await client.post(
        apiUrl,
        body: jsonEncode(model.toJson()),
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json"
        },
      );
      if (response.statusCode == 200 || response.statusCode == 201) {
        return true;
      } else {
        return false;
      }
    } finally {
      client.close();
    }
  }

  static Future<UserModel> loginUserByMasterUserInfo(int masterUserId) async {
    var client = http.Client();
    try {
      var parameters = {"masterUserInfoId": masterUserId.toString()};
      var apiUrl =
          Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.selectProperty)
              .replace(queryParameters: parameters);
      final response = await client.post(
        apiUrl,
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json"
        },
      );
      if (response.statusCode == 200 || response.statusCode == 201) {
        var data = jsonDecode(response.body);
        return UserModel.fromJson(data);
      } else {
        var body = jsonDecode(response.body);
        String errorMsg = body['Message'];
        throw errorMsg;
      }
    } catch (e) {
      throw "Something wrong happened";
    } finally {
      client.close();
    }
  }


  static Future<UserModel> guestOrMemberLogin(String userType, String userName, String password) async {
    var client = http.Client();
    try {
      var parameters = {"userType": userType, "userId": userName, "userPassword": password};
      var apiUrl =
          Uri.parse(Env.rootApiUrl + ApiEntPoint.guestOrMemberLogin)
              .replace(queryParameters: parameters);
      final response = await client.post(
        apiUrl,
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json"
        },
      );
      if (response.statusCode == 200 || response.statusCode == 201) {
        // var data = jsonDecode(response.body);
        // return UserModel.fromJson(data);
        return compute(userModelFromJson, response.body);
      } else {
        var body = jsonDecode(response.body);
        String errorMsg = body['Message'];
        throw errorMsg;
      }
    }  finally {
      client.close();
    }
  }
}
