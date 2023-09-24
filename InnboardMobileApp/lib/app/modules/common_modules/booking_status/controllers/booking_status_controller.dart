import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/utils/utils_function.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/res/guest_member_user_model.dart';
import '../../../../core/environment/environment.dart';
import '../../../../data/models/res/property_model.dart';
import '../../../../data/models/res/reservation/reservation_list_model.dart';
import '../../../../data/services/account_service.dart';
import '../../../../data/services/common_service.dart';
import '../../../../data/services/reservation_service.dart';
import '../../../../routes/app_pages.dart';
import '../../../common/controllers/common_controller.dart';

class BookingStatusController extends GetxController {
  String? id;
  String userType = "";
  var isLoading = true.obs;
  var isLoadingForBookingData = true.obs;
  RxList<ReservationListModel> dataList = RxList<ReservationListModel>([]);

  TextEditingController vFromDateController = TextEditingController();
  TextEditingController vToDateController = TextEditingController();

  DateTime? fromDate;
  DateTime? toDate;
  PropertyModel? propertyModel;

  Rx<GuestMemberUserModel?> userInfo = Rx<GuestMemberUserModel?>(null);
  var commonController = Get.find<CommonController>();
  TextEditingController gNameController = TextEditingController();
  TextEditingController gPhoneController = TextEditingController();
  final formKeyForGuestInfo = GlobalKey<FormState>();


 TextEditingController reviewController = TextEditingController();
 ReservationListModel? selecteBookingData;
  @override
  void onInit() {
    // Receive the arguments
    var currentDate = DateTime.now();
    vFromDateController.text = dateFormat.format(currentDate);
    fromDate = currentDate;

    toDate = currentDate.add(const Duration(days: 15));
    vToDateController.text = dateFormat.format(toDate!);
    getInfo();

     
    super.onInit();
  }

  getInfo() async {
    id = await SharedPfnDBHelper.getGuestOrMemberId();
    userType = await SharedPfnDBHelper.getUserType() ?? "";

    if (id == null) {
      isLoading.value = false;
      return;
    }

    propertyModel = await SharedPfnDBHelper.getProperty();
    propertyModel?.logoUrl = await Env.propertryUrl() + propertyModel?.logoUrl;
    getUserInfo();
  }

  getDataList() {
    ReservationService.getReservationList("0", userType, id!,
            vFromDateController.text, vToDateController.text)
        .then((value) {
      dataList.value = value;
      isLoadingForBookingData.value = false;
    }).onError((error, stackTrace) {
      isLoadingForBookingData.value = false;
      Get.snackbar(
        "Error!",
        error.toString(),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
    });
  }

  void selectFromDate(context) async {
    DateTime? datePick = await showDatePicker(
        useRootNavigator: false,
        context: context,
        initialDate: fromDate ?? toDate ?? DateTime.now(),
        firstDate: DateTime(2023),
        lastDate: toDate ?? DateTime(2024));

    if (datePick != null) {
      vFromDateController.text = dateFormat.format(datePick);
      fromDate = datePick;
      // getDataList();
    } else {
      // fromDate = fromDate;
      vFromDateController.text = dateFormat.format(fromDate!);
    }
    // calTotlaDays();
    // update();
  }

  void selectToDate(context) async {
    DateTime? datePick = await showDatePicker(
      useRootNavigator: false,
      context: context,
      initialDate: toDate ?? fromDate ?? DateTime.now(),
      firstDate: fromDate ?? DateTime.now(),
      lastDate: DateTime(2024),
    );
    if (datePick != null) {
      vToDateController.text = dateFormat.format(datePick);
      toDate = datePick;
      // getDataList();
    } else {
      vToDateController.text = dateFormat.format(toDate!);
    }
    // calTotlaDays();
    // update();
  }

  getUserInfo() async {
    CommonService.getGuestOrMemberProfile(userType, id!).then(
      (value) {
        userInfo.value = value;
        getDataList();
        isLoading.value = false;
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
      // EasyLoading.showSuccess('Login Succesfully Done!');
      Get.rawSnackbar(
        message: "Login Succesfully Done!",
        backgroundColor: themeColor,
      );

      getInfo();
      update();
    }).onError((error, stackTrace) {
      EasyLoading.dismiss();
      Get.rawSnackbar(
        message: error.toString(),
        backgroundColor: errorColor,
      );
    });
  }

  void newBooking() {
    if (userType == UserTypeEnum.guest.value) {
      Get.toNamed(Routes.guestReservation, arguments: true);
    } else if (userType == UserTypeEnum.member.value) {
      Get.toNamed(Routes.memberReservation, arguments: true);
    }
  }
}
