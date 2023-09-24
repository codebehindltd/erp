import 'package:flutter/material.dart';

import 'package:get/get.dart';
import '../../../../../global_widgets/back_widget.dart';
import '../../../../../global_widgets/custom_button.dart';
import '../controllers/voucher_details_controller.dart';
import '../widgets/voucher_details_loading_view.dart';
import '../widgets/voucher_table_view.dart';

class VoucherDetailsView extends GetView<VoucherDetailsController> {
  const VoucherDetailsView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    const smallSizedBox = SizedBox(
      height: 5,
    );
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        // title: const Text('VoucherDetailsView'),
        // centerTitle: true,
      ),
      body: GetBuilder<VoucherDetailsController>(builder: (data) {
        if (controller.isLoading) {
          return const Padding(
            padding: EdgeInsets.all(8),
            child: VoucherDLoadingView(),
          );
        } else {
          return SingleChildScrollView(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                children: [
                  const SizedBox(
                    height: 10,
                  ),
                  Container(
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Flexible(
                          flex: 1,
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              RichText(
                                text: TextSpan(
                                    text: "Company: ",
                                    style: titleStyle(),
                                    children: [
                                      TextSpan(
                                          text: controller
                                              .voucherInfo.first.companyName,
                                          style: valueStyle()),
                                    ]),
                              ),
                              smallSizedBox,
                              RichText(
                                text: TextSpan(
                                    text: "Voucher No: ",
                                    style: titleStyle(),
                                    children: [
                                      TextSpan(
                                          text: controller
                                              .voucherInfo.first.voucherNo,
                                          style: valueStyle()),
                                    ]),
                              ),
                              smallSizedBox,
                              RichText(
                                text: TextSpan(
                                    text: "Created By: ",
                                    style: titleStyle(),
                                    children: [
                                      TextSpan(
                                          text: controller
                                              .voucherInfo.first.createdByName,
                                          style: valueStyle()),
                                    ]),
                              ),
                            ],
                          ),
                        ),
                        Flexible(
                          flex: 1,
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              RichText(
                                text: TextSpan(
                                    text: "Project: ",
                                    style: titleStyle(),
                                    children: [
                                      TextSpan(
                                          text: controller
                                              .voucherInfo.first.projectName,
                                          style: valueStyle()),
                                    ]),
                              ),
                              smallSizedBox,
                              RichText(
                                text: TextSpan(
                                    text: "Date: ",
                                    style: titleStyle(),
                                    children: [
                                      TextSpan(
                                          text: controller.voucherInfo.first
                                              .voucherDateDisplay,
                                          style: valueStyle()),
                                    ]),
                              ),
                              smallSizedBox,
                              RichText(
                                text: TextSpan(
                                    text: "Created Date: ",
                                    style: titleStyle(),
                                    children: [
                                      TextSpan(
                                          text: controller.voucherInfo.first
                                              .createdDateDisplay,
                                          style: valueStyle()),
                                    ]),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(
                    height: 10,
                  ),
                  RichText(
                    text: TextSpan(
                        text: "Narration: ",
                        style: titleStyle(),
                        children: [
                          TextSpan(
                              text: controller.voucherInfo.first.narration,
                              style: valueStyle()),
                        ]),
                  ),
                  const SizedBox(
                    height: 20,
                  ),
                  // Table(
                  //   defaultVerticalAlignment: TableCellVerticalAlignment.middle,
                  //   columnWidths: const {
                  //     0: FlexColumnWidth(5),
                  //     1: FlexColumnWidth(2),
                  //     2: FlexColumnWidth(2),
                  //   },
                  //   children: [
                  //     TableRow(
                  //       decoration: const BoxDecoration(color: Color.fromARGB(60, 207, 207, 207)),
                  //       children: [
                  //       Padding(
                  //         padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  //         child: Text("Accounts Head", style: tableTitleStyle(),),
                  //       ),
                  //       Padding(
                  //         padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  //         child: Text("Dr.", style: tableTitleStyle()),
                  //       ),
                  //       Padding(
                  //         padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  //         child: Text("Cr.", style: tableTitleStyle()),
                  //       ),
                  //     ]),
                  //     // TableRow(children: [
                  //     //   Text("Lorem Ipsum is simply dummy text of the printing and typesetting industry"),
                  //     //   Text("500"),
                  //     //   Text("500"),
                  //     // ])
                  //   ],
                  // )
                  VoucherTableView(
                    voucherInfo: controller.voucherInfo,
                  ),
                  const SizedBox(
                    height: 20,
                  ),
                  Row(
                    // mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Flexible(
                        flex: 1,
                        fit: FlexFit.tight,
                        child: RichText(
                          text: TextSpan(
                              text: "Checked By: ",
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                    text: controller
                                        .voucherInfo.first.checkedByName,
                                    style: valueStyle()),
                              ]),
                        ),
                      ),
                      Flexible(
                        flex: 1,
                        fit: FlexFit.tight,
                        child: RichText(
                          text: TextSpan(
                              text: "Approved By: ",
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                    text: controller
                                        .voucherInfo.first.approvedByName,
                                    style: valueStyle()),
                              ]),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(
                    height: 30,
                  ),
                  Visibility(
                    visible: controller.voucherInfo.first.isCanApprove!,
                    child: CustomButton(
                        submit: controller.approved,
                        name: "Approve",
                        fullWidth: false),
                  ),
                  Visibility(
                    visible: controller.voucherInfo.first.isCanCheck!,
                    child: CustomButton(
                        submit: controller.checked,
                        name: "Check",
                        fullWidth: false),
                  )
                ],
              ),
            ),
          );
        }
      }),
    );
  }

  TextStyle valueStyle() {
    return const TextStyle(
      fontWeight: FontWeight.normal,
      color: Colors.black87,
      fontSize: 12,
    );
  }

  TextStyle titleStyle() {
    return const TextStyle(
        fontWeight: FontWeight.w500, color: Colors.black, fontSize: 13);
  }
}
