import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../../core/utils/utils_function.dart';
import '../../../../../core/values/colors.dart';
import '../../../../../data/models/req/leave_criteria_model.dart';
import '../../../../../global_widgets/custom_button.dart';
import '../../../../../global_widgets/input_text_field.dart';
import '../../../../common/controllers/common_controller.dart';

class LeaveFilterView extends StatefulWidget {
  final ValueChanged<LeaveCriteriaModel>? leaveCriteriaApply;
  final LeaveCriteriaModel? oldVCriteraiData;
  const LeaveFilterView(
      {Key? key, this.leaveCriteriaApply, this.oldVCriteraiData})
      : super(key: key);

  @override
  State<LeaveFilterView> createState() => _LeaveFilterViewState();
}

class _LeaveFilterViewState extends State<LeaveFilterView> {
  final TextEditingController vFromDateController = TextEditingController();
  final TextEditingController vToDateController = TextEditingController();

  final GlobalKey<ScaffoldState> scaffoldKey = GlobalKey();
  DateTime? selectFromDate;
  DateTime? selectToDate;
  final commonController = Get.find<CommonController>();

  @override
  void initState() {
    vFromDateController.text = widget.oldVCriteraiData!.fromDate != null
        ? dateFormat.format(DateTime.parse(widget.oldVCriteraiData!.fromDate!))
        : '';
    vToDateController.text = widget.oldVCriteraiData!.toDate != null
        ? dateFormat.format(DateTime.parse(widget.oldVCriteraiData!.fromDate!))
        : '';
    selectFromDate = widget.oldVCriteraiData!.fromDate != null
        ? DateTime.parse(widget.oldVCriteraiData!.fromDate!)
        : null;
    selectToDate = widget.oldVCriteraiData!.toDate != null
        ? DateTime.parse(widget.oldVCriteraiData!.toDate!)
        : null;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      color: Theme.of(context).scaffoldBackgroundColor,
      child: Column(
        children: [
          AppBar(
            key: scaffoldKey,
            backgroundColor: whiteColor,
            elevation: .5,
            leading: IconButton(
              icon: const Icon(
                Icons.close_rounded,
                color: blackColor,
              ),
              onPressed: () {
                // Navigator.of(context).pop();
                Get.back();
              },
            ),
            title: const Text('Filter'),
            centerTitle: true,
          ),
          Expanded(
            child: ListView(
              padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 10),
              children: [
                // InputField(
                //   controllerText: vNameController,
                //   labelText: "leave Number",
                // ),
                // const SizedBox(
                //   height: 20,
                // ),
                InputField(
                    controllerText: vFromDateController,
                    labelText: "From Date",
                    isReadOnly: true,
                    iconButton: IconButton(
                        onPressed: () async {
                          DateTime? datePick = await showDatePicker(
                              useRootNavigator: false,
                              context: context,
                              initialDate: selectFromDate ??
                                  selectToDate ??
                                  DateTime.now(),
                              firstDate: DateTime(2010),
                              lastDate: selectToDate ?? DateTime(2025));
                          if (datePick != null) {
                            vFromDateController.text =
                                dateFormat.format(datePick);
                            selectFromDate = datePick;
                          } else {
                            vFromDateController.text = "";
                            selectFromDate = null;
                          }
                        },
                        icon: const Icon(Icons.calendar_month_outlined))),
                const SizedBox(
                  height: 12,
                ),
                InputField(
                    controllerText: vToDateController,
                    labelText: "To Date",
                    isReadOnly: true,
                    iconButton: IconButton(
                        onPressed: () async {
                          DateTime? datePick = await showDatePicker(
                              useRootNavigator: false,
                              context: context,
                              initialDate: selectToDate ??
                                  selectFromDate ??
                                  DateTime.now(),
                              firstDate: selectFromDate ?? DateTime(2010),
                              lastDate: DateTime(2025));
                          if (datePick != null) {
                            vToDateController.text =
                                dateFormat.format(datePick);
                            selectToDate = datePick;
                          } else {
                            vToDateController.text = "";
                            selectToDate = null;
                          }
                        },
                        icon: const Icon(Icons.calendar_month_outlined))),
                const SizedBox(
                  height: 10,
                ),
                buildLeaveStatusDropdownMenu(["Pending"])
              ],
            ),
          ),
          filterButton()
        ],
      ),
    );
  }

  Widget filterButton() {
    return Container(
        padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
        child: CustomButton(
            submit: (v) {
              LeaveCriteriaModel data = LeaveCriteriaModel(
                  pageNumber: 1,
                  pageSize: 10,
                  userInfoId: commonController.propertyUserInfoId.toString(),
                  fromDate: selectFromDate != null
                      ? selectFromDate!.toIso8601String()
                      : null,
                  toDate: selectToDate != null
                      ? selectToDate!.toIso8601String()
                      : null);
              widget.leaveCriteriaApply!(data);
              Get.back();
            },
            name: "Apply",
            fullWidth: true));
  }

  String? selectedLeaveStatus;
  selectLeaveStatus(v) {
    selectedLeaveStatus = v;
  }

  DropdownSearch<String> buildLeaveStatusDropdownMenu(
      List<String> leaveTypeList) {
    return DropdownSearch<String>(
      dropdownDecoratorProps: const DropDownDecoratorProps(
          dropdownSearchDecoration: InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 8, horizontal: 15),
              labelText: "Leave Status",
              border: OutlineInputBorder())),
      itemAsString: (item) => item,
      items: leaveTypeList,
      onChanged: selectLeaveStatus,
      enabled: leaveTypeList != null ? true : false,
      selectedItem: selectedLeaveStatus,
      validator: (value) {
        if (value == null) {
          return "Please select leave Status";
        } else {
          return null;
        }
      },
    );
  }
}
