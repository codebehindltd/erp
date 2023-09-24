import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/localDB/sharedPfnDBHelper.dart';
import 'package:leading_edge/app/data/models/res/emp_model.dart';
import 'package:leading_edge/app/data/services/emp_service.dart';
import 'package:pull_to_refresh_flutter3/pull_to_refresh_flutter3.dart';

import '../../../../data/models/res/user_model.dart';

class EmployeeListController extends GetxController {
  bool isLoading = true;
  List<EmpModel> empList = [];
  int pageNumber = 1;
  int pageSize = 10;
  final RefreshController refreshController =
      RefreshController(initialRefresh: false);
  final TextEditingController searchKeyController = TextEditingController();
  UserModel? userInformation;

  @override
  void onReady() async {
    userInformation = await SharedPfnDBHelper.getPropertyUserData();
    getEmpList();
    super.onReady();
  }

  Future<void> getEmpList() async {
    try {
      empList = await EmpService.getEmpList(
          userInfoId: userInformation!.userInfoId!,
          pageNumber: pageNumber,
          pageSize: pageSize,
          searchKey: searchKeyController.text.trim());
      isLoading = false;
      update();
    } catch (e) {
      isLoading = false;
      empList = [];
      update();
    }
  }

  void onLoading() async {
    pageNumber++;
    var data = await EmpService.getEmpList(
        userInfoId: userInformation!.userInfoId!,
        pageNumber: pageNumber,
        pageSize: pageSize,
        searchKey: searchKeyController.text.trim());
    empList.addAll(data);
    update();
    refreshController.loadComplete();
  }

  void onRefresh() async {
    pageNumber = 1;
    await getEmpList();
    refreshController.refreshCompleted();
  }

  searchEmp(value) async {
    pageNumber = 1;
    isLoading = true;
    update();
    await getEmpList();
  }
}
