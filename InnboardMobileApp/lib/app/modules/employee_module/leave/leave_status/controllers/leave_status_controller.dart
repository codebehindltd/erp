import 'dart:convert';

import 'package:flutter/widgets.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/services/emp_service.dart';

import '../../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../../data/models/req/leave_criteria_model.dart';
import '../../../../../data/models/res/leave/leave_application_list_model.dart';
import '../../../../common/controllers/common_controller.dart';

class LeaveStatusController extends GetxController {
  final commonController = Get.find<CommonController>();
  TextEditingController searchKeyController = TextEditingController();
  LeaveCriteriaModel leaveCriteriaModel = LeaveCriteriaModel(
      pageNumber: 1,
      pageSize: 10,
      fromDate: DateTime.now().subtract(const Duration(days: 1)).toString(),
      toDate: DateTime.now().toString());
  List<LeaveApplicationListModel> leaveList = [];
  bool isLoading = true;
  @override
  void onInit() {
    leaveCriteriaModel.userInfoId = commonController.propertyUserInfoId.toString();
    getInfo();
    super.onInit();
  }

  getInfo() async {
    var user = await SharedPfnDBHelper.getPropertyUserData();
    leaveCriteriaModel.employeeId = user!.empId.toString();
    getLeaveInformation();
  }

  @override
  void onReady() {
    super.onReady();
  }

  @override
  void onClose() {
    super.onClose();
  }

  searchEmp(v) {}

  leaveFilterApply(v) {
    leaveCriteriaModel = v;
    isLoading = true;
    update();
    getLeaveInformation();
    print(jsonEncode(v));
    update();
  }

  getLeaveInformation() {
    EmpService.getLeaveInformation(leaveCriteriaModel).then((value) {
      leaveList = value;
      isLoading = false;
      update();
    }).onError((error, stackTrace) {
      leaveList = [];
      isLoading = false;
      update();
    });
  }
}
