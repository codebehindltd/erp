import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/enums/user_type_enum.dart';
import '../../../../core/values/colors.dart';
import '../controllers/promotional_offer_controller.dart';

class PromotionalOfferView extends GetView<PromotionalOfferController> {
  const PromotionalOfferView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text(
            'Promotional Offer',
            style: TextStyle(
                color: Colors.white, fontWeight: FontWeight.w500, fontSize: 22),
          ),
          automaticallyImplyLeading: false,
        ),
        body: GetBuilder<PromotionalOfferController>(builder: (_) {
          return Obx(
            () => controller.isLoading.value && controller.data.isEmpty
                ? const Center(
                    child: CircularProgressIndicator(),
                  )
                : SingleChildScrollView(
                    child: Padding(
                      padding: const EdgeInsets.all(12.0),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          controller.userType == UserTypeEnum.member.value
                              ? buildMemberRoomNightWidget()
                              : const Text(""),
                          Row(
                            children: [
                              Icon(
                                Icons.double_arrow,
                                color: themeColor,
                                size: 18,
                              ),
                              Text(" Discount",
                                  // maxLines: 1,
                                  // overflow: TextOverflow.ellipsis,
                                  textAlign: TextAlign.center,
                                  style: TextStyle(
                                      color: themeColor,
                                      fontWeight: FontWeight.w600,
                                      fontSize: 20)),
                            ],
                          ),
                          Card(
                            elevation: .5,
                            color: bodyColor.withOpacity(.9),
                            child: Padding(
                              padding: const EdgeInsets.all(10.0),
                              child: ListView.builder(
                                  physics: const ScrollPhysics(),
                                  shrinkWrap: true,
                                  itemCount: controller.data.length,
                                  itemBuilder: (context, index) {
                                    return Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      children: [
                                        Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                          children: [
                                            Text(
                                                "${controller.data[index].benefitsName}",
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis,
                                                style: TextStyle(
                                                    color: themeColor,
                                                    fontWeight: FontWeight.w400,
                                                    fontSize: 18)),
                                            Text(
                                                "${controller.data[index].remainingBenefitsValue} ${controller.data[index].benefitsTransactionType} ${controller.data[index].benefitsDetails ?? ''}",
                                                maxLines: 1,
                                                overflow: TextOverflow.ellipsis,
                                                style: TextStyle(
                                                    color: themeColor,
                                                    fontWeight: FontWeight.w400,
                                                    fontSize: 20)),
                                          ],
                                        ),
                                      ],
                                    );
                                  }),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
          );
        }));
  }

  Widget buildMemberRoomNightWidget() {
    if (controller.isLoadingNightInfo.value &&
        controller.memberRoomNightInfo.isEmpty) {
      return const Center(
        child: CircularProgressIndicator(),
      );
    } else {
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(
                Icons.double_arrow,
                color: themeColor,
                size: 18,
              ),
              Text(" Room Night",
                  // maxLines: 1,
                  // overflow: TextOverflow.ellipsis,
                  textAlign: TextAlign.center,
                  style: TextStyle(
                      color: themeColor,
                      fontWeight: FontWeight.w600,
                      fontSize: 20)),
            ],
          ),
          const SizedBox(
            height: 6,
          ),
          Container(
            // margin: const EdgeInsets.all(12),
            width: Get.width,
            decoration: BoxDecoration(
                color: bodyColor,
                borderRadius: BorderRadius.circular(8),
                boxShadow: [
                  const BoxShadow(
                    color: whiteColor,
                    spreadRadius: 2,
                    blurRadius: 6,
                    offset: Offset(-5, -5),
                  ),
                  BoxShadow(
                    color: bottonShadowColor.withOpacity(.5),
                    spreadRadius: 1,
                    blurRadius: 8,
                    offset: const Offset(4, 4),
                  )
                ]),
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                children: [
                  Row(
                    children: [
                      Flexible(
                        flex: 2,
                        child: Container(
                          height: 50,
                          decoration: BoxDecoration(
                            border: Border(
                              right: BorderSide(
                                color: Colors.grey.shade500,
                                width: 1,
                              ),
                              bottom: BorderSide(
                                color: Colors.grey.shade500,
                                width: 1,
                              ),
                            ),
                          ),
                          child: Row(
                            children: [
                              Text("Property",
                                  // maxLines: 1,
                                  // overflow: TextOverflow.ellipsis,
                                  //textAlign: TextAlign.left,
                                  style: titleTextStyle()),
                            ],
                          ),
                        ),
                      ),
                      Flexible(
                        child: Container(
                          height: 50,
                          decoration: BoxDecoration(
                            border: Border(
                              bottom: BorderSide(
                                color: Colors.grey.shade500,
                                width: 1,
                              ),
                            ),
                          ),
                          child: Center(
                            child: Text("Night",
                                // maxLines: 1,
                                // overflow: TextOverflow.ellipsis,
                                textAlign: TextAlign.center,
                                style: titleTextStyle()),
                          ),
                        ),
                      ),
                      Flexible(
                        child: Container(
                          height: 50,
                          decoration: BoxDecoration(
                            border: Border(
                              bottom: BorderSide(
                                color: Colors.grey.shade500,
                                width: 1,
                              ),
                            ),
                          ),
                          child: Center(
                            child: Text("Avail",
                                // maxLines: 1,
                                // overflow: TextOverflow.ellipsis,
                                textAlign: TextAlign.center,
                                style: titleTextStyle()),
                          ),
                        ),
                      ),
                      Flexible(
                        child: Container(
                          height: 50,
                          decoration: BoxDecoration(
                            border: Border(
                              bottom: BorderSide(
                                color: Colors.grey.shade500,
                                width: 1,
                              ),
                            ),
                          ),
                          child: Center(
                            child: Text("Balance",
                                // maxLines: 1,
                                // overflow: TextOverflow.ellipsis,
                                textAlign: TextAlign.center,
                                style: titleTextStyle()),
                          ),
                        ),
                      ),
                    ],
                  ),

                  //===================================
                  Column(
                    children: controller.memberRoomNightInfo.map(
                      (element) {
                        return Row(
                          children: [
                            Flexible(
                              flex: 2,
                              child: Container(
                                height: 50,
                                decoration: BoxDecoration(
                                  border: Border(
                                    right: BorderSide(
                                      color: Colors.grey.shade500,
                                      width: 1,
                                    ),
                                    bottom: BorderSide(
                                      color: Colors.grey.shade500,
                                      width: 1,
                                    ),
                                  ),
                                ),
                                child: Row(
                                  children: [
                                    Text(element.propertyName ?? "",
                                        style: commonTextStyle()),
                                  ],
                                ),
                              ),
                            ),
                            Flexible(
                              child: Container(
                                height: 50,
                                decoration: BoxDecoration(
                                  border: Border(
                                    bottom: BorderSide(
                                      color: Colors.grey.shade500,
                                      width: 1,
                                    ),
                                  ),
                                ),
                                child: Center(
                                  child: Text("${element.roomNights ?? ""}",
                                      textAlign: TextAlign.center,
                                      style: commonTextStyle()),
                                ),
                              ),
                            ),
                            Flexible(
                              child: Container(
                                height: 50,
                                decoration: BoxDecoration(
                                  border: Border(
                                    bottom: BorderSide(
                                      color: Colors.grey.shade500,
                                      width: 1,
                                    ),
                                  ),
                                ),
                                child: Center(
                                  child: Text("${element.availNights ?? ""}",
                                      textAlign: TextAlign.center,
                                      style: commonTextStyle()),
                                ),
                              ),
                            ),
                            Flexible(
                              child: Container(
                                height: 50,
                                decoration: BoxDecoration(
                                  border: Border(
                                    bottom: BorderSide(
                                      color: Colors.grey.shade500,
                                      width: 1,
                                    ),
                                  ),
                                ),
                                child: Center(
                                  child: Text("${element.balanceNight ?? ""}",
                                      textAlign: TextAlign.center,
                                      style: commonTextStyle()),
                                ),
                              ),
                            ),
                          ],
                        );
                      },
                    ).toList(),
                  ),
                  /////////////////Total==//////////////////
                  Row(
                    children: [
                      Flexible(
                        flex: 2,
                        child: Container(
                          height: 50,
                          decoration: BoxDecoration(
                            border: Border(
                              right: BorderSide(
                                color: Colors.grey.shade500,
                                width: 1,
                              ),
                              // bottom: BorderSide(
                              //   color: Colors.grey.shade500,
                              //   width: 1,
                              // ),
                            ),
                          ),
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              Text("Total",
                                  // maxLines: 1,
                                  // overflow: TextOverflow.ellipsis,
                                  //textAlign: TextAlign.left,
                                  style: titleTextStyle()),
                            ],
                          ),
                        ),
                      ),
                      Flexible(
                        child: Container(
                          height: 50,
                          child: Center(
                            child: Text(controller.totalNight.toString(),
                                // maxLines: 1,
                                // overflow: TextOverflow.ellipsis,
                                textAlign: TextAlign.center,
                                style: titleTextStyle()),
                          ),
                        ),
                      ),
                      Flexible(
                        child: Container(
                          height: 50,
                          child: Center(
                            child: Text(controller.totalAvail.toString(),
                                // maxLines: 1,
                                // overflow: TextOverflow.ellipsis,
                                textAlign: TextAlign.center,
                                style: titleTextStyle()),
                          ),
                        ),
                      ),
                      Flexible(
                        child: Container(
                          height: 50,
                          child: Center(
                            child: Text(controller.totalBalance.toString(),
                                // maxLines: 1,
                                // overflow: TextOverflow.ellipsis,
                                textAlign: TextAlign.center,
                                style: titleTextStyle()),
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(
            height: 30,
          ),
        ],
      );
    }
  }

  TextStyle titleTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w600, fontSize: 16);
  }

  TextStyle commonTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w500, fontSize: 16);
  }
}
