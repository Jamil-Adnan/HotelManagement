SELECT * 
FROM Bookings
WHERE RoomId IN (
    SELECT RoomId FROM Rooms WHERE PricePerNight >= 599
);
