# Oracle Database 26ai Setup Guide for Kumari Cinemas

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Oracle 26ai Overview](#oracle-26ai-overview)
3. [Docker Setup (Recommended)](#docker-setup)
4. [Database Initialization](#database-initialization)
5. [Connection Configuration](#connection-configuration)
6. [Verification Steps](#verification-steps)
7. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### System Requirements
- **Docker Desktop** (Latest version)
  - [Download Docker Desktop](https://www.docker.com/products/docker-desktop)
  - Minimum 4GB RAM allocated to Docker
  - 50GB free disk space (for Oracle container and data)
  - Virtualization enabled in BIOS (VT-x/AMD-v)

- **Development Tools**
  - .NET 8.0 SDK or higher
  - Visual Studio 2022 / VS Code with C# extension
  - SQL Developer or DBeaver for database management
  - Git

### Required Ports
- **1521** - Oracle Listener (Database connections)
- **5500** - Oracle Enterprise Manager Express (optional, for admin console)

---

## Oracle 26ai Overview

### What is Oracle 26ai?

Oracle Database 26ai is the latest generation of Oracle Database featuring:

- **AI Integration**: Built-in AI capabilities using vector databases and machine learning
- **Performance**: Latest optimization engine for faster queries
- **Security**: Enhanced encryption and authentication mechanisms
- **Cloud-Native**: Better container support and Kubernetes integration
- **JSON Support**: Native JSON data type and operations
- **AutoML**: Automatic machine learning capabilities

### Edition Information

| Feature | Free Edition | Standard Edition | Enterprise Edition |
|---------|-------------|-----------------|-------------------|
| Pricing | Free | Paid (Per Core) | Paid (Per Core) |
| AI Features | Limited | Full | Full |
| Containers | Docker Support | Docker Support | Full Support |
| Max Datafiles | 32GB | Unlimited | Unlimited |
| Database Links | Limited | Full | Full |
| Recommended For | Dev/Test/Learning | Production (Small-Medium) | Production (Enterprise) |

---

## Docker Setup

### Step 1: Install Docker Desktop

#### Windows/Mac
```bash
# Download from: https://www.docker.com/products/docker-desktop
# Run installer and follow setup wizard
# Verify installation
docker --version
```

#### Linux (Ubuntu/Debian)
```bash
# Install Docker
sudo apt-get update
sudo apt-get install docker.io docker-compose

# Add current user to docker group (optional, avoid sudo)
sudo usermod -aG docker $USER
newgrp docker

# Verify
docker --version
docker-compose --version
```

### Step 2: Create Docker Compose File

Create `docker-compose.yml` in your project root:

```yaml
version: '3.8'

services:
  oracle-26ai:
    image: container-registry.oracle.com/database/enterprise:26.0.0.0
    container_name: kumari-oracle-26ai
    environment:
      ORACLE_SID: ORCL
      ORACLE_PDB: ORCLAPDB
      ORACLE_ADMIN_PASSWORD: YourSecurePassword123!
      ORACLE_CHARACTERSET: AL32UTF8
      ORACLE_EDITION: enterprise
    ports:
      - "1521:1521"
      - "5500:5500"
    volumes:
      - oracle-data:/opt/oracle/oradata
      - oracle-scripts:/docker-entrypoint-initdb.d
      - ./scripts:/docker-entrypoint-initdb.d/sql
    healthcheck:
      test: ["CMD", "sqlplus", "-h"]
      interval: 10s
      timeout: 5s
      retries: 5
    networks:
      - kumari-net
    restart: unless-stopped

  sqlplus-client:
    image: container-registry.oracle.com/database/sqlcl:latest
    container_name: kumari-sqlplus
    depends_on:
      - oracle-26ai
    networks:
      - kumari-net
    profiles:
      - tools

volumes:
  oracle-data:
    driver: local
  oracle-scripts:
    driver: local

networks:
  kumari-net:
    driver: bridge
```

### Step 3: Create Directory Structure

```bash
# Create project directories
mkdir -p oracle_setup/scripts
mkdir -p oracle_setup/backups
mkdir -p oracle_setup/logs
mkdir -p oracle_setup/config

# Create docker-compose.yml in oracle_setup directory
cd oracle_setup
```

### Step 4: Pull Oracle 26ai Image

```bash
# Login to Oracle Container Registry (requires Oracle account)
docker login container-registry.oracle.com

# Username: your_oracle_account@example.com
# Password: your_oracle_password

# Pull the latest Oracle Database 26ai image
docker pull container-registry.oracle.com/database/enterprise:26.0.0.0

# Verify the image
docker images | grep oracle
```

### Step 5: Start Oracle Container

```bash
# Navigate to oracle_setup directory
cd oracle_setup

# Start the container
docker-compose up -d

# Check if container is running
docker-compose ps

# View initialization logs (wait for "Database Opened Successfully")
docker-compose logs -f oracle-26ai
```

### Step 6: Wait for Initialization

The Oracle initialization process typically takes 5-15 minutes:

```bash
# Monitor the startup
docker-compose logs --tail=50 -f oracle-26ai

# Once you see: "DATABASE IS READY TO USE !!!" - you're good to go
```

---

## Database Initialization

### Step 1: Access SQLPlus Container

```bash
# Method 1: Using docker exec
docker exec -it kumari-oracle-26ai sqlplus sys/YourSecurePassword123!@localhost:1521/ORCL as sysdba

# Method 2: Using docker-compose
docker-compose exec oracle-26ai sqlplus sys/YourSecurePassword123!@localhost:1521/ORCL as sysdba
```

### Step 2: Create Database User for Cinema Application

```sql
-- Connect as SYSDBA first
CONNECT sys/YourSecurePassword123!@ORCL as sysdba;

-- Create tablespace for cinema data
CREATE TABLESPACE kumari_ts 
  DATAFILE '/opt/oracle/oradata/kumari_data.dbf' 
  SIZE 500M 
  AUTOEXTEND ON 
  NEXT 50M;

-- Create cinema application user
CREATE USER kumari_user 
  IDENTIFIED BY Kumari@Cinema123
  DEFAULT TABLESPACE kumari_ts 
  QUOTA UNLIMITED ON kumari_ts;

-- Grant necessary privileges
GRANT CONNECT, RESOURCE, CREATE SESSION TO kumari_user;
GRANT CREATE TABLE, CREATE VIEW, CREATE PROCEDURE, CREATE TRIGGER TO kumari_user;
GRANT SELECT ANY TABLE TO kumari_user;
GRANT INSERT ANY TABLE TO kumari_user;
GRANT UPDATE ANY TABLE TO kumari_user;
GRANT DELETE ANY TABLE TO kumari_user;

-- Grant tablespace quota
ALTER USER kumari_user QUOTA UNLIMITED ON kumari_ts;

-- Verify user creation
SELECT username, default_tablespace FROM dba_users WHERE username='KUMARI_USER';

-- Exit SQLPlus
EXIT;
```

### Step 3: Load Initial Data

```bash
# Copy SQL initialization scripts to container
docker cp ./scripts/01-create-schema.sql kumari-oracle-26ai:/docker-entrypoint-initdb.d/

# Connect and run scripts
docker exec -it kumari-oracle-26ai sqlplus kumari_user/Kumari@Cinema123@ORCL @/docker-entrypoint-initdb.d/01-create-schema.sql
```

---

## Connection Configuration

### Step 1: Create tnsnames.ora

Create file: `oracle_setup/config/tnsnames.ora`

```ora
ORCL =
  (DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = ORCL)
    )
  )

ORCLAPDB =
  (DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = ORCLAPDB)
    )
  )
```

### Step 2: .NET Connection String

Update your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=kumari_user;Password=Kumari@Cinema123;Integrated Security=false;"
  },
  "Database": {
    "Provider": "Oracle",
    "Version": "26ai",
    "AutoMigrate": true
  }
}
```

### Step 3: Entity Framework Configuration

Update your `DbContext`:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        var connectionString = _configuration.GetConnectionString("OracleConnection");
        optionsBuilder.UseOracle(connectionString, options =>
        {
            options.UseOracleSQLCompatibility("23");
            options.UseRowNumberForPaging();
        });
    }
}
```

---

## Verification Steps

### Step 1: Test Docker Container

```bash
# Check container status
docker ps | grep oracle

# Expected output: container should be "Up X minutes"

# View container logs
docker logs kumari-oracle-26ai

# Check container resource usage
docker stats kumari-oracle-26ai
```

### Step 2: Verify Database Connection

```bash
# Using sqlplus from container
docker exec -it kumari-oracle-26ai sqlplus kumari_user/Kumari@Cinema123@ORCL

# Once connected, run:
SELECT * FROM all_users WHERE username = 'KUMARI_USER';
SELECT TABLE_NAME FROM user_tables;
DESCRIBE USER_OBJECTS;

# Exit
EXIT;
```

### Step 3: Verify Database Objects

```sql
-- List all tables
SELECT table_name FROM user_tables;

-- List all sequences
SELECT sequence_name FROM user_sequences;

-- List all procedures/functions
SELECT object_name, object_type FROM user_objects WHERE object_type IN ('PROCEDURE', 'FUNCTION');

-- Check table row counts
SELECT table_name, num_rows FROM user_tables;
```

### Step 4: Test Network Connectivity

```bash
# From host machine
sqlplus kumari_user/Kumari@Cinema123@localhost:1521/ORCL

# Or using SQL Developer
# Host: localhost
# Port: 1521
# SID: ORCL
# Service Name: ORCL
# Username: kumari_user
# Password: Kumari@Cinema123
```

---

## Backup and Restoration

### Step 1: Create Backup Directory

```bash
# Create backup directory on host
mkdir -p oracle_setup/backups

# Create backup inside container
docker exec kumari-oracle-26ai mkdir -p /opt/oracle/backups
```

### Step 2: Full Database Backup

```bash
# Using RMAN (Recovery Manager)
docker exec -it kumari-oracle-26ai rman target=/ <<EOF
BACKUP DATABASE PLUS ARCHIVELOG;
BACKUP CURRENT CONTROLFILE;
EXIT;
EOF

# Copy backup to host
docker cp kumari-oracle-26ai:/opt/oracle/backups ./oracle_setup/backups/
```

### Step 3: Schema Export (Expdp)

```bash
# Create export directory
docker exec kumari-oracle-26ai mkdir -p /opt/oracle/export

# Export schema
docker exec kumari-oracle-26ai expdp kumari_user/Kumari@Cinema123 \
  DIRECTORY=export_dir \
  DUMPFILE=kumari_schema.dmp \
  LOGFILE=kumari_export.log

# Copy to host
docker cp kumari-oracle-26ai:/opt/oracle/export ./oracle_setup/backups/
```

### Step 4: Restore Schema (Impdp)

```bash
# Copy backup to container
docker cp ./oracle_setup/backups/kumari_schema.dmp kumari-oracle-26ai:/opt/oracle/import/

# Import schema
docker exec kumari-oracle-26ai impdp kumari_user/Kumari@Cinema123 \
  DIRECTORY=import_dir \
  DUMPFILE=kumari_schema.dmp \
  LOGFILE=kumari_import.log
```

---

## Troubleshooting

### Issue: Container fails to start

```bash
# Check Docker daemon
docker info

# View container logs
docker logs kumari-oracle-26ai

# Restart container
docker-compose restart oracle-26ai

# Rebuild container
docker-compose down -v
docker-compose up -d --build
```

### Issue: Connection refused on port 1521

```bash
# Check if port is already in use
netstat -an | grep 1521  # Linux/Mac
netstat -ano | findstr :1521  # Windows

# Kill existing process and restart
docker-compose down
docker-compose up -d
```

### Issue: Out of memory errors

```bash
# Increase Docker memory allocation
# Docker Desktop > Preferences > Resources > Memory: Set to 8GB+

# Or in docker-compose.yml:
services:
  oracle-26ai:
    mem_limit: 8g
    memswap_limit: 8g
```

### Issue: Slow database queries

```bash
# Enable SQL tracing
docker exec -it kumari-oracle-26ai sqlplus kumari_user/Kumari@Cinema123@ORCL

-- In SQLPlus:
ALTER SESSION SET sql_trace = TRUE;
-- Run your queries
ALTER SESSION SET sql_trace = FALSE;

-- View trace files
SELECT VALUE FROM v$parameter WHERE name = 'user_dump_dest';
```

### Issue: Insufficient disk space

```bash
# Check Docker disk usage
docker system df

# Clean up unused images and containers
docker system prune -a

# Increase volume size
# Stop container and recreate volume:
docker-compose down -v
docker-compose up -d

# Or mount external volume:
volumes:
  - /large-disk-path:/opt/oracle/oradata
```

---

## Performance Tuning

### Step 1: Enable Automatic Memory Management

```sql
-- Connect as SYSDBA
CONNECT sys/YourSecurePassword123!@ORCL as sysdba;

-- Set SGA (System Global Area) size
ALTER SYSTEM SET sga_target=2G SCOPE=BOTH;

-- Set PGA (Program Global Area) size
ALTER SYSTEM SET pga_aggregate_target=1G SCOPE=BOTH;

-- Verify settings
SHOW PARAMETER sga_target;
SHOW PARAMETER pga_aggregate_target;
```

### Step 2: Create Indexes for Cinema Tables

```sql
-- Covered in 02-create-indexes.sql
-- Indexes on frequently queried columns
CREATE INDEX idx_user_email ON users(email);
CREATE INDEX idx_booking_user_id ON bookings(user_id);
CREATE INDEX idx_show_movie_id ON shows(movie_id);
```

### Step 3: Enable Query Result Cache

```sql
-- Connect as SYSDBA
ALTER SYSTEM SET result_cache_mode=FORCE SCOPE=BOTH;
ALTER SYSTEM SET result_cache_max_size=256M SCOPE=BOTH;

-- For specific queries
SELECT /*+ result_cache */ * FROM users;
```

---

## Security Best Practices

### Step 1: Change Default Passwords

```sql
-- Connect as SYSDBA
ALTER USER sys IDENTIFIED BY NewSecurePassword123!;
ALTER USER kumari_user IDENTIFIED BY NewCinemaPassword456!;
```

### Step 2: Enable Audit Logging

```sql
-- Audit all DDL statements
AUDIT CREATE TABLE, DROP TABLE, ALTER TABLE;

-- Audit all DML on sensitive tables
AUDIT INSERT, UPDATE, DELETE ON kumari_user.users;

-- View audit trail
SELECT * FROM dba_audit_trail;
```

### Step 3: Setup Database Encryption

```sql
-- Enable Transparent Data Encryption (TDE)
ADMINISTER KEY MANAGEMENT CREATE KEYSTORE '/opt/oracle/tde' IDENTIFIED BY TDEPassword123!;

ALTER SYSTEM SET db_recovery_file_dest='/opt/oracle/tde' SCOPE=BOTH;

-- Create encryption key
ADMINISTER KEY MANAGEMENT CREATE KEY IDENTIFIED BY TDEPassword123! WITH ALGORITHM AES256;
```

---

## Next Steps

1. **Run initialization scripts** (See SQL files)
2. **Configure .NET application** (See connection configuration)
3. **Run Entity Framework migrations** (See .NET setup guide)
4. **Setup monitoring** (Enable Oracle metrics)
5. **Configure backups** (Automated daily backups)

---

## Support & Documentation

- [Oracle 26ai Official Documentation](https://docs.oracle.com/en/database/oracle/oracle-database/26/index.html)
- [Oracle Docker Images](https://github.com/oracle/docker-images)
- [Oracle SQL Language Reference](https://docs.oracle.com/en/database/oracle/oracle-database/26/sqlrf/)
- [Entity Framework Core Oracle Guide](https://learn.microsoft.com/en-us/ef/core/providers/oracle/)

---

## Version Information

- **Oracle Database Version**: 26ai
- **Docker Image**: container-registry.oracle.com/database/enterprise:26.0.0.0
- **.NET Framework**: .NET 8.0+
- **Entity Framework Core**: 8.0+
- **Date Created**: 2024
- **Last Updated**: March 2024

---

**Setup Status**: Ready for implementation ✓
