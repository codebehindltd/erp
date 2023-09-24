import 'dart:convert';
import 'package:leading_edge/app/data/models/req/members/member_registration_mode.dart';
import '../../core/environment/environment.dart';
import '../../core/utils/api_end_point.dart';
import '../models/req/members/member_payment_save_model.dart';
import '../models/res/members/membership_type.dart';
import 'package:collection/collection.dart';
import 'package:http/http.dart' as http;

class MemberService {
  static Future<String?> memberRegistration(
      MemberRegistrationModel registration) async {
    var client = http.Client();
    try {
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.memberRegistration);
      final response = await client.post(url,
          body: jsonEncode(registration.toJson()),
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          });
      if (response.statusCode == 200) {
        return response.body;
      } else {
        var body = jsonDecode(response.body);
        String errorMsg = body['Message'];
        throw errorMsg;
      }
    } finally {
      client.close();
    }
  }

  static List<MemberShipType> prepareMemberTypeData(
      List<MemberShipType> model) { 
    List<MemberShipType> members = [];
    var types = groupBy(model, (MemberShipType model) => model.typeId);
    types.forEach((key, value) {
      MemberShipType member;
      member = value.first;
      // var a = groupBy(value, (MemberShipType model) => model.costCenterId);
      // a.forEach((key, e) => member.benefits!.add(e.first));
      Set<int> seenB = <int>{};
      member.benefits =
          value.where((element) => seenB.add(element.costCenterId!)).toList();

      var terms = groupBy(value, (MemberShipType model) => model.tncType);
      List<MemberShipType> termTitles = [];
      terms.forEach((k, term) {
        MemberShipType termT = term.first;
        
        Set<String> seenT = <String>{};
        termT.termSubTitiles = term
            .where((element) => seenT.add(element.tncDetails ?? ""))
            .toList();
        member.termTitles!.add(termT);
        //termTitles.add(termT);
      });
      //member.termTitles=termTitles;
      // var seenT = Set<String>();
      // member.terms = value
      //     .where((element) => seenT.add(element.tncDetails ?? ""))
      //     .toList();

      members.add(member);
    });
    return members;
  }

  static Future<List<MemberShipType>> getMembershipSetupData() async {
    var client = http.Client();

    try {
      Uri url =
          Uri.parse(Env.rootApiUrl + ApiEntPoint.membershipSetupData);
      final response = await client.get(url);
      if (response.statusCode == 200) {
        var model = memberShipTypeFromJson(response.body);
        return prepareMemberTypeData(model);
      } else {
        return [];
      }
    } finally {
      client.close();
    }
  }

  static Future<bool> memberPayment(MemberPaymentSaveModel model) async {
    var client = http.Client();
    try {
      Uri url = Uri.parse(Env.rootApiUrl + ApiEntPoint.memberPaymentSave);
      final response = await client.post(url,
          body: jsonEncode(model.toJson()),
          headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
          });
      if (response.statusCode == 200) {
        return true;
      } else {
        var body = jsonDecode(response.body);
        String errorMsg = body['Message'];
        throw errorMsg;
      }
    } finally {
      client.close();
    }
  }
}
