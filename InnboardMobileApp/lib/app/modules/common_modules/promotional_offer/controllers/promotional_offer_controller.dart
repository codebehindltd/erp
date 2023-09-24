import 'package:get/get.dart';

import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/res/promotional_offer.dart';
import '../../../../data/services/common_service.dart';

class PromotionalOfferController extends GetxController {
  String userType = "";
  String id = "0";
  var isLoading = Rx<bool>(true);
  var isLoadingNightInfo = Rx<bool>(true);
  var data = RxList<PromotionalOfferModel>([]);
  var memberRoomNightInfo = RxList<PromotionalOfferModel>([]);

  @override
  void onInit() {
    getInfo();
    super.onInit();
  }

  getInfo() async {
    userType = await SharedPfnDBHelper.getUserType() ?? "";
    id = await SharedPfnDBHelper.getGuestOrMemberId() ?? "0";
    getData();
    if (userType == UserTypeEnum.member.value) {
      getMemberRoomNightInfo();
    }
  }

  getData() async {
    CommonService.getGuestOrMemberOffer(userType, id).then((value) {
      isLoading.value = false;
      data.value = value;
    }).onError((error, stackTrace) {
      Get.snackbar(
        "Error!",
        error.toString(),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
    });
  }

  getMemberRoomNightInfo() async {
    CommonService.getMemberRoomNightInfo(userType, id).then((value) {
      isLoadingNightInfo.value = false;
      memberRoomNightInfo.value = value;

      totalCal();
    }).onError((error, stackTrace) {
      Get.snackbar(
        "Error!",
        error.toString(),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
    });
  }

  var totalNight = Rx<num>(0);
  var totalAvail = Rx<num>(0);
  var totalBalance = Rx<num>(0);

  totalCal() {
    totalNight.value = memberRoomNightInfo.isEmpty
        ? 0
        : memberRoomNightInfo
                .map((item) => item.roomNights)
                .reduce((v, e) => v! + e!) ??
            0;

    totalAvail.value = memberRoomNightInfo.isEmpty
        ? 0
        : memberRoomNightInfo
                .map((item) => item.availNights)
                .reduce((v, e) => v! + e!) ??
            0;

    totalBalance.value = memberRoomNightInfo.isEmpty
        ? 0
        : memberRoomNightInfo
                .map((item) => item.balanceNight)
                .reduce((v, e) => v! + e!) ??
            0;
  }
}
