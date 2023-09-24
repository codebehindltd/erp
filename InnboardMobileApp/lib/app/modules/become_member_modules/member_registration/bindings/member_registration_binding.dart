import 'package:get/get.dart';

import '../controllers/member_registration_controller.dart';

class MemberRegistrationBinding extends Bindings {
  @override
  void dependencies() {
    // Get.put<MemberRegistrationController>(MemberRegistrationController(), permanent: true);
    Get.lazyPut<MemberRegistrationController>(
      () => MemberRegistrationController(),
    );
  }
}
