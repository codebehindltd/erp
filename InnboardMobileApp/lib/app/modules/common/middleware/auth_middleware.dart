import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../routes/app_pages.dart';
import '../controllers/common_controller.dart';

class AuthMiddleware extends GetMiddleware {
  // final authController = Get.find<CommonController>();
  @override
  int? get priority => 1;

  @override
  RouteSettings? redirect(String? route) {
    return Get.find<CommonController>().isLogin || route == Routes.userTypeSelection
        ? null
        : const RouteSettings(name: Routes.userTypeSelection);
  }
}  
