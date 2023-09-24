import 'package:get/get.dart';
import 'package:leading_edge/app/modules/common/middleware/auth_middleware.dart';

import '../controllers/common_controller.dart';

class CommonBinding extends Bindings {
  @override
  void dependencies() {
    Get.put<CommonController>(CommonController(), permanent: true);
    Get.put<AuthMiddleware>(AuthMiddleware(), permanent: true);
  }
}
