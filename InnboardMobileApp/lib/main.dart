import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:get/get.dart';
import 'app/core/utils/utils_function.dart';
import 'app/core/values/colors.dart';
import 'app/core/values/strings.dart';
import 'app/modules/common/bindings/common_binding.dart';
import 'app/routes/app_pages.dart';

Future<void> main() async {
  //rotate off
  WidgetsFlutterBinding.ensureInitialized();
  SystemChrome.setPreferredOrientations(
      [DeviceOrientation.portraitUp, DeviceOrientation.portraitDown]);
  //rotate off end

  SystemChrome.setSystemUIOverlayStyle(SystemUiOverlayStyle(
      statusBarColor: statusBarcolor,
      statusBarIconBrightness: Brightness.light));
  //await initializeService();
  Get.put<CommonBinding>(CommonBinding());
  runApp(GetMaterialApp(
    title: appName,
    debugShowCheckedModeBanner: false,
    builder: EasyLoading.init(),
    theme: ThemeData(
      primarySwatch: generateMaterialColorFromColor(themeColor),
      fontFamily: balooDa2,
      appBarTheme: const AppBarTheme(elevation: .5),
      scaffoldBackgroundColor: bodyColor,
      textTheme: TextTheme(
        titleLarge: const TextStyle(
            color: Colors.white, fontWeight: FontWeight.w500, fontSize: 22),
        labelLarge: TextStyle(
            color: themeColor, fontWeight: FontWeight.w400, fontSize: 20),
        labelMedium: TextStyle(
            color: themeColor, fontWeight: FontWeight.w500, fontSize: 20),
        labelSmall: TextStyle(
            color: themeColor, fontWeight: FontWeight.w400, fontSize: 18,),
        headlineLarge: TextStyle(
            color: themeColor, fontWeight: FontWeight.w500, fontSize: 18),
        headlineMedium: TextStyle(
            color: themeColor, fontWeight: FontWeight.w400, fontSize: 16),
        headlineSmall: TextStyle(
            color: themeColor, fontWeight: FontWeight.w400, fontSize: 16),
      ),
    ),

    // home: const SplashScreen(),
    initialBinding: CommonBinding(),
    initialRoute: AppPages.initial,
    getPages: AppPages.routes,
  ));
  configLoading();
}

// class MyApp extends StatelessWidget {
//   const MyApp({Key? key}) : super(key: key);

//   // This widget is the root of your application.
//   @override
//   Widget build(BuildContext context) {
//     return GetMaterialApp(
//       title: appName,
//       debugShowCheckedModeBanner: false,
//       builder: EasyLoading.init(),
//       theme: ThemeData(primarySwatch: themeColor, fontFamily: balooDa2),
//       home: const SplashScreen(),
//     );
//     // return MultiProvider(
//     //   providers: [
//     //     ChangeNotifierProvider<AttendanceSubmitViewModel>.value(
//     //       value: AttendanceSubmitViewModel(),
//     //     ),
//     //     ChangeNotifierProvider<ConnectivityViewModel>.value(
//     //       value: ConnectivityViewModel(),
//     //     ),

//     //     // ChangeNotifierProvider(create: (_) => AttendanceSubmitViewModel())
//     //   ],
//     //   child: MaterialApp(
//     //     title: appName,
//     //     debugShowCheckedModeBanner: false,
//     //     builder: EasyLoading.init(),
//     //     theme: ThemeData(
//     //         primarySwatch: themeColor,
//     //         fontFamily: balooDa2),
//     //     home: const SplashScreen(),
//     //   ),
//     // );
//   }
// }

void configLoading() {
  EasyLoading.instance
    ..displayDuration = const Duration(milliseconds: 2000)
    ..indicatorType = EasyLoadingIndicatorType.fadingCircle
    ..loadingStyle = EasyLoadingStyle.light
    ..indicatorSize = 45.0
    ..radius = 10.0
    ..progressColor = Colors.yellow
    ..backgroundColor = Colors.green
    ..indicatorColor = Colors.yellow
    ..textColor = Colors.yellow
    ..maskColor = Colors.blue.withOpacity(0.5)
    ..maskType = EasyLoadingMaskType.black
    ..userInteractions = false
    ..dismissOnTap = false;
  // ..customAnimation = CustomAnimation();
}
