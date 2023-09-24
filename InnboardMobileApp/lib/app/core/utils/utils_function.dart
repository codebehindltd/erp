// import 'package:connectivity_plus/connectivity_plus.dart';

// Future<bool> checkNetConnectivity() async {
//     var result = await (Connectivity().checkConnectivity());

//     if (result == ConnectivityResult.mobile) {
//       return true;
//     } else if (result == ConnectivityResult.wifi) {
//       return true;
//     } else if (result == ConnectivityResult.none) {
//       return false;
//     }else{
//       return false;
//     }
//   }
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

DateFormat dateTimeFormatCustom = DateFormat("dd MMM yyyy - hh:mm aaa");
final DateFormat dateFormat = DateFormat("dd MMM yyyy");

DateFormat timeFormatCustom = DateFormat("hh:mm aaa");

Color hexToColor(String code) {
  return Color(int.parse(code.substring(1, 7), radix: 16) + 0xFF000000);
}

MaterialColor generateMaterialColorFromColor(Color color) {
  return MaterialColor(color.value, {
    50: Color.fromRGBO(color.red, color.green, color.blue, 0.1),
    100: Color.fromRGBO(color.red, color.green, color.blue, 0.2),
    200: Color.fromRGBO(color.red, color.green, color.blue, 0.3),
    300: Color.fromRGBO(color.red, color.green, color.blue, 0.4),
    400: Color.fromRGBO(color.red, color.green, color.blue, 0.5),
    500: Color.fromRGBO(color.red, color.green, color.blue, 0.6),
    600: Color.fromRGBO(color.red, color.green, color.blue, 0.7),
    700: Color.fromRGBO(color.red, color.green, color.blue, 0.8),
    800: Color.fromRGBO(color.red, color.green, color.blue, 0.9),
    900: Color.fromRGBO(color.red, color.green, color.blue, 1.0),
  });
}