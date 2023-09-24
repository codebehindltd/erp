import 'package:file_picker/file_picker.dart';
import 'package:get/get.dart';
import 'package:open_file/open_file.dart';
import '../../../routes/app_pages.dart';

class FilePickerController extends GetxController {
  List<PlatformFile> selectedFiles = [];

  @override
  void onInit() {
    super.onInit();
  }

  void pickFile() async {
    try {
      FilePickerResult? result =
          await FilePicker.platform.pickFiles(allowMultiple: true);

      if (result == null) return;

      selectedFiles.addAll(result.files);

      Get.toNamed(
        Routes.filePicker + Routes.fileListView,
      );
    } catch (e) {
      print("Error picking files: $e");
    }
  }

  void viewFile(PlatformFile file) {
    OpenFile.open(file.path);
  }

  void removeItem(int index) {
    selectedFiles.removeAt(index);
    update();
  }
}
