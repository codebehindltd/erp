import 'package:get/get.dart';

import '../controllers/member_login_controller.dart';

class MemberLoginBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<MemberLoginController>(
      () => MemberLoginController(),
    );
  }
}
