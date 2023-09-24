import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;

import '../../core/environment/environment.dart';
import '../../core/utils/api_end_point.dart';
import '../models/req/profile_update_model.dart';
import '../models/res/about_us_model.dart';
import '../models/res/guest_member_user_model.dart';
import '../models/res/promotional_offer.dart';
import '../models/res/property_model.dart';
import '../models/res/user_type_model.dart';

class CommonService {
  static Future<List<UserTypeModel>> getUserTypes() async {
    var client = http.Client();

    try {
      var parameters = {'fieldType': "MobileAppsFeatures"};

      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.getUserType)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return compute(userTypeModelFromJson, response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<List<PropertyModel>> getPropertyList(String userType) async {
    var client = http.Client();
    try {
      var parameters = {'transactionType': userType, 'transactionId': '1'};
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.getProperty)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return compute(propertyModelFromJson, response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<List<AppInfoModel>> fetchAppInfo() async {
    var client = http.Client();
    try {
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.aboutUs);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return compute(appInfoModelFromJson, response.body);
      } else {
        throw ("No data found");
      }
    } catch (e) {
      throw e.toString();
    } finally {
      client.close();
    }
  }

  static Future<GuestMemberUserModel> getGuestOrMemberProfile(
      String type, String id) async {
    var client = http.Client();
    try {
      var parameters = {'transactionType': type, 'transactionId': id};
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.guestOrMemberProfile)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return compute(guestMemberUserModelFromJson, response.body);
      } else {
        throw "No data found";
      }
    } finally {
      client.close();
    }
  }

  static Future<List<PromotionalOfferModel>> getGuestOrMemberOffer(
      String type, String id) async {
    var client = http.Client();
    try {
      var parameters = {'transactionType': type, 'transactionId': id};
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.guestOrMemberOffer)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return compute(promotionalOfferModelFromJson, response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> profileUpdate(ProfileUpdateModel model) async {
    var client = http.Client();
    try {
      var apiUrl = Uri.parse(Env.rootApiUrl + ApiEntPoint.profileUpdate);
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

  static Future<List<PromotionalOfferModel>> getMemberRoomNightInfo(
      String userType, String memId) async {
    var client = http.Client();
    try {
      var parameters = {'transactionType': userType, 'transactionId': memId};
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.memberRoomNightInfo)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return compute(promotionalOfferModelFromJson, response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }
}
