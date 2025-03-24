SELECT *
FROM Rooms 
INNER JOIN Bookings ON Rooms.RoomId = Bookings.RoomId 
INNER JOIN Guests ON Guests.GuestId = Bookings.GuestId
INNER JOIN Payments on Payments.BookingId = Bookings.BookingId