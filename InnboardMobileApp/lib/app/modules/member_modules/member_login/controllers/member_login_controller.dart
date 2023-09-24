import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';

import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/res/user_model.dart';
import '../../../../data/services/account_service.dart';
import '../../../../routes/app_pages.dart';
import '../../../common/controllers/common_controller.dart';

class MemberLoginController extends GetxController {
  final formKeyForMemberLogin = GlobalKey<FormState>();
  final TextEditingController mUsername = TextEditingController();
  final TextEditingController mPassword = TextEditingController();
  bool obscureText = true;
  UserModel? userModel;

  void changeObscureText() {
    obscureText = !obscureText;
    update();
  }

  @override
  void onInit() {
    super.onInit();
  }

  memberLoginInfoSubmit(v) {
    if (!formKeyForMemberLogin.currentState!.validate()) {
      return;
    }
    EasyLoading.show(status: 'Loading');
    AccountService.guestOrMemberLogin(
            UserTypeEnum.member.value, mUsername.text, mPassword.text)
        .then((value) async {
      userModel = value;
      String guestOrMemberId = userModel!.guestOrMemberId.toString();
      await SharedPfnDBHelper.saveGuestOrMemberId(guestOrMemberId);
      await Get.find<CommonController>().checkMemberOrGuestLogin();
      EasyLoading.dismiss();
      //EasyLoading.showSuccess('Login Succesfully Done!');
      Get.rawSnackbar(
        message: "Login Succesfully Done!",
        backgroundColor: themeColor,
      );
      Get.offNamed(Routes.propertySelection);
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }
}
