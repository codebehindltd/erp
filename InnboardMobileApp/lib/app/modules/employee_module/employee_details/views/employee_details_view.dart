import 'package:flutter/material.dart';

import 'package:get/get.dart';
import 'package:url_launcher/url_launcher.dart';

import '../../../../core/utils/size_config.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/back_widget.dart';
import '../controllers/employee_details_controller.dart';

class EmployeeDetailsView extends GetView<EmployeeDetailsController> {
  const EmployeeDetailsView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          leading: const BackButtonWidget(),
          elevation: 0,
        ),
        body: GetBuilder<EmployeeDetailsController>(builder: (data) {
          return SingleChildScrollView(
            child: Container(
              padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 12),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const SizedBox(
                    height: 15,
                  ),
                  Center(
                    child: SizedBox(
                        width: SizeConfig.screenWidth! * .35,
                        height: SizeConfig.screenWidth! * .35,
                        child: Hero(
                          tag: "${data.empModel.empId}",
                          // transitionOnUserGestures: true,
                          child: Container(
                            decoration: BoxDecoration(
                                borderRadius: BorderRadius.circular(100),
                                border:
                                    Border.all(width: 1, color: themeColor)),
                            child: ClipRRect(
                              borderRadius: BorderRadius.circular(100),
                              child: data.empModel.imageUrl != null
                                  ? Image.network(data.empModel.imageUrl ?? '')
                                  : Image.asset("assets/images/guest.png"),
                            ),
                          ),
                        )),
                  ),
                  divider(),
                  Center(
                    child: Text(
                      data.empModel.displayName ?? '',
                      style: Theme.of(context).textTheme.headline5,
                    ),
                  ),
                  const SizedBox(
                    height: 50,
                  ),
                  Row(
                    children: [
                      SizedBox(
                          width: SizeConfig.screenWidth! * .26,
                          child: Text(
                            "Employee Id: ",
                            style: titleStyle(),
                          )),
                      SizedBox(
                        width: SizeConfig.screenWidth! * .66,
                        child: Text(
                          data.empModel.empCode ?? '',
                          style: valueStyle(),
                        ),
                      )
                    ],
                  ),
                  divider(),
                  Row(
                    children: [
                      SizedBox(
                          width: SizeConfig.screenWidth! * .26,
                          child: Text(
                            "Designation: ",
                            style: titleStyle(),
                          )),
                      SizedBox(
                        width: SizeConfig.screenWidth! * .66,
                        child: Text(
                          data.empModel.designationName ?? '',
                          style: valueStyle(),
                        ),
                      )
                    ],
                  ),
                  divider(),
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      SizedBox(
                          width: SizeConfig.screenWidth! * .26,
                          child: Text(
                            "Department: ",
                            style: titleStyle(),
                          )),
                      SizedBox(
                        width: SizeConfig.screenWidth! * .66,
                        child: Text(
                          data.empModel.departmentName ?? '',
                          style: valueStyle(),
                        ),
                      )
                    ],
                  ),
                  divider(),
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      SizedBox(
                          width: SizeConfig.screenWidth! * .26,
                          child: Text(
                            "Address: ",
                            style: titleStyle(),
                          )),
                      SizedBox(
                        width: SizeConfig.screenWidth! * .66,
                        child: Text(data.empModel.presentAddress ?? '',
                            style: valueStyle()),
                      )
                    ],
                  ),
                  divider(),
                  // Row(
                  //   crossAxisAlignment: CrossAxisAlignment.center,
                  //   children: [
                  //     SizedBox(
                  //         width: SizeConfig.screenWidth! * .26,
                  //         child: Text(
                  //           "Permanent Address: ",
                  //           style: titleStyle(),
                  //         )),
                  //     SizedBox(
                  //       width: SizeConfig.screenWidth! * .66,
                  //       child: Text(data.empModel.permanentAddress ?? '',
                  //           style: valueStyle()),
                  //     )
                  //   ],
                  // ),
                  // divider(),
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      SizedBox(
                          width: SizeConfig.screenWidth! * .26,
                          child: Text(
                            "Device Info: ",
                            style: titleStyle(),
                          )),
                      SizedBox(
                        width: SizeConfig.screenWidth! * .66,
                        child: Text(
                          data.empModel.deviceInfo ?? '',
                          style: valueStyle(),
                        ),
                      )
                    ],
                  ),
                  divider(),
                  TextButton(
                    onPressed: () {
                      String? link = data.empModel.googleMapUrl;
                      openGoogleMap(context, link);
                    },
                    child: const Text(
                      "Open Google Map",
                      textAlign: TextAlign.start,
                    ),
                  ),
                ],
              ),
            ),
          );
        }));
  }

  TextStyle valueStyle() => const TextStyle(fontSize: 14);

  TextStyle titleStyle() {
    return const TextStyle(fontSize: 16, fontWeight: FontWeight.w600);
  }

  SizedBox divider() {
    return const SizedBox(
      height: 30,
    );
  }

  openGoogleMap(context, String? link) async {
    if (link == null) {
      Get.snackbar(
        "Failed!",
        'Current location not available right now.',
        icon: const Icon(Icons.warning_amber_rounded),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
      return;
    }
    Uri url = Uri.parse(link);
    if (!await launchUrl(url)) {
      Get.snackbar(
        "Oops!",
        'Could not launch google map. Please, try again!',
        icon: const Icon(Icons.warning_amber_rounded),
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: warningColor,
      );
    }
  }
}
