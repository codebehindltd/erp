
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';

import 'package:get/get.dart';

import '../../../../../core/utils/size_config.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../core/values/strings.dart';
import '../../../../../data/services/account_service.dart';
import '../../../../../routes/app_pages.dart';
import '../../../../common/controllers/common_controller.dart';
import '../controllers/attendance_submit_controller.dart';

class AttendanceSubmitView extends GetView<AttendanceSubmitController> {
  AttendanceSubmitView({Key? key}) : super(key: key);
//  final AttendanceSubmitController attendanceSubmitController =
//       Get.put(AttendanceSubmitController());
  final CommonController commonController = Get.find<CommonController>();
  // DateFormat dateFormat = DateFormat("dd MMM yyyy - hh:mm aaa");
  // DateTime currentDateTime = DateTime.now();

  // final SqfliteDBHelper sqfliteDBHelper=SqfliteDBHelper();
  // UserModel? userInfo;

  // @override
  // void initState() {
  //   super.initState();

  //   sqfliteDBHelper = SqfliteDBHelper();
  //   // attendanceSubmitViewModel =
  //   //     Provider.of<AttendanceSubmitViewModel>(context, listen: false);
  //   currentDateTime = DateTime.now();
  //   // attendanceSubmitViewModel.attendanceModel.image =
  //   //     widget.image;
  //   // var data = attendanceSubmitViewModel.attendanceModel;
  //   // attendanceSubmitViewModel.setAttendanceData(attendanceSubmitViewModel.attendanceModel);
  //   updateCurrentLocation();
  //   getUserInfo();
  //   // print("widget.image");
  //   // String base64String = base64Encode(widget.image);
  //   // print("ds ${widget.image} end");
  //   // print("end image");
  // }

  // Future<void> updateCurrentLocation() async {
  //   EasyLoading.show(status: "Loading..");
  //   Position position =
  //       await LocationService().determineCurrentPosition(LocationAccuracy.best);
  //   String address = await LocationService().getAddressFromLatLong(position);

  //   controller.attendanceModel.latitude = position.latitude;
  //   controller.attendanceModel.longitude = position.longitude;
  //   controller.attendanceModel.address = address;
  //   controller.attendanceModel.dateTime =
  //       currentDateTime.toString();
  //   // var data = attendanceSubmitViewModel.attendanceModel;
  //   controller.attendanceModel.imageByte =
  //       base64Encode(controller.image!);
  //   controller.attendanceModel.imageName = controller.imageName;
  //   controller
  //       .setAttendanceData(controller.attendanceModel);
  //   EasyLoading.dismiss();
  //   // setState(() {});
  // }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
          leading: IconButton(
            icon: const Icon(
              Icons.arrow_back,
              color: Colors.white,
            ),
            onPressed: () {
              Navigator.pop(context);
            },
          ),
          iconTheme: const IconThemeData(
            color: Colors.white, //change your color here
          ),
          title: const Text(
            "Attendance",
            style: TextStyle(color: Colors.white),
          )),
      body: Container(
        padding: const EdgeInsets.all(10),
        child: GetBuilder<AttendanceSubmitController>(builder: (data) {
          return ListView(
            children: [
              Column(
                mainAxisAlignment: MainAxisAlignment.start,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    "Image:",
                    style: lebalStyle(),
                  ),
                  const SizedBox(
                    height: 10,
                  ),
                  Container(
                      padding: const EdgeInsets.only(left: 5, right: 5),
                      child: Image.memory(
                        controller.image!,
                        width: 200,
                        height: 200,
                        fit: BoxFit.contain,
                      ))
                ],
              ),
              const SizedBox(
                height: 15,
              ),
              Column(
                // crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    // mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(
                        "Address: ",
                        style: lebalStyle(),
                      ),
                      // Text("${attendanceSubmitViewModel.attendanceModel.address??""}")
                      SizedBox(
                          width: SizeConfig.screenWidth! * .75,
                          child: Text(
                            controller.attendanceModel.address ?? "",
                          ))
                    ],
                  ),
                ],
              ),
              const SizedBox(
                height: 10,
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    // crossAxisAlignment: CrossAxisAlignment.start,
                    // mainAxisAlignment: MainAxisAlignment.start,
                    children: [
                      Text(
                        "DateTime: ",
                        style: lebalStyle(),
                      ),
                      // Text("${attendanceSubmitViewModel.attendanceModel.address??""}")
                      SizedBox(
                          width: SizeConfig.screenWidth! * .72,
                          child: Text(controller.dateFormat
                              .format(controller.currentDateTime)))
                    ],
                  ),
                ],
              ),
              const SizedBox(
                height: 10,
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    // crossAxisAlignment: CrossAxisAlignment.start,
                    // mainAxisAlignment: MainAxisAlignment.start,
                    children: [
                      Text(
                        "Latitude: ",
                        style: lebalStyle(),
                      ),
                      // Text("${attendanceSubmitViewModel.attendanceModel.address??""}")
                      SizedBox(
                          width: SizeConfig.screenWidth! * .72,
                          child: Text("${controller.attendanceModel.latitude??''}"))
                    ],
                  ),
                ],
              ),
              const SizedBox(
                height: 10,
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    // crossAxisAlignment: CrossAxisAlignment.start,
                    // mainAxisAlignment: MainAxisAlignment.start,
                    children: [
                      Text(
                        "Longitude: ",
                        style: lebalStyle(),
                      ),
                      // Text("${attendanceSubmitViewModel.attendanceModel.address??""}")
                      SizedBox(
                          width: SizeConfig.screenWidth! * .72,
                          child:
                              Text("${controller.attendanceModel.longitude??''}"))
                    ],
                  ),
                ],
              ),
              Column(
                children: [
                  buildConfirmBtn(context),
                ],
              ),
            ],
          );
        }),
      ),
    );
  }

  Widget buildConfirmBtn(context) {
    return Container(
      padding: const EdgeInsets.symmetric(vertical: 25, horizontal: 20),
      // width: double.infinity,
      // color: Colors.amber,

      child: ElevatedButton(
        onPressed: () async {
          controller.attendanceModel.empId = controller.userInfo!.empId;
          if (!commonController.isConnection) {
            Get.rawSnackbar(
                message: "No internet connection",
                backgroundColor: errorColor,
                icon: const Icon(Icons.wifi_off_rounded, color: whiteColor,));
            return;
          }
          EasyLoading.show(status: 'Loading...');
          AccountService.attendancePost(controller.attendanceModel)
              .then((value) {
            EasyLoading.dismiss();
            if (value) {
              Get.snackbar("Success!", "Successfully done.",
                  snackPosition: SnackPosition.BOTTOM,
                  icon: const Icon(Icons.check_circle_outline_rounded));
              Get.offAllNamed(Routes.home);
            } else {
              Get.snackbar("Error!", "Please try again.",
                  snackPosition: SnackPosition.BOTTOM,
                  icon: const Icon(Icons.info_outline_rounded),
                  backgroundColor: errorColor);
            }
          }).onError((error, stackTrace) {
            EasyLoading.dismiss();
            Get.snackbar("Error!", "Please try again.",
                snackPosition: SnackPosition.BOTTOM,
                icon: const Icon(Icons.info_outline_rounded),
                backgroundColor: errorColor);
          });
          // } else if (!commonController.isConnection) {
          //   // var remove =
          //   //     await controller.sqfliteDBHelper.deleteAttedanceById(null);
          //   // var data = await controller.sqfliteDBHelper
          //   //     .saveAttendanceData(controller.attendanceModel);
          //   //   EasyLoading.showToast("Successfully Store data in local",
          //   // toastPosition: EasyLoadingToastPosition.bottom);
          //   // CustomSnackbar.snackbar(
          //   //     context, "Successfully Store data in local", Colors.black);
          //     Get.snackbar("Success!", "Successfully Store data in local.", snackPosition: SnackPosition.BOTTOM, icon: const Icon(Icons.check_circle_outline_rounded));

          //   Get.offAllNamed(Routes.home);
          // }
        },
        child: Text(
          'Submit',
          style: TextStyle(
              // color: Colors.white,
              // letterSpacing: 1.5,
              fontSize: 18,
              color: Colors.white,
              fontWeight: FontWeight.bold,
              fontFamily: balooDa2),
        ),
      ),
    );
  }

  TextStyle lebalStyle() => const TextStyle(fontWeight: FontWeight.w800);
}
