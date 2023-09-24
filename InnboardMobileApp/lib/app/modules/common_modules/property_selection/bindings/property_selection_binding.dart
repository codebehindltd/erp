import 'package:get/get.dart';

import '../controllers/property_selection_controller.dart';

class PropertySelectionBinding extends Bindings {
  @override
  void dependencies() {
    Get.put<PropertySelectionController>(PropertySelectionController());
  }
}
