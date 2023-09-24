import 'package:get/get.dart';

import '../controllers/member_reservation_controller.dart';

class MemberReservationBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<MemberReservationController>(
      () => MemberReservationController(),
    );
  }
}
