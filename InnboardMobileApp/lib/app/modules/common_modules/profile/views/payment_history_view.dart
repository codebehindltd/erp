import 'package:flutter/material.dart';

import 'package:get/get.dart';

import '../../../../global_widgets/back_widget.dart';
import '../widgets/transaction_history_card.dart';

class PaymentHistoryView extends GetView {
  const PaymentHistoryView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('Payment History'),
          centerTitle: false,
          leading: const BackButtonWidget(),
        ),
        body: SingleChildScrollView(
          child: Column(
            children: [
              ListView.separated(
                  physics: const ScrollPhysics(),
                  shrinkWrap: true,
                  scrollDirection: Axis.vertical,
                  itemCount: 8,
                  separatorBuilder: (context, index) => Container(
                        height: 1.5,
                        color: Colors.grey.withOpacity(0.2),
                      ),
                  itemBuilder: (context, index) {
                    return const Padding(
                      padding: EdgeInsets.all(8.0),
                      child: TransactionHistoryCard(),
                    );
                  }),
            ],
          ),
        ));
  }
}
