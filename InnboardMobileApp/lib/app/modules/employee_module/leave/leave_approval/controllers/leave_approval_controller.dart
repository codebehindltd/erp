import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/services/emp_service.dart';

import '../../../../../core/values/colors.dart';
import '../../../../../data/models/res/leave/leave_application_list_model.dart';
import '../../../../common/controllers/common_controller.dart';

class LeaveApprovalController extends GetxController {
  LeaveApplicationListModel? leaveData;
  final commonController = Get.find<CommonController>();
  bool isLoading = true;
  int? userId;
  int? leaveId;
  @override
  void onInit() {
    userId = commonController.propertyUserInfoId;
    leaveId = Get.arguments;
    getLeaveInfo();
    super.onInit();
  }

  @override
  void onReady() {
    super.onReady();
  }

  @override
  void onClose() {
    super.onClose();
  }

  Future<void> getLeaveInfo() async {
    try {
      leaveData = await EmpService.getLeaveInformationById(
          userId: userId!.toString(), leaveId: leaveId!.toString());
      isLoading = false;
      update();
    } catch (e) {
      isLoading = false;
      leaveData = null;
      update();
    }
  }

  approved(v) async {
    try {
      EasyLoading.show(status: 'Loading');
      var result = await EmpService.leaveApproved(userId!, leaveId!, "Approve");

      if (result) {
        Get.snackbar("Success!", "Approved successfully done.",
            snackPosition: SnackPosition.BOTTOM);
        getLeaveInfo();
      } else {
        Get.snackbar(
          "Error!",
          "Approve failed.",
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: warningColor,
        );
      }
      EasyLoading.dismiss();
    } catch (e) {
      EasyLoading.dismiss();
    }
  }

  checked(v) async {
    try {
      EasyLoading.show(status: 'Loading');
      var result = await EmpService.leaveApproved(userId!, leaveId!, "Check");

      if (result) {
        Get.snackbar("Success!", "Checked successfully done.",
            snackPosition: SnackPosition.BOTTOM);
        getLeaveInfo();
        EasyLoading.dismiss();
      } else {
        Get.snackbar(
          "Error!",
          "Check failed.",
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: warningColor,
        );
      }
    } catch (e) {
      EasyLoading.dismiss();
    }
  }
}
