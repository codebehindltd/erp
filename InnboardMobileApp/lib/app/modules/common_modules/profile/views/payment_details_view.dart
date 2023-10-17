import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/back_widget.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../routes/app_pages.dart';
import '../controllers/profile_controller.dart';

class PaymentDetailsView extends GetView<ProfileController> {
  const PaymentDetailsView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('PaymentDetails'),
          centerTitle: false,
          leading: const BackButtonWidget(),
        ),
        body: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Column(
            children: [
              Card(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(8.0),
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: [
                        Padding(
                          padding: const EdgeInsets.all(4.0),
                          child: Column(
                            children: [
                              Text("Total Paid",
                                  style:
                                      Theme.of(context).textTheme.labelLarge),
                              Text("5000 Tk",
                                  style:
                                      Theme.of(context).textTheme.labelMedium),
                            ],
                          ),
                        ),
                        Container(
                          width: 1.5,
                          height: 66,
                          color: Colors.grey.withOpacity(0.2),
                        ),
                        Padding(
                          padding: const EdgeInsets.all(4.0),
                          child: Column(
                            children: [
                              Text("Total Due",
                                  style:
                                      Theme.of(context).textTheme.labelLarge),
                              Text("15000 Tk",
                                  style:
                                      Theme.of(context).textTheme.labelMedium),
                            ],
                          ),
                        ),
                      ],
                    ),
                    Container(
                      height: 1.5,
                      color: Colors.grey.withOpacity(0.2),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(4.0),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Column(
                            children: [
                              Text("Total Payable",
                                  style:
                                      Theme.of(context).textTheme.labelLarge),
                              Text("20000 Tk",
                                  style:
                                      Theme.of(context).textTheme.labelMedium),
                            ],
                          ),
                        ],
                      ),
                    ),
                    Container(
                      height: 5,
                      color: Colors.grey.withOpacity(0.2),
                    ),
                  ],
                ),
              ),
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
                          Text("500000 BDT", style: commonTextStyle()),
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
                          Text("2000000 BDT", style: commonTextStyle()),
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
                              // controller.signUp();
                            },
                            name: "Online Payment",
                            fullWidth: false,
                            horizontalPadding: 48,
                            fontSize: 18),
                      ],
                    ),
                  ],
                ),
              ),
              CustomButton(
                  submit: (v) {
                    Get.toNamed(Routes.profile +
                        Routes.paymentDetailsView +
                        Routes.paymentHistoryView);
                  },
                  name: "Payment History",
                  fullWidth: true),
            ],
          ),
        ));
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
