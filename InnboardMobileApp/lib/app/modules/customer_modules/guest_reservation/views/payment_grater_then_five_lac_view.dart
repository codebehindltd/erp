import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/enums/stalment_title_enum.dart';
import '../../../../core/values/colors.dart';
import '../controllers/guest_reservation_controller.dart';
import '../widgets/stalment_payment_card.dart';

class GuestPaymentGraterThenFiveLacView
    extends GetView<GuestReservationController> {
  const GuestPaymentGraterThenFiveLacView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return WillPopScope(
       onWillPop: () async => await onBackButtonPress(context),
      child: Scaffold(
          appBar: AppBar(
            title: const Text(
              'Payment',
              style: TextStyle(
                  color: Colors.white, fontWeight: FontWeight.w500, fontSize: 22),
            ),
            // centerTitle: true,
            // leading: const BackButtonWidget(),
            automaticallyImplyLeading: false,
          ),
          body: GetBuilder<GuestReservationController>(builder: (_) {
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
                          return StalmentPaymentCard(
                            stalmentTitle: StalmentTitleEnum.values[index],
                            paymentStStep: controller.paymentStStepList[index],
                            paySubmit: (amount) {
                              controller.payWithStalment(amount, index);
                            },
                          );
                        })
                  ],
                ),
              ),
            );
          })),
    );
  }

  TextStyle commonTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w400, fontSize: 18);
  }
}



Future<bool> onBackButtonPress(BuildContext context) async {
  return await showDialog(
      context: context,
      builder: (context) => AlertDialog(
            contentPadding:
                const EdgeInsets.symmetric(vertical: 10, horizontal: 22),
            // title: Text(
            //   "Exit?",
            //   style: TextStyle(
            //       fontFamily: inter, fontSize: 20, fontWeight: FontWeight.w600),
            // ),
            title: const SizedBox(
              height: 4,
            ),
            content: Text(
              "Please compleate this process",
              style: TextStyle(color: themeColor, fontWeight: FontWeight.w600, fontSize: 18),
            ),
            actions: [
             
              TextButton(
                  onPressed: () {
                    Navigator.of(context).pop(false);
                  },
                  child: Text(
                    "Yes",
                    style: TextStyle(color: themeColor, fontWeight: FontWeight.w600, fontSize: 16),
                  ))
            ],
          ));
}
