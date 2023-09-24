import 'package:get/get.dart';

import '../controllers/voucher_details_controller.dart';

class VoucherDetailsBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<VoucherDetailsController>(
      () => VoucherDetailsController(),
    );
  }
}
