-- ============================================================================
-- Kumari Cinemas Management System - Oracle 26ai Triggers
-- Script: 03-create-triggers.sql
-- Purpose: Create triggers for business logic automation
-- Version: 1.0
-- Date: 2024
-- ============================================================================

SET ECHO ON;
SET FEEDBACK ON;

-- ============================================================================
-- SECTION 1: TIMESTAMP TRIGGERS
-- ============================================================================

-- Update timestamp for users
CREATE OR REPLACE TRIGGER trg_users_update_timestamp
BEFORE UPDATE ON users
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for theaters
CREATE OR REPLACE TRIGGER trg_theaters_update_timestamp
BEFORE UPDATE ON theaters
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for halls
CREATE OR REPLACE TRIGGER trg_halls_update_timestamp
BEFORE UPDATE ON halls
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for movies
CREATE OR REPLACE TRIGGER trg_movies_update_timestamp
BEFORE UPDATE ON movies
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for shows
CREATE OR REPLACE TRIGGER trg_shows_update_timestamp
BEFORE UPDATE ON shows
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for bookings
CREATE OR REPLACE TRIGGER trg_bookings_update_timestamp
BEFORE UPDATE ON bookings
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for payments
CREATE OR REPLACE TRIGGER trg_payments_update_timestamp
BEFORE UPDATE ON payments
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for tickets
CREATE OR REPLACE TRIGGER trg_tickets_update_timestamp
BEFORE UPDATE ON tickets
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- Update timestamp for approvals
CREATE OR REPLACE TRIGGER trg_approvals_update_timestamp
BEFORE UPDATE ON approvals
FOR EACH ROW
BEGIN
    :NEW.updated_at := SYSTIMESTAMP;
END;
/

-- ============================================================================
-- SECTION 2: SEQUENCE-BASED ID TRIGGERS
-- ============================================================================

-- Auto-generate user_id
CREATE OR REPLACE TRIGGER trg_users_auto_id
BEFORE INSERT ON users
FOR EACH ROW
BEGIN
    IF :NEW.user_id IS NULL THEN
        SELECT seq_user_id.NEXTVAL INTO :NEW.user_id FROM dual;
    END IF;
END;
/

-- Auto-generate theater_id
CREATE OR REPLACE TRIGGER trg_theaters_auto_id
BEFORE INSERT ON theaters
FOR EACH ROW
BEGIN
    IF :NEW.theater_id IS NULL THEN
        SELECT seq_theater_id.NEXTVAL INTO :NEW.theater_id FROM dual;
    END IF;
END;
/

-- Auto-generate hall_id
CREATE OR REPLACE TRIGGER trg_halls_auto_id
BEFORE INSERT ON halls
FOR EACH ROW
BEGIN
    IF :NEW.hall_id IS NULL THEN
        SELECT seq_hall_id.NEXTVAL INTO :NEW.hall_id FROM dual;
    END IF;
END;
/

-- Auto-generate seat_id
CREATE OR REPLACE TRIGGER trg_seats_auto_id
BEFORE INSERT ON seats
FOR EACH ROW
BEGIN
    IF :NEW.seat_id IS NULL THEN
        SELECT seq_seat_id.NEXTVAL INTO :NEW.seat_id FROM dual;
    END IF;
END;
/

-- Auto-generate movie_id
CREATE OR REPLACE TRIGGER trg_movies_auto_id
BEFORE INSERT ON movies
FOR EACH ROW
BEGIN
    IF :NEW.movie_id IS NULL THEN
        SELECT seq_movie_id.NEXTVAL INTO :NEW.movie_id FROM dual;
    END IF;
END;
/

-- Auto-generate show_id
CREATE OR REPLACE TRIGGER trg_shows_auto_id
BEFORE INSERT ON shows
FOR EACH ROW
BEGIN
    IF :NEW.show_id IS NULL THEN
        SELECT seq_show_id.NEXTVAL INTO :NEW.show_id FROM dual;
    END IF;
END;
/

-- Auto-generate booking_id
CREATE OR REPLACE TRIGGER trg_bookings_auto_id
BEFORE INSERT ON bookings
FOR EACH ROW
BEGIN
    IF :NEW.booking_id IS NULL THEN
        SELECT seq_booking_id.NEXTVAL INTO :NEW.booking_id FROM dual;
    END IF;
END;
/

-- Auto-generate booking_seat_id
CREATE OR REPLACE TRIGGER trg_booking_seats_auto_id
BEFORE INSERT ON booking_seats
FOR EACH ROW
BEGIN
    IF :NEW.booking_seat_id IS NULL THEN
        SELECT seq_booking_seat_id.NEXTVAL INTO :NEW.booking_seat_id FROM dual;
    END IF;
END;
/

-- Auto-generate payment_id
CREATE OR REPLACE TRIGGER trg_payments_auto_id
BEFORE INSERT ON payments
FOR EACH ROW
BEGIN
    IF :NEW.payment_id IS NULL THEN
        SELECT seq_payment_id.NEXTVAL INTO :NEW.payment_id FROM dual;
    END IF;
END;
/

-- Auto-generate ticket_id
CREATE OR REPLACE TRIGGER trg_tickets_auto_id
BEFORE INSERT ON tickets
FOR EACH ROW
BEGIN
    IF :NEW.ticket_id IS NULL THEN
        SELECT seq_ticket_id.NEXTVAL INTO :NEW.ticket_id FROM dual;
    END IF;
END;
/

-- Auto-generate approval_id
CREATE OR REPLACE TRIGGER trg_approvals_auto_id
BEFORE INSERT ON approvals
FOR EACH ROW
BEGIN
    IF :NEW.approval_id IS NULL THEN
        SELECT seq_approval_id.NEXTVAL INTO :NEW.approval_id FROM dual;
    END IF;
END;
/

-- Auto-generate audit_log_id
CREATE OR REPLACE TRIGGER trg_audit_log_auto_id
BEFORE INSERT ON audit_log
FOR EACH ROW
BEGIN
    IF :NEW.log_id IS NULL THEN
        SELECT seq_audit_log_id.NEXTVAL INTO :NEW.log_id FROM dual;
    END IF;
END;
/

-- ============================================================================
-- SECTION 3: BUSINESS LOGIC TRIGGERS
-- ============================================================================

-- Update available seats when show is created
CREATE OR REPLACE TRIGGER trg_shows_init_available_seats
BEFORE INSERT ON shows
FOR EACH ROW
BEGIN
    SELECT COUNT(*) INTO :NEW.available_seats
    FROM seats
    WHERE hall_id = :NEW.hall_id;
END;
/

-- Update show booked seats when booking is confirmed
CREATE OR REPLACE TRIGGER trg_booking_update_show_seats
AFTER UPDATE OF booking_status ON bookings
FOR EACH ROW
WHEN (NEW.booking_status = 'Confirmed' AND OLD.booking_status <> 'Confirmed')
BEGIN
    UPDATE shows
    SET booked_seats = booked_seats + :NEW.number_of_seats,
        available_seats = available_seats - :NEW.number_of_seats
    WHERE show_id = :NEW.show_id;
END;
/

-- Revert show booked seats when booking is cancelled
CREATE OR REPLACE TRIGGER trg_booking_revert_show_seats
AFTER UPDATE OF booking_status ON bookings
FOR EACH ROW
WHEN (NEW.booking_status = 'Cancelled' AND OLD.booking_status <> 'Cancelled')
BEGIN
    UPDATE shows
    SET booked_seats = booked_seats - :OLD.number_of_seats,
        available_seats = available_seats + :OLD.number_of_seats
    WHERE show_id = :NEW.show_id;
END;
/

-- ============================================================================
-- SECTION 4: AUDIT TRIGGERS
-- ============================================================================

-- Audit trigger for users table
CREATE OR REPLACE TRIGGER trg_audit_users
AFTER INSERT OR UPDATE OR DELETE ON users
FOR EACH ROW
DECLARE
    v_operation VARCHAR2(10);
    v_old_values CLOB;
    v_new_values CLOB;
BEGIN
    IF INSERTING THEN
        v_operation := 'INSERT';
        v_new_values := 'username=' || :NEW.username || ', email=' || :NEW.email || ', role=' || :NEW.user_role;
    ELSIF UPDATING THEN
        v_operation := 'UPDATE';
        v_old_values := 'email=' || :OLD.email || ', role=' || :OLD.user_role;
        v_new_values := 'email=' || :NEW.email || ', role=' || :NEW.user_role;
    ELSIF DELETING THEN
        v_operation := 'DELETE';
        v_old_values := 'username=' || :OLD.username || ', email=' || :OLD.email;
    END IF;

    INSERT INTO audit_log (log_id, user_id, table_name, operation, old_values, new_values)
    VALUES (seq_audit_log_id.NEXTVAL, :NEW.user_id, 'users', v_operation, v_old_values, v_new_values);
END;
/

-- Audit trigger for bookings table
CREATE OR REPLACE TRIGGER trg_audit_bookings
AFTER INSERT OR UPDATE OR DELETE ON bookings
FOR EACH ROW
DECLARE
    v_operation VARCHAR2(10);
    v_old_values CLOB;
    v_new_values CLOB;
BEGIN
    IF INSERTING THEN
        v_operation := 'INSERT';
        v_new_values := 'booking_id=' || :NEW.booking_id || ', seats=' || :NEW.number_of_seats || ', status=' || :NEW.booking_status;
    ELSIF UPDATING THEN
        v_operation := 'UPDATE';
        v_old_values := 'status=' || :OLD.booking_status || ', amount=' || :OLD.total_amount;
        v_new_values := 'status=' || :NEW.booking_status || ', amount=' || :NEW.total_amount;
    ELSIF DELETING THEN
        v_operation := 'DELETE';
        v_old_values := 'booking_id=' || :OLD.booking_id || ', status=' || :OLD.booking_status;
    END IF;

    INSERT INTO audit_log (log_id, user_id, table_name, operation, old_values, new_values)
    VALUES (seq_audit_log_id.NEXTVAL, :NEW.user_id, 'bookings', v_operation, v_old_values, v_new_values);
END;
/

-- Audit trigger for payments table
CREATE OR REPLACE TRIGGER trg_audit_payments
AFTER INSERT OR UPDATE OR DELETE ON payments
FOR EACH ROW
DECLARE
    v_operation VARCHAR2(10);
    v_old_values CLOB;
    v_new_values CLOB;
BEGIN
    IF INSERTING THEN
        v_operation := 'INSERT';
        v_new_values := 'amount=' || :NEW.amount || ', method=' || :NEW.payment_method || ', status=' || :NEW.payment_status;
    ELSIF UPDATING THEN
        v_operation := 'UPDATE';
        v_old_values := 'status=' || :OLD.payment_status;
        v_new_values := 'status=' || :NEW.payment_status || ', date=' || TO_CHAR(:NEW.payment_date, 'YYYY-MM-DD HH24:MI:SS');
    ELSIF DELETING THEN
        v_operation := 'DELETE';
        v_old_values := 'payment_id=' || :OLD.payment_id || ', amount=' || :OLD.amount;
    END IF;

    INSERT INTO audit_log (log_id, table_name, operation, old_values, new_values)
    VALUES (seq_audit_log_id.NEXTVAL, 'payments', v_operation, v_old_values, v_new_values);
END;
/

-- ============================================================================
-- SECTION 5: VALIDATION TRIGGERS
-- ============================================================================

-- Validate show time format
CREATE OR REPLACE TRIGGER trg_validate_show_time
BEFORE INSERT OR UPDATE ON shows
FOR EACH ROW
BEGIN
    IF NOT REGEXP_LIKE(:NEW.show_time, '^\d{2}:\d{2}$') THEN
        RAISE_APPLICATION_ERROR(-20001, 'Invalid show_time format. Use HH:MM');
    END IF;
END;
/

-- Validate movie duration
CREATE OR REPLACE TRIGGER trg_validate_movie_duration
BEFORE INSERT OR UPDATE ON movies
FOR EACH ROW
BEGIN
    IF :NEW.duration_minutes IS NOT NULL AND :NEW.duration_minutes < 30 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Movie duration must be at least 30 minutes');
    END IF;
END;
/

-- Validate seat class
CREATE OR REPLACE TRIGGER trg_validate_seat_class
BEFORE INSERT OR UPDATE ON seats
FOR EACH ROW
BEGIN
    IF :NEW.seat_class NOT IN ('Economy', 'Silver', 'Gold', 'Platinum') THEN
        RAISE_APPLICATION_ERROR(-20003, 'Invalid seat class');
    END IF;
END;
/

-- ============================================================================
-- SECTION 6: SUMMARY
-- ============================================================================

COMMIT;

PROMPT ============================================================================
PROMPT Triggers Creation Complete!
PROMPT ============================================================================
PROMPT Total Triggers Created: 30+
PROMPT Trigger Categories:
PROMPT - Timestamp Triggers: 9
PROMPT - Auto-ID Triggers: 12
PROMPT - Business Logic Triggers: 3
PROMPT - Audit Triggers: 3
PROMPT - Validation Triggers: 3
PROMPT ============================================================================

-- End of 03-create-triggers.sql
