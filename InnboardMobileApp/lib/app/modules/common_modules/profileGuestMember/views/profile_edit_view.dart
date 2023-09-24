import 'dart:convert';
import 'dart:io';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../controllers/profile_guest_member_controller.dart';

class ProfileEditView extends GetView<ProfileGuestMemberController> {
  const ProfileEditView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(' Edit Profile'),
      ),
      body: GetBuilder<ProfileGuestMemberController>(builder: (_) {
        return Obx(() => Container(
              child: controller.isLoading.value
                  ? const Center(child: CircularProgressIndicator())
                  : ListView(
                      children: [
                        const SizedBox(
                          height: 8,
                        ),
                        Center(
                          child: Stack(children: [
                            ClipRRect(
                              borderRadius: BorderRadius.circular(100),
                              child: (controller.userInfo.value?.imagePath !=
                                              null &&
                                          controller
                                                  .userInfo.value?.imageName !=
                                              null) &&
                                      controller.image == null
                                  ? Image.network(
                                      "${Env.rootUrl}${controller.userInfo.value?.imagePath ?? ''}${controller.userInfo.value?.imageName ?? ''}",
                                      //"",
                                      height: 156,
                                      width: 156,
                                       fit: BoxFit.cover,
                                      loadingBuilder: (context, Widget child,
                                          ImageChunkEvent? loadingProgress) {
                                        if (loadingProgress == null) {
                                          return child;
                                        }
                                        return Image.asset(
                                          "assets/images/profile.png",
                                          height: 156,
                                          width: 156,
                                          fit: BoxFit.cover,
                                        );
                                      },
                                      errorBuilder:
                                          (context, error, stackTrace) {
                                        return Image.asset(
                                          "assets/images/profile.png",
                                          height: 156,
                                          width: 156,
                                          fit: BoxFit.cover,
                                        );
                                      },
                                    )
                                  : controller.imagePath != null
                                      ? ClipRRect(
                                          borderRadius:
                                              BorderRadius.circular(100),
                                          child: Image.file(
                                            File(
                                              controller.imagePath!,
                                            ),
                                            height: 156,
                                            width: 156,
                                            fit: BoxFit.cover,
                                          ),
                                        )
                                      : controller.imagePath != null
                                          ? Image.memory(
                                              base64Decode(
                                                  controller.imagePath!),
                                              height: 156,
                                              width: 156,
                                              fit: BoxFit.cover,
                                            )
                                          : Image.asset(
                                              "assets/images/profile.png",
                                              height: 156,
                                              width: 156,
                                              fit: BoxFit.cover,
                                            ),
                            ),
                            Positioned(
                              bottom: 30,
                              right: 0,
                              child: GestureDetector(
                                onTap: () {
                                  _showImagePicker(context);
                                },
                                child: Container(
                                  width: 32,
                                  height: 32,
                                  decoration: BoxDecoration(
                                    borderRadius: BorderRadius.circular(22),
                                    color: themeColor,
                                  ),
                                  child: const Icon(
                                    Icons.add_a_photo_outlined,
                                    size: 16,
                                    color: Colors.white,
                                  ),
                                ),
                              ),
                            )
                          ]),
                        ),
                        Padding(
                          padding: const EdgeInsets.all(18),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Form(
                                key: controller.formKeyForEditProfile,
                                child: InputField(
                                    controllerText:
                                        controller.nameEditController,
                                    keyboardType: TextInputType.text,
                                    validationText: "Please enter name",
                                    autovalidateMode:
                                        AutovalidateMode.onUserInteraction,
                                    labelText: "Name",
                                    isReadOnly: false),
                              ),
                              const SizedBox(
                                height: 10,
                              ),

                              InputField(
                                  controllerText:
                                      controller.phoneEditController,
                                  keyboardType: TextInputType.text,
                                  validationText: "Please enter phone number",
                                  autovalidateMode:
                                      AutovalidateMode.onUserInteraction,
                                  labelText: "Phone Number",
                                  isReadOnly: true),
                              const SizedBox(
                                height: 10,
                              ),

                              InputField(
                                  controllerText:
                                      controller.nationalityEditController,
                                  keyboardType: TextInputType.text,
                                  validationText: "Please enter nationality",
                                  autovalidateMode:
                                      AutovalidateMode.onUserInteraction,
                                  labelText: "Nationality",
                                  isReadOnly: true),
                              const SizedBox(
                                height: 10,
                              ),

                              InputField(
                                  controllerText:
                                      controller.nationalIdEditController,
                                  keyboardType: TextInputType.text,
                                  validationText: "Please enterNational Id",
                                  autovalidateMode:
                                      AutovalidateMode.onUserInteraction,
                                  labelText: "National Id",
                                  isReadOnly: true),
                              const SizedBox(
                                height: 10,
                              ),

                              InputField(
                                  controllerText:
                                      controller.passportEditController,
                                  keyboardType: TextInputType.text,
                                  validationText:
                                      "Please enter Passport number",
                                  autovalidateMode:
                                      AutovalidateMode.onUserInteraction,
                                  labelText: "Passport Number",
                                  isReadOnly: true),
                              const SizedBox(
                                height: 10,
                              ),

                              InputField(
                                controllerText: controller.emailEditController,
                                keyboardType: TextInputType.text,
                                validationText: "Please enter email",
                                autovalidateMode:
                                    AutovalidateMode.onUserInteraction,
                                labelText: "Email",
                                isReadOnly: true,
                              ),
                              const SizedBox(
                                height: 10,
                              ),

                              InputField(
                                  controllerText:
                                      controller.addressEditController,
                                  keyboardType: TextInputType.text,
                                  validationText: "Please enter Address",
                                  autovalidateMode:
                                      AutovalidateMode.onUserInteraction,
                                  labelText: "Address",
                                  isReadOnly: true),
                              const SizedBox(
                                height: 20,
                              ),

                              // buildLogOutBtn(context),
                            ],
                          ),
                        )
                      ],
                    ),
            ));
      }),
      bottomNavigationBar: Padding(
        padding: const EdgeInsets.all(12.0),
        child: Container(
          height: 48,
          child: CustomButton(
              submit: controller.updateProfileData,
              name: "Save",
              fullWidth: true),
        ),
      ),
    );
  }

  void _showImagePicker(context) {
    showModalBottomSheet(
        context: context,
        builder: (BuildContext bc) {
          return SafeArea(
            child: Container(
              child: Wrap(
                children: <Widget>[
                  ListTile(
                      leading: const Icon(Icons.photo_library),
                      title: const Text('Photo Library'),
                      onTap: () {
                        controller.imgFromGallery();
                        Navigator.of(context).pop();
                      }),
                  ListTile(
                    leading: const Icon(Icons.photo_camera),
                    title: const Text('Camera'),
                    onTap: () {
                      controller.imgFromCamera();
                      Navigator.of(context).pop();
                    },
                  ),
                ],
              ),
            ),
          );
        });
  }
}
