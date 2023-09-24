import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../../core/utils/size_config.dart';
import '../../../../core/values/colors.dart';
import '../../../../data/models/res/emp_model.dart';
import '../../../../routes/app_pages.dart';

class EmployeeCard extends StatelessWidget {
  final EmpModel empModel;
  const EmployeeCard({Key? key, required this.empModel}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(10),
      ),
      child: InkWell(
        
        onTap: () {
          // var data={
          //   'EmpId': 1
          // };
          Get.toNamed(Routes.employeeDetails, arguments: empModel.toJson());
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
                      tag: "${empModel.empId}",
                      child: Container(
                        decoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(100),
                            border: Border.all(width: 1, color: themeColor)),
                        child: ClipRRect(
                          borderRadius: BorderRadius.circular(100),
                          child: empModel.imageUrl != null
                              ? Image.network(empModel.imageUrl ?? '')
                              : Image.asset("assets/images/guest.png"),
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
                              text: 'Employee Id: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: empModel.empCode ?? '',
                                  style: valueStyle(),
                                ),
                              ]),
                        ),
                        buildSpace(),
                        RichText(
                          text: TextSpan(
                              text: 'Name: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: empModel.displayName ?? '',
                                  style: valueStyle(),
                                ),
                              ]),
                        ),
                        buildSpace(),
                        RichText(
                          text: TextSpan(
                              text: 'Designation: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: empModel.designationName ?? '',
                                  style: valueStyle(),
                                ),
                              ]),
                        ),
                        buildSpace(),
                        RichText(
                          text: TextSpan(
                              text: 'Department: ',
                              style: titleStyle(),
                              children: [
                                TextSpan(
                                  text: empModel.departmentName ?? '',
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
