import 'package:flutter/material.dart';

import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';


import '../controllers/home_controller.dart';

class HomeView extends GetView<HomeController> {
  const HomeView({Key? key}) : super(key: key);
  // final ProfileController profileController=Get.put(ProfileController());
  @override
  Widget build(BuildContext context) {
    return GetBuilder<HomeController>(builder: (data) {
      return Scaffold(
          body: controller.currentPage,
          bottomNavigationBar: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 0, vertical: 0),
              child: ClipRRect(
                  //    borderRadius: BorderRadius.only(
                  // topRight: Radius.circular(40),
                  // topLeft: Radius.circular(40),
                  //    ),
                  borderRadius: BorderRadius.circular(30),
                  child: BottomNavigationBar(
                      // showSelectedLabels: false,
                      // showUnselectedLabels: false,
                      type: BottomNavigationBarType.fixed,
                      elevation: 1,
                      backgroundColor: Colors.white,
                      //selectedItemColor: Colors.blueAccent,
                      onTap: Get.find<HomeController>().onTabTapped,
                      currentIndex: data.currentIndex.value,
                      items: [
                        BottomNavigationBarItem(
                          icon: Icon(
                            Icons.dashboard_outlined,
                            color: data.currentIndex.value == 0
                                ? themeColor
                                : Colors.grey,
                            size: 26,
                          ),
                          label: "Dashboard",
                        ),
                        BottomNavigationBarItem(
                          icon: Icon(
                            Icons.person_outline_outlined,
                            color: data.currentIndex.value == 1
                                ? themeColor
                                : Colors.grey,
                            size: 26,
                          ),
                          label: "Profile",
                        ),
                      ]))));
    });
  }
}
