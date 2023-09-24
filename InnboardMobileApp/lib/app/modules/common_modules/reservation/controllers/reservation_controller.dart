import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:flutter_sslcommerz/model/SSLCTransactionInfoModel.dart';
import 'package:get/get.dart';
import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/utils/utils_function.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/models/req/reservation/Room_reservation_paymentInfo_save_model.dart';
import '../../../../data/models/req/reservation/room_reservation_info_Model.dart';
import '../../../../data/models/res/reservation/room_type_info_model.dart';
import '../../../../data/services/reservation_service.dart';
import '../../../../global_widgets/payment_gateway.dart';
import '../../../../routes/app_pages.dart';

class ReservationController extends GetxController {
  TextEditingController vFromDateController = TextEditingController();
  TextEditingController vToDateController = TextEditingController();
  TextEditingController paxController = TextEditingController();
  TextEditingController childController = TextEditingController();
  TextEditingController extraBedController = TextEditingController();
  TextEditingController noteController = TextEditingController();

  TextEditingController nameController = TextEditingController();
  TextEditingController phoneController = TextEditingController();

  DateTime? fromDate;
  DateTime? toDate;
  final formKey = GlobalKey<FormState>();
  final formKeyForCustomerInfo = GlobalKey<FormState>();

  static var isLoading = true.obs;
  var roomTypeInfo = <RoomTypeInfoModel>[].obs;
  Rx<RoomTypeInfoModel?> selectedType = Rx<RoomTypeInfoModel?>(null);

  void selectRoomType(RoomTypeInfoModel? option) {
    selectedType.value = option!;
    paxController.text = selectedType.value!.paxQuantity.toString();
    childController.text = selectedType.value!.childQuantity!.toString();
  }

  SSLCTransactionInfoModel? sslPaymentResult;
  String? reservationId;

  @override
  void onInit() {
    getRoomTypeInfo();

    super.onInit();
  }

  void selectFromDate(DateTime? datePick) {
    if (datePick != null) {
      vToDateController.text = dateFormat.format(datePick);
      fromDate = datePick;
    } else {
      vToDateController.text = "";
      fromDate = null;
    }
    update();
  }

  void selectToDate(DateTime? datePick) {
    if (datePick != null) {
      vFromDateController.text = dateFormat.format(datePick);
      toDate = datePick;
    } else {
      vFromDateController.text = "";
      toDate = null;
    }
    update();
  }

  getRoomTypeInfo() async {
    // isLoading(true);
    // await ReservationService.fetchRoomTypeInfo().then((value) {
    //   roomTypeInfo.value = value;
    //   isLoading(false);
    // }).onError((error, stackTrace) {
    //   isLoading(false);
    //   print('Error: $error');
    // });
  }

  checkAvailableRoomInfo(v) async {
    if (!formKey.currentState!.validate()) {
      return;
    }
    EasyLoading.show();
    await ReservationService.checkAvailableRoomInfo(
            selectedType.value!.roomTypeId!,
            vFromDateController.text,
            vToDateController.text)
        .then((value) {
      EasyLoading.dismiss();
      if (value > 0) {
        Get.toNamed(Routes.reservation + Routes.setCustomerInfo);
      } else {
        Get.snackbar(
          "Warning",
          "Room are not available",
          snackPosition: SnackPosition.BOTTOM,
          backgroundColor: warningColor,
        );
      }
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.snackbar(
        "Error!",
        error.toString(),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      // error msge show
    });
  }

  Future<void> saveRoomReservationInfo(v) async {
    if (!formKeyForCustomerInfo.currentState!.validate()) {
      return;
    }
    RoomReservationInfoModel roomReservationInfoModel =
        RoomReservationInfoModel(
      fromDate: vFromDateController.text,
      toDate: vToDateController.text,
      // roomTypeId: selectedType.value!.roomTypeId,
      // paxQuantity: selectedType.value!.paxQuantity,
      // childQuantity: selectedType.value!.childQuantity,
      // extraBedQuantity: extraBedController.text,
      // guestNotes: noteController.text,
      guestName: nameController.text,
      phoneNumber: phoneController.text,
      transactionType: UserTypeEnum.guest.value,
    );
    EasyLoading.show(status: 'Loading');
    ReservationService.roomReservationInfoPost(roomReservationInfoModel)
        .then((value) async {
      EasyLoading.dismiss();
      reservationId = value?.reservationId.toString();
      print(reservationId);

      // payment
      double amount = 0;
      amount = selectedType.value!.roomRate!;

      SSLCTransactionInfoModel? sslPaymentResult =
          await PaymentGateway.sslCommerzGeneralCall(amount);
      if (sslPaymentResult!.status!.toLowerCase() == "valid") {
        saveRoomReservationPaymentData(sslPaymentResult);
      } else {
        Get.offAllNamed(Routes.reservation + Routes.reservationSuccessScreen);
      }
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }

  // Future<bool> sslCommerzGeneralCall(double amount) async {
  //   Sslcommerz sslcommerz = Sslcommerz(
  //     initializer: SSLCommerzInitialization(
  //       currency: SSLCurrencyType.BDT,
  //       product_category: "product",
  //       store_id: Env.sslcStoreId,
  //       store_passwd: Env.sslcStorePasswd,
  //       sdkType: 1 == 2 ? SSLCSdkType.LIVE : SSLCSdkType.TESTBOX,
  //       total_amount: amount,
  //       tran_id: DateTime.now().millisecondsSinceEpoch.toRadixString(16),
  //     ),
  //   );
  //   try {
  //     sslPaymentResult = await sslcommerz.payNow();
  //     if (sslPaymentResult!.status!.toLowerCase() == "failed") {
  //       Get.rawSnackbar(
  //         message: "Transaction is Failed.",
  //         backgroundColor: errorColor,
  //       );
  //       return false;
  //     } else if (sslPaymentResult!.status!.toLowerCase() == "closed") {
  //       Get.rawSnackbar(
  //         message: "Transaction closed by user.",
  //         backgroundColor: errorColor,
  //       );
  //       return false;
  //     } else if (sslPaymentResult!.status!.toLowerCase() == "valid") {
  //       EasyLoading.showSuccess("Transaction successfully done.");
  //       Get.rawSnackbar(
  //         message: "Transaction successfully done.",
  //         backgroundColor: themeColor,
  //       );
  //       return true;
  //     } else {
  //       Get.rawSnackbar(
  //         message: "Transaction is Failed.",
  //         backgroundColor: errorColor,
  //       );
  //       return false;
  //     }
  //   } catch (e) {
  //     Get.rawSnackbar(
  //       message: e.toString(),
  //       backgroundColor: errorColor,
  //     );
  //     return false;
  //   }
  // }

  saveRoomReservationPaymentData(SSLCTransactionInfoModel model) {
    RoomReservationPaymentInfoSaveModel data =
        RoomReservationPaymentInfoSaveModel(
      reservationId: int.parse(reservationId!),
      transactionType: "SSLCOM",
      transactionId: model.tranId!,
      transactionAmount: double.parse(model.amount!),
      transactionDetails: "Payment Posting By Mobile Apps Reservation",
      createdBy: 0,
    );
    EasyLoading.show();
    ReservationService.customerPayment(data).then((value) {
      EasyLoading.dismiss();
      Get.offAllNamed(Routes.reservation + Routes.reservationSuccessScreen);
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.offAllNamed(Routes.reservation + Routes.reservationSuccessScreen);
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }
}
