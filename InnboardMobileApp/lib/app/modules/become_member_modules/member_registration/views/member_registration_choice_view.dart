import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:leading_edge/app/data/models/res/members/membership_type.dart';
import 'package:leading_edge/app/global_widgets/back_widget.dart';
import 'package:leading_edge/app/routes/app_pages.dart';
import '../../../../global_widgets/custom_button.dart';
import '../controllers/member_registration_controller.dart';

class MemberRegistrationChoiceView
    extends GetView<MemberRegistrationController> {
  const MemberRegistrationChoiceView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title:  Text(
            'Member Registration Choice',
            style: Theme.of(context).textTheme.titleLarge,
          ),
          centerTitle: true,
          leading: const BackButtonWidget(),
        ),
        body: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.all(12.0),
            child: Obx(() {
              if (MemberRegistrationController.isLoading.value) {
                return const Center(child: CircularProgressIndicator());
              } else {
                return Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      // Container(
                      //   padding: const EdgeInsets.only(left: 16, right: 16),
                      //   decoration: BoxDecoration(
                      //       borderRadius: BorderRadius.circular(7),
                      //       border: Border.all(
                      //         color: Colors.grey,
                      //         width: 1,
                      //       )),
                      //   child: DropdownButton(
                      //       value: controller.selectedType,
                      //       hint: const Text(
                      //         "Select",
                      //         style: TextStyle(
                      //           color: Colors.grey,
                      //           fontWeight: FontWeight.w700,
                      //         ),
                      //       ),
                      //       isExpanded: true,
                      //       underline: const SizedBox(),
                      //       icon: const Icon(
                      //         Icons.expand_more,
                      //         color: Colors.grey,
                      //       ),
                      //       items: controller.items.map((valueItem) {
                      //         return DropdownMenuItem(
                      //           value: valueItem,
                      //           child: Text(
                      //             valueItem,
                      //             style: const TextStyle(
                      //                 color: Color(0xff1B488B),
                      //                 fontFamily: 'Baloo Da 2',
                      //                 fontWeight: FontWeight.w400,
                      //                 fontSize: 20),
                      //           ),
                      //         );
                      //       }).toList(),
                      //       onChanged: (newValue) {
                      //         controller.setSelectedOption(newValue.toString());
                      //       }),
                      // ),
                      const SizedBox(
                        height: 15,
                      ),
                      buildTypeDropdownMenu(controller.membershipSetupData),
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: [
                          Padding(
                            padding: const EdgeInsets.only(top: 12, bottom: 5),
                            child: Text(
                              'Price',
                              style: Theme.of(context).textTheme.labelLarge,
                            ),
                          ),
                          Container(
                            width: double.infinity,
                            padding: const EdgeInsets.all(12),
                            decoration: BoxDecoration(
                              border: Border.all(color: borderColor),
                              borderRadius: BorderRadius.circular(6),
                            ),
                            child: Text(
                                "${controller.selectedType.value.subscriptionFee} BDT",
                                style: commonTextStyle()),
                          ),
                          Padding(
                            padding: const EdgeInsets.only(top: 12, bottom: 5),
                            child: Text(
                              'Benefits',
                             style: Theme.of(context).textTheme.labelLarge,
                            ),
                          ),
                          Container(
                            width: double.infinity,
                            padding: const EdgeInsets.all(12),
                            decoration: BoxDecoration(
                              border: Border.all(color: borderColor),
                              borderRadius: BorderRadius.circular(6),
                            ),
                            child: ListView.builder(
                                physics: const ScrollPhysics(),
                                shrinkWrap: true,
                                itemCount: controller
                                    .selectedType.value.benefits!.length,
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
                                              "${controller.selectedType.value.benefits![index].benefitsName}",
                                              maxLines: 1,
                                              overflow: TextOverflow.ellipsis,
                                              style: commonTextStyle()),
                                          Text(
                                              "${controller.selectedType.value.benefits![index].benefitsValue} ${controller.selectedType.value.benefits![index].benefitsTransactionType}",
                                              maxLines: 1,
                                              overflow: TextOverflow.ellipsis,
                                              style: commonTextStyle()),
                                        ],
                                      ),
                                    ],
                                  );
                                }),
                          ),
                          Padding(
                            padding: const EdgeInsets.only(top: 12, bottom: 5),
                            child: Text(
                              'Terms & Conditions',
                              style: Theme.of(context).textTheme.labelLarge,
                            ),
                          ),
                          Container(
                              width: double.infinity,
                              //height: 300,
                              padding: const EdgeInsets.all(12),
                              decoration: BoxDecoration(
                                border: Border.all(color: borderColor),
                                borderRadius: BorderRadius.circular(6),
                              ),
                              child: ListView.builder(
                                  physics: const ScrollPhysics(),
                                  shrinkWrap: true,
                                  // itemCount: controller.lng.value,
                                  itemCount: controller
                                      .selectedType.value.termTitles!.length,
                                  itemBuilder: (context, index) {
                                    bool isHasTitle = controller
                                                .selectedType
                                                .value
                                                .termTitles![index]
                                                .tncType !=
                                            null
                                        ? true
                                        : false;
                                    return Padding(
                                      padding: const EdgeInsets.only(bottom: 5),
                                      child: Column(
                                        children: [
                                          Visibility(
                                            visible: isHasTitle,
                                            child: Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.start,
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.start,
                                              children: [
                                                // Text("\u2022 ",
                                                //     style: commonTextStyle()),
                                                Padding(
                                                  padding:
                                                      const EdgeInsets.only(
                                                          top: 2),
                                                  child: Icon(
                                                    Icons.east_outlined,
                                                    color: themeColor,
                                                    size: 18,
                                                  ),
                                                ),
                                                Flexible(
                                                  child: Text(
                                                      " ${controller.selectedType.value.termTitles![index].tncType}",
                                                      style: termsTextStyle()),
                                                ),
                                              ],
                                            ),
                                          ),
                                          Row(
                                            children: [
                                              SizedBox(
                                                width: isHasTitle ? 15 : 0,
                                              ),
                                              Flexible(
                                                child: ListView.builder(
                                                  physics:
                                                      const ScrollPhysics(),
                                                  shrinkWrap: true,
                                                  itemCount: controller
                                                      .selectedType
                                                      .value
                                                      .termTitles![index]
                                                      .termSubTitiles!
                                                      .length,
                                                  itemBuilder:
                                                      (context, indexS) {
                                                    var subTitle = controller
                                                            .selectedType
                                                            .value
                                                            .termTitles![index]
                                                            .termSubTitiles ??
                                                        [];
                                                    return Row(
                                                      mainAxisAlignment:
                                                          MainAxisAlignment
                                                              .start,
                                                      crossAxisAlignment:
                                                          CrossAxisAlignment
                                                              .start,
                                                      children: [
                                                        // Text("\u2022 ",
                                                        //     style: commonTextStyle()),
                                                        Padding(
                                                          padding:
                                                              const EdgeInsets
                                                                  .only(top: 2),
                                                          child: Icon(
                                                            Icons
                                                                .remove_outlined,
                                                            color: themeColor,
                                                            size: 18,
                                                          ),
                                                        ),
                                                        Flexible(
                                                          child: Text(
                                                              " ${subTitle[indexS].tncDetails}",
                                                              style:
                                                                  termsTextStyle()),
                                                        ),
                                                      ],
                                                    );
                                                  },
                                                ),
                                              ),
                                            ],
                                          ),
                                        ],
                                      ),
                                    );
                                  })),
                          //TextButton(onPressed: (){controller.toggleExpanded(true);}, child: Text("Show More")),
                          const SizedBox(
                            height: 30,
                          ),
                          Padding(
                            padding: const EdgeInsets.all(18.0),
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                CustomButton(
                                    submit: (value) {
                                      Get.toNamed(
                                        Routes.memberRegistration +
                                            Routes.payment,
                                      );
                                    },
                                    name: "Next",
                                    fullWidth: false,
                                    horizontalPadding: 44,
                                    fontSize: 22),
                              ],
                            ),
                          ),
                          const SizedBox(
                            height: 10,
                          ),
                        ],
                      )
                    ]);
              }
            }),
          ),
          // bottomNavigationBar: Padding(
          //   padding: const EdgeInsets.only(left: 14, right: 14, bottom: 4),
          //   child: Row(
          //     mainAxisAlignment: MainAxisAlignment.end,
          //     children: [
          //       CustomButtonN(
          //           submit: (value) {
          //             // controller.signUp(context);
          //           },
          //           name: "Next",
          //           width: 180,
          //           height: 60,
          //           fontSize: 22),
          //     ],
          //   ),
          // ),
        ));
  }

  DropdownSearch<MemberShipType> buildTypeDropdownMenu(
      List<MemberShipType> list) {
    return DropdownSearch<MemberShipType>(
      //compareFn: false,
      popupProps: PopupPropsMultiSelection.menu(
        showSelectedItems: false,
        itemBuilder: _customPopupItemBuilder,
        // showSearchBox: true,
      ),
      dropdownDecoratorProps: DropDownDecoratorProps(
        baseStyle: TextStyle(
            color: themeColor, fontWeight: FontWeight.w400, fontSize: 16),
        dropdownSearchDecoration: const InputDecoration(
            contentPadding: EdgeInsets.symmetric(vertical: 6, horizontal: 12),
            labelText: "Choice Membership",
            enabledBorder: OutlineInputBorder(
              borderSide: BorderSide(width: 1, color: borderColor),
              borderRadius: BorderRadius.all(Radius.circular(6)),
            ),
            border: OutlineInputBorder()),
      ),

      itemAsString: (item) => item.name ?? '',
      items: list,
      onChanged: (value) {
        controller.setSelectedOption(value);
      },
      enabled: list.isNotEmpty ? true : false,
      selectedItem: controller.selectedType.value,

      validator: (value) {
        if (value == null) {
          return "Please select type";
        } else {
          return null;
        }
      },
    );
  }

  Widget _customPopupItemBuilder(
      BuildContext context, MemberShipType item, bool isSelected) {
    return Container(
      margin: const EdgeInsets.symmetric(horizontal: 8),
      decoration: !isSelected
          ? null
          : BoxDecoration(
              border: Border.all(color: Theme.of(context).primaryColor),
              borderRadius: BorderRadius.circular(5),
              color: Colors.white,
            ),
      child: ListTile(
        selected: isSelected,
        title: Text(item.name.toString(), style: commonTextStyle()),
      ),
    );
  }

  TextStyle commonTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w400, fontSize: 16);
  }

  TextStyle termsTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w400, fontSize: 14);
  }

  
}
