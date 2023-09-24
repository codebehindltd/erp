import 'package:flutter/widgets.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/models/res/leave/leave_type_model.dart';
import 'package:leading_edge/app/data/services/emp_service.dart';

import '../../../../../core/utils/utils_function.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../../data/models/req/leave_application_model.dart';
import '../../../../../data/models/res/emp_model.dart';
import '../../../../../data/models/res/user_model.dart';
import '../../../../../routes/app_pages.dart';

class LeaveApplicationController extends GetxController {
  TextEditingController vFromDateController = TextEditingController();
  TextEditingController vToDateController = TextEditingController();
  TextEditingController descriptionController = TextEditingController();
  TextEditingController vNoOfDaysController = TextEditingController();
  DateTime? fromDate;
  DateTime? toDate;
  final formKey = GlobalKey<FormState>();
  List<LeaveTypeModel> leaveTypes = [];
  LeaveTypeModel? selectedLeaveType;
  UserModel? currentUser;

  List<EmpModel> empActiveList = [];
  EmpModel? selectedActiveEmp;

  @override
  void onInit() {
    vNoOfDaysController.text = "0";
    getUserInfo();
    getActiveEmpList();
    super.onInit();
  }

  getUserInfo() async {
    currentUser = await SharedPfnDBHelper.getPropertyUserData();
    getLeaveTypes();
  }

  submit(v) {
    if (formKey.currentState!.validate()) {
      try {
        EasyLoading.show();
        LeaveApplicationModel data = LeaveApplicationModel(
            //leaveId: selectedLeaveType!.leaveTypeId,
            empId: currentUser!.empId,
            leaveMode: "LWithP",
            leaveTypeId: selectedLeaveType!.leaveTypeId,
            fromDate: fromDate!,
            toDate: toDate!,
            noOfDays: int.parse(vNoOfDaysController.text),
            reason: descriptionController.text,
            workHandover: selectedActiveEmp?.empId ?? 0,
            createdBy: currentUser!.userInfoId,
            transactionType: "",
            expireDate: "",
            reportingTo: 0,
            leaveStatus: "Pending");

        EmpService.leaveApplication(data).then((value) {
          EasyLoading.dismiss();
          if (value) {
            Get.snackbar("Success!", "Leave Application Successfully Posted.",
                snackPosition: SnackPosition.BOTTOM);
            Get.offNamed(Routes.home);
          } else {
            Get.snackbar(
              "Error!",
              "Please try again.",
              snackPosition: SnackPosition.BOTTOM,
              backgroundColor: warningColor,
            );
          }
        }).onError((error, stackTrace) {
          EasyLoading.dismiss();
          Get.snackbar(
            "Error!",
            error.toString(),
            snackPosition: SnackPosition.BOTTOM,
            backgroundColor: warningColor,
          );
        });
      } catch (e) {
        EasyLoading.dismiss();
      }
    }
  }

  selectLeaveType(LeaveTypeModel? data) {
    selectedLeaveType = data;
    update();
  }

  selectActiveEmp(EmpModel? data) {
    selectedActiveEmp = data;
    update();
  }

  void selectFromDate(DateTime? datePick) {
    if (datePick != null) {
      vToDateController.text = dateFormat.format(datePick);
      fromDate = datePick;
    } else {
      vToDateController.text = "";
      fromDate = null;
    }
    calOffDays();
    update();
  }

  void selectToDate(DateTime? datePick) {
    if (datePick != null) {
      vFromDateController.text = dateFormat.format(datePick);
      toDate = datePick;
    } else {
      vFromDateController.text = "";
      toDate = null;
    }
    calOffDays();
    update();
  }

  calOffDays() {
    if (fromDate != null && toDate != null) {
      Duration days = Duration(days: toDate!.difference(fromDate!).inDays + 1);
      vNoOfDaysController.text = days.inDays.toString();
    }
  }

  getLeaveTypes() {
    EasyLoading.show();
    EmpService.getLeaveType(empId: currentUser!.empId!).then((value) {
      leaveTypes = value;
      EasyLoading.dismiss();
      update();
    }).onError((error, stackTrace) {
      leaveTypes = [];
      EasyLoading.dismiss();
      update();
    });
  }

  getActiveEmpList() {
    EasyLoading.show();
    EmpService.getActiveEmp().then((value) {
      empActiveList =
          value.where((x) => x.empId != currentUser!.empId).toList();
      EasyLoading.dismiss();
      update();
    }).onError((error, stackTrace) {
      empActiveList = [];
      EasyLoading.dismiss();
      update();
    });
  }
}
