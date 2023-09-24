import 'package:get/get.dart';
import 'package:leading_edge/app/data/models/res/emp_model.dart';

class EmployeeDetailsController extends GetxController {
  EmpModel empModel = EmpModel();
  @override
  void onInit() {
    var data = Get.arguments;
    empModel = EmpModel.fromJson(data);
    update();
    super.onInit();
  }
}
