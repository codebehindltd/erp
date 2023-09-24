import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../../core/utils/size_config.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../data/models/res/leave/leave_application_list_model.dart';
import '../../../../../routes/app_pages.dart';

class LeaveCard extends StatelessWidget {
  final LeaveApplicationListModel leaveData;
  const LeaveCard({super.key, required this.leaveData});

  @override
  Widget build(BuildContext context) {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(10),
      ),
      child: InkWell(
        onTap: () {
          Get.toNamed(Routes.leaveApproval, arguments: leaveData.leaveId);
        },
        child: Container(
            color: themeColor.withOpacity(.03),
            padding: const EdgeInsets.all(8),
            child: Row(
              children: [
                SizedBox(
                    width: SizeConfig.screenWidth! * .28,
                    height: SizeConfig.screenWidth! * .28,
                    child: Container(
                      decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(100),
                          border: Border.all(width: 1, color: themeColor)),
                      child: ClipRRect(
                        borderRadius: BorderRadius.circular(100),
                        child: leaveData.profilePictureUrl != null
                            ? Image.network(leaveData.profilePictureUrl ?? '')
                            : Image.asset("assets/images/guest.png"),
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
                              text: 'Employee Name: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: "${leaveData.employeeName}",
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
                                  text: "${leaveData.typeName}",
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
                                  text: "${leaveData.createdDateString}",
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
                                      "${leaveData.showFromDate} to ${leaveData.showToDate}",
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
                                  text: "${leaveData.noOfDays}",
                                  style: valueStyle(),
                                ),
                              ]),
                        ),
                        buildSpace(),
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
