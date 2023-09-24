import 'dart:convert';

import 'package:http/http.dart' as http;

import '../../core/environment/environment.dart';
import '../../core/utils/api_end_point.dart';
import '../models/req/voucher_criteria_model.dart';
import '../models/res/voucher_list_model.dart';

class GeneralLedgerService {
  static Future<List<VoucherResModel>> getVoucherList(
      VoucherCriteriaModel model) async {
    var client = http.Client();

    try {
      var parameters = {'pageParams.PageNumber': model.pageNumber.toString()};
      parameters.addAll({'pageParams.PageSize': model.pageSize.toString()});
      parameters.addAll({'companyId': model.companyId.toString()});
      parameters.addAll({'projectId': model.projectId.toString()});
      parameters.addAll({'userGroupId': model.userGroupId.toString()});
      parameters.addAll({'userInfoId': model.userInfoId.toString()});

      if (model.fromDate != null && model.fromDate!.isNotEmpty) {
        parameters.addAll({'fromDate': model.fromDate.toString()});
      }
      if (model.toDate != null && model.toDate!.isNotEmpty) {
        parameters.addAll({'toDate': model.toDate.toString()});
      }

      if (model.voucherNo != null && model.voucherNo!.isNotEmpty) {
        parameters.addAll({'voucherNo': model.voucherNo.toString()});
      }

      Uri url = Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.voucherListGet)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return voucherResModelFromJson(response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<List<VoucherResModel>> getVoucherInformation(
      int userId, int ledgerMasterId) async {
    var client = http.Client();

    try {
      var parameters = {'ledgerMasterId': ledgerMasterId.toString()};
      parameters.addAll({'userId': userId.toString()});
      Uri url = Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.voucherInfo)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      print(response.body);
      if (response.statusCode == 200) {
        return voucherResModelFromJson(response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> voucherApproved(
      int userId, int ledgerMasterId, String approvalStatus) async {
    var client = http.Client();
    try {
      var data = {
        "UserInfoId": userId,
        "LedgerMasterId": ledgerMasterId,
        "GLStatus": approvalStatus
      };
      Uri url = Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.voucherApproved);
      final response = await client.post(url, body: jsonEncode(data), headers: {
        "Accept": "application/json",
        "Content-Type": "application/json"
      });
      if (response.statusCode == 200) {
        return true;
      } else {
        return false;
      }
    } finally {
      client.close();
    }
  }
}
