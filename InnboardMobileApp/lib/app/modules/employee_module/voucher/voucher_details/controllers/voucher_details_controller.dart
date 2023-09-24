import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';

import '../../../../../core/values/colors.dart';
import '../../../../../data/models/res/voucher_list_model.dart';
import '../../../../../data/services/general_ledger_service.dart';
import '../../../../common/controllers/common_controller.dart';

class VoucherDetailsController extends GetxController {
  final commonController = Get.find<CommonController>();
  bool isLoading = true;
  int? userId;
  int? ledgerMasterId;
  List<VoucherResModel> voucherInfo = [];

  @override
  void onInit() {
    userId = commonController.propertyUserInfoId;
    ledgerMasterId = Get.arguments;
    getVoucherInfo();
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

  Future<void> getVoucherInfo() async {
    try {
      voucherInfo = await GeneralLedgerService.getVoucherInformation(
          userId!, ledgerMasterId!);
      isLoading = false;
      print(voucherInfo.first.isCanCheck);
      update();
    } catch (e) {
      print(e);
      isLoading = false;
      voucherInfo = [];
      update();
    }
  }

  approved(v) async {
    try {
      EasyLoading.show(status: 'Loading');
      var result = await GeneralLedgerService.voucherApproved(
          userId!, ledgerMasterId!, "Approved");

      if (result) {
        Get.snackbar("Success!", "Approved successfully done.",
            snackPosition: SnackPosition.BOTTOM);
        getVoucherInfo();
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
      var result = await GeneralLedgerService.voucherApproved(
          userId!, ledgerMasterId!, "Checked");
      if (result) {
        Get.snackbar("Success!", "Checked successfully done.",
            snackPosition: SnackPosition.BOTTOM);
        getVoucherInfo();
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
