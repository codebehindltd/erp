import 'package:flutter/material.dart';

import 'package:get/get.dart';
import 'package:pull_to_refresh_flutter3/pull_to_refresh_flutter3.dart';

import '../../../../../core/values/colors.dart';
import '../../../../../global_widgets/back_widget.dart';
import '../../../../../global_widgets/custom_footer_refresher.dart';
import '../../../../../global_widgets/empty_widget.dart';
import '../../../../../global_widgets/search_bar.dart';
import '../controllers/voucher_approval_controller.dart';
import '../widgets/loading_voucher_card.dart';
import '../widgets/voucher_card.dart';
import 'voucher_filter_view.dart';

class VoucherApprovalView extends GetView<VoucherApprovalController> {
  const VoucherApprovalView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    var statusBarHeight = MediaQuery.of(context).viewPadding.top;
    return Scaffold(
        appBar: AppBar(
          leading: const BackButtonWidget(),
          title: SearchBarWidget(
              controllerText: controller.searchKeyController,
              changed: controller.searchEmp,
              hintText: "Voucher No",
              autofocus: false),
          actions: [
            Stack(
              children: [
                IconButton(
                    onPressed: () {
                      // Get.to(VoucherFilterView(), transition: Transition.rightToLeft );
                      // Dialog.fullscreen(child: VoucherFilterView(),);
                      Get.bottomSheet(
                          Container(
                              padding: EdgeInsets.only(top: statusBarHeight),
                              child: VoucherFilterView(
                                oldVCriteraiData: controller.vCriteriaModel,
                                voucherCriteriaApply:
                                    controller.voucherFilterApply,
                              )),
                          isScrollControlled: true);
                    },
                    icon: const Icon(
                      Icons.filter_list_outlined,
                      color: whiteColor,
                    )),
                Positioned(
                    right: 10,
                    top: 13,
                    child: Container(
                      decoration: BoxDecoration(
                          color: warningColor2,
                          border: Border.all(
                              width: 1, color: Theme.of(context).primaryColor),
                          borderRadius: BorderRadius.circular(100)),
                      child: Icon(
                        Icons.circle,
                        size: 8,
                        color: warningColor2,
                      ),
                    ))
              ],
            )
          ],
        ),
        body: GetBuilder<VoucherApprovalController>(builder: (data) {
          if (controller.isLoading == false &&
              controller.voucherList.isNotEmpty) {
            return SafeArea(
              child: SmartRefresher(
                controller: controller.refreshController,
                enablePullUp: true,
                enablePullDown: true,
                onLoading: controller.onLoading,
                onRefresh: controller.onRefresh,
                physics: const BouncingScrollPhysics(),
                footer: const CustomFooterRefresher(),
                child: ListView.builder(
                    scrollDirection: Axis.vertical,
                    shrinkWrap: true,
                    physics: const ScrollPhysics(),
                    itemCount: controller.voucherList.length,
                    itemBuilder: ((context, index) {
                      return VoucherCard(
                          voucherData: controller.voucherList[index]);
                    })),
              ),
            );
          }
          if (controller.isLoading == false && controller.voucherList.isEmpty) {
            return const SafeArea(
                child: SingleChildScrollView(
              child: EmptyScreen(
                  title: "Data Not Found!",
                  imageUrl: "assets/images/empty.png"),
            ));
          } else {
            return SafeArea(
                child: ListView.builder(
                    scrollDirection: Axis.vertical,
                    shrinkWrap: true,
                    physics: const ScrollPhysics(),
                    itemCount: 6,
                    itemBuilder: ((context, index) {
                      return const LoadingVoucherCard();
                    })));
          }
        }));
  }
}
