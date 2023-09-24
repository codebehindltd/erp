import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:leading_edge/app/data/localDB/sharedPfnDBHelper.dart';
import 'package:leading_edge/app/data/models/req/user_login_model.dart';
import 'package:leading_edge/app/data/services/account_service.dart';
import 'package:leading_edge/app/routes/app_pages.dart';

import '../../../data/services/backgound_location_service.dart';
import '../../common/controllers/common_controller.dart';

class LoginController extends GetxController {
  bool obscureText = true;
  bool isChecked = true;
  final TextEditingController username = TextEditingController();
  final TextEditingController password = TextEditingController();
  var commonController = Get.find<CommonController>();
  String usertype = "Customer";

  void changeObscureText() {
    obscureText = !obscureText;
    update();
  }

  changeRemember() {
    isChecked = !isChecked;
    update();
  }

  @override
  void onInit() {
    usertype = Get.arguments;
    setCredentialData();
    super.onInit();
  }

  setCredentialData() async {
    var data = await SharedPfnDBHelper.getCredentialData();
    if (data != null) {
      var userCredential = userLoginModelFromJson(data);
      username.text = userCredential.userName ?? '';
      password.text = userCredential.password ?? '';
    }
  }

  void login(UserLoginModel lmr, context) async {
    EasyLoading.show(status: 'Loading...');
    SharedPfnDBHelper.removeLoginUserData();
    AccountService.loginUser(lmr).then((value) async {
      var data = jsonEncode(value);
      SharedPfnDBHelper.saveLoginUserData(data);
      SharedPfnDBHelper.saveCredentialData(userLoginModelToJson(lmr));
      await initializeService(); // reset location service
      await commonController.checkIsLogin();
      EasyLoading.dismiss();
      Get.snackbar("Success!", 'Login Succesfully Done!',
          snackPosition: SnackPosition.BOTTOM);
      Get.toNamed(Routes.propertySelection);
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }
}
