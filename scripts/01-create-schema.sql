-- ============================================================================
-- Kumari Cinemas Management System - Oracle 26ai Database Schema
-- Script: 01-create-schema.sql
-- Purpose: Create all tables, sequences, and primary structures
-- Version: 1.0
-- Date: 2024
-- ============================================================================

-- SET SQL Formatting Options
SET ECHO ON;
SET FEEDBACK ON;
SET LINESIZE 120;
SET PAGESIZE 50;

-- ============================================================================
-- SECTION 1: USERS TABLE
-- ============================================================================

CREATE TABLE users (
    user_id               NUMBER PRIMARY KEY,
    username              VARCHAR2(50) NOT NULL UNIQUE,
    email                 VARCHAR2(100) NOT NULL UNIQUE,
    password_hash         VARCHAR2(255) NOT NULL,
    full_name             VARCHAR2(100) NOT NULL,
    phone                 VARCHAR2(20),
    user_role             VARCHAR2(20) DEFAULT 'customer',
    is_active             NUMBER(1) DEFAULT 1,
    is_verified           NUMBER(1) DEFAULT 0,
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    last_login            TIMESTAMP,
    CONSTRAINT ck_user_role CHECK (user_role IN ('admin', 'manager', 'customer', 'guest')),
    CONSTRAINT ck_user_active CHECK (is_active IN (0, 1)),
    CONSTRAINT ck_user_verified CHECK (is_verified IN (0, 1))
);

COMMENT ON TABLE users IS 'System users - customers, managers, administrators';
COMMENT ON COLUMN users.user_id IS 'Unique user identifier';
COMMENT ON COLUMN users.username IS 'Login username';
COMMENT ON COLUMN users.email IS 'Email address (used for password recovery)';
COMMENT ON COLUMN users.password_hash IS 'Bcrypt hashed password';
COMMENT ON COLUMN users.user_role IS 'User role: admin, manager, customer, guest';
COMMENT ON COLUMN users.is_active IS '1 = active, 0 = inactive';

-- ============================================================================
-- SECTION 2: THEATERS TABLE
-- ============================================================================

CREATE TABLE theaters (
    theater_id            NUMBER PRIMARY KEY,
    theater_name          VARCHAR2(100) NOT NULL,
    city                  VARCHAR2(50) NOT NULL,
    address               VARCHAR2(200),
    phone                 VARCHAR2(20),
    manager_id            NUMBER,
    total_capacity        NUMBER,
    is_active             NUMBER(1) DEFAULT 1,
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_theater_manager FOREIGN KEY (manager_id) REFERENCES users(user_id),
    CONSTRAINT ck_theater_active CHECK (is_active IN (0, 1))
);

COMMENT ON TABLE theaters IS 'Cinema theater locations';
COMMENT ON COLUMN theaters.theater_id IS 'Unique theater identifier';
COMMENT ON COLUMN theaters.theater_name IS 'Theater name/brand';
COMMENT ON COLUMN theaters.city IS 'City where theater is located';

-- ============================================================================
-- SECTION 3: HALLS TABLE
-- ============================================================================

CREATE TABLE halls (
    hall_id               NUMBER PRIMARY KEY,
    theater_id            NUMBER NOT NULL,
    hall_name             VARCHAR2(50) NOT NULL,
    capacity              NUMBER NOT NULL,
    hall_type             VARCHAR2(20),
    screen_size           VARCHAR2(20),
    sound_system          VARCHAR2(50),
    is_active             NUMBER(1) DEFAULT 1,
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_hall_theater FOREIGN KEY (theater_id) REFERENCES theaters(theater_id),
    CONSTRAINT ck_hall_type CHECK (hall_type IN ('Standard', '3D', 'IMAX', 'Premium')),
    CONSTRAINT ck_hall_active CHECK (is_active IN (0, 1))
);

COMMENT ON TABLE halls IS 'Screening halls within theaters';
COMMENT ON COLUMN halls.hall_id IS 'Unique hall identifier';
COMMENT ON COLUMN halls.hall_type IS 'Hall type: Standard, 3D, IMAX, Premium';

-- ============================================================================
-- SECTION 4: SEATS TABLE
-- ============================================================================

CREATE TABLE seats (
    seat_id               NUMBER PRIMARY KEY,
    hall_id               NUMBER NOT NULL,
    seat_row              VARCHAR2(2) NOT NULL,
    seat_number           NUMBER NOT NULL,
    seat_class            VARCHAR2(20),
    is_disabled           NUMBER(1) DEFAULT 0,
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_seat_hall FOREIGN KEY (hall_id) REFERENCES halls(hall_id),
    CONSTRAINT ck_seat_class CHECK (seat_class IN ('Economy', 'Silver', 'Gold', 'Platinum')),
    CONSTRAINT ck_seat_disabled CHECK (is_disabled IN (0, 1)),
    CONSTRAINT uk_seat_location UNIQUE (hall_id, seat_row, seat_number)
);

COMMENT ON TABLE seats IS 'Individual seats in screening halls';
COMMENT ON COLUMN seats.seat_class IS 'Seat category for pricing: Economy, Silver, Gold, Platinum';

-- ============================================================================
-- SECTION 5: MOVIES TABLE
-- ============================================================================

CREATE TABLE movies (
    movie_id              NUMBER PRIMARY KEY,
    title                 VARCHAR2(200) NOT NULL,
    genre                 VARCHAR2(50),
    language              VARCHAR2(30),
    duration_minutes      NUMBER,
    release_date          DATE,
    end_date              DATE,
    rating                VARCHAR2(10),
    director              VARCHAR2(100),
    cast_members          CLOB,
    description           CLOB,
    poster_url            VARCHAR2(500),
    is_active             NUMBER(1) DEFAULT 1,
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT ck_movie_rating CHECK (rating IN ('U', 'UA', 'A', 'S', 'Unrated')),
    CONSTRAINT ck_movie_active CHECK (is_active IN (0, 1))
);

COMMENT ON TABLE movies IS 'Movie catalog';
COMMENT ON COLUMN movies.movie_id IS 'Unique movie identifier';
COMMENT ON COLUMN movies.rating IS 'CBFC rating: U, UA, A, S, Unrated';

-- ============================================================================
-- SECTION 6: SHOWS TABLE
-- ============================================================================

CREATE TABLE shows (
    show_id               NUMBER PRIMARY KEY,
    movie_id              NUMBER NOT NULL,
    hall_id               NUMBER NOT NULL,
    show_date             DATE NOT NULL,
    show_time             VARCHAR2(5) NOT NULL,
    end_time              VARCHAR2(5),
    price                 NUMBER(7,2) NOT NULL,
    available_seats       NUMBER,
    booked_seats          NUMBER DEFAULT 0,
    show_status           VARCHAR2(20) DEFAULT 'Scheduled',
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_show_movie FOREIGN KEY (movie_id) REFERENCES movies(movie_id),
    CONSTRAINT fk_show_hall FOREIGN KEY (hall_id) REFERENCES halls(hall_id),
    CONSTRAINT ck_show_status CHECK (show_status IN ('Scheduled', 'Ongoing', 'Completed', 'Cancelled')),
    CONSTRAINT ck_show_price CHECK (price > 0)
);

COMMENT ON TABLE shows IS 'Movie showtimes/screenings';
COMMENT ON COLUMN shows.show_id IS 'Unique show identifier';
COMMENT ON COLUMN shows.show_status IS 'Show status: Scheduled, Ongoing, Completed, Cancelled';

-- ============================================================================
-- SECTION 7: BOOKINGS TABLE
-- ============================================================================

CREATE TABLE bookings (
    booking_id            NUMBER PRIMARY KEY,
    user_id               NUMBER NOT NULL,
    show_id               NUMBER NOT NULL,
    booking_date          TIMESTAMP DEFAULT SYSTIMESTAMP,
    number_of_seats       NUMBER NOT NULL,
    total_amount          NUMBER(10,2) NOT NULL,
    booking_status        VARCHAR2(20) DEFAULT 'Pending',
    special_requests      VARCHAR2(500),
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_booking_user FOREIGN KEY (user_id) REFERENCES users(user_id),
    CONSTRAINT fk_booking_show FOREIGN KEY (show_id) REFERENCES shows(show_id),
    CONSTRAINT ck_booking_status CHECK (booking_status IN ('Pending', 'Confirmed', 'Cancelled', 'Completed')),
    CONSTRAINT ck_booking_seats CHECK (number_of_seats > 0),
    CONSTRAINT ck_booking_amount CHECK (total_amount >= 0)
);

COMMENT ON TABLE bookings IS 'Customer ticket bookings';
COMMENT ON COLUMN bookings.booking_id IS 'Unique booking identifier';
COMMENT ON COLUMN bookings.booking_status IS 'Booking status: Pending, Confirmed, Cancelled, Completed';

-- ============================================================================
-- SECTION 8: BOOKING_SEATS TABLE
-- ============================================================================

CREATE TABLE booking_seats (
    booking_seat_id       NUMBER PRIMARY KEY,
    booking_id            NUMBER NOT NULL,
    seat_id               NUMBER NOT NULL,
    seat_price            NUMBER(7,2),
    reserved_at           TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_bkseat_booking FOREIGN KEY (booking_id) REFERENCES bookings(booking_id),
    CONSTRAINT fk_bkseat_seat FOREIGN KEY (seat_id) REFERENCES seats(seat_id),
    CONSTRAINT uk_booking_seat UNIQUE (booking_id, seat_id)
);

COMMENT ON TABLE booking_seats IS 'Individual seats reserved for a booking';

-- ============================================================================
-- SECTION 9: PAYMENTS TABLE
-- ============================================================================

CREATE TABLE payments (
    payment_id            NUMBER PRIMARY KEY,
    booking_id            NUMBER NOT NULL,
    amount                NUMBER(10,2) NOT NULL,
    payment_method        VARCHAR2(30),
    payment_status        VARCHAR2(20) DEFAULT 'Pending',
    transaction_id        VARCHAR2(100),
    gateway_response      CLOB,
    payment_date          TIMESTAMP,
    notes                 VARCHAR2(500),
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_payment_booking FOREIGN KEY (booking_id) REFERENCES bookings(booking_id),
    CONSTRAINT ck_payment_method CHECK (payment_method IN ('Credit Card', 'Debit Card', 'Wallet', 'UPI', 'Cash')),
    CONSTRAINT ck_payment_status CHECK (payment_status IN ('Pending', 'Processing', 'Confirmed', 'Failed', 'Refunded')),
    CONSTRAINT ck_payment_amount CHECK (amount > 0)
);

COMMENT ON TABLE payments IS 'Payment transactions for bookings';
COMMENT ON COLUMN payments.payment_id IS 'Unique payment identifier';
COMMENT ON COLUMN payments.payment_status IS 'Payment status: Pending, Processing, Confirmed, Failed, Refunded';

-- ============================================================================
-- SECTION 10: TICKETS TABLE
-- ============================================================================

CREATE TABLE tickets (
    ticket_id             NUMBER PRIMARY KEY,
    booking_id            NUMBER NOT NULL,
    user_id               NUMBER NOT NULL,
    seat_id               NUMBER NOT NULL,
    show_id               NUMBER NOT NULL,
    ticket_number         VARCHAR2(20) UNIQUE NOT NULL,
    qr_code               VARCHAR2(500),
    ticket_status         VARCHAR2(20) DEFAULT 'Active',
    check_in_time         TIMESTAMP,
    is_used               NUMBER(1) DEFAULT 0,
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_ticket_booking FOREIGN KEY (booking_id) REFERENCES bookings(booking_id),
    CONSTRAINT fk_ticket_user FOREIGN KEY (user_id) REFERENCES users(user_id),
    CONSTRAINT fk_ticket_seat FOREIGN KEY (seat_id) REFERENCES seats(seat_id),
    CONSTRAINT fk_ticket_show FOREIGN KEY (show_id) REFERENCES shows(show_id),
    CONSTRAINT ck_ticket_status CHECK (ticket_status IN ('Active', 'Used', 'Cancelled', 'Refunded')),
    CONSTRAINT ck_ticket_used CHECK (is_used IN (0, 1))
);

COMMENT ON TABLE tickets IS 'Individual tickets issued for bookings';
COMMENT ON COLUMN tickets.ticket_id IS 'Unique ticket identifier';
COMMENT ON COLUMN tickets.ticket_status IS 'Ticket status: Active, Used, Cancelled, Refunded';

-- ============================================================================
-- SECTION 11: APPROVALS TABLE
-- ============================================================================

CREATE TABLE approvals (
    approval_id           NUMBER PRIMARY KEY,
    booking_id            NUMBER NOT NULL,
    user_id               NUMBER NOT NULL,
    approval_type         VARCHAR2(30),
    status                VARCHAR2(20) DEFAULT 'Pending',
    approved_by           NUMBER,
    approval_date         TIMESTAMP,
    comments              VARCHAR2(500),
    created_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    updated_at            TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_approval_booking FOREIGN KEY (booking_id) REFERENCES bookings(booking_id),
    CONSTRAINT fk_approval_user FOREIGN KEY (user_id) REFERENCES users(user_id),
    CONSTRAINT fk_approval_approver FOREIGN KEY (approved_by) REFERENCES users(user_id),
    CONSTRAINT ck_approval_type CHECK (approval_type IN ('Payment', 'Booking', 'Refund', 'Discount')),
    CONSTRAINT ck_approval_status CHECK (status IN ('Pending', 'Approved', 'Rejected', 'Cancelled'))
);

COMMENT ON TABLE approvals IS 'Approval workflows for payments and bookings';

-- ============================================================================
-- SECTION 12: AUDIT_LOG TABLE
-- ============================================================================

CREATE TABLE audit_log (
    log_id                NUMBER PRIMARY KEY,
    user_id               NUMBER,
    table_name            VARCHAR2(30),
    operation             VARCHAR2(10),
    old_values            CLOB,
    new_values            CLOB,
    ip_address            VARCHAR2(45),
    log_timestamp         TIMESTAMP DEFAULT SYSTIMESTAMP,
    CONSTRAINT fk_audit_user FOREIGN KEY (user_id) REFERENCES users(user_id),
    CONSTRAINT ck_operation CHECK (operation IN ('INSERT', 'UPDATE', 'DELETE'))
);

COMMENT ON TABLE audit_log IS 'Audit trail for data modifications';

-- ============================================================================
-- SECTION 13: SEQUENCES
-- ============================================================================

CREATE SEQUENCE seq_user_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_theater_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_hall_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_seat_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_movie_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_show_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_booking_id
    START WITH 1000
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_booking_seat_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_payment_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_ticket_id
    START WITH 10000
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_approval_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

CREATE SEQUENCE seq_audit_log_id
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

-- ============================================================================
-- SECTION 14: SUMMARY
-- ============================================================================

COMMIT;

SPOOL OFF;

PROMPT ============================================================================
PROMPT Database Schema Creation Complete!
PROMPT ============================================================================
PROMPT Total Tables Created: 12
PROMPT - users
PROMPT - theaters
PROMPT - halls
PROMPT - seats
PROMPT - movies
PROMPT - shows
PROMPT - bookings
PROMPT - booking_seats
PROMPT - payments
PROMPT - tickets
PROMPT - approvals
PROMPT - audit_log
PROMPT
PROMPT Sequences Created: 12
PROMPT ============================================================================

-- End of 01-create-schema.sql
