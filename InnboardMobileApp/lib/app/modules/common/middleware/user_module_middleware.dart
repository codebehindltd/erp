import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../routes/app_pages.dart';
import '../controllers/common_controller.dart';

class UserModuleMiddleware extends GetMiddleware {
  @override
  int? get priority => 2;

  @override
  RouteSettings? redirect(String? route) {
    return Get.find<CommonController>().propertyUserInfoId != null ||
            route == Routes.userTypeSelection
        ? null
        : const RouteSettings(name: Routes.userTypeSelection);
  }
}
