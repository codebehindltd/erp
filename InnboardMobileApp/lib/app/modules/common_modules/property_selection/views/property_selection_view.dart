import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../global_widgets/back_widget.dart';
import '../../../../global_widgets/custom_shimmer_loader.dart';
import '../controllers/property_selection_controller.dart';
import '../widgets/property_card.dart';

class PropertySelectionView extends GetView<PropertySelectionController> {
  const PropertySelectionView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return GetBuilder<PropertySelectionController>(builder: (_) {
      return Scaffold(
        appBar: controller.isShowBackbtn
            ? AppBar(
                centerTitle: true,
                leading: const BackButtonWidget(),
              )
            : null,
        body: SafeArea(
            child: SingleChildScrollView(
                child: Container(
          width: double.infinity,
          padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 10),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 18),
                child: controller.isLoading == false
                    ? ListView.separated(
                        itemCount: controller.propertyList.length,
                        shrinkWrap: true,
                        separatorBuilder: (context, index) => const SizedBox(
                              height: 25,
                            ),
                        physics: const ScrollPhysics(),
                        itemBuilder: (context, index) {
                          return InkWell(
                            onTap: () {
                              controller.propertySelect(
                                  controller.propertyList[index]);
                            },
                            child: PropertyCard(
                                property: controller.propertyList[index]),
                          );
                        })
                    : ListView.separated(
                        itemCount: 5,
                        shrinkWrap: true,
                        separatorBuilder: (context, index) => const SizedBox(
                              height: 20,
                            ),
                        physics: const ScrollPhysics(),
                        itemBuilder: (context, index) {
                          return CustomShimmerLoader(
                            height: 138,
                            //width: SizeConfig.blockSizeHorizontal! * 30,
                            radius: 10,
                          );
                        }),
              )
            ],
          ),
        ))),
      );
    });
  }
}
