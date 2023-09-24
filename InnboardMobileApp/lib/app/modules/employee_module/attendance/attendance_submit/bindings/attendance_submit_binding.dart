import 'package:get/get.dart';

import '../controllers/attendance_submit_controller.dart';

class AttendanceSubmitBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<AttendanceSubmitController>(
      () => AttendanceSubmitController(),
    );
  }
}
