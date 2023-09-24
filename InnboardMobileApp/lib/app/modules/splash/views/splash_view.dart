import 'dart:io';
import 'package:flutter/material.dart';
import 'package:geolocator/geolocator.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/enums/user_type_enum.dart';
import 'package:leading_edge/app/core/utils/size_config.dart';
import 'package:leading_edge/app/core/values/strings.dart';
import 'package:leading_edge/app/data/localDB/sharedPfnDBHelper.dart';
import 'package:leading_edge/app/data/models/res/user_model.dart';
import 'package:leading_edge/app/data/services/location_service.dart';
import 'package:leading_edge/app/modules/common/controllers/common_controller.dart';
import 'package:leading_edge/app/routes/app_pages.dart';
import '../controllers/splash_controller.dart';

// ignore: must_be_immutable
class SplashView extends GetView<SplashController> {
  SplashView({Key? key}) : super(key: key);
  LocationPermission? permission;
  bool serviceEnabled = false;
  String? address;
  // final AttendanceSubmitController attendanceSubmitController =
  //     Get.put(AttendanceSubmitController());

  var commonController = Get.find<CommonController>();
  // late ConnectivityViewModel connectivityViewModel;
  UserModel? userInfo;
  // @override
  // void initState() {
  //   // // getCurrentLocation();
  //   // attendanceSubmitController =
  //   //     Provider.of<AttendanceSubmitController>(context, listen: false);
  //   // connectivityViewModel =
  //   //     Provider.of<ConnectivityViewModel>(context, listen: false);
  //   // connectivityViewModel.subscripCheckConnection();
  //   getUserInfo();
  //   configCheck();

  //   super.initState();
  // }

  getUserInfo() async {
    var data = await SharedPfnDBHelper.getLoginUserData();
    if (data != null) {
      userInfo = data;
    }
  }

  configCheck() async {
    try {
      var commonController = Get.find<CommonController>();
      Position position =
          await LocationService.determineCurrentPosition(LocationAccuracy.low);
      commonController.latitude = position.latitude;
      commonController.longitude = position.longitude;

      String? uType = await SharedPfnDBHelper.getUserType();
      await commonController.checkMemberOrGuestLogin();
      await Future.delayed(const Duration(milliseconds: 2), () {
        if (uType == UserTypeEnum.member.value) {
          Get.offNamed(Routes.memberDashboard);
        } else if (uType == UserTypeEnum.guest.value &&
            commonController.guestOrMemberId != null) {
          Get.offNamed(Routes.customerDashboard);
        } else {
          Get.offNamed(Routes.home);
        }
      });
    } catch (error) {
      serviceEnabled = await Geolocator.isLocationServiceEnabled();
      if (!serviceEnabled ||
          permission == LocationPermission.denied ||
          permission == LocationPermission.denied) {
        exit(0);
      }
    }
    // Position position = await Geolocator.getCurrentPosition(desiredAccuracy: LocationAccuracy.high);
  }

  @override
  Widget build(BuildContext context) {
    commonController.statusBarHeight = MediaQuery.of(context).viewPadding.top;
    commonController.keyboardHeight.value = MediaQuery.of(context).viewInsets.bottom;
    Get.find<CommonController>().subscripCheckConnection();
    commonController.checkIsLogin();
    getUserInfo();
    configCheck();
    SizeConfig().init(context);
    return Scaffold(
      body: Center(
          child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          SizedBox(height: 100, child: Image.asset("assets/images/logo.png")),
          const Text("Welcome To $appName"),
          const SizedBox(
            height: 15,
          ),
          const SizedBox(
            height: 35,
            width: 35,
            child: CircularProgressIndicator(),
          ),
        ],
      )),
    );
  }
}
