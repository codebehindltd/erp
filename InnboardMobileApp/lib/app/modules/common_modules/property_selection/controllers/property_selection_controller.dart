import 'dart:convert';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/res/property_model.dart';
import '../../../../data/services/account_service.dart';
import '../../../../data/services/common_service.dart';
import '../../../../routes/app_pages.dart';
import '../../../common/controllers/common_controller.dart';

class PropertySelectionController extends GetxController {
  bool isLoading = true;
  List<PropertyModel> propertyList = [];
  String usertype = "Customer";
  final commonController = Get.find<CommonController>();
  int? userId;
  bool isShowBackbtn = false;

  @override
  void onInit() {
    isShowBackbtn = Get.arguments ?? false;
    getInfo();
    super.onInit();
  }

  void getInfo() async {
    var type = await SharedPfnDBHelper.getUserType();
    usertype = type!;
    userId = commonController.masterUserInfoId;
    getPropertyList();
  }



  getPropertyList() {
    CommonService.getPropertyList(usertype).then((value) async {
      // if (value.length == 1) {
      if (value.length == 1) {
        await propertySelect(value.first);
        return;
      }
      propertyList = value;
      isLoading = false;
      update();
    }).onError((error, stackTrace) {
      propertyList = [];
      isLoading = false;
    });
  }

  propertySelect(PropertyModel property) async {
    if (property.endPointIp == null || property.endPointIp!.isEmpty) {
      Get.snackbar(
        "Message!",
        "Coming Soon",
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      return;
    }
    EasyLoading.show();
    var saved = await SharedPfnDBHelper.savePropertyUrl(property.endPointIp!);
    await SharedPfnDBHelper.saveProperty(property);
    if (!saved) {
      Get.snackbar(
        "Error!",
        "Url Not Found",
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      await SharedPfnDBHelper.removePropertyUrl();
      EasyLoading.dismiss();
      return;
    }
    var userType = await SharedPfnDBHelper.getUserType();
    if (UserTypeEnum.user.value == userType) {
      if (Env.rootUrl == property.endPointIp) {
        var user = await SharedPfnDBHelper.getLoginUserData();
        var data = jsonEncode(user);
        await SharedPfnDBHelper.savePropertyUserData(data);
        EasyLoading.dismiss();
        commonController.propertyUserInfoId = user?.userInfoId;
        Get.offAllNamed(Routes.home);
      } else {
        AccountService.loginUserByMasterUserInfo(userId!).then((value) async {
          var data = jsonEncode(value);
          await SharedPfnDBHelper.savePropertyUserData(data);
          EasyLoading.dismiss();
          commonController.propertyUserInfoId = value.userInfoId;
          //commonController.checkIsLogin();
          Get.offAllNamed(Routes.home);
        }).onError((error, stackTrace) async {
          await SharedPfnDBHelper.removePropertyUrl();
          EasyLoading.dismiss();
          Get.snackbar(
            "Error!",
            error.toString(),
            snackPosition: SnackPosition.BOTTOM,
            backgroundColor: warningColor,
          );
        });
      }
    } else if (UserTypeEnum.guest.value == userType) {
      EasyLoading.dismiss();
      // Get.toNamed(Routes.guestReservation);
      Get.offAllNamed(Routes.customerDashboard);
    } else if (UserTypeEnum.member.value == userType) {
      EasyLoading.dismiss();
      // Get.toNamed(Routes.guestReservation);
      Get.offAllNamed(Routes.memberDashboard);
    }
  }
}
