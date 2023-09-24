import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:leading_edge/app/data/models/res/user_model.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../models/res/property_model.dart';

class SharedPfnDBHelper {
  static Future<SharedPreferences> prefs = SharedPreferences.getInstance();

  static Future<bool> saveLoginUserData(String data) async {
    bool result =
        await prefs.then((value) => value.setString("userData", data));
    return result;
  }

  static Future<UserModel?> getLoginUserData() async {
    var result = await prefs.then((value) => value.getString("userData"));
    if (result != null && result != "") {
      var data = parseUserInfo(result);
      return data;
    } else {
      return null;
    }
  }

  static removeLoginUserData() {
    prefs.then((value) => value.remove("userData"));
    prefs.then((value) => value.remove("pUserData"));
    prefs.then((value) => value.remove("prUrl"));
    //prefs.then((value) => value.remove("userType"));
    prefs.then((value) => value.remove("guestOrMemberId"));
  }

  static Future<bool> savePropertyUserData(String data) async {
    bool result =
        await prefs.then((value) => value.setString("pUserData", data));
    return result;
  }

  static Future<UserModel?> getPropertyUserData() async {
    var result = await prefs.then((value) => value.getString("pUserData"));
    if (result != null && result != "") {
      var data = parseUserInfo(result);
      return data;
    } else {
      return null;
    }
  }

  static UserModel parseUserInfo(String data) {
    var resultdata = jsonDecode(data) as dynamic;
    UserModel userInfo = UserModel.fromJson(resultdata);
    return userInfo;
  }

  //save remember_me data
  static Future<bool> saveCredentialData(String data) async {
    bool result =
        await prefs.then((value) => value.setString("credential", data));
    return result;
  }

  static Future<String?> getCredentialData() async {
    String? result = await prefs.then((value) => value.getString("credential"));
    return result;
  }

  static Future<bool> savePropertyUrl(String data) async {
    bool result = await prefs.then((value) => value.setString("prUrl", data));
    return result;
  }

  static Future<String?> getPropertyUrl() async {
    var result = await prefs.then((value) => value.getString("prUrl"));
    return result;
  }

  static Future<bool> saveProperty(PropertyModel data) async {
    var jsonData = await compute(singlePropertyModelToJson, data);
    bool result =
        await prefs.then((value) => value.setString("property", jsonData));
    return result;
  }

  static Future<PropertyModel> getProperty() async {
    var result = await prefs.then((value) => value.getString("property"));
    var resultdata = jsonDecode(result ?? '') as dynamic;
    PropertyModel data = PropertyModel.fromJson(resultdata);
    return data;
  }

  static Future<bool> removePropertyUrl() async {
    return await prefs.then((value) => value.remove("prUrl"));
  }

  static Future<bool> saveUserType(String data) async {
    //await removeUserType();
    bool result =
        await prefs.then((value) => value.setString("userType", data));
    return result;
  }

  static Future<String?> getUserType() async {
    var result = await prefs.then((value) => value.getString("userType"));
    return result;
  }

  static Future<bool> removeUserType() async {
    return await prefs.then((value) => value.remove("userType"));
  }

  static Future<bool> saveGuestOrMemberId(String data) async {
    bool result =
        await prefs.then((value) => value.setString("guestOrMemberId", data));
    return result;
  }

  static Future<String?> getGuestOrMemberId() async {
    return await prefs.then((value) => value.getString("guestOrMemberId"));
  }
}
