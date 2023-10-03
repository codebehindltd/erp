// To parse this JSON data, do
//
//     final paymentStListModel = paymentStListModelFromJson(jsonString);

import 'dart:convert';

PaymentStListModel paymentStListModelFromJson(String str) => PaymentStListModel.fromJson(json.decode(str));

String paymentStListModelToJson(PaymentStListModel data) => json.encode(data.toJson());

class PaymentStListModel {
    double? amount;
    bool? isPaid;

    PaymentStListModel({
        this.amount,
        this.isPaid=false,
    });

    factory PaymentStListModel.fromJson(Map<String, dynamic> json) => PaymentStListModel(
        amount: json["amount"]?.toDouble(),
        isPaid: json["isPaid"],
    );

    Map<String, dynamic> toJson() => {
        "amount": amount,
        "isPaid": isPaid,
    };
}
