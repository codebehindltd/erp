import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/values/colors.dart';
import '../../../../core/values/strings.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../routes/app_pages.dart';
import '../controllers/profile_controller.dart';

class ProfileView extends GetView<ProfileController> {
  const ProfileView({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
          elevation: 0,
          title: const Text(
            "Profile Info",
            style: TextStyle(color: Colors.white),
          ),
        ),
        body: GetBuilder<ProfileController>(builder: (data) {
          return Container(
            child: data.isLoading == true
                ? const Center(child: CircularProgressIndicator())
                : data.userInfo == null && data.isLoading == false
                    ? const Center(
                        child: Text(
                          "Guest User",
                          style: TextStyle(fontSize: 18),
                        ),
                      )
                    : ListView(
                        children: [
                          // Stack(
                          //   alignment: Alignment.bottomCenter,
                          //   children: [
                          //     // Image.asset("assets/images/drawer.png"),
                          //     // ClipRRect(
                          //     //   // borderRadius: BorderRadius.only(
                          //     //   //     bottomLeft: Radius.circular(100),
                          //     //   //     bottomRight: Radius.circular(100)),
                          //     //   child: Container(height: 120, color: themeColor),
                          //     // ),

                          //     Positioned(
                          //         // left: 30,
                          //         // bottom: 50,
                          //         top: 55,
                          //         child: Container(
                          //           height: 150,
                          //           width: 150,
                          //           // child: Image.asset(
                          //           //   "assets/images/user.png",
                          //           // ),
                          //           decoration: BoxDecoration(
                          //               shape: BoxShape.circle,
                          //               image: DecorationImage(
                          //                   fit: BoxFit.cover,
                          //                   image: AssetImage("assets/images/user.png")),
                          //               border: Border.all(color: Colors.white, width: 6)),
                          //         )),

                          //   ],
                          // ),
                          const SizedBox(
                            height: 100,
                          ),

                          ListTile(
                            title: Center(
                              child: Text(
                                data.userInfo!.userName ?? '',
                                style: const TextStyle(
                                    fontSize: 18, fontWeight: FontWeight.bold),
                              ),
                            ),
                          ),
                          Padding(
                            padding: const EdgeInsets.all(20),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                const Text(
                                  "Email",
                                  style: TextStyle(color: Colors.grey),
                                ),
                                const SizedBox(
                                  height: 8,
                                ),
                                Text(
                                  data.userInfo!.userEmail ?? '',
                                  style: const TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.w500),
                                ),
                                const Divider(),
                                const SizedBox(
                                  height: 20,
                                ),
                                const Text(
                                  "Phone Number",
                                  style: TextStyle(color: Colors.grey),
                                ),
                                const SizedBox(
                                  height: 8,
                                ),
                                Text(
                                  data.userInfo!.userPhone ?? '',
                                  style: const TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.w500),
                                ),
                                const Divider(),
                                const SizedBox(
                                  height: 20,
                                ),
                                const Divider(),
                                buildLogOutBtn(context),
                                const SizedBox(
                                  height: 20,
                                ),
                                CustomButton(
                                    submit: (v) {
                                      Get.toNamed(Routes.profile +
                                          Routes.paymentDetailsView);
                                    },
                                    name: "Payment Deatils",
                                    fullWidth: true),
                              ],
                            ),
                          )
                        ],
                      ),
          );
        }));
  }

  Widget buildLogOutBtn(context) {
    return InkWell(
      onTap: () {
        controller.logOut();
      },
      child: SizedBox(
        // padding: const EdgeInsets.symmetric(vertical: 25, horizontal: 20),
        width: double.infinity,
        // color: Colors.amber,

        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 22, vertical: 8),
          decoration: BoxDecoration(
              color: bodyColor,
              borderRadius: BorderRadius.circular(8),
              boxShadow: [
                const BoxShadow(
                  color: whiteColor,
                  spreadRadius: 2,
                  blurRadius: 6,
                  offset: Offset(-5, -5),
                ),
                BoxShadow(
                  color: bottonShadowColor.withOpacity(.5),
                  spreadRadius: 1,
                  blurRadius: 5,
                  offset: const Offset(4, 4),
                )
              ]),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                Icons.logout,
                color: themeColor,
              ),
              const SizedBox(
                width: 10,
              ),
              Text(
                'Log out',
                style: TextStyle(
                    // color: Colors.white,
                    // letterSpacing: 1.5,
                    fontSize: 18,
                    color: themeColor,
                    fontWeight: FontWeight.bold,
                    fontFamily: balooDa2),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
