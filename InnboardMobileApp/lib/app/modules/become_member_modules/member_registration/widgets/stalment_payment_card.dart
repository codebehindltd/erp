import 'package:flutter/material.dart';
import '../../../../core/enums/stalment_title_enum.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/models/res/payment_st_list_model.dart';
import '../../../../global_widgets/custom_button.dart';

class StalmentPaymentCard extends StatelessWidget {
  final StalmentTitleEnum? stalmentTitle;
  final PaymentStListModel? paymentStStep;
  final ValueChanged<double> paySubmit;

  const StalmentPaymentCard(
      {super.key,
      this.stalmentTitle,
      this.paymentStStep,
      required this.paySubmit});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 16),
      child: Column(
        children: [
          Stack(children: [
            Container(
              width: double.infinity,
              padding: const EdgeInsets.only(
                  left: 10, right: 10, top: 8, bottom: 18),
              margin: const EdgeInsets.only(top: 12),
              decoration: BoxDecoration(
                border: Border.all(color: borderColor),
                borderRadius: BorderRadius.circular(6),
              ),
              child: Column(
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      Icon(
                        Icons.done_all,
                        color: paymentStStep!.isPaid! == true
                            ? Colors.green
                            : Colors.grey,
                        size: 28.0,
                      ),
                    ],
                  ),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: [
                      Text('Amount :', style: commonTextStyle()),
                      Text("${paymentStStep!.amount?.toStringAsFixed(2)} BDT",
                          style: commonTextStyle()),
                    ],
                  ),
                  const SizedBox(
                    height: 16,
                  ),
                  Visibility(
                    visible: paymentStStep!.isButtonVisible==true,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        CustomButton(
                            submit: paymentStStep!.isPaid == true
                                ? ((value) {})
                                : (v) {
                                    paySubmit(paymentStStep!.amount!);
                                  },
                            // bgColor:paymentStStep!.isPaid == true? Colors.red,
                            textColor: paymentStStep!.isPaid == true
                                ? Colors.grey
                                : themeColor,
                            name: "Pay",
                            fullWidth: false,
                            horizontalPadding: 42,
                            fontSize: 18)
                      ],
                    ),
                  ),
                ],
              ),
            ),
            Positioned(
              top: -3,
              left: 15,
              child: Container(
                color: bodyColor,
                child: Text(stalmentTitle!.value.toString(),
                    style: Theme.of(context)
                        .textTheme
                        .headlineLarge!
                        .merge(const TextStyle(color: Colors.black45))),
              ),
            ),
          ])
        ],
      ),
    );
  }

  TextStyle commonTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w500, fontSize: 18);
  }
}
