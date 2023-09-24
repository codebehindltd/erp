import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import 'package:image_picker/image_picker.dart';
import '../../../../core/utils/size_config.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sqfliteDBHelper.dart';
import '../../../../routes/app_pages.dart';
import '../controllers/dashboard_controller.dart';

// ignore: must_be_immutable
class DashboardView extends GetView<DashboardController> {
  DashboardView({Key? key}) : super(key: key);
  final ImagePicker _picker = ImagePicker();
  XFile? attendanceImage;
  File? aImage;
  String? aImageName;
  final SqfliteDBHelper sqfliteDBHelper = SqfliteDBHelper();
  // AttendanceModel? attendanceData;
  final double boxHeight = 135;
  // late ConnectivityViewModel connectivityViewModel;

  // void getAttendanceData() async {
  //   var data = await sqfliteDBHelper.getAttendancesData();
  //   print(jsonEncode(data));
  //   print(data.length);
  //   if (data.isNotEmpty) {
  //     attendanceData = data[0];
  //   }
  // }

  @override
  Widget build(BuildContext context) {
    // getAttendanceData();
    return Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
          elevation: 0,
          title: const Text(
            "Dashboard",
            style: TextStyle(color: Colors.white),
          ),
          // actions: [
          //   Visibility(
          //       visible: (connectivityViewModel.isConnection==true &&
          //           attendanceData != null),
          //       child: IconButton(
          //           onPressed: () {},
          //           icon: const Icon(
          //             Icons.cloud_sync,
          //             color: Colors.white,
          //           )))
          // ],
        ),
        body: Column(
          children: [
            // Consumer<ConnectivityViewModel>(builder: (context, data, _) {
            //             return Visibility(
            //                 visible: !data.isConnection,
            //                 child: Container(child: const Text("No Internet Connection", style: TextStyle(color: Colors.red),)));
            //           }),
            SingleChildScrollView(
                child: Container(
              padding: const EdgeInsets.only(top: 15, left: 5, right: 5),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                // crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      // tracking employe
                      Container(
                        margin: const EdgeInsets.all(6),
                        decoration: BoxDecoration(
                            color: bodyColor,
                            borderRadius: BorderRadius.circular(14),
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
                                blurRadius: 8,
                                offset: const Offset(4, 4),
                              )
                            ]),
                        // elevation: 2.0,
                        // shadowColor: Colors.black38,
                        // color: const Color(0xFFEEEEEE),
                        // shape: RoundedRectangleBorder(
                        //   borderRadius: BorderRadius.circular(10),
                        // ),
                        child: InkWell(
                          onTap: () async {
                            Get.toNamed(Routes.employeeList);
                            // Navigator.push(
                            //     context,
                            //     MaterialPageRoute(
                            //         builder: (context) => EmplyeListScreen()));
                          },
                          child: SizedBox(
                            height: boxHeight,
                            width: SizeConfig.screenWidth! * .29,
                            // height: 120,
                            // width: 100,
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.center,
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                Image.asset(
                                  "assets/images/employes.png",
                                  height: 80,
                                  // width: SizeConfig.safeBlockHorizontal! * 15,
                                ),
                                const SizedBox(
                                  height: 5,
                                ),
                                const Text(
                                  "Tracking Employee",
                                  textAlign: TextAlign.center,
                                  overflow: TextOverflow.ellipsis,
                                  maxLines: 2,
                                  style: TextStyle(
                                      color: Colors.black,
                                      fontWeight: FontWeight.w600),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),
                      // attendance
                      Container(
                        margin: const EdgeInsets.all(6),
                        decoration: BoxDecoration(
                            color: bodyColor,
                            borderRadius: BorderRadius.circular(14),
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
                                blurRadius: 8,
                                offset: const Offset(4, 4),
                              )
                            ]),
                        child: InkWell(
                          onTap: () async {
                            Get.bottomSheet(buildAttendanceModal(context));
                          },
                          child: SizedBox(
                            height: boxHeight,
                            width: SizeConfig.screenWidth! * .29,
                            // height: 120,
                            // width: 100,
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.center,
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                Image.asset(
                                  "assets/images/attendance.png",
                                  height: 80,
                                  // width: SizeConfig.safeBlockHorizontal! * 15,
                                ),
                                const SizedBox(
                                  height: 5,
                                ),
                                const Text(
                                  "Attendance",
                                  textAlign: TextAlign.center,
                                  overflow: TextOverflow.ellipsis,
                                  maxLines: 2,
                                  style: TextStyle(
                                      color: Colors.black,
                                      fontWeight: FontWeight.w600),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),

                      // leave box
                      Container(
                        margin: const EdgeInsets.all(6),
                        decoration: BoxDecoration(
                            color: bodyColor,
                            borderRadius: BorderRadius.circular(14),
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
                                blurRadius: 8,
                                offset: const Offset(4, 4),
                              )
                            ]),
                        child: InkWell(
                          onTap: () {
                            Get.bottomSheet(buildLeaveModal(context));
                            // Get.snackbar("Warning!", "Not available!",
                            //     icon: const Icon(Icons.warning_amber_rounded),
                            //     backgroundColor: warningColor,
                            //     snackPosition: SnackPosition.BOTTOM);
                          },
                          child: SizedBox(
                            height: boxHeight,
                            width: SizeConfig.screenWidth! * .29,
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.center,
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                Image.asset(
                                  "assets/images/leave.png",
                                  height: 80,
                                ),
                                const SizedBox(
                                  height: 5,
                                ),
                                const Text(
                                  "Leave",
                                  textAlign: TextAlign.center,
                                  overflow: TextOverflow.ellipsis,
                                  maxLines: 2,
                                  style: TextStyle(
                                      color: Colors.black,
                                      fontWeight: FontWeight.w600),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),
                    ],
                  ),
                  // Row(
                  //   crossAxisAlignment: CrossAxisAlignment.center,
                  //   mainAxisAlignment: MainAxisAlignment.center,
                  //   children: [
                  // attendance
                  // Container(
                  //   margin: const EdgeInsets.all(6),
                  //   decoration: BoxDecoration(
                  //       color: bodyColor,
                  //       borderRadius: BorderRadius.circular(14),
                  //       boxShadow: [
                  //         const BoxShadow(
                  //           color: whiteColor,
                  //           spreadRadius: 2,
                  //           blurRadius: 6,
                  //           offset: Offset(-5, -5),
                  //         ),
                  //         BoxShadow(
                  //           color: bottonShadowColor.withOpacity(.5),
                  //           spreadRadius: 1,
                  //           blurRadius: 8,
                  //           offset: const Offset(4, 4),
                  //         )
                  //       ]),
                  //   child: InkWell(
                  //     onTap: () async {
                  //       Get.toNamed(Routes.voucherApproval);
                  //     },
                  //     child: SizedBox(
                  //       height: boxHeight,
                  //       width: SizeConfig.screenWidth! * .29,
                  //       // height: 120,
                  //       // width: 100,
                  //       child: Column(
                  //         crossAxisAlignment: CrossAxisAlignment.center,
                  //         mainAxisAlignment: MainAxisAlignment.center,
                  //         children: [
                  //           Image.asset(
                  //             "assets/images/voucher.png",
                  //             height: 80,
                  //             // width: SizeConfig.safeBlockHorizontal! * 15,
                  //           ),
                  //           const SizedBox(
                  //             height: 5,
                  //           ),
                  //           const Text(
                  //             "Voucher Authorization",
                  //             textAlign: TextAlign.center,
                  //             overflow: TextOverflow.ellipsis,
                  //             maxLines: 2,
                  //             style: TextStyle(
                  //                 color: Colors.black,
                  //                 fontWeight: FontWeight.w600),
                  //           ),
                  //         ],
                  //       ),
                  //     ),
                  //   ),
                  // ),
                  //   ],
                  // )
                ],
              ),
            )),
          ],
        ));
  }

  Widget buildAttendanceModal(context) {
    return Container(
      color: Theme.of(context).scaffoldBackgroundColor,
      child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              width: double.infinity,
              padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
              decoration: BoxDecoration(color: Theme.of(context).primaryColor),
              child: const Text(
                "Attendance",
                textAlign: TextAlign.center,
                style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.w700,
                    color: whiteColor),
              ),
            ),
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 10),
              child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Divider(),
                    TextButton.icon(
                        onPressed: () {
                          liveAttendance();
                        },
                        icon: const Icon(Icons.camera_outlined,
                            color: blackColor),
                        label: const Text(
                          "Live Attendance",
                          style: TextStyle(color: blackColor),
                        ),
                        style: ButtonStyle(
                          padding: MaterialStateProperty.resolveWith<
                              EdgeInsetsGeometry>(
                            (Set<MaterialState> states) {
                              return const EdgeInsets.symmetric(
                                  horizontal: 18, vertical: 10);
                            },
                          ),
                        )),
                    TextButton.icon(
                        onPressed: () {
                          Get.toNamed(Routes.attendanceApplication);
                        },
                        icon: const Icon(Icons.save_as_outlined,
                            color: blackColor),
                        label: const Text(
                          "Attendance Application",
                          style: TextStyle(color: blackColor),
                        ),
                        style: ButtonStyle(
                          padding: MaterialStateProperty.resolveWith<
                              EdgeInsetsGeometry>(
                            (Set<MaterialState> states) {
                              return const EdgeInsets.symmetric(
                                  horizontal: 18, vertical: 10);
                            },
                          ),
                        )),
                    // TextButton.icon(
                    //     onPressed: () {},
                    //     icon: const Icon(Icons.pending_actions_outlined,
                    //         color: blackColor),
                    //     label: const Text(
                    //       "Late Attendance Application",
                    //       style: TextStyle(color: blackColor),
                    //     ),
                    //     style: ButtonStyle(
                    //       padding: MaterialStateProperty.resolveWith<
                    //           EdgeInsetsGeometry>(
                    //         (Set<MaterialState> states) {
                    //           return const EdgeInsets.symmetric(
                    //               horizontal: 18, vertical: 10);
                    //         },
                    //       ),
                    //     )),
                  ]),
            )
            // Container(
            //   decoration: BoxDecoration(color: gray1Color),
            //   padding: EdgeInsets.all(8),
            //   child: Row(
            //     children: [
            //       Icon(Icons.camera_outlined),
            //       Container(
            //         width: 2,
            //         color: blackColor,
            //       ),
            //       Text("Live Attendance")
            //     ],
            //   ),
            // ),
            // InkWell(
            //   onTap: () {
            //     liveAttendance();
            //   },
            //   child: const ListTile(
            //     leading: Icon(Icons.camera_outlined),
            //     title: Text(
            //       "Live Attendance",
            //     ),
            //   ),
            // ),
            // InkWell(
            //   onTap: () {
            //     Get.back();
            //   },
            //   child: const ListTile(
            //     leading: Icon(Icons.save_as_outlined),
            //     title: Text("Attendance Application"),
            //   ),
            // ),
          ]),
    );
  }

  Future<void> liveAttendance() async {
    EasyLoading.show(status: "Loading..");
    attendanceImage = await _picker.pickImage(
      imageQuality: 25,
      source: ImageSource.camera,
      // preferredCameraDevice: CameraDevice.front
    );
    EasyLoading.dismiss();
    if (attendanceImage == null) {
      Get.snackbar("Error!", "Image not take!",
          snackPosition: SnackPosition.BOTTOM,
          icon: const Icon(Icons.warning_amber_rounded),
          backgroundColor: warningColor);
      // CustomSnackbar.snackbar(
      //     context:context, msg: "Image not take!", );
    } else {
      // print(photo.path);

      // var image=base64Decode(attendanceImage!.path);
      // print(image);
      print(attendanceImage!.name);
      aImage = File(attendanceImage!.path);
      aImageName = attendanceImage!.name;
      var argData = {
        "image": aImage!.readAsBytesSync(),
        "imageName": aImageName!
      };
      Get.toNamed(Routes.attendanceSubmit, arguments: argData);
    }
  }

  // leave modal
  Widget buildLeaveModal(context) {
    return Container(
      color: Theme.of(context).scaffoldBackgroundColor,
      child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              width: double.infinity,
              padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
              decoration: BoxDecoration(color: Theme.of(context).primaryColor),
              child: const Text(
                "Leave",
                textAlign: TextAlign.center,
                style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.w700,
                    color: whiteColor),
              ),
            ),
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 10),
              child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Divider(),
                    TextButton.icon(
                        onPressed: () {
                          Get.toNamed(Routes.leaveStatus);
                        },
                        icon: const Icon(Icons.live_help_outlined,
                            color: blackColor),
                        label: const Text(
                          "Leave Status",
                          style: TextStyle(color: blackColor),
                        ),
                        style: ButtonStyle(
                          padding: MaterialStateProperty.resolveWith<
                              EdgeInsetsGeometry>(
                            (Set<MaterialState> states) {
                              return const EdgeInsets.symmetric(
                                  horizontal: 18, vertical: 10);
                            },
                          ),
                        )),
                    TextButton.icon(
                        onPressed: () {
                          Get.toNamed(Routes.leaveApplication);
                        },
                        icon: const Icon(Icons.save_as_outlined,
                            color: blackColor),
                        label: const Text(
                          "Leave Application",
                          style: TextStyle(color: blackColor),
                        ),
                        style: ButtonStyle(
                          padding: MaterialStateProperty.resolveWith<
                              EdgeInsetsGeometry>(
                            (Set<MaterialState> states) {
                              return const EdgeInsets.symmetric(
                                  horizontal: 18, vertical: 10);
                            },
                          ),
                        )),
                  ]),
            )
          ]),
    );
  }
}
