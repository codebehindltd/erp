import 'dart:io';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../controllers/file_picker_controller.dart';

class FileListView extends GetView<FilePickerController> {
  const FileListView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('FileListView'),
          centerTitle: true,
        ),
        body: GetBuilder<FilePickerController>(builder: (_) {
          return ListView.builder(
              itemCount: controller.selectedFiles.length,
              itemBuilder: (context, index) {
                final kb = controller.selectedFiles[index].size / 1024;
                final mb = kb / 1024;
                final size = (mb >= 1)
                    ? '${mb.toStringAsFixed(2)} MB'
                    : '${kb.toStringAsFixed(2)} KB';
                // final file = controller.selectedFiles[index];
                return InkWell(
                  onTap: () => controller.viewFile(controller.selectedFiles[index]),
                  child: ListTile(
                      leading: (controller.selectedFiles[index].extension ==
                                  'jpg' ||
                              controller.selectedFiles[index].extension == 'png')
                          ? Image.file(
                              File(controller.selectedFiles[index].path
                                  .toString()),
                              width: 80,
                              height: 80,
                            )
                          : Container(
                              width: 80,
                              height: 80,
                            ),
                      title: Text(controller.selectedFiles[index].name),
                      subtitle: Text(
                        size,
                        style: const TextStyle(fontWeight: FontWeight.w700),
                      ),
                      trailing: IconButton(
                        icon: Icon(Icons.delete),
                        onPressed: () => controller.removeItem(index),
                      )),
                );
              });
        }));
  }
}
