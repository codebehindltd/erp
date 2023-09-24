import 'dart:convert';

import 'package:leading_edge/app/core/environment/environment.dart';
import 'package:leading_edge/app/core/utils/api_end_point.dart';
import 'package:leading_edge/app/data/models/req/emp_location_tr_save_model.dart';
import 'package:leading_edge/app/data/models/res/emp_model.dart';
import 'package:http/http.dart' as http;

import '../../core/values/static_value.dart';
import '../models/req/leave_application_model.dart';
import '../models/req/leave_criteria_model.dart';
import '../models/res/leave/leave_application_list_model.dart';
import '../models/res/leave/leave_type_model.dart';

class EmpService {
  static Future<List<EmpModel>> getEmpList(
      {required int userInfoId,
      required int pageNumber,
      int? pageSize,
      String? searchKey}) async {
    var client = http.Client();

    try {
      var parameters = {'pageParams.PageNumber': pageNumber.toString()};
      if (pageSize != null) {
        parameters.addAll({'UserInfoId': userInfoId.toString()});
      }

      if (pageSize != null) {
        parameters.addAll({'pageParams.PageSize': pageSize.toString()});
      }
      if (searchKey != null) {
        parameters.addAll({'pageParams.SearchKey': searchKey.toString()});
      }
      Uri url = Uri.parse(
              await Env.propertryApiUrl() + ApiEntPoint.empInfoWithLocation)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return empListModelFromJson(response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<http.Response> saveEmpLocationTracking(
      EmpLocationTrSaveModel model) async {
    var client = http.Client();
    try {
      Uri url =
          Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.empLocationSave);
      final response = await client.post(url,
          body: empLocationTrSaveModelToJson(model),
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          }).timeout(const Duration(seconds: StaticValue.bgServiceCkInSecond));
      return response;
    } finally {
      client.close();
    }
  }

  static Future<List<LeaveTypeModel>> getLeaveType({required int empId}) async {
    var client = http.Client();

    try {
      DateTime currentDate = DateTime.now();
      var parameters = {'EmpId': empId.toString()};
      parameters.addAll({'CurrentDate': currentDate.toString()});

      Uri url = Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.leaveType)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return leaveTypeModelFromJson(response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<List<EmpModel>> getActiveEmp() async {
    var client = http.Client();
    try {
      Uri url = Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.activeEmp);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        return empListModelFromJson(response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> leaveApplication(LeaveApplicationModel model) async {
    var client = http.Client();
    try {
      Uri url =
          Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.leaveApplication);
      final response = await client.post(url,
          body: jsonEncode(model.toJson()),
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          });
      if (response.statusCode == 200) {
        return true;
      } else {
        var erro = jsonDecode(response.body);
        throw erro['Message'];
      }
    } finally {
      client.close();
    }
  }

  static Future<List<LeaveApplicationListModel>> getLeaveInformation(
      LeaveCriteriaModel criteriaModel) async {
    var client = http.Client();
    try {
      var parameters = {
        'pageParams.PageNumber': criteriaModel.pageNumber.toString()
      };
      if (criteriaModel.fromDate != null) {
        parameters.addAll({'FromDate': criteriaModel.fromDate.toString()});
      }
      if (criteriaModel.toDate != null) {
        parameters.addAll({'ToDate': criteriaModel.toDate.toString()});
      }
      if (criteriaModel.userInfoId != null) {
        parameters.addAll({'CreatedBy': criteriaModel.userInfoId.toString()});
      }
      if (criteriaModel.employeeId != null) {
        parameters.addAll({'EmployeeId': criteriaModel.employeeId.toString()});
      }
      Uri url = Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.leaveList)
          .replace(queryParameters: parameters);
      final response = await client.get(url);
      print(parameters);
      print(response.body);
      if (response.statusCode == 200) {
        return leaveApplicationListModelFromJson(response.body);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<LeaveApplicationListModel?> getLeaveInformationById(
      {required String userId, required String leaveId}) async {
    var client = http.Client();
    try {
      var parameters = {'leaveId': leaveId};
      parameters.addAll({'userId': userId});
      Uri url =
          Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.leaveGetById)
              .replace(queryParameters: parameters);
      final response = await client.get(url);
      print(parameters);
      print(response.body);
      if (response.statusCode == 200) {
        return leaveApplicationListModelFromJson(response.body).first;
      } else {
        return null;
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> leaveApproved(
      int userId, int leaveId, String approvalStatus) async {
    var client = http.Client();
    try {
      var data = {
        "LastModifiedBy": userId,
        "LeaveId": leaveId,
        "LeaveStatus": approvalStatus
      };
      Uri url =
          Uri.parse(await Env.propertryApiUrl() + ApiEntPoint.leaveApproval);
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
