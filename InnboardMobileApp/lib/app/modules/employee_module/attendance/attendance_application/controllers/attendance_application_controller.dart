import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/services/account_service.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../../data/models/req/attendance_model.dart';
import '../../../../../routes/app_pages.dart';
import '../../../../common/controllers/common_controller.dart';

class AttendanceApplicationController extends GetxController {
  final commonController = Get.find<CommonController>();
  TextEditingController vDateController = TextEditingController();
  TextEditingController entryTimeController = TextEditingController();
  TextEditingController exitTimeController = TextEditingController();
  TextEditingController remarksController = TextEditingController();
  DateTime? selectDate;
  TimeOfDay? entryTimePick;
  TimeOfDay? exitTimePick;
  final formKey = GlobalKey<FormState>();
  @override
  void onInit() {
    super.onInit();
  }

  @override
  void onReady() {
    // entryTimePick=TimeOfDay.fromDateTime(DateTime.now());
    // exitTimeController.text = timeFormatCustom.format(DateTime.now());
    super.onReady();
  }

  @override
  void onClose() {
    super.onClose();
  }

  submit(v) async {
    if (formKey.currentState!.validate()) {
      var data = await SharedPfnDBHelper.getLoginUserData();

      AttendanceModel model = AttendanceModel();
      model.empId = data!.empId;
      model.attendanceDate = selectDate;

      model.entryTime = entryTimePick == null
          ? null
          : DateTime(selectDate!.year, selectDate!.month, selectDate!.day,
              entryTimePick!.hour, entryTimePick!.minute);

      model.exitTime = exitTimePick == null
          ? null
          : DateTime(selectDate!.year, selectDate!.month, selectDate!.day,
              exitTimePick!.hour, exitTimePick!.minute);
      model.userInfoId = commonController.propertyUserInfoId;
      model.remark = remarksController.text;
      EasyLoading.show();
      AccountService.attendanceApplicationPost(model).then((value) {
        EasyLoading.dismiss();
        if (value) {
          Get.snackbar("Success!", "Successfully done.",
              snackPosition: SnackPosition.BOTTOM,
              icon: const Icon(Icons.check_circle_outline_rounded));
          Get.offNamed(Routes.home);
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

      // try {
      //   bool result = await AccountService.attendanceApplicationPost(model);
      //   EasyLoading.dismiss();
      //   if (result) {
      //     Get.snackbar("Success!", "Successfully done.",
      //         snackPosition: SnackPosition.BOTTOM,
      //         icon: const Icon(Icons.check_circle_outline_rounded));
      //     Get.offNamed(Routes.home);
      //   } else {
      //     Get.snackbar("Error!", "Please try again.",
      //         snackPosition: SnackPosition.BOTTOM,
      //         icon: const Icon(Icons.info_outline_rounded),
      //         backgroundColor: errorColor);
      //   }
      // } catch (e) {
      //   EasyLoading.dismiss();
      //   Get.snackbar("Error!", "Please try again.",
      //       snackPosition: SnackPosition.BOTTOM,
      //       icon: const Icon(Icons.info_outline_rounded),
      //       backgroundColor: errorColor);
      // }
    }
  }
}
