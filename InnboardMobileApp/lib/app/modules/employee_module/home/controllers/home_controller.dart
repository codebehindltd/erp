import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/modules/employee_module/dashboard/controllers/dashboard_controller.dart';
import 'package:leading_edge/app/modules/employee_module/dashboard/views/dashboard_view.dart';
import 'package:leading_edge/app/modules/common_modules/profile/controllers/profile_controller.dart';
import 'package:leading_edge/app/modules/common_modules/profile/views/profile_view.dart';


class HomeController extends GetxController {

  var currentIndex = 0.obs;
  void onTabTapped(int index) {
    currentIndex.value = index;
    refreshPage(index);
    update();
  }
  
  List<Widget> pages = [
    DashboardView(),
    const ProfileView(),
  ];

  Widget get currentPage => pages[currentIndex.value];
  Future<void> refreshPage(int index) async {
    switch (index) {
      case 0:
        {
          Get.lazyPut(()=>DashboardController());
          break;
        }
      case 1:
        {
          Get.lazyPut(()=>ProfileController());
          break;
        }
    }
  }
}
