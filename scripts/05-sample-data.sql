-- ============================================================================
-- Kumari Cinemas Management System - Oracle 26ai Sample Data
-- Script: 05-sample-data.sql
-- Purpose: Load sample data for testing and development
-- Version: 1.0
-- Date: 2024
-- ============================================================================

SET ECHO ON;
SET FEEDBACK ON;

-- ============================================================================
-- SECTION 1: INSERT SAMPLE USERS
-- ============================================================================

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'admin', 'admin@kumarinema.com', '$2b$10$abc123...', 'System Administrator', '9800001111', 'admin', 1, 1, SYSTIMESTAMP);

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'manager1', 'manager1@kumarinema.com', '$2b$10$abc123...', 'Theater Manager - Kathmandu', '9800002222', 'manager', 1, 1, SYSTIMESTAMP);

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'manager2', 'manager2@kumarinema.com', '$2b$10$abc123...', 'Theater Manager - Pokhara', '9800003333', 'manager', 1, 1, SYSTIMESTAMP);

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'customer1', 'customer1@email.com', '$2b$10$abc123...', 'Rajesh Kumar', '9841111111', 'customer', 1, 1, SYSTIMESTAMP);

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'customer2', 'customer2@email.com', '$2b$10$abc123...', 'Priya Sharma', '9842222222', 'customer', 1, 1, SYSTIMESTAMP);

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'customer3', 'customer3@email.com', '$2b$10$abc123...', 'Amit Singh', '9843333333', 'customer', 1, 1, SYSTIMESTAMP);

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'customer4', 'customer4@email.com', '$2b$10$abc123...', 'Neha Patel', '9844444444', 'customer', 1, 1, SYSTIMESTAMP);

INSERT INTO users (user_id, username, email, password_hash, full_name, phone, user_role, is_active, is_verified, created_at)
VALUES (seq_user_id.NEXTVAL, 'customer5', 'customer5@email.com', '$2b$10$abc123...', 'Rahul Gupta', '9845555555', 'customer', 1, 1, SYSTIMESTAMP);

-- ============================================================================
-- SECTION 2: INSERT SAMPLE THEATERS
-- ============================================================================

INSERT INTO theaters (theater_id, theater_name, city, address, phone, manager_id, total_capacity, is_active, created_at)
VALUES (seq_theater_id.NEXTVAL, 'Kumari Cinemas - Kathmandu', 'Kathmandu', 'Thamel, Kathmandu', '9801234567', 2, 2000, 1, SYSTIMESTAMP);

INSERT INTO theaters (theater_id, theater_name, city, address, phone, manager_id, total_capacity, is_active, created_at)
VALUES (seq_theater_id.NEXTVAL, 'Kumari Cinemas - Pokhara', 'Pokhara', 'Lakeside, Pokhara', '9802345678', 3, 1500, 1, SYSTIMESTAMP);

INSERT INTO theaters (theater_id, theater_name, city, address, phone, manager_id, total_capacity, is_active, created_at)
VALUES (seq_theater_id.NEXTVAL, 'Kumari Cinemas - Biratnagar', 'Biratnagar', 'Main Road, Biratnagar', '9803456789', NULL, 1000, 1, SYSTIMESTAMP);

-- ============================================================================
-- SECTION 3: INSERT SAMPLE HALLS
-- ============================================================================

INSERT INTO halls (hall_id, theater_id, hall_name, capacity, hall_type, screen_size, sound_system, is_active, created_at)
VALUES (seq_hall_id.NEXTVAL, 1, 'Hall A', 250, 'Premium', '70mm', 'Dolby Atmos', 1, SYSTIMESTAMP);

INSERT INTO halls (hall_id, theater_id, hall_name, capacity, hall_type, screen_size, sound_system, is_active, created_at)
VALUES (seq_hall_id.NEXTVAL, 1, 'Hall B', 200, '3D', '60mm', 'Dolby Atmos', 1, SYSTIMESTAMP);

INSERT INTO halls (hall_id, theater_id, hall_name, capacity, hall_type, screen_size, sound_system, is_active, created_at)
VALUES (seq_hall_id.NEXTVAL, 1, 'Hall C', 180, 'Standard', '50mm', 'Stereo', 1, SYSTIMESTAMP);

INSERT INTO halls (hall_id, theater_id, hall_name, capacity, hall_type, screen_size, sound_system, is_active, created_at)
VALUES (seq_hall_id.NEXTVAL, 2, 'Hall 1', 200, 'Premium', '70mm', 'Dolby Atmos', 1, SYSTIMESTAMP);

INSERT INTO halls (hall_id, theater_id, hall_name, capacity, hall_type, screen_size, sound_system, is_active, created_at)
VALUES (seq_hall_id.NEXTVAL, 2, 'Hall 2', 150, 'Standard', '50mm', 'Stereo', 1, SYSTIMESTAMP);

INSERT INTO halls (hall_id, theater_id, hall_name, capacity, hall_type, screen_size, sound_system, is_active, created_at)
VALUES (seq_hall_id.NEXTVAL, 3, 'Main Hall', 300, 'Standard', '50mm', 'Stereo', 1, SYSTIMESTAMP);

-- ============================================================================
-- SECTION 4: INSERT SAMPLE SEATS
-- ============================================================================

-- Hall A (250 seats) - Premium
BEGIN
    FOR i IN 1..10 LOOP
        FOR j IN 1..25 LOOP
            INSERT INTO seats (seat_id, hall_id, seat_row, seat_number, seat_class, is_disabled, created_at)
            VALUES (seq_seat_id.NEXTVAL, 1, CHR(64+i), j, 
                CASE WHEN i <= 2 THEN 'Platinum'
                     WHEN i <= 5 THEN 'Gold'
                     WHEN i <= 8 THEN 'Silver'
                     ELSE 'Economy' END, 0, SYSTIMESTAMP);
        END LOOP;
    END LOOP;
    COMMIT;
END;
/

-- Hall B (200 seats) - 3D
BEGIN
    FOR i IN 1..10 LOOP
        FOR j IN 1..20 LOOP
            INSERT INTO seats (seat_id, hall_id, seat_row, seat_number, seat_class, is_disabled, created_at)
            VALUES (seq_seat_id.NEXTVAL, 2, CHR(64+i), j,
                CASE WHEN i <= 3 THEN 'Gold'
                     WHEN i <= 7 THEN 'Silver'
                     ELSE 'Economy' END, 0, SYSTIMESTAMP);
        END LOOP;
    END LOOP;
    COMMIT;
END;
/

-- Hall C (180 seats) - Standard
BEGIN
    FOR i IN 1..9 LOOP
        FOR j IN 1..20 LOOP
            INSERT INTO seats (seat_id, hall_id, seat_row, seat_number, seat_class, is_disabled, created_at)
            VALUES (seq_seat_id.NEXTVAL, 3, CHR(64+i), j,
                CASE WHEN i <= 2 THEN 'Silver'
                     ELSE 'Economy' END, 0, SYSTIMESTAMP);
        END LOOP;
    END LOOP;
    COMMIT;
END;
/

-- Hall 1 (200 seats) - Pokhara Premium
BEGIN
    FOR i IN 1..10 LOOP
        FOR j IN 1..20 LOOP
            INSERT INTO seats (seat_id, hall_id, seat_row, seat_number, seat_class, is_disabled, created_at)
            VALUES (seq_seat_id.NEXTVAL, 4, CHR(64+i), j,
                CASE WHEN i <= 2 THEN 'Platinum'
                     WHEN i <= 5 THEN 'Gold'
                     ELSE 'Silver' END, 0, SYSTIMESTAMP);
        END LOOP;
    END LOOP;
    COMMIT;
END;
/

-- Hall 2 (150 seats) - Pokhara Standard
BEGIN
    FOR i IN 1..10 LOOP
        FOR j IN 1..15 LOOP
            INSERT INTO seats (seat_id, hall_id, seat_row, seat_number, seat_class, is_disabled, created_at)
            VALUES (seq_seat_id.NEXTVAL, 5, CHR(64+i), j,
                CASE WHEN i <= 3 THEN 'Silver'
                     ELSE 'Economy' END, 0, SYSTIMESTAMP);
        END LOOP;
    END LOOP;
    COMMIT;
END;
/

-- Main Hall (300 seats) - Biratnagar
BEGIN
    FOR i IN 1..15 LOOP
        FOR j IN 1..20 LOOP
            INSERT INTO seats (seat_id, hall_id, seat_row, seat_number, seat_class, is_disabled, created_at)
            VALUES (seq_seat_id.NEXTVAL, 6, CHR(64+i), j,
                CASE WHEN i <= 3 THEN 'Silver'
                     ELSE 'Economy' END, 0, SYSTIMESTAMP);
        END LOOP;
    END LOOP;
    COMMIT;
END;
/

-- ============================================================================
-- SECTION 5: INSERT SAMPLE MOVIES
-- ============================================================================

INSERT INTO movies (movie_id, title, genre, language, duration_minutes, release_date, end_date, rating, director, description, is_active, created_at)
VALUES (seq_movie_id.NEXTVAL, 'Pathaan', 'Action/Thriller', 'Hindi', 146, TO_DATE('2023-01-25', 'YYYY-MM-DD'), TO_DATE('2024-03-30', 'YYYY-MM-DD'), 'UA', 'Siddharth Anand', 
'A spy thriller with high-octane action sequences', 1, SYSTIMESTAMP);

INSERT INTO movies (movie_id, title, genre, language, duration_minutes, release_date, end_date, rating, director, description, is_active, created_at)
VALUES (seq_movie_id.NEXTVAL, 'Hariyo Van', 'Drama', 'Nepali', 142, TO_DATE('2023-02-10', 'YYYY-MM-DD'), TO_DATE('2024-04-10', 'YYYY-MM-DD'), 'U', 'Prasad Oak', 
'A heartwarming family drama set in Nepal', 1, SYSTIMESTAMP);

INSERT INTO movies (movie_id, title, genre, language, duration_minutes, release_date, end_date, rating, director, description, is_active, created_at)
VALUES (seq_movie_id.NEXTVAL, 'Avatar: The Way of Water', 'Sci-Fi', 'English', 192, TO_DATE('2022-12-16', 'YYYY-MM-DD'), TO_DATE('2024-05-30', 'YYYY-MM-DD'), 'U', 'James Cameron', 
'Epic science fiction adventure with stunning visuals', 1, SYSTIMESTAMP);

INSERT INTO movies (movie_id, title, genre, language, duration_minutes, release_date, end_date, rating, director, description, is_active, created_at)
VALUES (seq_movie_id.NEXTVAL, 'Shikshanachya Aadhaar', 'Comedy', 'Nepali', 130, TO_DATE('2023-03-01', 'YYYY-MM-DD'), TO_DATE('2024-04-15', 'YYYY-MM-DD'), 'U', 'Dipendra K Khanal', 
'A hilarious comedy about education', 1, SYSTIMESTAMP);

INSERT INTO movies (movie_id, title, genre, language, duration_minutes, release_date, end_date, rating, director, description, is_active, created_at)
VALUES (seq_movie_id.NEXTVAL, 'Oppenheimer', 'Biography/Drama', 'English', 180, TO_DATE('2023-07-21', 'YYYY-MM-DD'), TO_DATE('2024-06-30', 'YYYY-MM-DD'), 'UA', 'Christopher Nolan', 
'The story of the man who built the atomic bomb', 1, SYSTIMESTAMP);

-- ============================================================================
-- SECTION 6: INSERT SAMPLE SHOWS
-- ============================================================================

-- Shows for Pathaan in Hall A (today and next 7 days)
BEGIN
    FOR i IN 0..7 LOOP
        INSERT INTO shows (show_id, movie_id, hall_id, show_date, show_time, end_time, price, available_seats, booked_seats, show_status, created_at)
        VALUES (seq_show_id.NEXTVAL, 1, 1, TRUNC(SYSDATE) + i, '11:00', '14:46', 450, 250, 0, 'Scheduled', SYSTIMESTAMP);
        
        INSERT INTO shows (show_id, movie_id, hall_id, show_date, show_time, end_time, price, available_seats, booked_seats, show_status, created_at)
        VALUES (seq_show_id.NEXTVAL, 1, 1, TRUNC(SYSDATE) + i, '15:30', '19:16', 500, 250, 0, 'Scheduled', SYSTIMESTAMP);
        
        INSERT INTO shows (show_id, movie_id, hall_id, show_date, show_time, end_time, price, available_seats, booked_seats, show_status, created_at)
        VALUES (seq_show_id.NEXTVAL, 1, 1, TRUNC(SYSDATE) + i, '20:00', '23:46', 550, 250, 0, 'Scheduled', SYSTIMESTAMP);
    END LOOP;
    COMMIT;
END;
/

-- Shows for Hariyo Van in Hall B
BEGIN
    FOR i IN 0..7 LOOP
        INSERT INTO shows (show_id, movie_id, hall_id, show_date, show_time, end_time, price, available_seats, booked_seats, show_status, created_at)
        VALUES (seq_show_id.NEXTVAL, 2, 2, TRUNC(SYSDATE) + i, '10:30', '13:52', 350, 200, 0, 'Scheduled', SYSTIMESTAMP);
        
        INSERT INTO shows (show_id, movie_id, hall_id, show_date, show_time, end_time, price, available_seats, booked_seats, show_status, created_at)
        VALUES (seq_show_id.NEXTVAL, 2, 2, TRUNC(SYSDATE) + i, '16:00', '19:22', 400, 200, 0, 'Scheduled', SYSTIMESTAMP);
        
        INSERT INTO shows (show_id, movie_id, hall_id, show_date, show_time, end_time, price, available_seats, booked_seats, show_status, created_at)
        VALUES (seq_show_id.NEXTVAL, 2, 2, TRUNC(SYSDATE) + i, '21:00', '23:22', 450, 200, 0, 'Scheduled', SYSTIMESTAMP);
    END LOOP;
    COMMIT;
END;
/

-- ============================================================================
-- SECTION 7: INSERT SAMPLE BOOKINGS
-- ============================================================================

INSERT INTO bookings (booking_id, user_id, show_id, booking_date, number_of_seats, total_amount, booking_status, created_at)
VALUES (seq_booking_id.NEXTVAL, 4, 1, SYSTIMESTAMP, 2, 900, 'Confirmed', SYSTIMESTAMP);

INSERT INTO bookings (booking_id, user_id, show_id, booking_date, number_of_seats, total_amount, booking_status, created_at)
VALUES (seq_booking_id.NEXTVAL, 5, 1, SYSTIMESTAMP, 3, 1350, 'Confirmed', SYSTIMESTAMP);

INSERT INTO bookings (booking_id, user_id, show_id, booking_date, number_of_seats, total_amount, booking_status, created_at)
VALUES (seq_booking_id.NEXTVAL, 6, 4, SYSTIMESTAMP, 2, 700, 'Confirmed', SYSTIMESTAMP);

INSERT INTO bookings (booking_id, user_id, show_id, booking_date, number_of_seats, total_amount, booking_status, created_at)
VALUES (seq_booking_id.NEXTVAL, 7, 7, SYSTIMESTAMP, 4, 1600, 'Pending', SYSTIMESTAMP);

-- ============================================================================
-- SECTION 8: INSERT SAMPLE PAYMENTS
-- ============================================================================

INSERT INTO payments (payment_id, booking_id, amount, payment_method, transaction_id, payment_status, payment_date, created_at)
VALUES (seq_payment_id.NEXTVAL, 1001, 900, 'Credit Card', 'TXN20240101001', 'Confirmed', SYSTIMESTAMP, SYSTIMESTAMP);

INSERT INTO payments (payment_id, booking_id, amount, payment_method, transaction_id, payment_status, payment_date, created_at)
VALUES (seq_payment_id.NEXTVAL, 1002, 1350, 'Debit Card', 'TXN20240101002', 'Confirmed', SYSTIMESTAMP, SYSTIMESTAMP);

INSERT INTO payments (payment_id, booking_id, amount, payment_method, transaction_id, payment_status, payment_date, created_at)
VALUES (seq_payment_id.NEXTVAL, 1003, 700, 'UPI', 'TXN20240101003', 'Confirmed', SYSTIMESTAMP, SYSTIMESTAMP);

INSERT INTO payments (payment_id, booking_id, amount, payment_method, transaction_id, payment_status, payment_date, created_at)
VALUES (seq_payment_id.NEXTVAL, 1004, 1600, 'Wallet', 'TXN20240101004', 'Pending', NULL, SYSTIMESTAMP);

-- ============================================================================
-- SECTION 9: INSERT SAMPLE TICKETS
-- ============================================================================

INSERT INTO tickets (ticket_id, booking_id, user_id, seat_id, show_id, ticket_number, ticket_status, is_used, created_at)
VALUES (seq_ticket_id.NEXTVAL, 1001, 4, 1, 1, 'TKT20240001', 'Active', 0, SYSTIMESTAMP);

INSERT INTO tickets (ticket_id, booking_id, user_id, seat_id, show_id, ticket_number, ticket_status, is_used, created_at)
VALUES (seq_ticket_id.NEXTVAL, 1001, 4, 2, 1, 'TKT20240002', 'Active', 0, SYSTIMESTAMP);

INSERT INTO tickets (ticket_id, booking_id, user_id, seat_id, show_id, ticket_number, ticket_status, is_used, created_at)
VALUES (seq_ticket_id.NEXTVAL, 1002, 5, 3, 1, 'TKT20240003', 'Active', 0, SYSTIMESTAMP);

INSERT INTO tickets (ticket_id, booking_id, user_id, seat_id, show_id, ticket_number, ticket_status, is_used, created_at)
VALUES (seq_ticket_id.NEXTVAL, 1002, 5, 4, 1, 'TKT20240004', 'Active', 0, SYSTIMESTAMP);

INSERT INTO tickets (ticket_id, booking_id, user_id, seat_id, show_id, ticket_number, ticket_status, is_used, created_at)
VALUES (seq_ticket_id.NEXTVAL, 1002, 5, 5, 1, 'TKT20240005', 'Active', 0, SYSTIMESTAMP);

-- ============================================================================
-- SECTION 10: VERIFY DATA
-- ============================================================================

COMMIT;

PROMPT ============================================================================
PROMPT Sample Data Insertion Complete!
PROMPT ============================================================================
PROMPT Records Inserted:
PROMPT - Users: 8
PROMPT - Theaters: 3
PROMPT - Halls: 6
PROMPT - Seats: 1,230
PROMPT - Movies: 5
PROMPT - Shows: 48
PROMPT - Bookings: 4
PROMPT - Payments: 4
PROMPT - Tickets: 5
PROMPT ============================================================================

-- Display summary
SELECT 'Users' as table_name, COUNT(*) as record_count FROM users
UNION ALL
SELECT 'Theaters', COUNT(*) FROM theaters
UNION ALL
SELECT 'Halls', COUNT(*) FROM halls
UNION ALL
SELECT 'Seats', COUNT(*) FROM seats
UNION ALL
SELECT 'Movies', COUNT(*) FROM movies
UNION ALL
SELECT 'Shows', COUNT(*) FROM shows
UNION ALL
SELECT 'Bookings', COUNT(*) FROM bookings
UNION ALL
SELECT 'Payments', COUNT(*) FROM payments
UNION ALL
SELECT 'Tickets', COUNT(*) FROM tickets;

-- End of 05-sample-data.sql
