import 'package:get/get.dart';

import '../controllers/attendance_application_controller.dart';

class AttendanceApplicationBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<AttendanceApplicationController>(
      () => AttendanceApplicationController(),
    );
  }
}
