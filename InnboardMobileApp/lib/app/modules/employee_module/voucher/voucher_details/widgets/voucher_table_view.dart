import 'package:flutter/material.dart';

import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';

import '../../../../../data/models/res/voucher_list_model.dart';

class VoucherTableView extends GetView {
  final List<VoucherResModel> voucherInfo;
  const VoucherTableView({Key? key, required this.voucherInfo})
      : super(key: key);
  @override
  Widget build(BuildContext context) {
   
    return Container(
      child: Column(
        children: [
          Container(
            color: const Color.fromARGB(60, 207, 207, 207),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Flexible(
                  flex: 5,
                  fit: FlexFit.tight,
                  child: Padding(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                    child: Text(
                      "Accounts Head",
                      style: tableTitleStyle(),
                    ),
                  ),
                ),
                Flexible(
                  flex: 2,
                  fit: FlexFit.tight,
                  child: Padding(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                    child: Text("Dr.", style: tableTitleStyle()),
                  ),
                ),
                Flexible(
                  flex: 2,
                  fit: FlexFit.tight,
                  child: Padding(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                    child: Text("Cr.", style: tableTitleStyle()),
                  ),
                ),
              ],
            ),
          ),
          Container(
            padding: const EdgeInsets.only(bottom: 2),
            decoration: const BoxDecoration(
                border: Border(
                    bottom: BorderSide(
                        width: .4,
                        style: BorderStyle.solid,
                        color: gray1Color))),
            child: ListView.builder(
                shrinkWrap: true,
                physics: const ScrollPhysics(),
                itemCount: voucherInfo.length,
                itemBuilder: (context, index) {
                  return Container(
                    color: index % 2 == 0
                        ? null
                        : const Color.fromARGB(60, 207, 207, 207),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Flexible(
                          flex: 5,
                          fit: FlexFit.tight,
                          child: Padding(
                            padding: const EdgeInsets.symmetric(
                                horizontal: 10, vertical: 4),
                            child: Text(
                              voucherInfo[index].nodeHead ?? '',
                              style: tableDataStyle(),
                            ),
                          ),
                        ),
                        Flexible(
                          flex: 2,
                          fit: FlexFit.tight,
                          child: Padding(
                            padding: const EdgeInsets.symmetric(
                                horizontal: 10, vertical: 4),
                            child: Text(
                                voucherInfo[index].debitAmount == 0
                                    ? ''
                                    : voucherInfo[index].debitAmount.toString(),
                                style: tableDataStyle()),
                          ),
                        ),
                        Flexible(
                          flex: 2,
                          fit: FlexFit.tight,
                          child: Padding(
                            padding: const EdgeInsets.symmetric(
                                horizontal: 10, vertical: 4),
                            child: Text(
                                voucherInfo[index].creditAmount == 0
                                    ? ''
                                    : voucherInfo[index]
                                        .creditAmount
                                        .toString(),
                                style: tableDataStyle()),
                          ),
                        ),
                      ],
                    ),
                  );
                }),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.start,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Flexible(
                flex: 5,
                fit: FlexFit.tight,
                child: Padding(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  child: Text(
                    "",
                    style: tableDataStyle(),
                  ),
                ),
              ),
              Flexible(
                flex: 2,
                fit: FlexFit.tight,
                child: Padding(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  child: Text(voucherInfo.fold<double>(0.0, (sum, next)=> sum + next.debitAmount!).toString(), style: tableDataStyle()),
                ),
              ),
              Flexible(
                flex: 2,
                fit: FlexFit.tight,
                child: Padding(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  child: Text(voucherInfo.fold<double>(0.0, (sum, next)=> sum + next.creditAmount!).toString(), style: tableDataStyle()),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  TextStyle tableTitleStyle() =>
      const TextStyle(fontWeight: FontWeight.bold, fontSize: 15);
  TextStyle tableDataStyle() =>
      const TextStyle(fontWeight: FontWeight.normal, fontSize: 12);
}
