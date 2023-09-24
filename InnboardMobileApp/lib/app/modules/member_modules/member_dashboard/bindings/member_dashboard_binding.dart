import 'package:get/get.dart';

import '../../member_reservation/controllers/member_reservation_controller.dart';
import '../controllers/member_dashboard_controller.dart';

class MemberDashboardBinding extends Bindings {
  @override
  void dependencies() {
    Get.put<MemberDashboardController>(MemberDashboardController());

    Get.put<MemberReservationController>(MemberReservationController());
  }
}
