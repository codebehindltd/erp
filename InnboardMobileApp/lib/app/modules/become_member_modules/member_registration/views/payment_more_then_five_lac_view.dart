import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/enums/stalment_title_enum.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/back_widget.dart';
import '../controllers/member_registration_controller.dart';
import '../widgets/stalment_payment_card.dart';

class PaymentMoreThenFiveLacView extends GetView<MemberRegistrationController> {
  const PaymentMoreThenFiveLacView({Key? key}) : super(key: key);
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
      body:GetBuilder<MemberRegistrationController>(builder: (_) {
        return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(18.0),
          child: Column(
            children: [
              Text(
                '''Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s ''',
                style: commonTextStyle(),
              ),
              
              ListView.builder(
                  physics: const ScrollPhysics(),
                  shrinkWrap: true,
                  itemCount: controller.paymentStStepList.length,
                  itemBuilder: (context, index) {
                    return StalmentPaymentCard(stalmentTitle: StalmentTitleEnum.values[index],paymentStStep:controller.paymentStStepList[index],paySubmit:(amount){
                      controller.payWithStalment(amount, index);
                    },);
                  })
            ],
          ),
        ),
      );
    
  }));
  }

  TextStyle commonTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w400, fontSize: 18);
  }
}
