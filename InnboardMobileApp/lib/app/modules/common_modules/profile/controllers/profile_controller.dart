import 'package:get/get.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';
import '../../../../data/models/res/user_model.dart';
import '../../../../data/services/backgound_location_service.dart';
import '../../../../routes/app_pages.dart';
import '../../../common/controllers/common_controller.dart';

class ProfileController extends GetxController {
  UserModel? userInfo;
  bool isLoading = true;
  var testV = 0.obs;
  var commonController = Get.find<CommonController>();

  RxInt selectedPaymentType = 0.obs;

  void selectValue(int value) {
    selectedPaymentType.value = value;
  }

  @override
  void onReady() {
    getUserInfo();
    super.onReady();
  }

  updateV() {
    testV.value = testV.value + 1;
    update();
  }

  logOut() async {
    SharedPfnDBHelper.removeLoginUserData();
    await initializeService(); // reset location service
    commonController.isLogin = false;
    Get.offAllNamed(Routes.splash);
  }

  getUserInfo() async {
    var data = await SharedPfnDBHelper.getPropertyUserData();
    if (data != null) {
      isLoading = false;
      userInfo = data;
    } else {
      isLoading = false;
    }
    update();
  }
}
