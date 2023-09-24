import 'package:flutter/material.dart';

import 'package:get/get.dart';
import 'package:webview_flutter/webview_flutter.dart';

import '../../../../global_widgets/back_widget.dart';
import '../controllers/property_deatails_controller.dart';

class PropertyDeatailsView extends GetView<PropertyDeatailsController> {
  const PropertyDeatailsView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        title: const Text('Property Deatails'),
      ),
      body: Center(
        child: Obx(() => controller.isLoading.value
            ? const CircularProgressIndicator()
            : WebViewWidget(controller: controller.webViewController)),
      ),
    );
  }
}
