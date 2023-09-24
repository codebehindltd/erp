import 'dart:async';

import 'package:connectivity_plus/connectivity_plus.dart';
import 'package:get/get.dart';

import '../../../data/localDB/sharedPfnDBHelper.dart';

class CommonController extends GetxController {
  final Connectivity _connectivity = Connectivity();
  bool isConnection = false;
  late double latitude;
  late double longitude;
  bool isLogin = false;
  int? masterUserInfoId;
  int? propertyUserInfoId;
  double statusBarHeight= 0 ;
  var keyboardHeight=0.0.obs;
  String? guestOrMemberId;

  @override
  void onInit() {
    checkIsLogin();
    super.onInit();
  }

  subscripCheckConnection() {
    _connectivity.onConnectivityChanged.listen(_updateConnectionStatus);
  }

  Future<void> checkIsLogin() async {
    var data = await SharedPfnDBHelper.getLoginUserData();
    var prUser = await SharedPfnDBHelper.getPropertyUserData();
    // userId=1;
    isLogin = data != null ? true : false;
    if (data != null) {
      masterUserInfoId = data.userInfoId;
    }
    if (prUser != null) {
      propertyUserInfoId = prUser.userInfoId;
    }
        
  }

  Future<void> checkMemberOrGuestLogin() async{
    guestOrMemberId = await SharedPfnDBHelper.getGuestOrMemberId();
  }

  Future<void> _updateConnectionStatus(ConnectivityResult result) async {
    if (result == ConnectivityResult.mobile) {
      isConnection = true;
    } else if (result == ConnectivityResult.wifi) {
      isConnection = true;
    } else if (result == ConnectivityResult.none) {
      isConnection = false;
    } else {
      isConnection = false;
    }
    print(isConnection);
    // notifyListeners();
  }

  Future<void> checkNetConnectivity() async {
    var result = await (Connectivity().checkConnectivity());

    if (result == ConnectivityResult.mobile) {
      isConnection = true;
    } else if (result == ConnectivityResult.wifi) {
      isConnection = true;
    } else if (result == ConnectivityResult.none) {
      isConnection = false;
    } else {
      isConnection = false;
    }
    print(isConnection);
    // notifyListeners();
  }
}
