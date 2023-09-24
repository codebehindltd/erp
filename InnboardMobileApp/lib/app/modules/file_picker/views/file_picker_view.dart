import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../controllers/file_picker_controller.dart';

class FilePickerView extends GetView<FilePickerController> {
  const FilePickerView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('FilePickerView'),
        centerTitle: true,
      ),
      body: Center(
        child: ElevatedButton(
          onPressed: () async {
            controller.pickFile();
          },
          child: Text('Pick file'),
        ),
      ),
    );
  }
}
