import 'dart:convert';
import 'dart:io';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../../../../routes/app_pages.dart';
import '../controllers/profile_guest_member_controller.dart';

class ProfileGuestMemberView extends GetView<ProfileGuestMemberController> {
  const ProfileGuestMemberView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
          elevation: 0,
          title: const Text(
            "My Profile",
            style: TextStyle(color: Colors.white),
          ),
        ),
        body: GetBuilder<ProfileGuestMemberController>(builder: (_) {
          return Obx(() => Container(
                child: controller.isLoading.value
                    ? const Center(child: CircularProgressIndicator())
                    : controller.userInfo.value == null &&
                            controller.isLoading.value == false
                        ? Form(
                            key: controller.formKeyForGuestInfo,
                            child: buildLogin(context))
                        // const Center(
                        //     child: Text(
                        //       "Guest User",
                        //       style: TextStyle(fontSize: 18),
                        //     ),
                        //   )
                        : ListView(
                            children: [
                              const SizedBox(
                                height: 8,
                              ),
                              Stack(children: [
                                Container(
                                  //color: Colors.red,
                                  child: Center(
                                    child: ClipRRect(
                                      borderRadius: BorderRadius.circular(100),
                                      child: controller.userInfo.value
                                                      ?.imagePath !=
                                                  null &&
                                              controller.userInfo.value
                                                      ?.imageName !=
                                                  null
                                          ? Image.network(
                                              "${Env.rootUrl}${controller.userInfo.value?.imagePath ?? ''}${controller.userInfo.value?.imageName ?? ''}",
                                              //"",
                                              height: 156,
                                              width: 156,
                                              fit: BoxFit.cover,
                                              loadingBuilder: (context,
                                                  Widget child,
                                                  ImageChunkEvent?
                                                      loadingProgress) {
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
                                                      BorderRadius.circular(
                                                          100),
                                                  child: Image.file(
                                                    File(
                                                      controller.imagePath!,
                                                    ),
                                                    height: 160,
                                                    width: 160,
                                                    fit: BoxFit.cover,
                                                  ),
                                                )
                                              : controller.imagePath != null
                                                  ? Image.memory(
                                                      base64Decode(controller
                                                          .imagePath!),
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
                                  ),
                                ),
                                Positioned(
                                  top: 10,
                                  right: 12,
                                  child: GestureDetector(
                                    onTap: () {
                                      controller.image = null;
                                      Get.toNamed(
                                        Routes.profileGuestMember +
                                            Routes.profileEditView,
                                      );
                                    },
                                    child: Container(
                                      width: 44,
                                      height: 28,
                                      decoration: BoxDecoration(
                                        border: Border.all(
                                            color: themeColor, width: .5),
                                        borderRadius: BorderRadius.circular(8),
                                        color: Colors.white,
                                      ),
                                      child: Icon(
                                        Icons.edit,
                                        size: 22,
                                        color: themeColor,
                                      ),
                                    ),
                                  ),
                                )
                              ]),
                              ListTile(
                                title: Center(
                                  child: Text(
                                    controller.userInfo.value?.guestName ?? '',
                                    style: const TextStyle(
                                        fontSize: 18,
                                        fontWeight: FontWeight.bold),
                                  ),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.all(20),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    const Text(
                                      "Phone Number",
                                      style: TextStyle(color: Colors.grey),
                                    ),
                                    Text(
                                      controller.userInfo.value?.guestPhone ??
                                          '',
                                      style: const TextStyle(
                                          fontSize: 16,
                                          fontWeight: FontWeight.w500),
                                    ),
                                    const Divider(),
                                    const SizedBox(
                                      height: 10,
                                    ),
                                    const Text(
                                      "Nationality",
                                      style: TextStyle(color: Colors.grey),
                                    ),
                                    Text(
                                      controller.userInfo.value
                                              ?.guestNationality ??
                                          '',
                                      style: const TextStyle(
                                          fontSize: 16,
                                          fontWeight: FontWeight.w500),
                                    ),
                                    const Divider(),
                                    const SizedBox(
                                      height: 10,
                                    ),
                                    const Text(
                                      "National Id",
                                      style: TextStyle(color: Colors.grey),
                                    ),
                                    Text(
                                      controller.userInfo.value?.nationalId ??
                                          '',
                                      style: const TextStyle(
                                          fontSize: 16,
                                          fontWeight: FontWeight.w500),
                                    ),
                                    const Divider(),
                                    const SizedBox(
                                      height: 10,
                                    ),
                                    const Text(
                                      "Passport Number",
                                      style: TextStyle(color: Colors.grey),
                                    ),
                                    Text(
                                      controller
                                              .userInfo.value?.passportNumber ??
                                          '',
                                      style: const TextStyle(
                                          fontSize: 16,
                                          fontWeight: FontWeight.w500),
                                    ),
                                    const Divider(),
                                    const SizedBox(
                                      height: 10,
                                    ),
                                    const Text(
                                      "Email",
                                      style: TextStyle(color: Colors.grey),
                                    ),
                                    Text(
                                      controller.userInfo.value?.guestEmail ??
                                          '',
                                      style: const TextStyle(
                                          fontSize: 16,
                                          fontWeight: FontWeight.w500),
                                    ),
                                    const Divider(),
                                    const SizedBox(
                                      height: 10,
                                    ),
                                    const Text(
                                      "Address",
                                      style: TextStyle(color: Colors.grey),
                                    ),
                                    Text(
                                      controller.userInfo.value?.guestAddress ??
                                          '',
                                      style: const TextStyle(
                                          fontSize: 16,
                                          fontWeight: FontWeight.w500),
                                    ),
                                    const Divider(),
                                    const SizedBox(
                                      height: 20,
                                    ),
                                    const Divider(),
                                    buildLogOutBtn(context),
                                  ],
                                ),
                              )
                            ],
                          ),
              ));
        }));
  }

  Widget buildLogOutBtn(context) {
    return InkWell(
      onTap: () {
        controller.logOut();
      },
      child: SizedBox(
        // padding: const EdgeInsets.symmetric(vertical: 25, horizontal: 20),
        width: double.infinity,
        // color: Colors.amber,

        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 22, vertical: 8),
          decoration: BoxDecoration(
              color: bodyColor,
              borderRadius: BorderRadius.circular(8),
              boxShadow: [
                const BoxShadow(
                  color: whiteColor,
                  spreadRadius: 2,
                  blurRadius: 6,
                  offset: Offset(-5, -5),
                ),
                BoxShadow(
                  color: bottonShadowColor.withOpacity(.5),
                  spreadRadius: 1,
                  blurRadius: 5,
                  offset: const Offset(4, 4),
                )
              ]),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                Icons.logout,
                color: themeColor,
              ),
              const SizedBox(
                width: 10,
              ),
              Text(
                'Log out',
                style: TextStyle(
                  // color: Colors.white,
                  // letterSpacing: 1.5,
                  fontSize: 18,
                  color: themeColor,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget buildLogin(context) {
    return SingleChildScrollView(
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            const SizedBox(
              height: 40,
            ),
            Text(
              "Enter Your Infomation",
              style: TextStyle(
                  color: themeColor, fontSize: 24, fontWeight: FontWeight.w700),
            ),
            const SizedBox(
              height: 80,
            ),
            InputField(
              controllerText: controller.gNameController,
              keyboardType: TextInputType.text,
              validationText: "Please enter name",
              autovalidateMode: AutovalidateMode.onUserInteraction,
              labelText: "Name",
            ),
            const SizedBox(
              height: 10,
            ),
            InputField(
              controllerText: controller.gPhoneController,
              keyboardType: TextInputType.number,
              isPhoneNumber: true,
              validationText: "Please enter phone number",
              autovalidateMode: AutovalidateMode.onUserInteraction,
              labelText: "Phone Number",
              maxLength: 11,
            ),
            const SizedBox(
              height: 90,
            ),
            CustomButton(
                submit: controller.guestLoginInfoSubmit,
                name: "Submit",
                fullWidth: true)
          ],
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
