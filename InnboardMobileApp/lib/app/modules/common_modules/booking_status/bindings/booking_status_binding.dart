import 'package:get/get.dart';

import '../controllers/booking_status_controller.dart';

class BookingStatusBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<BookingStatusController>(
      () => BookingStatusController(),
    );
  }
}
