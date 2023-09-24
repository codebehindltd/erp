import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:flutter_sslcommerz/model/SSLCTransactionInfoModel.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/enums/user_type_enum.dart';
import 'package:leading_edge/app/core/environment/environment.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:leading_edge/app/data/models/req/members/member_registration_mode.dart';
import 'package:leading_edge/app/data/models/res/members/membership_type.dart';
import 'package:leading_edge/app/data/services/member_service.dart';
import '../../../../core/enums/payment_type_enum.dart';
import '../../../../data/models/req/members/member_payment_save_model.dart';
import '../../../../data/models/res/property_model.dart';
import '../../../../data/services/common_service.dart';
import '../../../../global_widgets/payment_gateway.dart';
import '../../../../routes/app_pages.dart';

class MemberRegistrationController extends GetxController {
  TextEditingController nameControler = TextEditingController();
  TextEditingController mobileControler = TextEditingController();
  TextEditingController emailControler = TextEditingController();

  Rx<MemberShipType> selectedType = MemberShipType().obs;

  static var isLoading = true.obs;

  var membershipSetupData = <MemberShipType>[].obs;

  void setSelectedOption(MemberShipType? option) {
    selectedType.value = option!;
    //checkLng();
  }

  RxInt selectedPaymentType = 0.obs;
  String? memberId;
  SSLCTransactionInfoModel? sslPaymentResult;

  PropertyModel? propertyData;
  void selectValue(int value) {
    selectedPaymentType.value = value;
  }

  @override
  void onInit() {
    getMembershipTypes();
    getPropertyList();
    super.onInit();
  }


  getPropertyList() {
    CommonService.getPropertyList(UserTypeEnum.member.value).then((value) async {
      propertyData= value.where((element) => element.endPointIp==Env.rootUrl).first;
    }).onError((error, stackTrace) {
     
    });
  }

  getMembershipTypes() async {
    await MemberService.getMembershipSetupData().then((value) {
      membershipSetupData.value = value;
      selectedType.value = membershipSetupData.first;
      //checkLng();
      isLoading(false);
    }).onError((error, stackTrace) {
      isLoading(false);
      print('Error: $error');
    });
  }

  checkLng() {
    lng.value = selectedType.value.terms!.length >= 2
        ? 2
        : selectedType.value.terms!.length;
  }

  Rx<int> lng = 2.obs;
  void toggleExpanded(bool isShow) {
    lng.value = isShow ? selectedType.value.terms!.length : 2;
  }

  void signUp() async {
    MemberRegistrationModel memberRegistrationModel = MemberRegistrationModel(
        typeId: selectedType.value.typeId,
        fullName: nameControler.text,
        mobileNumber: mobileControler.text,
        personalEmail: emailControler.text,
        createdBy: 0);
    EasyLoading.show(status: 'Loading');
    MemberService.memberRegistration(memberRegistrationModel)
        .then((value) async {
      EasyLoading.dismiss();
      memberId = value;
      // payment
      double amount = 0;
      if (selectedPaymentType.value == PaymentTypeEnum.fullPayment.value) {
        amount = selectedType.value.subscriptionFee!;
      } else if (selectedPaymentType.value ==
          PaymentTypeEnum.instalment.value) {
        amount = selectedType.value.minimumInstallmentSubscriptionFee!;
      }
     
      SSLCTransactionInfoModel? sslPaymentResult =
          await PaymentGateway.sslCommerzGeneralCall(amount, propertyData: propertyData);
      if (sslPaymentResult!.status!.toLowerCase() == "valid") {
        savePaymentData(sslPaymentResult);

      } else {
        Get.offAllNamed(Routes.memberRegistration + Routes.successScreen);
      }
      //Get.toNamed(Routes.memberRegistration + Routes.successScreen);
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }


  savePaymentData(SSLCTransactionInfoModel model) {
    MemberPaymentSaveModel data = MemberPaymentSaveModel(
        memberId: int.parse(memberId!),
        transactionType: "SSLCOM",
        transactionDetails: "Payment Posting By Mobile Apps Registration",
        securityDeposit: double.parse(model.amount!),
        transactionId: model.tranId!,
        createdBy: 0);
    EasyLoading.show();
    MemberService.memberPayment(data).then((value) {
      EasyLoading.dismiss();
      Get.offAllNamed(Routes.memberRegistration + Routes.successScreen);
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.offAllNamed(Routes.memberRegistration + Routes.successScreen);
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }
}
