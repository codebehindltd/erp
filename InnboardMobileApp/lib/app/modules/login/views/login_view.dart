import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/strings.dart';
import 'package:leading_edge/app/data/models/req/user_login_model.dart';
import 'package:leading_edge/app/global_widgets/custom_button.dart';
import 'package:leading_edge/app/modules/common/controllers/common_controller.dart';
import '../../../core/environment/environment.dart';
import '../../../core/values/colors.dart';
import '../../common_modules/user_type_selection/controllers/user_type_selection_controller.dart';
import '../controllers/login_controller.dart';

class LoginView extends GetView<LoginController> {
  LoginView({Key? key}) : super(key: key);

  final formKey = GlobalKey<FormState>();
  final CommonController commonController = Get.find<CommonController>();
  final userTypeSelectionController = Get.find<UserTypeSelectionController>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: GetBuilder<LoginController>(builder: (data) {
        return SafeArea(
          child: Form(
            key: formKey,
            child: SingleChildScrollView(
              child: Container(
                padding: const EdgeInsets.all(25),
                child: Column(
                  children: [
                    const SizedBox(
                      height: 30,
                    ),
                    // Image.asset(
                    //   "assets/images/logo.png",
                    //   width: 150,
                    //   height: 150,
                    // ),

                    Image.network(
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
                    ),
                    const SizedBox(
                      height: 10,
                    ),
                    const Text(
                      "Welcome to $appName",
                      style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(
                      height: 10,
                    ),
                    const SizedBox(
                      height: 20,
                    ),

                    Padding(
                      padding: const EdgeInsets.symmetric(vertical: 7.0),
                      child: TextFormField(
                        controller: controller.username,
                        keyboardType: TextInputType.text,
                        decoration: InputDecoration(
                          contentPadding:
                              const EdgeInsets.symmetric(vertical: 12),
                          border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(10)),
                          hintText: "username",
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
                            return "Enter Your Username";
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
                        controller: controller.password,
                        decoration: InputDecoration(
                            contentPadding:
                                const EdgeInsets.symmetric(vertical: 12),
                            border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(10)),
                            hintText: "password",
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
                    Row(
                      children: [
                        Checkbox(
                          checkColor: Colors.white,
                          activeColor: themeColor,
                          value: controller.isChecked,
                          onChanged: (bool? value) {
                            controller.changeRemember();
                          },
                        ),
                        Text(
                          "Remember Me",
                          style: TextStyle(color: themeColor),
                        ),
                      ],
                    ),
                    const SizedBox(
                      height: 15,
                    ),
                    CustomButton(
                      submit: (value) {
                        if (formKey.currentState!.validate()) {
                          if (commonController.isConnection == false) {
                            // CustomSnackbar.snackbar(context: context, msg: "No Internet Connection!", bgColor: errorColor, icon: Icons.wifi_off_rounded);
                            return;
                          }
                          UserLoginModel lmr = UserLoginModel(
                              userName: controller.username.text,
                              password: controller.password.text);
                          controller.login(lmr, context);
                        }
                      },
                      name: "Login",
                      fullWidth: true,
                    ),
                  ],
                ),
              ),
            ),
          ),
        );
      }),
    );
  }
}
