import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

import 'package:get/get.dart';

import '../../../../core/values/colors.dart';
import '../controllers/member_dashboard_controller.dart';

class MemberDashboardView extends GetView<MemberDashboardController> {
  const MemberDashboardView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return GetBuilder<MemberDashboardController>(builder: (data) {
      return WillPopScope(
          onWillPop: () async => await onBackButtonPress(context),
        child: Scaffold(
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
                        onTap:
                            Get.find<MemberDashboardController>().onTabTapped,
                        currentIndex: data.currentIndex.value,
                        items: [
                          BottomNavigationBarItem(
                            icon: Icon(
                              Icons.home_outlined,
                              color: data.currentIndex.value == 0
                                  ? themeColor
                                  : Colors.grey,
                              size: 26,
                            ),
                            label: "Home",
                          ),
                          BottomNavigationBarItem(
                            icon: Icon(
                              Icons.local_offer_outlined,
                              color: data.currentIndex.value == 1
                                  ? themeColor
                                  : Colors.grey,
                              size: 26,
                            ),
                            label: "Promotional",
                          ),
                          BottomNavigationBarItem(
                            icon: Icon(
                              Icons.shopping_bag_outlined,
                              color: data.currentIndex.value == 2
                                  ? themeColor
                                  : Colors.grey,
                              size: 26,
                            ),
                            label: "Booking",
                          ),
                          BottomNavigationBarItem(
                            icon: Icon(
                              Icons.person_outline_outlined,
                              color: data.currentIndex.value == 3
                                  ? themeColor
                                  : Colors.grey,
                              size: 26,
                            ),
                            label: "Profile",
                          ),
                        ])))),
      );
    });
  }
}


Future<bool> onBackButtonPress(BuildContext context) async {
  return await showDialog(
      context: context,
      builder: (context) => AlertDialog(
            contentPadding:
                const EdgeInsets.symmetric(vertical: 10, horizontal: 22),
            // title: Text(
            //   "Exit?",
            //   style: TextStyle(
            //       fontFamily: inter, fontSize: 20, fontWeight: FontWeight.w600),
            // ),
            title: const SizedBox(
              height: 4,
            ),
            content: Text(
              "Do you want to close the App?",
              style: TextStyle(color: themeColor, fontWeight: FontWeight.w600, fontSize: 18),
            ),
            actions: [
              TextButton(
                  onPressed: () {
                    SystemNavigator.pop();
                  },
                  child: Text(
                    "Yes",
                    style: TextStyle(color: themeColor, fontWeight: FontWeight.w600, fontSize: 16),
                  )),
              TextButton(
                  onPressed: () {
                    Navigator.of(context).pop(false);
                  },
                  child: Text(
                    "No",
                    style: TextStyle(color: themeColor, fontWeight: FontWeight.w600, fontSize: 16),
                  ))
            ],
          ));
}
