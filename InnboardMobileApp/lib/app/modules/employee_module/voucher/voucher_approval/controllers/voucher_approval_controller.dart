import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/models/req/voucher_criteria_model.dart';
import 'package:pull_to_refresh_flutter3/pull_to_refresh_flutter3.dart';

import '../../../../../data/models/res/voucher_list_model.dart';
import '../../../../../data/services/general_ledger_service.dart';
import '../../../../common/controllers/common_controller.dart';

class VoucherApprovalController extends GetxController {
  final commonController = Get.find<CommonController>();
  final TextEditingController searchKeyController = TextEditingController();
  VoucherCriteriaModel? vCriteriaModel;
  List<VoucherResModel> voucherList = [];
  bool isLoading = true;
  final RefreshController refreshController =
      RefreshController(initialRefresh: false);
  @override
  void onInit() {
    super.onInit();
  }

  @override
  void onReady() {
    vCriteriaModel = VoucherCriteriaModel(
      pageNumber: 1,
      pageSize: 10,
      companyId: "1",
      projectId: "1",
      userGroupId: "1",
      userInfoId: commonController.propertyUserInfoId.toString(),
      fromDate: DateTime.now().toIso8601String(),
      toDate: DateTime.now().toIso8601String(),
      // toDate: "2023-03-02"
    );

    getVoucherList();
    super.onReady();
  }

  @override
  void onClose() {
    super.onClose();
  }

  searchEmp(value) {
    vCriteriaModel!.pageNumber = 1;
    vCriteriaModel!.voucherNo = value;
    vCriteriaModel!.fromDate = null;
    vCriteriaModel!.toDate = null;
    isLoading = true;
    update();
    getVoucherList();
  }

  voucherFilterApply(criteriaData) {
    vCriteriaModel = criteriaData;
    isLoading = true;
    update();
    getVoucherList();
    update();
  }

  Future<void> getVoucherList() async {
    try {
      voucherList = await GeneralLedgerService.getVoucherList(vCriteriaModel!);
      isLoading = false;
      update();
    } catch (e) {
      isLoading = false;
      voucherList = [];
      update();
    }
  }

  void onLoading() async {
    vCriteriaModel!.pageNumber++;
    var data = await GeneralLedgerService.getVoucherList(vCriteriaModel!);
    voucherList.addAll(data);
    update();
    refreshController.loadComplete();
  }

  void onRefresh() async {
    vCriteriaModel!.pageNumber = 1;
    await getVoucherList();
    refreshController.refreshCompleted();
  }
}
