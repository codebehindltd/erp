import 'dart:convert';
import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import 'package:image_picker/image_picker.dart';
import 'package:leading_edge/app/data/services/common_service.dart';
import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/req/profile_update_model.dart';
import '../../../../data/models/res/guest_member_user_model.dart';
import '../../../../data/services/account_service.dart';
import '../../../../routes/app_pages.dart';
import '../../../common/controllers/common_controller.dart';

class ProfileGuestMemberController extends GetxController {
  Rx<GuestMemberUserModel?> userInfo = Rx<GuestMemberUserModel?>(null);
  String? id;
  String userType = "";
  var isLoading = Rx<bool>(true);
  // var testV = 0.obs;
  var commonController = Get.find<CommonController>();
  TextEditingController gNameController = TextEditingController();
  TextEditingController gPhoneController = TextEditingController();
  final formKeyForGuestInfo = GlobalKey<FormState>();

  final formKeyForEditProfile = GlobalKey<FormState>();
  // UserModel? userModel;

  TextEditingController nameEditController = TextEditingController();
  TextEditingController phoneEditController = TextEditingController();
  TextEditingController nationalityEditController = TextEditingController();
  TextEditingController nationalIdEditController = TextEditingController();
  TextEditingController passportEditController = TextEditingController();
  TextEditingController emailEditController = TextEditingController();
  TextEditingController addressEditController = TextEditingController();

  @override
  void onReady() {
    getUserInfo();
    super.onReady();
  }

  logOut() async {
    SharedPfnDBHelper.removeLoginUserData();
    commonController.isLogin = false;
    commonController.guestOrMemberId = null;
    Get.offAllNamed(Routes.splash);
  }

  getUserInfo() async {
    id = await SharedPfnDBHelper.getGuestOrMemberId();
    userType = await SharedPfnDBHelper.getUserType() ?? "";
    if (id == null) {
      isLoading.value = false;
      return;
    }
    CommonService.getGuestOrMemberProfile(userType, id!).then(
      (value) {
        userInfo.value = value;
        isLoading.value = false;
        imagePath = userInfo.value?.imageName ?? '';
        nameEditController.text = userInfo.value!.guestName.toString();
        phoneEditController.text = userInfo.value!.guestPhone.toString();
        nationalityEditController.text =
            userInfo.value!.guestNationality != null
                ? userInfo.value!.guestNationality.toString()
                : "";
        nationalIdEditController.text = userInfo.value!.nationalId != null
            ? userInfo.value!.nationalId.toString()
            : "";
        passportEditController.text = userInfo.value!.passportNumber != null
            ? userInfo.value!.passportNumber.toString()
            : "";
        emailEditController.text = userInfo.value!.guestEmail != null
            ? userInfo.value!.guestEmail.toString()
            : "";
        addressEditController.text = userInfo.value!.guestAddress != null
            ? userInfo.value!.guestAddress.toString()
            : "";
      },
    ).onError((error, stackTrace) {
      isLoading.value = false;
      Get.snackbar(
        "Error!",
        error.toString(),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
    });
  }

  guestLoginInfoSubmit(v) {
    if (!formKeyForGuestInfo.currentState!.validate()) {
      return;
    }
    EasyLoading.show(status: 'Loading');
    AccountService.guestOrMemberLogin(UserTypeEnum.guest.value,
            gNameController.text, gPhoneController.text)
        .then((value) async {
      String guestOrMemberId = value.guestOrMemberId.toString();
      await SharedPfnDBHelper.saveGuestOrMemberId(guestOrMemberId);
      await commonController.checkMemberOrGuestLogin();
      EasyLoading.dismiss();
      //EasyLoading.showSuccess('Login Succesfully Done!');
      Get.rawSnackbar(
        message: "Login Succesfully Done!",
        backgroundColor: themeColor,
      );
      getUserInfo();
      // update();
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }

  XFile? image;
  final ImagePicker _picker = ImagePicker();
  String? imagePath;

  imgFromCamera() async {
    try {
      image =
          await _picker.pickImage(imageQuality: 25, source: ImageSource.camera);

      imagePath = image?.path;
      update();
    } catch (error) {
      print(error.toString());
    }
  }

  imgFromGallery() async {
    try {
      image = await _picker.pickImage(
          imageQuality: 25, source: ImageSource.gallery);

      print(image);

      imagePath = image?.path;
      update();
    } catch (error) {
      print(error.toString());
    }
  }

  updateProfileData(v) {
    if (!formKeyForEditProfile.currentState!.validate()) {
      return;
    }
    EasyLoading.show(status: 'Loading');
    //img convert
  
    String? imageByte = image!=null? base64Encode(File(image!.path).readAsBytesSync()):null;

    ProfileUpdateModel profileUpdateModel = ProfileUpdateModel(
        transactionType: userType,
        memberId: userInfo.value!.profileId.toString(),
        fullName: nameEditController.text,
        passportNumber: passportEditController.text,
        personalEmail: emailEditController.text,
        memberAddress: addressEditController.text,
        memberAppsProfilePicture:
            image == null ? userInfo.value!.imageName : image?.name,
        memberAppsProfilePictureByte: imageByte);

    CommonService.profileUpdate(profileUpdateModel).then((value) async {
      EasyLoading.dismiss();

      Get.rawSnackbar(
        message: "Profile update Succesfully",
        backgroundColor: themeColor,
      );

      getUserInfo();
      //  Get.back();

      Get.offNamed(Routes.memberDashboard);
      update();
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }
}
