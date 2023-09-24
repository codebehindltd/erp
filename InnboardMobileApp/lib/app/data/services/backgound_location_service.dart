// backgourd servcie start
import 'dart:async';
import 'dart:io';
import 'dart:ui';
import 'package:flutter/material.dart';
import 'package:flutter_background_service/flutter_background_service.dart';
import 'package:geolocator/geolocator.dart';
import 'package:flutter_background_service_android/flutter_background_service_android.dart';
import 'package:leading_edge/app/core/values/static_value.dart';
import 'package:leading_edge/app/core/values/strings.dart';
import 'package:leading_edge/app/data/localDB/sharedPfnDBHelper.dart';
import 'package:leading_edge/app/data/models/req/emp_location_tr_save_model.dart';
import 'package:leading_edge/app/data/services/emp_service.dart';
import 'package:leading_edge/app/data/services/location_service.dart';
import 'package:device_info_plus/device_info_plus.dart';

Position? sposition;
bool isFirst = true;
int? empId;
String? device;

Future<void> initializeService() async {
  empId = null;
  isFirst = true;
  final service = FlutterBackgroundService();
  var isRunning = await service.isRunning();
  if (isRunning) {
    service.invoke("stopService");
  }
  Future.delayed(const Duration(seconds: 2), () async {
    await service.configure(
      androidConfiguration: AndroidConfiguration(
        // this will be executed when app is in foreground or background in separated isolate
        onStart: onStart,
        initialNotificationTitle: appName,
        initialNotificationContent: "Welcome to $appName",
        // auto start service
        autoStartOnBoot: true,
        autoStart: true,
        isForegroundMode: true,
      ),
      iosConfiguration: IosConfiguration(
        // auto start service
        autoStart: true,

        // this will be executed when app is in foreground in separated isolate
        onForeground: onStart,

        // you have to enable background fetch capability on xcode project
        onBackground: onIosBackground,
      ),
    );
    await service.startService();
  });
}

bool onIosBackground(ServiceInstance service) {
  WidgetsFlutterBinding.ensureInitialized();
  print('FLUTTER BACKGROUND FETCH');

  return true;
}

@pragma('vm:entry-point')
void onStart(ServiceInstance service) async {
  // Only available for flutter 3.0.0 and later
  DartPluginRegistrant.ensureInitialized();

  if (service is AndroidServiceInstance) {
    service.on('start_service').listen((event) {
      service.setAsForegroundService();
    });

    service.on('setAsForeground').listen((event) {
      service.setAsForegroundService();
    });

    service.on('setAsBackground').listen((event) {
      service.setAsBackgroundService();
      // service.
    });
  }

  service.on('stopService').listen((event) {
    service.stopSelf();
  });

  LocationService.locationSetting();
  // var position = await determinePosition();
  // print(position);

  // StreamSubscription positionStream = getPositionStream(service);

  // positionStream.asFuture().;

  // bring to foreground
  if (service is AndroidServiceInstance) {
    // notification
    service.setForegroundNotificationInfo(
      title: appName,
      content: "Welcome to $appName",
    );

    // Geolocator.getServiceStatusStream().listen((ServiceStatus status) {
    //   // print(status);
    // });
  }
  // device info

  final deviceInfo = DeviceInfoPlugin();

  if (Platform.isAndroid) {
    final androidInfo = await deviceInfo.androidInfo;
    device = androidInfo.model;
  }

  if (Platform.isIOS) {
    final iosInfo = await deviceInfo.iosInfo;
    device = iosInfo.model;
  }
  // end device info
  Timer.periodic(const Duration(seconds: StaticValue.bgServiceCkInSecond),
      (timer) async {
    try {
      bool locationEnable = await Geolocator.isLocationServiceEnabled();
      if (locationEnable) {
        getPosition(service);
      } else {
        isFirst = true;
      }
    } catch (e) {
      isFirst = true;
    }
  });
}

getPosition(ServiceInstance service) async {
  if (empId == null) {
    var data = await SharedPfnDBHelper.getPropertyUserData();
    empId = data?.empId;
  }

  // print(empId);
  if (empId == null) {
    return;
  }
  Geolocator.getCurrentPosition(
          timeLimit: const Duration(seconds: StaticValue.bgServiceCkInSecond))
      .then((Position? position) {
    // print("location: ${jsonEncode(position)}");
    double distanceInMeters = sposition != null
        ? Geolocator.distanceBetween(sposition!.latitude, sposition!.longitude,
            position!.latitude, position.longitude)
        : 0;
    // print("g distance: $distanceInMeters");

    if (service is AndroidServiceInstance &&
        (isFirst || distanceInMeters > StaticValue.distanceInMeters)) {
      if (isFirst) {
        isFirst = false;
      }
      sposition = position;
      // here api call
      var model = EmpLocationTrSaveModel(
          empId: empId,
          latitude: position!.latitude,
          longitude: position.longitude,
          attDateTime: DateTime.now(),
          deviceInfo: device);
      EmpService.saveEmpLocationTracking(model).then((value) {
        print(value.body);
      });
    }
  }).timeout(const Duration(seconds: StaticValue.bgServiceCkInSecond));
}
  // backgourd servcie end
  