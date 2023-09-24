import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';

import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:leading_edge/app/data/models/req/voucher_criteria_model.dart';
import 'package:leading_edge/app/global_widgets/custom_button.dart';
import '../../../../../core/utils/utils_function.dart';
import '../../../../../data/models/res/company_model.dart';
import '../../../../../global_widgets/input_text_field.dart';
import '../../../../common/controllers/common_controller.dart';

class VoucherFilterView extends StatefulWidget {
  final ValueChanged<VoucherCriteriaModel>? voucherCriteriaApply;
  final VoucherCriteriaModel? oldVCriteraiData;
  const VoucherFilterView(
      {Key? key, this.voucherCriteriaApply, this.oldVCriteraiData})
      : super(key: key);

  @override
  State<VoucherFilterView> createState() => _VoucherFilterViewState();
}

class _VoucherFilterViewState extends State<VoucherFilterView> {
  final TextEditingController vNameController = TextEditingController();
  final TextEditingController vDateController = TextEditingController();

  final GlobalKey<ScaffoldState> scaffoldKey = GlobalKey();
  DateTime? selectDate;
  final commonController = Get.find<CommonController>();

  @override
  void initState() {
    vNameController.text = widget.oldVCriteraiData!.voucherNo ?? '';
    vDateController.text = widget.oldVCriteraiData!.fromDate != null
        ? dateFormat.format(DateTime.parse(widget.oldVCriteraiData!.fromDate!))
        : '';
    selectDate = widget.oldVCriteraiData!.fromDate != null
        ? DateTime.parse(widget.oldVCriteraiData!.fromDate!)
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
                InputField(
                  controllerText: vNameController,
                  labelText: "Voucher Number",
                ),
                const SizedBox(
                  height: 20,
                ),
                InputField(
                    controllerText: vDateController,
                    labelText: "Date",
                    isReadOnly: true,
                    iconButton: IconButton(
                        onPressed: () async {
                          DateTime? datePick = await showDatePicker(
                              useRootNavigator: false,
                              context: context,
                              initialDate: selectDate ?? DateTime.now(),
                              firstDate: DateTime(2000),
                              lastDate: DateTime(2024));
                          if (datePick != null) {
                            vDateController.text = dateFormat.format(datePick);
                            selectDate = datePick;
                          } else {
                            vDateController.text = "";
                            selectDate = null;
                          }
                        },
                        icon: const Icon(Icons.calendar_month_outlined))),
                const SizedBox(
                  height: 10,
                ),
                // buildCompanyDropdownMenu([]),
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
              VoucherCriteriaModel data = VoucherCriteriaModel(
                  pageNumber: 1,
                  pageSize: 10,
                  companyId: "1",
                  projectId: "1",
                  userGroupId: "1",
                  userInfoId: commonController.propertyUserInfoId.toString(),
                  voucherNo: vNameController.text,
                  fromDate:
                      selectDate != null ? selectDate!.toIso8601String() : null,
                  toDate: selectDate != null
                      ? selectDate!.toIso8601String()
                      : null);
              widget.voucherCriteriaApply!(data);
              Get.back();
            },
            name: "Apply",
            fullWidth: true));
  }

  CompanyModel? selectCompany;

  DropdownSearch<CompanyModel> buildCompanyDropdownMenu(
      List<CompanyModel>? companyList) {
    return DropdownSearch<CompanyModel>(
      // popupProps: PopupProps.menu(
      //   showSelectedItems: true,
      //   disabledItemFn: (String s) => s.startsWith('I'),
      // ),
      // dropdownSearchDecoration: InputDecoration(labelText: "Name"),
      // dropdownButtonProps: DropdownButtonProps(tooltip: ";slafkdj") ,
      dropdownDecoratorProps: const DropDownDecoratorProps(
          // baseStyle: TextStyle(fontSize: 12)
          dropdownSearchDecoration: InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 8, horizontal: 15),
              labelText: "Company",
              border: OutlineInputBorder(
                  // borderRadius: BorderRadius.circular(8)
                  ))),
      // popupProps: const PopupProps.dialog(),
      // popupProps: PopupProps.modalBottomSheet(
      // modalBottomSheetProps: ModalBottomSheetProps(),
      // listViewProps: ListViewProps(padding: EdgeInsets.all(5)),
      // containerBuilder:(context, popupWidget) {
      //   return Text("dfa");
      // },
      // modalBottomSheetProps: ModalBottomSheetProps(useRootNavigator: true,clipBehavior: Clip.antiAliasWithSaveLayer),
      // title: Stack(
      //   children: [
      //     Container(height: 50, width: double.infinity, color: Colors.amber,),

      //     Positioned(
      //       bottom: -15,
      //       child: Container(
      //         color: blackColor1,
      //         padding: EdgeInsets.only(top: 0, left: 8, right: 0, bottom: 0),
      //         child: Column(
      //           children: [
      //             Row(
      //               children: [
      //                 Text("Country"),
      //                 IconButton(
      //                   onPressed: () {
      //                     Navigator.of(context).pop();
      //                   },
      //                   icon: Icon(Icons.close),
      //                 )
      //               ],
      //             ),
      //             Divider(height: 1, color: blackColor2,)
      //           ],
      //         ),
      //       ),
      //     ),
      //   ],
      // ),
      // ),
      itemAsString: (item) => item.name!,
      items: companyList ?? [],
      // dropdownSearchDecoration: InputDecoration(
      //   labelText: "Menu mode",
      //   hintText: "country in menu mode",
      // ),
      onChanged: (value) {},
      enabled: companyList != null ? true : false,
      selectedItem: selectCompany,
    );
  }
}
