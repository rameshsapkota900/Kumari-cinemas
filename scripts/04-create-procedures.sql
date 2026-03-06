-- ============================================================================
-- Kumari Cinemas Management System - Oracle 26ai Stored Procedures
-- Script: 04-create-procedures.sql
-- Purpose: Create reusable database procedures for business operations
-- Version: 1.0
-- Date: 2024
-- ============================================================================

SET ECHO ON;
SET FEEDBACK ON;

-- ============================================================================
-- SECTION 1: USER MANAGEMENT PROCEDURES
-- ============================================================================

-- Create new user
CREATE OR REPLACE PROCEDURE sp_create_user(
    p_username IN VARCHAR2,
    p_email IN VARCHAR2,
    p_password_hash IN VARCHAR2,
    p_full_name IN VARCHAR2,
    p_phone IN VARCHAR2,
    p_user_role IN VARCHAR2 DEFAULT 'customer',
    p_user_id OUT NUMBER
)
AS
BEGIN
    INSERT INTO users (username, email, password_hash, full_name, phone, user_role, is_active, is_verified)
    VALUES (p_username, p_email, p_password_hash, p_full_name, p_phone, p_user_role, 1, 0)
    RETURNING user_id INTO p_user_id;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('User created with ID: ' || p_user_id);
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20101, 'Username or email already exists');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20100, 'Error creating user: ' || SQLERRM);
END sp_create_user;
/

-- Verify user login
CREATE OR REPLACE PROCEDURE sp_verify_user_login(
    p_username IN VARCHAR2,
    p_password_hash IN VARCHAR2,
    p_user_id OUT NUMBER,
    p_is_valid OUT NUMBER
)
AS
BEGIN
    SELECT user_id INTO p_user_id
    FROM users
    WHERE username = p_username
    AND password_hash = p_password_hash
    AND is_active = 1;
    
    p_is_valid := 1;
    UPDATE users SET last_login = SYSTIMESTAMP WHERE user_id = p_user_id;
    COMMIT;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        p_is_valid := 0;
        p_user_id := NULL;
    WHEN OTHERS THEN
        p_is_valid := 0;
        RAISE_APPLICATION_ERROR(-20102, 'Error verifying login: ' || SQLERRM);
END sp_verify_user_login;
/

-- ============================================================================
-- SECTION 2: BOOKING MANAGEMENT PROCEDURES
-- ============================================================================

-- Create new booking
CREATE OR REPLACE PROCEDURE sp_create_booking(
    p_user_id IN NUMBER,
    p_show_id IN NUMBER,
    p_number_of_seats IN NUMBER,
    p_total_amount IN NUMBER,
    p_booking_id OUT NUMBER
)
AS
    v_available_seats NUMBER;
BEGIN
    -- Check available seats
    SELECT available_seats INTO v_available_seats
    FROM shows
    WHERE show_id = p_show_id;
    
    IF v_available_seats < p_number_of_seats THEN
        RAISE_APPLICATION_ERROR(-20201, 'Not enough available seats');
    END IF;
    
    -- Create booking
    INSERT INTO bookings (user_id, show_id, number_of_seats, total_amount, booking_status)
    VALUES (p_user_id, p_show_id, p_number_of_seats, p_total_amount, 'Pending')
    RETURNING booking_id INTO p_booking_id;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Booking created with ID: ' || p_booking_id);
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20202, 'Show not found');
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20200, 'Error creating booking: ' || SQLERRM);
END sp_create_booking;
/

-- Confirm booking
CREATE OR REPLACE PROCEDURE sp_confirm_booking(
    p_booking_id IN NUMBER
)
AS
BEGIN
    UPDATE bookings
    SET booking_status = 'Confirmed'
    WHERE booking_id = p_booking_id;
    
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20203, 'Booking not found');
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Booking confirmed: ' || p_booking_id);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20204, 'Error confirming booking: ' || SQLERRM);
END sp_confirm_booking;
/

-- Cancel booking
CREATE OR REPLACE PROCEDURE sp_cancel_booking(
    p_booking_id IN NUMBER
)
AS
BEGIN
    UPDATE bookings
    SET booking_status = 'Cancelled'
    WHERE booking_id = p_booking_id;
    
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20205, 'Booking not found');
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Booking cancelled: ' || p_booking_id);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20206, 'Error cancelling booking: ' || SQLERRM);
END sp_cancel_booking;
/

-- ============================================================================
-- SECTION 3: PAYMENT PROCEDURES
-- ============================================================================

-- Create payment record
CREATE OR REPLACE PROCEDURE sp_create_payment(
    p_booking_id IN NUMBER,
    p_amount IN NUMBER,
    p_payment_method IN VARCHAR2,
    p_transaction_id IN VARCHAR2,
    p_payment_id OUT NUMBER
)
AS
BEGIN
    INSERT INTO payments (booking_id, amount, payment_method, transaction_id, payment_status)
    VALUES (p_booking_id, p_amount, p_payment_method, p_transaction_id, 'Pending')
    RETURNING payment_id INTO p_payment_id;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Payment created with ID: ' || p_payment_id);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20301, 'Error creating payment: ' || SQLERRM);
END sp_create_payment;
/

-- Confirm payment
CREATE OR REPLACE PROCEDURE sp_confirm_payment(
    p_payment_id IN NUMBER,
    p_booking_id IN NUMBER
)
AS
BEGIN
    UPDATE payments
    SET payment_status = 'Confirmed',
        payment_date = SYSTIMESTAMP
    WHERE payment_id = p_payment_id;
    
    -- Update booking status
    UPDATE bookings
    SET booking_status = 'Confirmed'
    WHERE booking_id = p_booking_id;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Payment confirmed: ' || p_payment_id);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20302, 'Error confirming payment: ' || SQLERRM);
END sp_confirm_payment;
/

-- Refund payment
CREATE OR REPLACE PROCEDURE sp_refund_payment(
    p_payment_id IN NUMBER,
    p_booking_id IN NUMBER
)
AS
BEGIN
    UPDATE payments
    SET payment_status = 'Refunded',
        payment_date = SYSTIMESTAMP
    WHERE payment_id = p_payment_id;
    
    -- Update booking status
    UPDATE bookings
    SET booking_status = 'Cancelled'
    WHERE booking_id = p_booking_id;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Payment refunded: ' || p_payment_id);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20303, 'Error refunding payment: ' || SQLERRM);
END sp_refund_payment;
/

-- ============================================================================
-- SECTION 4: TICKET PROCEDURES
-- ============================================================================

-- Generate ticket
CREATE OR REPLACE PROCEDURE sp_generate_ticket(
    p_booking_id IN NUMBER,
    p_user_id IN NUMBER,
    p_seat_id IN NUMBER,
    p_show_id IN NUMBER,
    p_ticket_number IN VARCHAR2,
    p_ticket_id OUT NUMBER
)
AS
BEGIN
    INSERT INTO tickets (booking_id, user_id, seat_id, show_id, ticket_number, ticket_status, is_used)
    VALUES (p_booking_id, p_user_id, p_seat_id, p_show_id, p_ticket_number, 'Active', 0)
    RETURNING ticket_id INTO p_ticket_id;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Ticket generated: ' || p_ticket_number);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20401, 'Error generating ticket: ' || SQLERRM);
END sp_generate_ticket;
/

-- Check in ticket
CREATE OR REPLACE PROCEDURE sp_checkin_ticket(
    p_ticket_number IN VARCHAR2
)
AS
BEGIN
    UPDATE tickets
    SET is_used = 1,
        ticket_status = 'Used',
        check_in_time = SYSTIMESTAMP
    WHERE ticket_number = p_ticket_number;
    
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20402, 'Ticket not found');
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Ticket checked in: ' || p_ticket_number);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20403, 'Error checking in ticket: ' || SQLERRM);
END sp_checkin_ticket;
/

-- Cancel ticket
CREATE OR REPLACE PROCEDURE sp_cancel_ticket(
    p_ticket_id IN NUMBER
)
AS
BEGIN
    UPDATE tickets
    SET ticket_status = 'Cancelled'
    WHERE ticket_id = p_ticket_id;
    
    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20404, 'Ticket not found');
    END IF;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Ticket cancelled: ' || p_ticket_id);
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20405, 'Error cancelling ticket: ' || SQLERRM);
END sp_cancel_ticket;
/

-- ============================================================================
-- SECTION 5: REPORT PROCEDURES
-- ============================================================================

-- Get user ticket history
CREATE OR REPLACE PROCEDURE sp_get_user_tickets(
    p_user_id IN NUMBER,
    p_cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR
    SELECT 
        t.ticket_id,
        t.ticket_number,
        m.title as movie_title,
        h.hall_name,
        th.theater_name,
        th.city,
        s.show_date,
        s.show_time,
        st.seat_row,
        st.seat_number,
        b.total_amount as price,
        t.ticket_status,
        t.is_used
    FROM tickets t
    JOIN bookings b ON t.booking_id = b.booking_id
    JOIN shows s ON t.show_id = s.show_id
    JOIN movies m ON s.movie_id = m.movie_id
    JOIN halls h ON s.hall_id = h.hall_id
    JOIN theaters th ON h.theater_id = th.theater_id
    JOIN seats st ON t.seat_id = st.seat_id
    WHERE b.user_id = p_user_id
    AND b.booking_status IN ('Confirmed', 'Completed')
    AND s.show_date >= TRUNC(SYSDATE - 180)
    ORDER BY s.show_date DESC;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20501, 'Error retrieving user tickets: ' || SQLERRM);
END sp_get_user_tickets;
/

-- Get theater occupancy by movie
CREATE OR REPLACE PROCEDURE sp_get_theater_occupancy(
    p_movie_id IN NUMBER,
    p_cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR
    SELECT 
        ROWNUM as rank,
        th.theater_name,
        h.hall_name,
        th.city,
        h.capacity,
        COUNT(DISTINCT t.ticket_id) as paid_tickets,
        ROUND((COUNT(DISTINCT t.ticket_id) / h.capacity) * 100, 2) as occupancy_percentage
    FROM shows s
    JOIN halls h ON s.hall_id = h.hall_id
    JOIN theaters th ON h.theater_id = th.theater_id
    LEFT JOIN bookings b ON s.show_id = b.show_id AND b.booking_status = 'Confirmed'
    LEFT JOIN tickets t ON b.booking_id = t.booking_id
    WHERE s.movie_id = p_movie_id
    GROUP BY th.theater_name, h.hall_name, th.city, h.capacity
    ORDER BY occupancy_percentage DESC
    FETCH FIRST 3 ROWS ONLY;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20502, 'Error retrieving occupancy: ' || SQLERRM);
END sp_get_theater_occupancy;
/

-- Get theater hall movie schedule
CREATE OR REPLACE PROCEDURE sp_get_hall_schedule(
    p_hall_id IN NUMBER,
    p_cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR
    SELECT 
        s.show_id,
        m.title as movie_title,
        m.genre,
        m.language,
        m.duration_minutes,
        s.show_date,
        s.show_time,
        s.end_time,
        s.price,
        h.capacity,
        s.booked_seats,
        (h.capacity - s.booked_seats) as available_seats,
        s.show_status
    FROM shows s
    JOIN movies m ON s.movie_id = m.movie_id
    JOIN halls h ON s.hall_id = h.hall_id
    WHERE s.hall_id = p_hall_id
    AND s.show_date >= TRUNC(SYSDATE)
    ORDER BY s.show_date, s.show_time;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20503, 'Error retrieving hall schedule: ' || SQLERRM);
END sp_get_hall_schedule;
/

-- ============================================================================
-- SECTION 6: MAINTENANCE PROCEDURES
-- ============================================================================

-- Update show status based on date/time
CREATE OR REPLACE PROCEDURE sp_update_show_status
AS
BEGIN
    -- Mark completed shows
    UPDATE shows
    SET show_status = 'Completed'
    WHERE show_status = 'Scheduled'
    AND show_date < TRUNC(SYSDATE)
    OR (show_date = TRUNC(SYSDATE) AND show_time < TO_CHAR(SYSDATE, 'HH24:MI'));
    
    -- Mark cancelled shows (30 days old)
    UPDATE shows
    SET show_status = 'Cancelled'
    WHERE show_status = 'Scheduled'
    AND show_date < TRUNC(SYSDATE - 30);
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Show statuses updated');
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20601, 'Error updating show status: ' || SQLERRM);
END sp_update_show_status;
/

-- Archive old data
CREATE OR REPLACE PROCEDURE sp_archive_old_bookings(
    p_days_old IN NUMBER DEFAULT 365
)
AS
    v_deleted_count NUMBER := 0;
BEGIN
    DELETE FROM booking_seats
    WHERE booking_id IN (
        SELECT booking_id FROM bookings
        WHERE booking_date < TRUNC(SYSDATE - p_days_old)
        AND booking_status IN ('Completed', 'Cancelled')
    );
    v_deleted_count := SQL%ROWCOUNT;
    
    DELETE FROM payments
    WHERE booking_id IN (
        SELECT booking_id FROM bookings
        WHERE booking_date < TRUNC(SYSDATE - p_days_old)
        AND booking_status IN ('Completed', 'Cancelled')
    );
    v_deleted_count := v_deleted_count + SQL%ROWCOUNT;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Archived ' || v_deleted_count || ' records');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20602, 'Error archiving data: ' || SQLERRM);
END sp_archive_old_bookings;
/

-- ============================================================================
-- SECTION 7: SUMMARY
-- ============================================================================

COMMIT;

PROMPT ============================================================================
PROMPT Stored Procedures Creation Complete!
PROMPT ============================================================================
PROMPT Total Procedures Created: 16
PROMPT Procedure Categories:
PROMPT - User Management: 2
PROMPT - Booking Management: 3
PROMPT - Payment Management: 3
PROMPT - Ticket Management: 3
PROMPT - Report Procedures: 3
PROMPT - Maintenance Procedures: 2
PROMPT ============================================================================

-- End of 04-create-procedures.sql
