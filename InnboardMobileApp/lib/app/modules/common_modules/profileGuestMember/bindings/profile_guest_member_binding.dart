import 'package:get/get.dart';

import '../controllers/profile_guest_member_controller.dart';

class ProfileGuestMemberBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<ProfileGuestMemberController>(
      () => ProfileGuestMemberController(),
    );
  }
}
