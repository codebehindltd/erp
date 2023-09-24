import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/values/strings.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../common_modules/user_type_selection/controllers/user_type_selection_controller.dart';
import '../controllers/member_login_controller.dart';

// ignore: must_be_immutable
class MemberLoginView extends GetView<MemberLoginController> {
  MemberLoginView({Key? key}) : super(key: key);
  var userTypeSelectionController = Get.find<UserTypeSelectionController>();
  @override
  Widget build(BuildContext context) {
    return Scaffold(body: GetBuilder<MemberLoginController>(builder: (data) {
      return SafeArea(
        child: Form(
          key: controller.formKeyForMemberLogin,
          child: SingleChildScrollView(
            child: Container(
              padding: const EdgeInsets.all(25),
              child: Column(
                children: [
                  const SizedBox(
                    height: 40,
                  ),
                  ClipRRect(
                      borderRadius: BorderRadius.circular(100),
                      child: Image.network(
                        "${Env.rootUrl}${userTypeSelectionController.appInfo?.imagePath ?? ''}${userTypeSelectionController.appInfo?.imageName ?? ''}",
                        height: 150,
                        width: 150,
                        loadingBuilder: (context, Widget child,
                            ImageChunkEvent? loadingProgress) {
                          if (loadingProgress == null) {
                            return child;
                          }
                          return SizedBox(
                            height: 150,
                            width: 150,
                            child: Padding(
                              padding: const EdgeInsets.all(18.0),
                              child: Image.asset(
                                "assets/images/empty_image.png",
                                // color:Colors.grey.shade300.withOpacity(0.6),
                              ),
                            ),
                          );
                        },
                        errorBuilder: (context, error, stackTrace) {
                          return SizedBox(
                            height: 150,
                            width: 150,
                            child: Padding(
                              padding: const EdgeInsets.all(18.0),
                              child: Image.asset(
                                "assets/images/empty_image.png",
                              ),
                            ),
                          );
                        },
                      )),
                  const SizedBox(
                    height: 20,
                  ),
                  const Text(
                    "Welcome to $appName",
                    style: TextStyle(
                      fontSize: 16,
                      fontWeight: FontWeight.bold,
                    ),
                  ),

                  const SizedBox(
                    height: 22,
                  ),

                  Padding(
                    padding: const EdgeInsets.symmetric(vertical: 7.0),
                    child: TextFormField(
                      controller: controller.mUsername,
                      keyboardType: TextInputType.text,
                      decoration: InputDecoration(
                        contentPadding:
                            const EdgeInsets.symmetric(vertical: 12),
                        border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(10)),
                        hintText: "Member id",
                        prefixIcon: const Padding(
                          padding: EdgeInsets.symmetric(horizontal: 16),
                          child: Icon(
                            Icons.person_outline_outlined,
                            size: 30,
                          ),
                        ),
                        //hintStyle: kBodyText,
                      ),
                      validator: (value) {
                        if (value!.trim().isEmpty) {
                          return "Enter Your ember id";
                        } else {
                          return null;
                        }
                      },
                    ),
                  ),
                  // PasswordInput(),
                  Padding(
                    padding: const EdgeInsets.symmetric(vertical: 7.0),
                    child: TextFormField(
                      controller: controller.mPassword,
                      decoration: InputDecoration(
                          contentPadding:
                              const EdgeInsets.symmetric(vertical: 12),
                          border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(10)),
                          hintText: "Password",
                          prefixIcon: const Padding(
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: Icon(Icons.lock),
                          ),
                          suffixIcon: IconButton(
                            icon: Icon(controller.obscureText
                                ? Icons.visibility
                                : Icons.visibility_off),
                            onPressed: () {
                              controller.changeObscureText();
                            },
                          )),
                      obscureText: controller.obscureText,
                      validator: (value) {
                        if (value!.isEmpty) {
                          return "Enter Your Password";
                        } else {
                          return null;
                        }
                      },
                    ),
                  ),

                  const SizedBox(
                    height: 46,
                  ),
                  CustomButton(
                    submit: controller.memberLoginInfoSubmit,
                    name: "Login",
                    fullWidth: true,
                  ),
                ],
              ),
            ),
          ),
        ),
      );
    }));
  }
}
