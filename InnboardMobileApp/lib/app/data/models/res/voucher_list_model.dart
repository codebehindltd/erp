// To parse this JSON data, do
//
//     final voucherListModel = voucherListModelFromJson(jsonString);

import 'dart:convert';

List<VoucherResModel> voucherResModelFromJson(String str) =>
    List<VoucherResModel>.from(
        json.decode(str).map((x) => VoucherResModel.fromJson(x)));

String voucherResModelToJson(List<VoucherResModel> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

class VoucherResModel {
  VoucherResModel({
    this.ledgerMasterId,
    this.companyId,
    this.projectId,
    this.donorId,
    this.voucherType,
    this.isBankExist,
    this.voucherNo,
    this.voucherDate,
    this.currencyId,
    this.convertionRate,
    this.narration,
    this.payerOrPayee,
    this.referenceNumber,
    this.glStatus,
    this.checkedBy,
    this.approvedBy,
    this.createdBy,
    this.createdDate,
    this.lastModifiedBy,
    this.lastModifiedDate,
    this.referenceVoucherNumber,
    this.isSynced,
    this.chequeNumber,
    this.chequeDate,
    this.nodeHead,
    this.nodeMode,
    this.nodeNarration,
    this.amount,
    this.debitAmount,
    this.creditAmount,
    this.ledgerMode,
    this.checkedByName,
    this.approvedByName,
    this.voucherDateDisplay,
    this.createdDateDisplay,
    this.createdByName,
    this.companyName,
    this.projectName,
    this.voucherTypeName,
    this.donorName,
    this.currencyName,
    this.canEditDeleteAfterApproved,
    this.voucherDateString,
    this.isCanEdit,
    this.isCanDelete,
    this.isCanCheck,
    this.isCanApprove,
    this.voucherTotalAmount,
    this.isModulesTransaction,
  });

  int? ledgerMasterId;
  int? companyId;
  int? projectId;
  dynamic donorId;
  dynamic voucherType;
  dynamic isBankExist;
  String? voucherNo;
  dynamic voucherDate;
  int? currencyId;
  dynamic convertionRate;
  String? narration;
  dynamic payerOrPayee;
  dynamic referenceNumber;
  dynamic glStatus;
  dynamic checkedBy;
  dynamic approvedBy;
  dynamic createdBy;
  dynamic createdDate;
  dynamic lastModifiedBy;
  dynamic lastModifiedDate;
  dynamic referenceVoucherNumber;
  bool? isSynced;
  dynamic chequeNumber;
  dynamic chequeDate;
  String? nodeHead;
  bool? nodeMode;
  String? nodeNarration;
  double? amount;
  double? debitAmount;
  double? creditAmount;
  int? ledgerMode;
  String? checkedByName;
  String? approvedByName;
  String? voucherDateDisplay;
  String? createdDateDisplay;
  String? createdByName;
  String? companyName;
  String? projectName;
  dynamic voucherTypeName;
  dynamic donorName;
  dynamic currencyName;
  int? canEditDeleteAfterApproved;
  dynamic voucherDateString;
  bool? isCanEdit;
  bool? isCanDelete;
  bool? isCanCheck;
  bool? isCanApprove;
  double? voucherTotalAmount;
  bool? isModulesTransaction;

  factory VoucherResModel.fromJson(Map<String, dynamic> json) =>
      VoucherResModel(
        ledgerMasterId: json["LedgerMasterId"],
        companyId: json["CompanyId"],
        projectId: json["ProjectId"],
        donorId: json["DonorId"],
        voucherType: json["VoucherType"],
        isBankExist: json["IsBankExist"],
        voucherNo: json["VoucherNo"],
        voucherDate: json["VoucherDate"],
        currencyId: json["CurrencyId"],
        convertionRate: json["ConvertionRate"],
        narration: json["Narration"],
        payerOrPayee: json["PayerOrPayee"],
        referenceNumber: json["ReferenceNumber"],
        glStatus: json["GLStatus"],
        checkedBy: json["CheckedBy"],
        approvedBy: json["ApprovedBy"],
        createdBy: json["CreatedBy"],
        createdDate: json["CreatedDate"],
        lastModifiedBy: json["LastModifiedBy"],
        lastModifiedDate: json["LastModifiedDate"],
        referenceVoucherNumber: json["ReferenceVoucherNumber"],
        isSynced: json["IsSynced"],
        chequeNumber: json["ChequeNumber"],
        chequeDate: json["ChequeDate"],
        nodeHead: json["NodeHead"],
        nodeMode: json["NodeMode"],
        nodeNarration: json["NodeNarration"],
        amount: json["Amount"],
        debitAmount: json["DebitAmount"],
        creditAmount: json["CreditAmount"],
        ledgerMode: json["LedgerMode"],
        checkedByName: json["CheckedByName"],
        approvedByName: json["ApprovedByName"],
        voucherDateDisplay: json["VoucherDateDisplay"],
        createdDateDisplay: json["CreatedDateDisplay"],
        createdByName: json["CreatedByName"],
        companyName: json["CompanyName"],
        projectName: json["ProjectName"],
        voucherTypeName: json["VoucherTypeName"],
        donorName: json["DonorName"],
        currencyName: json["CurrencyName"],
        canEditDeleteAfterApproved: json["CanEditDeleteAfterApproved"],
        voucherDateString: json["VoucherDateString"],
        isCanEdit: json["IsCanEdit"],
        isCanDelete: json["IsCanDelete"],
        isCanCheck: json["IsCanCheck"],
        isCanApprove: json["IsCanApprove"],
        voucherTotalAmount: json["VoucherTotalAmount"],
        isModulesTransaction: json["IsModulesTransaction"],
      );

  Map<String, dynamic> toJson() => {
        "LedgerMasterId": ledgerMasterId,
        "CompanyId": companyId,
        "ProjectId": projectId,
        "DonorId": donorId,
        "VoucherType": voucherType,
        "IsBankExist": isBankExist,
        "VoucherNo": voucherNo,
        "VoucherDate": voucherDate,
        "CurrencyId": currencyId,
        "ConvertionRate": convertionRate,
        "Narration": narration,
        "PayerOrPayee": payerOrPayee,
        "ReferenceNumber": referenceNumber,
        "GLStatus": glStatus,
        "CheckedBy": checkedBy,
        "ApprovedBy": approvedBy,
        "CreatedBy": createdBy,
        "CreatedDate": createdDate,
        "LastModifiedBy": lastModifiedBy,
        "LastModifiedDate": lastModifiedDate,
        "ReferenceVoucherNumber": referenceVoucherNumber,
        "IsSynced": isSynced,
        "ChequeNumber": chequeNumber,
        "ChequeDate": chequeDate,
        "NodeHead": nodeHead,
        "NodeMode": nodeMode,
        "NodeNarration": nodeNarration,
        "Amount": amount,
        "DebitAmount": debitAmount,
        "CreditAmount": creditAmount,
        "LedgerMode": ledgerMode,
        "CheckedByName": checkedByName,
        "ApprovedByName": approvedByName,
        "VoucherDateDisplay": voucherDateDisplay,
        "CreatedDateDisplay": createdDateDisplay,
        "CreatedByName": createdByName, 
        "CompanyName": companyName,
        "ProjectName": projectName,
        "VoucherTypeName": voucherTypeName,
        "DonorName": donorName,
        "CurrencyName": currencyName,
        "CanEditDeleteAfterApproved": canEditDeleteAfterApproved,
        "VoucherDateString": voucherDateString,
        "IsCanEdit": isCanEdit,
        "IsCanDelete": isCanDelete,
        "IsCanCheck": isCanCheck,
        "IsCanApprove": isCanApprove,
        "VoucherTotalAmount": voucherTotalAmount,
        "IsModulesTransaction": isModulesTransaction,
      };
}
