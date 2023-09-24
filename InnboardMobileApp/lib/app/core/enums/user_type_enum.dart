enum UserTypeEnum {
  aboutUs("AboutUs"),
  user("User"),
  guest("GuestRoomReservation"),
  member("MemberRoomReservation"),
  becomeMember("BecomeAMember"),
  reservation("RoomReservation");

  const UserTypeEnum(this.value);
  final String value;
}
