import 'package:get/get.dart';

import '../controllers/user_type_selection_controller.dart';

class UserTypeSelectionBinding extends Bindings {
  @override
  void dependencies() {
    Get.put<UserTypeSelectionController>(UserTypeSelectionController());
  }
  // fenix: true
}
