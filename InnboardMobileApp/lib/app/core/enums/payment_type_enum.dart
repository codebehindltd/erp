enum PaymentTypeEnum{
  fullPayment(0),
  instalment(1);

  const PaymentTypeEnum(this.value);
  final int value;
}