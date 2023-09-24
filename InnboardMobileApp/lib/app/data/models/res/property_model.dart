import 'dart:convert';

List<PropertyModel> propertyModelFromJson(String str) =>
    List<PropertyModel>.from(
        json.decode(str).map((x) => PropertyModel.fromJson(x)));

String propertyModelToJson(List<PropertyModel> data) =>
    json.encode(List<dynamic>.from(data.map((x) => x.toJson())));

String singlePropertyModelToJson(PropertyModel data) =>
    json.encode(data.toJson());

class PropertyModel {
  int id;
  String? propertyName;
  String? propertyAddress;
  String? webAddress;
  String? emailAddress;
  String? contactPerson;
  String? contactNumber;
  String? endPointIp;
  String? logoUrl;
  String? coverPictureURL;
  String? rankNumber;
  String? lineOne;
  String? lineTwo;
  String? lineThree;
  String? lineFour;
  String? lineFive;
  String? paymentGateWayStoreType;
  String? paymentGateWayStoreId;
  String? paymentGateWayStorePassword;
  int? createdBy;
  String? createdDate;
  int? lastModifiedBy;
  String? lastModifiedDate;

  PropertyModel({
    required this.id,
    required this.propertyName,
    required this.propertyAddress,
    this.webAddress,
    this.emailAddress,
    this.contactPerson,
    this.contactNumber,
    required this.endPointIp,
    this.logoUrl,
    this.coverPictureURL,
    this.rankNumber,
    this.lineOne,
    this.lineTwo,
    this.lineThree,
    this.lineFour,
    this.lineFive,
    this.paymentGateWayStoreType,
    this.paymentGateWayStoreId,
    this.paymentGateWayStorePassword,
    this.createdBy,
    this.createdDate,
    this.lastModifiedBy,
    this.lastModifiedDate,
  });

  factory PropertyModel.fromJson(Map<String, dynamic> json) => PropertyModel(
        id: json["Id"],
        propertyName: json["PropertyName"],
        propertyAddress: json["PropertyAddress"],
        webAddress: json["WebAddress"],
        emailAddress: json["EmailAddress"],
        contactPerson: json["ContactPerson"],
        contactNumber: json["ContactNumber"],
        endPointIp: json["EndPointIp"],
        logoUrl: json["LogoURL"],
        coverPictureURL: json["CoverPictureURL"],
        rankNumber: json["RankNumber"],
        lineOne: json["LineOne"],
        lineTwo: json["LineTwo"],
        lineThree: json["LineThree"],
        lineFour: json["LineFour"],
        lineFive: json["LineFive"],
        paymentGateWayStoreType: json["PaymentGateWayStoreType"],
        paymentGateWayStoreId: json["PaymentGateWayStoreId"],
        paymentGateWayStorePassword: json["PaymentGateWayStorePassword"],
        createdBy: json["CreatedBy"],
        createdDate: json["CreatedDate"],
        lastModifiedBy: json["LastModifiedBy"],
        lastModifiedDate: json["LastModifiedDate"],
      );

  Map<String, dynamic> toJson() => {
        "Id": id,
        "PropertyName": propertyName,
        "PropertyAddress": propertyAddress,
        "WebAddress": webAddress,
        "EmailAddress": emailAddress,
        "ContactPerson": contactPerson,
        "ContactNumber": contactNumber,
        "EndPointIp": endPointIp,
        "LogoURL": logoUrl,
        "CoverPictureURL": coverPictureURL,
        "RankNumber": rankNumber,
        "LineOne": lineOne,
        "LineTwo": lineTwo,
        "LineThree": lineThree,
        "LineFour": lineFour,
        "LineFive": lineFive,
        "PaymentGateWayStoreType": paymentGateWayStoreType,
        "PaymentGateWayStoreId": paymentGateWayStoreId,
        "PaymentGateWayStorePassword": paymentGateWayStorePassword,
        "CreatedBy": createdBy,
        "CreatedDate": createdDate,
        "LastModifiedBy": lastModifiedBy,
        "LastModifiedDate": lastModifiedDate,
      };
}
