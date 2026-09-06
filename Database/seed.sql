-- Insert Initial Members
INSERT INTO Members (FullName, Email, Password, ContactNumber, Address, PreferredSports, Role) 
VALUES 
('Admin User', 'admin@example.com', '1234', '07123456789', '12 High Street, Colombo', 'Tennis, Soccer', 'Admin'),
('Sarah Jenkins', 'sarah@example.com', 'password123', '07987654321', '45 Park Avenue, Kandy', 'Basketball, Badminton', 'Member');

-- Insert Sports Facilities
INSERT INTO Facilities (Name, FacilityType, Location, HourlyRate, Capacity, Description, ImageUrl)
VALUES
('Torrington Olympic Tennis Court A', 'Tennis', 'Colombo 07 (Torrington Grounds)', 2500.00, 4, 'Professional outdoor hard court with night floodlights.', 'https://images.unsplash.com/photo-1595435934249-5df7ed86e1c0?auto=format&fit=crop&w=800&q=80'),
('Racecourse 3G Football Pitch', 'Soccer', 'Colombo 07 (Racecourse Grounds)', 5500.00, 14, 'Full size 7-a-side 3G synthetic turf suitable for all weather matches.', 'https://images.unsplash.com/photo-1529900748604-07564a03e7a6?auto=format&fit=crop&w=800&q=80'),
('Sugathadasa Indoor Basketball Arena', 'Basketball', 'Colombo 13 (Kotahena)', 4000.00, 10, 'Indoor maple hardwood court with adjustable hoops.', 'https://images.unsplash.com/photo-1546519638-68e109498ffc?auto=format&fit=crop&w=800&q=80'),
('Havelock Town Badminton Court 1', 'Badminton', 'Colombo 05 (Havelock Town)', 1800.00, 4, 'Indoor court with non-slip sprung flooring.', 'https://images.unsplash.com/photo-1626224583764-f87db24ac4ea?auto=format&fit=crop&w=800&q=80'),
('CR & FC Municipal Swimming Pool', 'Swimming', 'Colombo 07 (Cinnamon Gardens)', 3000.00, 20, '25m heated indoor lap pool.', 'https://images.unsplash.com/photo-1576013551627-0cc20b96c2a7?auto=format&fit=crop&w=800&q=80');

-- Insert Initial Bookings
INSERT INTO Bookings (MemberId, FacilityId, BookingDate, TimeSlot, Status, TotalPrice)
VALUES 
(1, 1, '2025-01-20', '10:00 - 11:00', 'Confirmed', 2500.00);

-- Insert Sample Reviews
INSERT INTO Reviews (MemberId, FacilityId, Rating, Comment)
VALUES 
(1, 1, 5, 'Fantastic tennis court! The floodlights made evening play super clear.'),
(2, 3, 4, 'Great basketball court, clean changing rooms. Would definitely book again.');
