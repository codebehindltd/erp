import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/models/res/voucher_list_model.dart';

import '../../../../../core/utils/size_config.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../routes/app_pages.dart';

class VoucherCard extends StatelessWidget {
  final VoucherResModel voucherData;
  const VoucherCard({super.key, required this.voucherData});

  @override
  Widget build(BuildContext context) {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(10),
      ),
      child: InkWell(
        onTap: () {
          Get.toNamed(Routes.voucherDetails, arguments: voucherData.ledgerMasterId);
        },
        child: Container(
            color: themeColor.withOpacity(.03),
            padding: const EdgeInsets.all(8),
            child: Row(
              children: [
                SizedBox(
                    width: SizeConfig.screenWidth! * .28,
                    height: SizeConfig.screenWidth! * .28,
                    child: Hero(
                      tag: "ds",
                      child: Container(
                        decoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(20),
                            border: Border.all(width: 1, color: themeColor)),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.center,
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            Text("${voucherData.voucherTotalAmount}"),
                            const Divider(),
                            Text("${voucherData.glStatus}"),
                          ],
                        ),
                      ),
                    )),
                SizedBox(
                  width: SizeConfig.screenWidth! * .02,
                ),
                SizedBox(
                  width: SizeConfig.screenWidth! * .62,
                  child: Column(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        RichText(
                          text: TextSpan(
                              text: 'Voucher Number: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: "${voucherData.voucherNo}",
                                  style: valueStyle(),
                                ),
                              ]),
                        ),
                        buildSpace(),
                        RichText(
                          text: TextSpan(
                              text: 'Voucher Date: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: "${voucherData.voucherDateString}",
                                  style: valueStyle(),
                                ),
                              ]),
                        ),
                        buildSpace(),
                        RichText(
                          text: TextSpan(
                              text: 'Narration: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: "${voucherData.narration}",
                                  style: valueStyle(),
                                ),
                              ]),
                        ),
                        buildSpace(),
                        // RichText(
                        //   text: TextSpan(
                        //       text: 'Created By: ',
                        //       style: titleStyle(),
                        //       children: [
                        //         TextSpan(
                        //           text: "${voucherData.createdBy}",
                        //           style: valueStyle(),
                        //         ),
                        //       ]),
                        // ),
                        // buildSpace(),
                      ]),
                )
              ],
            )),
      ),
    );
  }

  TextStyle valueStyle() {
    return const TextStyle(fontWeight: FontWeight.normal, color: Colors.black);
  }

  TextStyle titleStyle() {
    return const TextStyle(fontWeight: FontWeight.w600, color: Colors.black);
  }

  SizedBox buildSpace() => const SizedBox(
        height: 10,
      );
}
