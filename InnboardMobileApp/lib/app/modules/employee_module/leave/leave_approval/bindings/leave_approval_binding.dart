import 'package:get/get.dart';

import '../controllers/leave_approval_controller.dart';

class LeaveApprovalBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<LeaveApprovalController>(
      () => LeaveApprovalController(),
      // fenix: true
    );
  }
}
