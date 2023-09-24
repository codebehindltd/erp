import 'package:get/get.dart';

import '../controllers/property_deatails_controller.dart';

class PropertyDeatailsBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<PropertyDeatailsController>(
      () => PropertyDeatailsController(),
    );
  }
}
