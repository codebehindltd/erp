import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:flutter_sslcommerz/model/SSLCTransactionInfoModel.dart';
import 'package:get/get.dart';
import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/utils/utils_function.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/req/reservation/Room_reservation_paymentInfo_save_model.dart';
import '../../../../data/models/req/reservation/hotel_room_reservation_details.dart';
import '../../../../data/models/req/reservation/room_reservation_info_Model.dart';
import '../../../../data/models/res/payment_st_list_model.dart';
import '../../../../data/models/res/property_model.dart';
import '../../../../data/models/res/reservation/room_type_info_model.dart';
import '../../../../data/services/reservation_service.dart';
import '../../../../global_widgets/modal_bottom_sheet.dart';
import '../../../../global_widgets/payment_gateway.dart';
import '../../../../routes/app_pages.dart';
import '../../../common/controllers/common_controller.dart';
import '../views/reservation_info_provide_modal_view.dart';

class MemberReservationController extends GetxController {
  var commonController = Get.find<CommonController>();
  TextEditingController vFromDateController = TextEditingController();
  TextEditingController vToDateController = TextEditingController();
  //Rx<TextEditingController>  vToDateController = TextEditingController().obs;
  // Rx<TextEditingController>  vFromDateController = TextEditingController().obs;
  TextEditingController reservationNoteController = TextEditingController();

  TextEditingController extraBedController = TextEditingController();
  TextEditingController qtyController = TextEditingController(text: "1");
  TextEditingController noteController = TextEditingController();

  TextEditingController nameController = TextEditingController();
  TextEditingController phoneController = TextEditingController();

  DateTime? fromDate;
  DateTime? toDate;

  final formKey = GlobalKey<FormState>();
  final formKeyForCustomerInfo = GlobalKey<FormState>();
  final itemForm = GlobalKey<FormState>();

  static var isLoading = true.obs;
  var roomTypeInfo = <RoomTypeInfoModel>[].obs;
  Rx<RoomTypeInfoModel?> selectedType = Rx<RoomTypeInfoModel?>(null);
  Rx<int?> selectedPax = Rx<int?>(null);
  Rx<int?> selectedChild = Rx<int?>(null);
  RxList<int> paxList = <int>[].obs;
  RxList<int> childList = <int>[].obs;
  Rx<int> totalDay = Rx<int>(0);
  Rx<double> itemTotal = Rx<double>(0); // room rate cost
  Rx<double> totalItemAmount = Rx<double>(0); //grand total
  Rx<double> itemDiscountAmount = Rx<double>(0);
  Rx<double> itemExtaBedAmount = Rx<double>(0);

  // var reservationItems = <ReservationItemCartModel>[].obs;
  var reservationDetailsItems = <RoomReservationDetail>[].obs;
  var totalAmount = Rx<double>(0.0);
  SSLCTransactionInfoModel? sslPaymentResult;
  String? reservationId;
  var isShowBackbtn = false;
  String? id;
  String userType = "";

  PropertyModel? property;
  String? propertyUrl;

  var paymentStStepList = <PaymentStListModel>[].obs;
  double limitAmount = 10000;

  @override
  void onInit() {
    isShowBackbtn = Get.arguments ?? false;
    getRoomTypeInfo();
    getProperty();
    super.onInit();
  }

  perparePamentList() {
    int step = (totalAmount.value / limitAmount).ceil();
    paymentStStepList.clear();
    for (var i = 0; i < step; i++) {
      paymentStStepList.add(PaymentStListModel(
          amount: totalAmount.value / step,
          isPaid: false,
          isButtonVisible: i == 0 ? true : false));
    }
  }

  void removeItem(int index) {
    reservationDetailsItems.removeAt(index);
    totalAmountCal();
  }

  reservationItemAdd() {
    RoomReservationDetail itemCartModel = RoomReservationDetail(
        roomType: selectedType.value!.roomType,
        roomTypeId: selectedType.value!.roomTypeId,
        // paxQuantity: selectedType.value!.paxQuantity,
        // childQuantity: selectedType.value!.childQuantity,

        paxQuantity: selectedPax.value,
        childQuantity: selectedChild.value,
        roomQuantity: int.parse(qtyController.text),
        extraBedQuantity: int.parse(
            extraBedController.text.isEmpty ? "0" : extraBedController.text),
        guestNotes: noteController.text,
        totalPrice:
            calItemTotal(withOutDiscount: false) + itemExtaBedAmount.value,
        regularPrice: calItemTotal() + itemExtaBedAmount.value,
        discountPercent: selectedType.value?.roomDiscountPercent,
        discountAmount: itemDiscountAmount.value,
        extraBedAmount: itemExtaBedAmount.value);

    reservationDetailsItems.add(itemCartModel);
    totalAmountCal();
    itemForm.currentState!.reset();

    itemFormClear();

    update();
    Get.back();
  }

  itemFormClear() {
    selectedPax.value = null;
    selectedChild.value = null;
    // qtyController.clear();
    extraBedController.clear();
    noteController.clear();
  }

  double calItemTotal({bool withOutDiscount = true}) {
    // double discountAmount = ((selectedType.value!.roomRate ?? 0) *
    //         int.parse(qtyController.text.isEmpty ? "0" : qtyController.text)) *
    //     selectedType.value!.roomDiscountPercent! /
    //     100;&&
    // withOutDiscount == false
    int calRm = totalDay.value;
    if (selectedType.value!.availableRoomNight! > 0) {
      int availableNight = selectedType.value!.availableRoomNight!;
      if (selectedType.value!.availableRoomNight! >
          selectedType.value!.maximumNightAvailPerReservation!) {
        availableNight = selectedType.value!.maximumNightAvailPerReservation!;
      } else {
        availableNight = selectedType.value!.availableRoomNight!;
      }
      calRm = totalDay.value - availableNight;
      calRm = calRm > 0 ? calRm : 0;
    }
    double total = (selectedType.value!.roomRate ?? 0) *
        calRm *
        int.parse(qtyController.text.isEmpty ? "0" : qtyController.text);
    //
    itemExtaBedAmount.value = totalDay.value *
        (int.parse(extraBedController.text.isEmpty
                ? "0"
                : extraBedController.text) *
            (selectedType.value!.extrabedRate ?? 0));
    var discountAmount =
        (total * selectedType.value!.roomDiscountPercent!) / 100;
    // total = total + itemExtaBedAmount.value;
    if (withOutDiscount) {
      return total;
    } else {
      return total - discountAmount;
    }
  }

  qtyOnChange(v) {
    itemTotal.value = calItemTotal();
    var gdTotal = calItemTotal(withOutDiscount: false);
    itemDiscountAmount.value = itemTotal.value - gdTotal;
    totalItemAmount.value = gdTotal + itemExtaBedAmount.value;
  }

  extraRmQtyOnChange(v) {
    itemTotal.value = calItemTotal();
    var gdTotal = calItemTotal(withOutDiscount: false);
    itemDiscountAmount.value = itemTotal.value - gdTotal;
    totalItemAmount.value = gdTotal + itemExtaBedAmount.value;
  }

  totalAmountCal() {
    totalAmount.value = reservationDetailsItems.isEmpty
        ? 0.0
        : reservationDetailsItems
                .map((item) => item.totalPrice)
                .reduce((v, e) => v! + e!) ??
            0.0;

    perparePamentList();
  }

  void selectRoomType(RoomTypeInfoModel? option, {context, screenHeight}) {
    if (toDate == null || fromDate == null) {
      Get.snackbar(
        "Warning",
        "Please select date.",
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      return;
    }
    selectedType.value = option!;
    preparePaxDropdownList();
    prepareChildDropdownList();
    qtyOnChange(true);

    ModalBottomSheet.showBottomSheet(context, ReservationInfoProvideModalView(),
        screenHeight: screenHeight
        //initialChildSize:0.7
        //topHeight: commonController.statusBarHeight
        );
  }

  preparePaxDropdownList() {
    int qty = selectedType.value!.paxQuantity ?? 0;
    List<int> list = [];
    for (int i = 1; i <= qty; i++) {
      list.add(i);
    }
    paxList.value = list;
  }

  prepareChildDropdownList() {
    int qty = selectedType.value!.childQuantity ?? 0;
    List<int> list = [];
    for (int i = 0; i <= qty; i++) {
      list.add(i);
    }
    childList.value = list;
  }

  selectPax(int? value) {
    selectedPax.value = value;
  }

  selectChild(int? value) {
    selectedChild.value = value;
  }

  void selectFromDate(context) async {
    if (reservationDetailsItems.isNotEmpty) {
      Get.snackbar(
        "Warning",
        "Please remove all selected item.",
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      return;
    }
    DateTime? datePick = await showDatePicker(
        useRootNavigator: false,
        context: context,
        initialDate: fromDate ??
            (toDate != null
                ? toDate!.subtract(const Duration(days: 1))
                : DateTime.now()),
        firstDate: DateTime.now(),
        lastDate: toDate != null
            ? toDate!.subtract(const Duration(days: 1))
            : DateTime(2024));

    if (datePick != null) {
      vFromDateController.text = dateFormat.format(datePick);
      fromDate = datePick;
    } else {
      vFromDateController.text = "";
      fromDate = null;
    }
    calTotlaDays();
    update();
  }

  void selectToDate(context) async {
    if (reservationDetailsItems.isNotEmpty) {
      Get.snackbar(
        "Warning",
        "Please remove all selected item.",
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      return;
    }
    var startDate = fromDate != null
        ? fromDate!.add(const Duration(days: 1))
        : DateTime.now().add(const Duration(days: 1));
    DateTime? datePick = await showDatePicker(
        useRootNavigator: false,
        context: context,
        initialDate: toDate ?? startDate,
        firstDate: startDate,
        lastDate: DateTime(2024));
    if (datePick != null) {
      vToDateController.text = dateFormat.format(datePick);
      toDate = datePick;
    } else {
      vToDateController.text = "";
      toDate = null;
    }
    calTotlaDays();
    update();
  }

  calTotlaDays() {
    if (fromDate != null && toDate != null) {
      Duration days = Duration(days: toDate!.difference(fromDate!).inDays);
      totalDay.value = days.inDays;
    }
  }

  getRoomTypeInfo() async {
    //isLoading.value = true;
    id = await SharedPfnDBHelper.getGuestOrMemberId();
    userType = await SharedPfnDBHelper.getUserType() ?? "";
    await ReservationService.fetchRoomTypeInfo(userType, id ?? "0")
        .then((value) {
      roomTypeInfo.value = value;
      isLoading(false);
    }).onError((error, stackTrace) {
      isLoading(false);
    });
  }

  next(v) {
    if (!formKey.currentState!.validate()) {
      return;
    }
    if (reservationDetailsItems.isEmpty) {
      Get.snackbar(
        "Warning",
        "Please select a room.",
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      return;
    }
    //Get.toNamed(Routes.guestReservation + Routes.setCustomerInfo);
    saveRoomReservationInfo(true);
  }

  addAndcheckAvailableRoomInfo(v) async {
    if (!itemForm.currentState!.validate()) {
      return;
    }
    if (!(int.parse(qtyController.text.isEmpty ? "0" : qtyController.text) >
        0)) {
      Get.snackbar(
        "Warning",
        "Minimum one room select",
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      return;
    }
    EasyLoading.show();
    await ReservationService.checkAvailableRoomInfo(
            selectedType.value!.roomTypeId!,
            vFromDateController.value.text,
            vToDateController.value.text)
        .then((value) {
      EasyLoading.dismiss();
      if (value >= int.parse(qtyController.text)) {
        // Get.toNamed(Routes.guestReservation + Routes.setCustomerInfo);
        reservationItemAdd();
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
    // if (!formKeyForCustomerInfo.currentState!.validate()) {
    //   return;
    // }

    RoomReservationInfoModel roomReservationInfoModel =
        RoomReservationInfoModel(
            fromDate: vFromDateController.text,
            toDate: vToDateController.text,
            guestName: "",
            phoneNumber: "",
            guestRemarks: reservationNoteController.text,
            transactionType: UserTypeEnum.member.value,
            memberId: int.parse(commonController.guestOrMemberId!),
            roomReservationDetails: reservationDetailsItems);

    EasyLoading.show(status: 'Loading');
    ReservationService.roomReservationInfoPost(roomReservationInfoModel)
        .then((value) async {
      EasyLoading.dismiss();
      reservationId = value?.reservationId.toString();

      // payment
      double amount = 0;
      amount = totalAmount.value;
      // if (amount == 0) {
      //   Get.offAllNamed(
      //       Routes.memberReservation + Routes.memberReservationSuccessView);
      //   return;
      // }

      if (limitAmount < amount) {
        Get.toNamed(
            Routes.memberReservation + Routes.paymentGraterThenFiveLacView);
        return;
      }

      SSLCTransactionInfoModel? sslPaymentResult =
          await PaymentGateway.sslCommerzGeneralCall(amount);
      if (sslPaymentResult!.status!.toLowerCase() == "valid") {
        saveRoomReservationPaymentData(sslPaymentResult);
      } else {
        Get.offAllNamed(
            Routes.memberReservation + Routes.memberReservationSuccessView);
      }
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }

  void payWithStalment(amount, index) async {
    SSLCTransactionInfoModel? sslPaymentResult =
        await PaymentGateway.sslCommerzGeneralCall(amount);
    if (sslPaymentResult!.status!.toLowerCase() == "valid") {
      saveRoomReservationPaymentData(sslPaymentResult, isRoute: false);
      paymentStStepList[index].isPaid = true;
      paymentStStepList[index].isButtonVisible = false;
      if (index + 1 == paymentStStepList.length) {
        Get.offAllNamed(
            Routes.memberReservation + Routes.memberReservationSuccessView);
      } else {
        paymentStStepList[index + 1].isButtonVisible = true;
      }
      update();
    }
  }

  saveRoomReservationPaymentData(SSLCTransactionInfoModel model,
      {bool isRoute = true}) {
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
      if (isRoute) {
        Get.offAllNamed(
            Routes.memberReservation + Routes.memberReservationSuccessView);
      }
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      if (isRoute) {
        Get.offAllNamed(
            Routes.memberReservation + Routes.memberReservationSuccessView);
      }
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }

  getProperty() async {
    property = await SharedPfnDBHelper.getProperty();
    propertyUrl = await Env.propertryUrl();
  }
}