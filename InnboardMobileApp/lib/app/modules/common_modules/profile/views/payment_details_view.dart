import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../global_widgets/back_widget.dart';
import '../widgets/transaction_history_card.dart';

class PaymentDetailsView extends GetView {
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
          child: Card(
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
                              style: Theme.of(context).textTheme.labelLarge),
                          Text("5000 Tk",
                              style: Theme.of(context).textTheme.labelMedium),
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
                              style: Theme.of(context).textTheme.labelLarge),
                          Text("15000 Tk",
                              style: Theme.of(context).textTheme.labelMedium),
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
                              style: Theme.of(context).textTheme.labelLarge),
                          Text("20000 Tk",
                              style: Theme.of(context).textTheme.labelMedium),
                        ],
                      ),
                    ],
                  ),
                ),
                Container(
                  height: 5,
                  color: Colors.grey.withOpacity(0.2),
                ),
                Container(
                  height: MediaQuery.of(context).size.height * 0.66,
                  child: ListView.separated(
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
                ),
              ],
            ),
          ),
        ));
  }
}
