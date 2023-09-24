import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../routes/app_pages.dart';
import '../controllers/common_controller.dart';

class GuestOrMemberMiddleware extends GetMiddleware {
  @override
  RouteSettings? redirect(String? route) {
    return Get.find<CommonController>().guestOrMemberId !=null || route == Routes.userTypeSelection
        ? null
        : const RouteSettings(name: Routes.userTypeSelection);
  }
}
