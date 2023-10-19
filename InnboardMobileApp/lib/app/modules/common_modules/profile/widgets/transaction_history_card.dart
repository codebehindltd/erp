import 'package:flutter/material.dart';

import '../../../../core/values/colors.dart';

class TransactionHistoryCard extends StatefulWidget {
  const TransactionHistoryCard({super.key});

  @override
  State<TransactionHistoryCard> createState() => _TransactionHistoryCardState();
}

class _TransactionHistoryCardState extends State<TransactionHistoryCard> {
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(10.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text("Transction ID : ", style: commonTextStyle()),
              Text("tenx10487876",
                  style: Theme.of(context).textTheme.labelSmall),
            ],
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text("Transction Date : ", style: commonTextStyle()),
              Text("12/10/2023", style: Theme.of(context).textTheme.labelSmall),
            ],
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text("Transction Amount : ", style: commonTextStyle()),
              Text("50000 Tk", style: Theme.of(context).textTheme.labelSmall),
            ],
          ),
        ],
      ),
    );
  }

  TextStyle commonTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w500, fontSize: 18);
  }

  TextStyle valueTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w400, fontSize: 16);
  }
}
