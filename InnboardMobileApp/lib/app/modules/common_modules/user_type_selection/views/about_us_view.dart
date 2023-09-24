import 'package:flutter/material.dart';
import 'package:flutter_widget_from_html/flutter_widget_from_html.dart';
import 'package:get/get.dart';
import '../../../../global_widgets/back_widget.dart';
import '../controllers/user_type_selection_controller.dart';

class AboutUsView extends GetView<UserTypeSelectionController> {
  const AboutUsView({Key? key}) : super(key: key);

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        title: Text(
          'About Us',
          style: Theme.of(context).textTheme.titleLarge,
        ),

        //centerTitle: true,
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(12.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              HtmlWidget(
                controller.appInfo!.aboutUsDetail ?? "",
                textStyle: const TextStyle(
                    color: Color(0xff4F4F4F),
                    fontSize: 14,
                    fontWeight: FontWeight.w500),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
