import '../../data/localDB/sharedPfnDBHelper.dart';

class Env {
// // local
// static const rootUrl = "http://192.168.5.230:1000/";
// static const rootApiUrl = "http://192.168.5.230:1000/api/";

// server 7869 
  static const rootUrl = "http://182.163.101.210:7869/";
  static const rootApiUrl = "http://182.163.101.210:7869/api/";

  // static const rootUrl = "http://103.134.89.202:786/";
  // static const rootApiUrl = "http://103.134.89.202:786/api/";

  // static const sslcStoreId = "rijvi6370979988314";
  // static const sslcStorePasswd = "rijvi6370979988314@ssl";
//6629
  static propertryUrl() async {
    return await SharedPfnDBHelper.getPropertyUrl();
  }

  static propertryApiUrl() async {
    var url = await SharedPfnDBHelper.getPropertyUrl();
    return "${url!}api/";
  }
}
