import 'package:get/get.dart';

import '../controllers/leave_status_controller.dart';

class LeaveStatusBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<LeaveStatusController>(
      () => LeaveStatusController(),
    );
  }
}
