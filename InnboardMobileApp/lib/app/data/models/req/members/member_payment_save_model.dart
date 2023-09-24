import 'dart:convert';

MemberPaymentSaveModel memberPaymentSaveModelFromJson(String str) => MemberPaymentSaveModel.fromJson(json.decode(str));

String memberPaymentSaveModelToJson(MemberPaymentSaveModel data) => json.encode(data.toJson());

class MemberPaymentSaveModel {
    int memberId;
    String transactionType;
    String transactionId;
    double securityDeposit;
    String transactionDetails;
    int createdBy;

    MemberPaymentSaveModel({
        required this.memberId,
        required this.transactionType,
        required this.transactionId,
        required this.securityDeposit,
        required this.transactionDetails,
        required this.createdBy,
    });

    factory MemberPaymentSaveModel.fromJson(Map<String, dynamic> json) => MemberPaymentSaveModel(
        memberId: json["MemberId"],
        transactionType: json["TransactionType"],
        transactionId: json["TransactionId"],
        securityDeposit: json["SecurityDeposit"],
        transactionDetails: json["TransactionDetails"],
        createdBy: json["CreatedBy"],
    );

    Map<String, dynamic> toJson() => {
        "MemberId": memberId,
        "TransactionType": transactionType,
        "TransactionId": transactionId,
        "SecurityDeposit": securityDeposit,
        "TransactionDetails": transactionDetails,
        "CreatedBy": createdBy,
    };
}
