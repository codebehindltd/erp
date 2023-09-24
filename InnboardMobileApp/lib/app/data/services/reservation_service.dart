import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../../core/environment/environment.dart';
import '../../core/utils/api_end_point.dart';
import '../models/req/reservation/Room_reservation_paymentInfo_save_model.dart';
import '../models/req/reservation/room_reservation_info_Model.dart';
import '../models/res/reservation/reservation_list_model.dart';
import '../models/res/reservation/reservation_res_model.dart';
import '../models/res/reservation/room_type_info_model.dart';

class ReservationService {
  static Future<List<RoomTypeInfoModel>> fetchRoomTypeInfo(
      String trType, String trId) async {
    var client = http.Client();
    try {
      var parameters = {
        'transactionType': trType,
        'transactionId': trId,
      };
      Uri url =
          Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.roomTypeInfo)
              .replace(queryParameters: parameters);

      final response = await client.get(url);
      if (response.statusCode == 200) {
        var model = roomTypeInfoModelFromJson(response.body);
        return model;
      } else {
        throw ("No data found");
      }
    } catch (e) {
      throw e.toString();
    } finally {
      client.close();
    }
  }

  static Future<int> checkAvailableRoomInfo(
      int roomTypeId, String dateFrom, String dateTo) async {
    var client = http.Client();
    try {
      var parameters = {'roomTypeId': roomTypeId.toString()};
      parameters.addAll({'dateFrom': dateFrom});
      parameters.addAll({'dateTo': dateTo});

      Uri url = Uri.parse(
              await Env.propertryApiUrl() + ApiEntPoint.checkRoomAvailable)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      var result = jsonDecode(response.body);
      if (response.statusCode == 200) {
        var data = int.parse(result);
        return data;
      } else {
        // //throw response.body[""];
        // var erro = jsonDecode(response.body);
        throw result['Message'];
      }
    } catch (e) {
      throw e.toString();
    } finally {
      client.close();
    }
  }

  static Future<ReservationResModel?> roomReservationInfoPost(
      RoomReservationInfoModel roomReservationInfoModel) async {
    var client = http.Client();
    try {
      roomReservationInfoModel.guestSourceId = 60;
      Uri url = Uri.parse(
          await Env.propertryApiUrl() + ApiEntPoint.roomReservationInfo);
      var body = jsonEncode(roomReservationInfoModel.toJson());
      final response = await client.post(url, body: body, headers: {
        "Accept": "application/json",
        "Content-Type": "application/json"
      });
      if (response.statusCode == 200) {
        return compute(reservationResModelFromJson, response.body);
      } else {
        var body = jsonDecode(response.body);
        String errorMsg = body['Message'];
        throw errorMsg;
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> customerPayment(
      RoomReservationPaymentInfoSaveModel
          roomReservationPaymentInfoSaveModel) async {
    var client = http.Client();
    try {
      Uri url = Uri.parse(await Env.propertryApiUrl() +
          ApiEntPoint.roomReservationPaymentInfoSave);
      final response = await client.post(url,
          body: jsonEncode(roomReservationPaymentInfoSaveModel.toJson()),
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          });
      if (response.statusCode == 200) {
        return true;
      } else {
        var body = jsonDecode(response.body);
        String errorMsg = body['Message'];
        throw errorMsg;
      }
    } finally {
      client.close();
    }
  }

  static Future<List<ReservationListModel>> getReservationList(
      String propertyId,
      String trType,
      String trId,
      String fromDate,
      String toDate) async {
    var client = http.Client();
    try {
      var parameters = {
        'propertyId': propertyId,
        'transactionType': trType,
        'transactionId': trId,
        'fromDate': fromDate,
        'toDate': toDate,
      };
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.reservationList)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return compute(reservationListModelFromJson, response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }
}
