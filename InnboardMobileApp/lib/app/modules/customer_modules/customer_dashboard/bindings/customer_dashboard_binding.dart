import 'package:get/get.dart';
import '../../guest_reservation/controllers/guest_reservation_controller.dart';
import '../controllers/customer_dashboard_controller.dart';

class CustomerDashboardBinding extends Bindings {
  @override
  void dependencies() {
    Get.put<CustomerDashboardController>(
      CustomerDashboardController(),
    );
    // Get.lazyPut<PropertySelectionController>(
    //   () => PropertySelectionController(),
    // );
    Get.put<GuestReservationController>(
      GuestReservationController(),
    );
  }
}
