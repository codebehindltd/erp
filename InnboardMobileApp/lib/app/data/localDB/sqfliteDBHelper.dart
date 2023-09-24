import 'package:leading_edge/app/data/models/req/attendance_model.dart';
import 'package:sqflite/sqflite.dart';
import 'package:path/path.dart';

class SqfliteDBHelper {
  static Database? _db;
  static const String attendanceTable = 'attendance';
  static const String dbName = 'attendance1.db';

  Future<Database> get db async {
    if (_db != null) {
      return _db!;
    }
    _db = await initDb();
    return _db!;
  }

  initDb() async {
    String path = join(await getDatabasesPath(), dbName);
    var db = await openDatabase(path, version: 1, onCreate: _onCreateTable);
    return db;
  }

  _onCreateTable(Database db, int version) async {
    await db.execute(
        "CREATE TABLE $attendanceTable (Id INTEGER PRIMARY KEY, EmpId INTEGER, ImageByte TEXT, ImageName TEXT, Latitude REAL, Longitude REAL, DateTime TEXT, Address TEXT)");
  }

  Future<AttendanceModel> saveAttendanceData(AttendanceModel model) async {
    var dbClient = await db;
    model.id = await dbClient.insert(attendanceTable, model.toJson());
    return model;
  }

  Future<List<AttendanceModel>> getAttendancesData() async {
    var dbClient = await db;
    // List<Map> maps = await dbClient
    //     .query(attendanceTable, columns: [ID, USERID, IMAGE]);
    List<Map> maps = await dbClient.rawQuery("SELECT * FROM $attendanceTable");
    List<AttendanceModel> data = [];
    if (maps.isNotEmpty) {
      for (int i = 0; i < maps.length; i++) {
        data.add(AttendanceModel.fromJson(Map.from(maps[i])));
      }
    }
    return data;
  }

  Future<int> deleteAttedanceById(int? id) async {
    var dbClient = await db;
    if (id != null) {
      return await dbClient
          .delete(attendanceTable, where: 'Id = ?', whereArgs: [id]);
    } else {
      return await dbClient.delete(attendanceTable);
    }
  }
}
