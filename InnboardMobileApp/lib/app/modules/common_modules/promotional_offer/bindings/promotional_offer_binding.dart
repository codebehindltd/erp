import 'package:get/get.dart';

import '../controllers/promotional_offer_controller.dart';

class PromotionalOfferBinding extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<PromotionalOfferController>(
      () => PromotionalOfferController(),
    );
  }
}
