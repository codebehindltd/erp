import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../global_widgets/back_widget.dart';
import '../../../../../global_widgets/empty_widget.dart';
import '../../../../../global_widgets/search_bar.dart';
import '../controllers/leave_status_controller.dart';
import '../widgets/leave_card.dart';
import '../widgets/leave_loading_card.dart';
import 'leave_filter_view.dart';

class LeaveStatusView extends GetView<LeaveStatusController> {
  const LeaveStatusView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    var statusBarHeight = MediaQuery.of(context).viewPadding.top;
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        title: SearchBarWidget(
            controllerText: controller.searchKeyController,
            changed: controller.searchEmp,
            hintText: "Employee",
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
                            child: LeaveFilterView(
                              oldVCriteraiData: controller.leaveCriteriaModel,
                              leaveCriteriaApply: controller.leaveFilterApply,
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
      body: GetBuilder<LeaveStatusController>(builder: (data) {
        if (controller.isLoading == false && controller.leaveList.isNotEmpty) {
          return SingleChildScrollView(
            child: Column(
              children: [
                ListView.builder(
                    scrollDirection: Axis.vertical,
                    shrinkWrap: true,
                    physics: const ScrollPhysics(),
                    itemCount: controller.leaveList.length,
                    itemBuilder: ((context, index) {
                      return LeaveCard(leaveData: controller.leaveList[index]);
                    }))
              ],
            ),
          );
        } else if (controller.isLoading == false &&
            controller.leaveList.isEmpty) {
          return const SafeArea(
              child: SingleChildScrollView(
            child: EmptyScreen(
                title: "Data Not Found!", imageUrl: "assets/images/empty.png"),
          ));
        } else {
          return SafeArea(
              child: ListView.builder(
                  scrollDirection: Axis.vertical,
                  shrinkWrap: true,
                  physics: const ScrollPhysics(),
                  itemCount: 6,
                  itemBuilder: ((context, index) {
                    return const LoadingLeaveCard();
                  })));
        }
      }),
    );
  }
}
