import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../data/models/res/emp_model.dart';
import '../../../../../data/models/res/leave/leave_type_model.dart';
import '../../../../../global_widgets/back_widget.dart';
import '../../../../../global_widgets/custom_button.dart';
import '../../../../../global_widgets/input_text_field.dart';
import '../controllers/leave_application_controller.dart';

class LeaveApplicationView extends GetView<LeaveApplicationController> {
  const LeaveApplicationView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        title: const Text(
          'Leave Application',
          style: TextStyle(color: whiteColor),
        ),
      ),
      body: GetBuilder<LeaveApplicationController>(builder: (data) {
        return SingleChildScrollView(
          child: Container(
            padding: const EdgeInsets.symmetric(vertical: 8, horizontal: 12),
            child: Form(
              key: controller.formKey,
              child: Column(
                children: [
                  const SizedBox(
                    height: 20,
                  ),
                  buildLeaveTypeDropdownMenu(controller.leaveTypes),
                  const SizedBox(
                    height: 12,
                  ),
                  InputField(
                      controllerText: controller.vToDateController,
                      labelText: "From Date",
                      isReadOnly: true,
                      validationText: "Please select date",
                      iconButton: IconButton(
                          onPressed: () async {
                            DateTime? datePick = await showDatePicker(
                                useRootNavigator: false,
                                context: context,
                                initialDate: controller.fromDate ??
                                    controller.toDate ??
                                    DateTime.now(),
                                firstDate: DateTime(2000),
                                lastDate: controller.toDate ?? DateTime(2024));
                            controller.selectFromDate(datePick);
                          },
                          icon: const Icon(Icons.calendar_month_outlined))),
                  const SizedBox(
                    height: 12,
                  ),
                  InputField(
                      controllerText: controller.vFromDateController,
                      labelText: "To Date",
                      isReadOnly: true,
                      validationText: "Please select date",
                      iconButton: IconButton(
                          onPressed: () async {
                            DateTime? datePick = await showDatePicker(
                                useRootNavigator: false,
                                context: context,
                                initialDate: controller.toDate ??
                                    controller.fromDate ??
                                    DateTime.now(),
                                firstDate:
                                    controller.fromDate ?? DateTime(2000),
                                lastDate: DateTime(2024));
                            controller.selectToDate(datePick);
                          },
                          icon: const Icon(Icons.calendar_month_outlined))),
                  const SizedBox(
                    height: 12,
                  ),
                  InputField(
                    controllerText: controller.vNoOfDaysController,
                    labelText: "No Of Days",
                    isReadOnly: true,
                    isBold: true,
                  ),
                  const SizedBox(
                    height: 12,
                  ),
                  buildActiveEmpDropdownMenu(controller.empActiveList),
                  const SizedBox(
                    height: 12,
                  ),
                  InputField(
                    controllerText: controller.descriptionController,
                    keyboardType: TextInputType.text,
                    validationText: "Please enter description",
                    labelText: "Description",
                    maxLines: 5,
                  ),
                  const SizedBox(
                    height: 20,
                  ),
                  CustomButton(
                      submit: controller.submit,
                      name: "Apply",
                      fullWidth: false)
                ],
              ),
            ),
          ),
        );
      }),
    );
  }

  DropdownSearch<LeaveTypeModel> buildLeaveTypeDropdownMenu(
      List<LeaveTypeModel> leaveTypeList) {
    return DropdownSearch<LeaveTypeModel>(
      dropdownDecoratorProps: const DropDownDecoratorProps(
          dropdownSearchDecoration: InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 8, horizontal: 15),
              labelText: "Leave Type",
              border: OutlineInputBorder())),
      itemAsString: (item) => item.leaveTypeName!,
      items: leaveTypeList,
      onChanged: controller.selectLeaveType,
      enabled: leaveTypeList != null ? true : false,
      selectedItem: controller.selectedLeaveType,
      validator: (value) {
        if (value == null) {
          return "Please select leave type";
        } else {
          return null;
        }
      },
    );
  }

  DropdownSearch<EmpModel> buildActiveEmpDropdownMenu(List<EmpModel> empList) {
    return DropdownSearch<EmpModel>(
      dropdownDecoratorProps: const DropDownDecoratorProps(
          dropdownSearchDecoration: InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 8, horizontal: 15),
              labelText: "Work Handover",
              border: OutlineInputBorder())),
      itemAsString: (item) => item.displayName ?? '',
      items: empList,
      onChanged: controller.selectActiveEmp,
      enabled: empList != null ? true : false,
      selectedItem: controller.selectedActiveEmp,
    );
  }
}
