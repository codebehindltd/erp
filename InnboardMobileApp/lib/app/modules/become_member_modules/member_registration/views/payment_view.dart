import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:leading_edge/app/global_widgets/back_widget.dart';
import 'package:leading_edge/app/modules/become_member_modules/member_registration/controllers/member_registration_controller.dart';
import '../../../../global_widgets/custom_button.dart';

class PaymentView extends GetView<MemberRegistrationController> {
  const PaymentView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Payment',
          style: TextStyle(
              color: Colors.white, fontWeight: FontWeight.w500, fontSize: 22),
        ),
        // centerTitle: true,
        leading: const BackButtonWidget(),
      ),
      body: Padding(
        padding: const EdgeInsets.all(12.0),
        child: Obx(
          () => Column(
            children: [
              RadioListTile<int>(
                title: Text(
                  'Full Payment',
                  style: paymentTitleStyle(),
                ),
                value: 0,
                groupValue: controller.selectedPaymentType.value,
                onChanged: (value) => controller.selectValue(value!),
              ),
              RadioListTile<int>(
                title: Text(
                  'Instalment',
                  style: paymentTitleStyle(),
                ),
                value: 1,
                groupValue: controller.selectedPaymentType.value,
                onChanged: (value) => controller.selectValue(value!),
              ),
              const SizedBox(
                height: 30,
              ),
              Container(
                width: double.infinity,
                padding: const EdgeInsets.all(30),
                decoration: BoxDecoration(
                  border: Border.all(color: borderColor),
                  borderRadius: BorderRadius.circular(6),
                ),
                child: Column(
                  children: [
                    Visibility(
                      visible: controller.selectedPaymentType.value == 0,
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                        children: [
                          Text('Full Payment :', style: commonTextStyle()),
                          Text(
                              "${controller.selectedType.value.subscriptionFee} BDT",
                              style: commonTextStyle()),
                        ],
                      ),
                    ),
                    Visibility(
                      visible: controller.selectedPaymentType.value == 1,
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                        children: [
                          Text(
                            'Instalment :',
                            style: commonTextStyle(),
                          ),
                          Text(
                              "${controller.selectedType.value.minimumInstallmentSubscriptionFee} BDT",
                              style: commonTextStyle()),
                        ],
                      ),
                    ),
                    const SizedBox(
                      height: 20,
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        CustomButton(
                            submit: (v) {
                              controller.signUp();
                            },
                            name: "Online Payment",
                            fullWidth: false,
                            horizontalPadding: 48,
                            fontSize: 18),
                      ],
                    ),
                  ],
                ),
              )
            ],
          ),
        ),
      ),
    );
  }

  TextStyle paymentTitleStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w500, fontSize: 16);
  }

  TextStyle commonTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w400, fontSize: 16);
  }
}
