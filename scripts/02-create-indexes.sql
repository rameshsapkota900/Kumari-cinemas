-- ============================================================================
-- Kumari Cinemas Management System - Oracle 26ai Indexes
-- Script: 02-create-indexes.sql
-- Purpose: Create indexes for optimal query performance
-- Version: 1.0
-- Date: 2024
-- ============================================================================

SET ECHO ON;
SET FEEDBACK ON;

-- ============================================================================
-- SECTION 1: PRIMARY KEY INDEXES (Automatically created)
-- ============================================================================

-- All primary keys automatically create indexes
-- No explicit creation needed

-- ============================================================================
-- SECTION 2: FOREIGN KEY INDEXES
-- ============================================================================

CREATE INDEX idx_theater_manager_id ON theaters(manager_id);
COMMENT ON INDEX idx_theater_manager_id IS 'For joining theaters with manager users';

CREATE INDEX idx_hall_theater_id ON halls(theater_id);
COMMENT ON INDEX idx_hall_theater_id IS 'For joining halls with theaters';

CREATE INDEX idx_seat_hall_id ON seats(hall_id);
COMMENT ON INDEX idx_seat_hall_id IS 'For joining seats with halls';

CREATE INDEX idx_show_movie_id ON shows(movie_id);
COMMENT ON INDEX idx_show_movie_id IS 'For joining shows with movies';

CREATE INDEX idx_show_hall_id ON shows(hall_id);
COMMENT ON INDEX idx_show_hall_id IS 'For joining shows with halls';

CREATE INDEX idx_booking_user_id ON bookings(user_id);
COMMENT ON INDEX idx_booking_user_id IS 'For finding bookings by user';

CREATE INDEX idx_booking_show_id ON bookings(show_id);
COMMENT ON INDEX idx_booking_show_id IS 'For finding bookings for a show';

CREATE INDEX idx_booking_seat_booking_id ON booking_seats(booking_id);
COMMENT ON INDEX idx_booking_seat_booking_id IS 'For finding seats in a booking';

CREATE INDEX idx_booking_seat_seat_id ON booking_seats(seat_id);
COMMENT ON INDEX idx_booking_seat_seat_id IS 'For checking if seat is booked';

CREATE INDEX idx_payment_booking_id ON payments(booking_id);
COMMENT ON INDEX idx_payment_booking_id IS 'For finding payments for bookings';

CREATE INDEX idx_ticket_booking_id ON tickets(booking_id);
COMMENT ON INDEX idx_ticket_booking_id IS 'For finding tickets in a booking';

CREATE INDEX idx_ticket_user_id ON tickets(user_id);
COMMENT ON INDEX idx_ticket_user_id IS 'For finding all tickets of a user';

CREATE INDEX idx_ticket_seat_id ON tickets(seat_id);
COMMENT ON INDEX idx_ticket_seat_id IS 'For checking ticket for a seat';

CREATE INDEX idx_ticket_show_id ON tickets(show_id);
COMMENT ON INDEX idx_ticket_show_id IS 'For finding tickets for a show';

CREATE INDEX idx_approval_booking_id ON approvals(booking_id);
COMMENT ON INDEX idx_approval_booking_id IS 'For finding approvals for a booking';

CREATE INDEX idx_approval_user_id ON approvals(user_id);
COMMENT ON INDEX idx_approval_user_id IS 'For finding approvals for a user';

CREATE INDEX idx_approval_approver_id ON approvals(approved_by);
COMMENT ON INDEX idx_approval_approver_id IS 'For tracking approvals by manager';

CREATE INDEX idx_audit_user_id ON audit_log(user_id);
COMMENT ON INDEX idx_audit_user_id IS 'For auditing user actions';

-- ============================================================================
-- SECTION 3: SEARCH AND FILTER INDEXES
-- ============================================================================

-- User lookups
CREATE INDEX idx_users_email ON users(email);
COMMENT ON INDEX idx_users_email IS 'For user authentication/lookup by email';

CREATE INDEX idx_users_username ON users(username);
COMMENT ON INDEX idx_users_username IS 'For user authentication/lookup by username';

CREATE INDEX idx_users_role ON users(user_role);
COMMENT ON INDEX idx_users_role IS 'For finding users by role';

CREATE INDEX idx_users_active ON users(is_active);
COMMENT ON INDEX idx_users_active IS 'For finding active users';

-- Theater lookups
CREATE INDEX idx_theaters_city ON theaters(city);
COMMENT ON INDEX idx_theaters_city IS 'For finding theaters by city';

CREATE INDEX idx_theaters_active ON theaters(is_active);
COMMENT ON INDEX idx_theaters_active IS 'For finding active theaters';

-- Hall lookups
CREATE INDEX idx_halls_active ON halls(is_active);
COMMENT ON INDEX idx_halls_active IS 'For finding active halls';

CREATE INDEX idx_halls_type ON halls(hall_type);
COMMENT ON INDEX idx_halls_type IS 'For finding halls by type (3D, IMAX, etc)';

-- Seat lookups
CREATE INDEX idx_seat_class ON seats(seat_class);
COMMENT ON INDEX idx_seat_class IS 'For finding seats by class (Economy, Silver, etc)';

CREATE INDEX idx_seat_disabled ON seats(is_disabled);
COMMENT ON INDEX idx_seat_disabled IS 'For finding available seats';

-- Movie lookups
CREATE INDEX idx_movies_genre ON movies(genre);
COMMENT ON INDEX idx_movies_genre IS 'For finding movies by genre';

CREATE INDEX idx_movies_language ON movies(language);
COMMENT ON INDEX idx_movies_language IS 'For finding movies by language';

CREATE INDEX idx_movies_rating ON movies(rating);
COMMENT ON INDEX idx_movies_rating IS 'For finding movies by rating';

CREATE INDEX idx_movies_active ON movies(is_active);
COMMENT ON INDEX idx_movies_active IS 'For finding active movies';

CREATE INDEX idx_movies_release_date ON movies(release_date);
COMMENT ON INDEX idx_movies_release_date IS 'For finding current/upcoming movies';

-- Show lookups
CREATE INDEX idx_shows_date ON shows(show_date);
COMMENT ON INDEX idx_shows_date IS 'For finding shows by date';

CREATE INDEX idx_shows_status ON shows(show_status);
COMMENT ON INDEX idx_shows_status IS 'For finding shows by status';

CREATE INDEX idx_show_movie_date ON shows(movie_id, show_date);
COMMENT ON INDEX idx_show_movie_date IS 'For finding showtimes for a movie';

CREATE INDEX idx_show_hall_date ON shows(hall_id, show_date);
COMMENT ON INDEX idx_show_hall_date IS 'For checking hall availability';

-- Booking lookups
CREATE INDEX idx_booking_status ON bookings(booking_status);
COMMENT ON INDEX idx_booking_status IS 'For finding bookings by status';

CREATE INDEX idx_booking_date ON bookings(booking_date);
COMMENT ON INDEX idx_booking_date IS 'For finding bookings by date';

CREATE INDEX idx_booking_user_date ON bookings(user_id, booking_date);
COMMENT ON INDEX idx_booking_user_date IS 'For finding user bookings by date';

-- Payment lookups
CREATE INDEX idx_payment_status ON payments(payment_status);
COMMENT ON INDEX idx_payment_status IS 'For finding payments by status';

CREATE INDEX idx_payment_method ON payments(payment_method);
COMMENT ON INDEX idx_payment_method IS 'For finding payments by method';

CREATE INDEX idx_payment_date ON payments(payment_date);
COMMENT ON INDEX idx_payment_date IS 'For finding payments by date';

CREATE INDEX idx_payment_transaction_id ON payments(transaction_id);
COMMENT ON INDEX idx_payment_transaction_id IS 'For finding payment by transaction ID';

-- Ticket lookups
CREATE INDEX idx_ticket_number ON tickets(ticket_number);
COMMENT ON INDEX idx_ticket_number IS 'For finding ticket by ticket number';

CREATE INDEX idx_ticket_status ON tickets(ticket_status);
COMMENT ON INDEX idx_ticket_status IS 'For finding tickets by status';

CREATE INDEX idx_ticket_is_used ON tickets(is_used);
COMMENT ON INDEX idx_ticket_is_used IS 'For finding used/unused tickets';

CREATE INDEX idx_ticket_check_in_time ON tickets(check_in_time);
COMMENT ON INDEX idx_ticket_check_in_time IS 'For finding checkin history';

-- Approval lookups
CREATE INDEX idx_approval_status ON approvals(status);
COMMENT ON INDEX idx_approval_status IS 'For finding approvals by status';

CREATE INDEX idx_approval_type ON approvals(approval_type);
COMMENT ON INDEX idx_approval_type IS 'For finding approvals by type';

CREATE INDEX idx_approval_date ON approvals(approval_date);
COMMENT ON INDEX idx_approval_date IS 'For finding approvals by date';

-- Audit lookups
CREATE INDEX idx_audit_timestamp ON audit_log(log_timestamp);
COMMENT ON INDEX idx_audit_timestamp IS 'For finding audit entries by timestamp';

CREATE INDEX idx_audit_table ON audit_log(table_name);
COMMENT ON INDEX idx_audit_table IS 'For finding audit entries by table';

CREATE INDEX idx_audit_operation ON audit_log(operation);
COMMENT ON INDEX idx_audit_operation IS 'For finding audit entries by operation';

-- ============================================================================
-- SECTION 4: COMPOSITE INDEXES FOR COMMON QUERIES
-- ============================================================================

-- Reports: User ticket history
CREATE INDEX idx_user_tickets ON tickets(user_id, created_at);
COMMENT ON INDEX idx_user_tickets IS 'For user ticket report queries';

-- Reports: Theater occupancy
CREATE INDEX idx_show_occupancy ON shows(movie_id, show_date, hall_id);
COMMENT ON INDEX idx_show_occupancy IS 'For theater occupancy report';

-- Reports: Theater city hall movie
CREATE INDEX idx_show_schedule ON shows(hall_id, show_date, show_time);
COMMENT ON INDEX idx_show_schedule IS 'For theater schedule report';

-- Dashboard: Recent bookings
CREATE INDEX idx_recent_bookings ON bookings(user_id, booking_date DESC);
COMMENT ON INDEX idx_recent_bookings IS 'For dashboard recent bookings';

-- Dashboard: Revenue
CREATE INDEX idx_confirmed_payments ON payments(payment_status, payment_date DESC);
COMMENT ON INDEX idx_confirmed_payments IS 'For revenue reports';

-- Temporal: Date range queries
CREATE INDEX idx_shows_temporal ON shows(show_date, show_time);
COMMENT ON INDEX idx_shows_temporal IS 'For temporal queries on shows';

CREATE INDEX idx_bookings_temporal ON bookings(booking_date, booking_status);
COMMENT ON INDEX idx_bookings_temporal IS 'For temporal queries on bookings';

-- ============================================================================
-- SECTION 5: BITMAP INDEXES (For low-cardinality columns)
-- ============================================================================

CREATE BITMAP INDEX idx_user_role_bitmap ON users(user_role);
COMMENT ON INDEX idx_user_role_bitmap IS 'Bitmap index for user role (low cardinality)';

CREATE BITMAP INDEX idx_show_status_bitmap ON shows(show_status);
COMMENT ON INDEX idx_show_status_bitmap IS 'Bitmap index for show status (low cardinality)';

CREATE BITMAP INDEX idx_booking_status_bitmap ON bookings(booking_status);
COMMENT ON INDEX idx_booking_status_bitmap IS 'Bitmap index for booking status (low cardinality)';

CREATE BITMAP INDEX idx_payment_status_bitmap ON payments(payment_status);
COMMENT ON INDEX idx_payment_status_bitmap IS 'Bitmap index for payment status (low cardinality)';

CREATE BITMAP INDEX idx_ticket_status_bitmap ON tickets(ticket_status);
COMMENT ON INDEX idx_ticket_status_bitmap IS 'Bitmap index for ticket status (low cardinality)';

-- ============================================================================
-- SECTION 6: FUNCTION-BASED INDEXES
-- ============================================================================

-- For case-insensitive email searches
CREATE INDEX idx_users_email_lower ON users(LOWER(email));
COMMENT ON INDEX idx_users_email_lower IS 'For case-insensitive email lookups';

-- For searching by year in date columns
CREATE INDEX idx_movies_release_year ON movies(EXTRACT(YEAR FROM release_date));
COMMENT ON INDEX idx_movies_release_year IS 'For finding movies by release year';

-- ============================================================================
-- SECTION 7: VERIFY INDEXES
-- ============================================================================

-- List all created indexes
SELECT index_name, table_name, uniqueness, status
FROM user_indexes
WHERE index_name NOT LIKE 'SYS_%'
ORDER BY table_name, index_name;

-- ============================================================================
-- SECTION 8: INDEX STATISTICS
-- ============================================================================

-- Gather statistics on all indexes
BEGIN
    DBMS_STATS.gather_schema_stats(
        ownname => USER,
        estimate_percent => DBMS_STATS.AUTO_SAMPLE_SIZE,
        granularity => 'ALL',
        cascade => TRUE
    );
END;
/

COMMIT;

PROMPT ============================================================================
PROMPT Index Creation Complete!
PROMPT ============================================================================
PROMPT Total Indexes Created: 80+
PROMPT Index Types:
PROMPT - Foreign Key Indexes: 18
PROMPT - Search/Filter Indexes: 45
PROMPT - Composite Indexes: 7
PROMPT - Bitmap Indexes: 5
PROMPT - Function-Based Indexes: 2
PROMPT
PROMPT All indexes have been created and statistics gathered.
PROMPT ============================================================================

-- End of 02-create-indexes.sql
