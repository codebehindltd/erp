import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:innboard/app/core/values/colors.dart';
import 'package:innboard/app/global_widgets/back_widget.dart';
import 'package:innboard/app/global_widgets/input_text_field.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../routes/app_pages.dart';
import '../controllers/member_registration_controller.dart';

class MemberRegistrationView extends GetView<MemberRegistrationController> {
  MemberRegistrationView({Key? key}) : super(key: key);

  final formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Member Registration',
          style: TextStyle(
              color: Colors.white, fontWeight: FontWeight.w500, fontSize: 22),
        ),
        leading: const BackButtonWidget(),
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(12.0),
          child: Form(
            key: formKey,
           
            child: Column(
              children: [
                const SizedBox(
                  height: 80,
                ),
                Text(
                  'Member Registration',
                  style: TextStyle(
                      color: themeColor,
                      fontWeight: FontWeight.w500,
                      fontSize: 25),
                ),
                const SizedBox(
                  height: 40,
                ),
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 8.0),
                  child: InputField(
                    controllerText: controller.nameControler,
                    labelText: "Name",
                    validationText: 'Enter your name',
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    autofocus: true,
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 8.0),
                  child: InputField(
                    controllerText: controller.mobileControler,
                    labelText: "Mobile",
                    validationText: 'Enter your mobile',
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    isPhoneNumber: true,
                    keyboardType: TextInputType.number,
                    maxLength: 11,
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 8.0),
                  child: InputField(
                    controllerText: controller.emailControler,
                    //hintText: "Email",
                    labelText: "Email",
                    validationText: 'Enter your Email',
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    isEmail: true,
                    keyboardType: TextInputType.emailAddress,
                  ),
                ),
                const SizedBox(
                  height: 50,
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    CustomButton(
                        submit: (value) {
                          if (formKey.currentState!.validate()) {
                            Get.toNamed(Routes.memberRegistration +
                                Routes.memberRegistrationChoice);
                          }
                        },
                        name: "Sign Up",
                        fullWidth: false,
                        horizontalPadding: 32,
                        fontSize: 22),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
