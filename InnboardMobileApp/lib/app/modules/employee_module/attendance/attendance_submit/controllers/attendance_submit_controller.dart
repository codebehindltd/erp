import 'dart:convert';
import 'dart:typed_data';

import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:geolocator/geolocator.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:leading_edge/app/data/localDB/sharedPfnDBHelper.dart';
import 'package:leading_edge/app/data/localDB/sqfliteDBHelper.dart';
import 'package:leading_edge/app/data/models/req/attendance_model.dart';
import 'package:leading_edge/app/data/models/res/user_model.dart';
import 'package:leading_edge/app/data/services/location_service.dart';

class AttendanceSubmitController extends GetxController {
  Uint8List? image;
  String? imageName;
  AttendanceModel attendanceModel = AttendanceModel();
  bool data = false;

  DateFormat dateFormat = DateFormat("dd MMM yyyy - hh:mm aaa");
  DateTime currentDateTime = DateTime.now();

  final SqfliteDBHelper sqfliteDBHelper = SqfliteDBHelper();
  UserModel? userInfo;

  // CommonController commonController = Get.find<CommonController>();

  @override
  void onInit() {
    image = Get.arguments['image'];
    print("file size: ${image!.lengthInBytes}");
    imageName = Get.arguments['imageName'];
    super.onInit();
  }

  @override
  void onReady() {
    updateCurrentLocation();
    getUserInfo();
    super.onReady();
  }

  getUserInfo() async {
    var data = await SharedPfnDBHelper.getPropertyUserData();
    if (data != null) {
      userInfo = data;
    }
  }

  Future<void> updateCurrentLocation() async {
    EasyLoading.show(status: "Loading..");
    Position position =
        await LocationService.determineCurrentPosition(LocationAccuracy.best);
    String address = await LocationService.getAddressFromLatLong(position);
    attendanceModel.latitude = position.latitude;
    attendanceModel.longitude = position.longitude;
    attendanceModel.address = address;
    attendanceModel.attDateTime = currentDateTime.toString();
    attendanceModel.imageByte = base64Encode(image!);
    attendanceModel.imageName = imageName;
    setAttendanceData(attendanceModel);
    EasyLoading.dismiss();
    // setState(() {});
  }

  setAttendanceData(AttendanceModel data) {
    attendanceModel = data;
    update();
  }

  updateTest(v) {
    data = v;
    update();
  }
}
