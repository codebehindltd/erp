
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:webview_flutter/webview_flutter.dart';
import '../../../../data/localDB/sharedPfnDBHelper.dart';

class PropertyDeatailsController extends GetxController {
  final WebViewController webViewController = WebViewController();
  var isLoading = true.obs;
  @override
  void onInit() {
    webView();
    super.onInit();
  }

  webView() async {
    var property = await SharedPfnDBHelper.getProperty();
    isLoading.value = true;
    webViewController
      ..setJavaScriptMode(JavaScriptMode.unrestricted)
      ..setBackgroundColor(bodyColor)
      ..setNavigationDelegate(
        NavigationDelegate(
          onProgress: (int progress) {
            if (progress == 100) {
              isLoading.value = false;
            }
          },
          onPageStarted: (String url) {
            // print("start: $url");
          },
          onPageFinished: (String url) {
            // print("end: $url");
          },
          onWebResourceError: (WebResourceError error) {},
          onNavigationRequest: (NavigationRequest request) {
            // if (request.url
            //     .startsWith('https://www.youtube.com/@vitalitycroatia9226')) {
            //   return NavigationDecision.prevent;
            // }
            return NavigationDecision.navigate;
          },
        ),
      )
      ..loadRequest(Uri.parse(
          //'http://www.hoteldmore.com/'
          "http://${property.webAddress}"));
  }
}
