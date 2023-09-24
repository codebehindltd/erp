import 'package:get/get.dart';

import '../controllers/voucher_approval_controller.dart';

class VoucherApprovalBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<VoucherApprovalController>(
      () => VoucherApprovalController(),
    );
  }
}
