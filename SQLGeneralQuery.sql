SELECT *
FROM Bookings
WHERE TotalCost > 4000 and Paid = 1 and CheckedIn = 1 
ORDER BY BookedFrom