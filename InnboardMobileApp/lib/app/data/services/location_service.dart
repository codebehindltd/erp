import 'dart:async';
import 'package:flutter/foundation.dart';
import 'package:geocoding/geocoding.dart' hide Location;
import 'package:geolocator/geolocator.dart';
import 'package:leading_edge/app/core/values/static_value.dart';

class LocationService {
  static Future<String> getAddressFromLatLong(Position position) async {
    try {
      List<Placemark> placemarks = await placemarkFromCoordinates(
          position.latitude, position.longitude,
          localeIdentifier: 'en');
      Placemark place = placemarks[0];
      String address =
          '${place.street == "" ? "" : "${place.street},"} ${place.subLocality == "" ? "" : "${place.subLocality},"} ${place.locality} ${place.postalCode}.';

      return address;
    } catch (e) {
      return "";
    }
  }

  static Future<Position> determineCurrentPosition(
      LocationAccuracy locationAccuracy) async {
    LocationPermission? permission = await Geolocator.checkPermission();
    if (permission == LocationPermission.denied) {
      permission = await Geolocator.requestPermission();
      if (permission == LocationPermission.denied) {
        return Future.error('Location permissions are denied');
      }
    }

    if (permission == LocationPermission.deniedForever) {
      // Permissions are denied forever, handle appropriately.
      return Future.error(
          'Location permissions are permanently denied, we cannot request permissions.');
    }
    // final prefs = await SharedPreferences.getInstance();
// test
    //locationPackage.Location location = locationPackage.Location();
// var _locationData = await location.getLocation();
// print("object");
// print(_locationData);
// print("object");
//     Geolocator.getPositionStream().listen((event) {
//       print("event");
//       print(event);
//     });
// test end
    // When we reach here, permissions are granted and we can
    // continue accessing the position of the device.
    // location.enableBackgroundMode(enable: true);

// Save an integer value to 'counter' key.

    // location.onLocationChanged
    //     .listen((locationPackage.LocationData currentLocation) async {
    //   // Use current location
    //   final int? counter = prefs.getInt('counter');
    //   if (counter == null) {
    //     await prefs.setInt('counter', 1);
    //   } else {
    //     await prefs.setInt('counter', counter + 1);
    //   }
    //   print("current location $counter");
    //   print(currentLocation);
    // });
    // end location package
    getPositionStream();
    return await Geolocator.getCurrentPosition(
        // forceAndroidLocationManager: true,
        desiredAccuracy: locationAccuracy);
  }

  static getPositionStream() {
    locationSetting();
    Geolocator.getPositionStream(locationSettings: locationSettings)
        .listen((Position? position) {});
  }

  static late LocationSettings locationSettings;
  static locationSetting() {
    if (defaultTargetPlatform == TargetPlatform.android) {
      locationSettings = AndroidSettings(
        accuracy: LocationAccuracy.best,
        distanceFilter: StaticValue.distanceInMeters,
        // forceLocationManager: true,
        intervalDuration:
            const Duration(seconds: StaticValue.bgServiceCkInSecond),
        //(Optional) Set foreground notification config to keep the app alive
        //when going to the background
        // foregroundNotificationConfig: const ForegroundNotificationConfig(
        //     notificationTitle: appName,
        //     notificationText: "Welcome to $appName",
        //     enableWakeLock: true,
        //     enableWifiLock: true)
      );
    } else if (defaultTargetPlatform == TargetPlatform.iOS ||
        defaultTargetPlatform == TargetPlatform.macOS) {
      locationSettings = AppleSettings(
        accuracy: LocationAccuracy.high,
        activityType: ActivityType.fitness,
        distanceFilter: StaticValue.distanceInMeters,
        pauseLocationUpdatesAutomatically: true,
        // Only set to true if our app will be started up in the background.
        showBackgroundLocationIndicator: false,
      );
    } else {
      locationSettings = const LocationSettings(
        accuracy: LocationAccuracy.high,
        distanceFilter: StaticValue.bgServiceCkInSecond,
      );
    }
  }
}
