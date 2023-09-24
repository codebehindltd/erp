import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../routes/app_pages.dart';
import '../../../common_modules/booking_status/controllers/booking_status_controller.dart';
import '../../../common_modules/booking_status/views/booking_status_view.dart';
import '../../../common_modules/profileGuestMember/controllers/profile_guest_member_controller.dart';
import '../../../common_modules/profileGuestMember/views/profile_guest_member_view.dart';
import '../../../common_modules/promotional_offer/controllers/promotional_offer_controller.dart';
import '../../../common_modules/promotional_offer/views/promotional_offer_view.dart';
import '../../guest_reservation/views/guest_reservation_view.dart';

class CustomerDashboardController extends GetxController {
  var currentIndex = 0.obs;
  void onTabTapped(int index) {
    currentIndex.value = index;
    refreshPage(index);
    update();
  }

  List<Widget> pages = [
    // PropertySelectionView(),
    GuestReservationView(),
    PromotionalOfferView(),
    BookingStatusView(),
    const ProfileGuestMemberView(),
  ];

  Widget get currentPage => pages[currentIndex.value];
  Future<void> refreshPage(int index) async {
    switch (index) {
      case 0:
        {
          Get.toNamed(Routes.propertySelection);
          //Get.lazyPut(() => GuestReservationController());
          break;
        }
      case 1:
        {
          Get.lazyPut(() => PromotionalOfferController());
          break;
        }
      case 2:
        {
          Get.lazyPut(() => BookingStatusController());
          break;
        }
      case 3:
        {
          Get.lazyPut(() => ProfileGuestMemberController());
          break;
        }
    }
  }
}
