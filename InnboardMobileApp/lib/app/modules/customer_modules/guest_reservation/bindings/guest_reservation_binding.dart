import 'package:get/get.dart';

import '../controllers/guest_reservation_controller.dart';

class GuestReservationBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<GuestReservationController>(
      () => GuestReservationController(),
    );
  }
}
