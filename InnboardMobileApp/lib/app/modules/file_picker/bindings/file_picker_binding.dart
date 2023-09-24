import 'package:get/get.dart';

import '../controllers/file_picker_controller.dart';

class FilePickerBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<FilePickerController>(
      () => FilePickerController(),
    );
  }
}
