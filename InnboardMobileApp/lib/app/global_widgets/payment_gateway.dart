import 'package:flutter_sslcommerz/model/SSLCSdkType.dart';
import 'package:flutter_sslcommerz/model/SSLCTransactionInfoModel.dart';
import 'package:flutter_sslcommerz/model/SSLCommerzInitialization.dart';
import 'package:flutter_sslcommerz/model/SSLCurrencyType.dart';
import 'package:flutter_sslcommerz/sslcommerz.dart';
import 'package:get/get.dart';
import '../core/values/colors.dart';
import '../data/localDB/sharedPfnDBHelper.dart';
import '../data/models/res/property_model.dart';

class PaymentGateway {
  static Future<SSLCTransactionInfoModel?> sslCommerzGeneralCall(
      double amount, {PropertyModel? propertyData}) async {
        PropertyModel property;
        if(propertyData==null){
          property = await SharedPfnDBHelper.getProperty();
        }else{
          property=propertyData;
        }
    
    SSLCTransactionInfoModel? sslPaymentResult;
    Sslcommerz sslcommerz = Sslcommerz(
      initializer: SSLCommerzInitialization(
        currency: SSLCurrencyType.BDT,
        product_category: "product",
        // store_id: Env.sslcStoreId,
        // store_passwd: Env.sslcStorePasswd,
        // sdkType: 1 == 2 ? SSLCSdkType.LIVE : SSLCSdkType.TESTBOX,
        store_id: property.paymentGateWayStoreId!,
        store_passwd: property.paymentGateWayStorePassword!,
        sdkType: property.paymentGateWayStoreType == "live"
            ? SSLCSdkType.LIVE
            : SSLCSdkType.TESTBOX,
        total_amount: amount,
        tran_id: DateTime.now().millisecondsSinceEpoch.toRadixString(16),
      ),
    );
    try {
      final sslPaymentResult = await sslcommerz.payNow();
      if (sslPaymentResult.status!.toLowerCase() == "failed") {
        Get.rawSnackbar(
          message: "Transaction is Failed.",
          backgroundColor: errorColor,
        );
        return sslPaymentResult;
      } else if (sslPaymentResult.status!.toLowerCase() == "closed") {
        Get.rawSnackbar(
          message: "Transaction closed by user.",
          backgroundColor: errorColor,
        );
        return sslPaymentResult;
      } else if (sslPaymentResult.status!.toLowerCase() == "valid") {
        // EasyLoading.showSuccess("Transaction successfully done.");
        Get.rawSnackbar(
          message: "Transaction successfully done.",
          backgroundColor: themeColor,
        );
        return sslPaymentResult;
      } else {
        Get.rawSnackbar(
          message: "Transaction is Failed.",
          backgroundColor: errorColor,
        );
        return sslPaymentResult;
      }
    } catch (e) {
      Get.rawSnackbar(
        message: e.toString(),
        backgroundColor: errorColor,
      );
      return sslPaymentResult;
    }
  }
}
