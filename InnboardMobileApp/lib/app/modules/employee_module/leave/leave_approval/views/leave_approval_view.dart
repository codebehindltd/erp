import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../global_widgets/back_widget.dart';
import '../../../../../global_widgets/custom_button.dart';
import '../controllers/leave_approval_controller.dart';

class LeaveApprovalView extends GetView<LeaveApprovalController> {
  const LeaveApprovalView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        title: const Text(
          "Leave Information",
          style: TextStyle(color: whiteColor),
        ),
      ),
      body: GetBuilder<LeaveApprovalController>(builder: (d) {
        if (controller.isLoading || controller.leaveData == null) {
          return const Center(
            child: CircularProgressIndicator(),
          );
        } else {
          return SingleChildScrollView(
            child: Container(
              padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 8),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  RichText(
                    text: TextSpan(
                        text: 'Employee Name: ',
                        style: titleStyle(),
                        children: [
                          TextSpan(
                            text: "${controller.leaveData!.employeeName}",
                            style: valueStyle(),
                          ),
                        ]),
                  ),
                  buildSpace(),
                  RichText(
                    text: TextSpan(
                        text: 'Leave Type: ',
                        style: titleStyle(),
                        children: [
                          TextSpan(
                            text: "${controller.leaveData!.typeName}",
                            style: valueStyle(),
                          ),
                        ]),
                  ),
                  buildSpace(),
                  RichText(
                    text: TextSpan(
                        text: 'Application Date: ',
                        style: titleStyle(),
                        children: [
                          TextSpan(
                            text: "${controller.leaveData!.showCreatedDate}",
                            style: valueStyle(),
                          ),
                        ]),
                  ),
                  buildSpace(),
                  RichText(
                    text: TextSpan(
                        text: 'Date: ',
                        style: titleStyle(),
                        children: [
                          TextSpan(
                            text:
                                "${controller.leaveData!.showFromDate} to ${controller.leaveData!.showToDate}",
                            style: valueStyle(),
                          ),
                        ]),
                  ),
                  buildSpace(),
                  RichText(
                    text: TextSpan(
                        text: 'No Of Days: ',
                        style: titleStyle(),
                        children: [
                          TextSpan(
                            text: "${controller.leaveData!.noOfDays}",
                            style: valueStyle(),
                          ),
                        ]),
                  ),
                  buildSpace(),
                  RichText(
                    text: TextSpan(
                        text: 'Leave Status: ',
                        style: titleStyle(),
                        children: [
                          TextSpan(
                            text: "${controller.leaveData!.leaveStatus}",
                            style: valueStyle(),
                          ),
                        ]),
                  ),
                  buildSpace(),
                  const SizedBox(
                    height: 12,
                  ),
                  Visibility(
                    visible: controller.leaveData!.isCanApprove!,
                    child: CustomButton(
                        submit: controller.approved,
                        name: "Approve",
                        fullWidth: false),
                  ),
                  Visibility(
                    visible: controller.leaveData!.isCanCheck!,
                    child: CustomButton(
                        submit: controller.checked,
                        name: "Check",
                        fullWidth: false),
                  ),
                ],
              ),
            ),
          );
        }
      }),
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
