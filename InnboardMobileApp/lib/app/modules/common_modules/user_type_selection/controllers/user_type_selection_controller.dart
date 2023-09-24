import 'package:get/get.dart';
import 'package:leading_edge/app/data/models/res/about_us_model.dart';
import 'package:leading_edge/app/data/services/common_service.dart';
import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/res/user_type_model.dart';
import '../../../../routes/app_pages.dart';

class UserTypeSelectionController extends GetxController {
  List<UserTypeModel> userTypeList = [];
  List<UserTypeModel> userTypeListSecondRender = [];
  bool isLoading = true;
  bool isLoadingForAppInfo = true;
  AppInfoModel? appInfo;
  @override
  void onInit() {
    getUserTypeList();
    getAppInfo();
    super.onInit();
  }

  @override
  void onReady() {
    //
    super.onReady();
  }

  @override
  void onClose() {
    //
    super.onClose();
  }

  getUserTypeList() {
    CommonService.getUserTypes().then((value) async {
      isLoading = false;
      if (value.length == 1) {
        await typeSelect(value.first);
        return;
      }
      if (value
          .where(
              (element) => element.fieldValue == UserTypeEnum.reservation.value)
          .toList()
          .isNotEmpty) {
        userTypeList = value
            .where((e) =>
                e.fieldValue != UserTypeEnum.guest.value &&
                e.fieldValue != UserTypeEnum.member.value)
            .toList();
        userTypeListSecondRender = value
            .where((e) =>
                e.fieldValue == UserTypeEnum.guest.value ||
                e.fieldValue == UserTypeEnum.member.value)
            .toList();
      } else {
        userTypeList = value;
      }

      update();
    }).onError((error, stackTrace) {
      isLoading = false;
      userTypeList = [];
      Get.snackbar(
        "Error!",
        error.toString(),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      update();
    });
  }

  getAppInfo() async {
    await CommonService.fetchAppInfo().then((value) {
      appInfo = value.first;
      isLoadingForAppInfo = false;
      update();
    }).onError((error, stackTrace) {
      isLoadingForAppInfo = false;
      Get.snackbar(
        "Error!",
        error.toString(),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      update();
    });
  }

  Future<void> typeSelect(UserTypeModel typeModel) async {
    try {
      await SharedPfnDBHelper.saveUserType(typeModel.fieldValue!);
      if (typeModel.fieldValue == UserTypeEnum.user.value) {
        Get.toNamed(Routes.login, arguments: typeModel.fieldValue);
      } else if (typeModel.fieldValue == UserTypeEnum.becomeMember.value) {
        Get.toNamed(Routes.memberRegistration);
      } else if (typeModel.fieldValue == UserTypeEnum.guest.value) {
        // Get.offAllNamed(Routes.customerDashboard);
        Get.toNamed(Routes.propertySelection);
      } else if (typeModel.fieldValue == UserTypeEnum.member.value) {
        // Get.toNamed(Routes.propertySelection);
        Get.toNamed(Routes.memberLogin);
      } else if (typeModel.fieldValue == UserTypeEnum.aboutUs.value) {
        Get.toNamed(Routes.userTypeSelection + Routes.aboutUs);
      } else if (typeModel.fieldValue == UserTypeEnum.reservation.value) {
        Get.toNamed(Routes.userTypeSelection + Routes.userTypeSelectionTwo);
      } else {
        Get.snackbar(
          "Fail!",
          "Not Found",
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: warningColor,
        );
      }
    } catch (e) {
      // Get.snackbar(
      //   "Fail!",
      //   e.toString(),
      //   snackPosition: SnackPosition.BOTTOM,
      //   backgroundColor: warningColor,
      // );
    }
  }
}
