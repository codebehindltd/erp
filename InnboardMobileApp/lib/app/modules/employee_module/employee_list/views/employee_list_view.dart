import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:pull_to_refresh_flutter3/pull_to_refresh_flutter3.dart';

import '../../../../global_widgets/back_widget.dart';
import '../../../../global_widgets/custom_footer_refresher.dart';
import '../../../../global_widgets/empty_widget.dart';
import '../../../../global_widgets/search_bar.dart';
import '../controllers/employee_list_controller.dart';
import '../widgets/employee_card.dart';
import '../widgets/employee_loading_card.dart';

class EmployeeListView extends GetView<EmployeeListController> {
  const EmployeeListView({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        title: SearchBarWidget(
            controllerText: controller.searchKeyController,
            changed: controller.searchEmp,
            autofocus: false),
      ),
      body: GetBuilder<EmployeeListController>(
        builder: (data) {
          if (!controller.isLoading && controller.empList.isNotEmpty) {
            return SafeArea(
                child: SmartRefresher(
                    controller: controller.refreshController,
                    enablePullUp: true,
                    enablePullDown: true,
                    onLoading: controller.onLoading,
                    onRefresh: controller.onRefresh,
                    physics: const BouncingScrollPhysics(),
                    footer: const CustomFooterRefresher(),
                    child: ListView.builder(
                        scrollDirection: Axis.vertical,
                        shrinkWrap: true,
                        physics: const ScrollPhysics(),
                        itemCount: data.empList.length,
                        itemBuilder: ((context, index) {
                          return EmployeeCard(empModel: data.empList[index]);
                        }))));
          } else if (!controller.isLoading && controller.empList.isEmpty) {
            return const SafeArea(
                child: SingleChildScrollView(
                  child: EmptyScreen(
                      title: "Data Not Found!",
                      imageUrl: "assets/images/empty.png"),
                ));
          } else {
            return SafeArea(
                child: ListView.builder(
                    scrollDirection: Axis.vertical,
                    shrinkWrap: true,
                    physics: const ScrollPhysics(),
                    itemCount: 6,
                    itemBuilder: ((context, index) {
                      return const LoadingEmpCard();
                    })));
          }
        },
      ),
    );
  }
}
