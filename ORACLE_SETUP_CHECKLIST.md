# Oracle 26ai Docker Setup - Complete Checklist

**Status**: Step-by-step guide for complete Oracle 26ai setup with Kumari Cinemas system  
**Duration**: 45-60 minutes  
**Difficulty**: Intermediate  

---

## PHASE 1: PRE-SETUP (5 minutes)

### ✓ System Requirements Check

- [ ] **OS Compatibility**
  - [ ] Windows 10/11 (with WSL2 or native Docker)
  - [ ] macOS 10.15+ (Intel or Apple Silicon)
  - [ ] Linux (Ubuntu 20.04+, CentOS 7+, or Debian)
  
- [ ] **Docker Installation**
  - [ ] Docker Desktop installed (latest version)
  - [ ] Docker daemon running (`docker ps` returns output)
  - [ ] Minimum 4GB RAM allocated to Docker
  - [ ] Minimum 50GB free disk space
  
- [ ] **Oracle Account**
  - [ ] Oracle account created (free at oracle.com)
  - [ ] Container Registry credentials ready
  - [ ] Can login to: container-registry.oracle.com

- [ ] **Development Tools**
  - [ ] .NET 8.0 SDK installed
  - [ ] Git installed
  - [ ] Text editor/IDE ready (VS Code or Visual Studio 2022)
  - [ ] Terminal/Command prompt available

### Commands to Verify Prerequisites

```bash
# Check Docker version
docker --version
# Expected: Docker version 20.10.0+

# Check Docker available RAM
docker info | grep Memory
# Expected: >=4GB

# Check .NET version
dotnet --version
# Expected: 8.0.0+

# Check disk space
df -h /
# Expected: >=50GB free
```

---

## PHASE 2: DOCKER SETUP (10 minutes)

### ✓ Create Project Directory Structure

```bash
# Navigate to your projects folder
cd ~/projects  # or C:\projects on Windows

# Create Kumari Cinemas project
mkdir kumari-cinemas
cd kumari-cinemas

# Create necessary directories
mkdir oracle-setup
mkdir oracle-setup/scripts
mkdir oracle-setup/backups
mkdir oracle-setup/logs
mkdir oracle-setup/config
mkdir oracle-setup/oracle-data
mkdir oracle-setup/oracle-scripts
mkdir oracle-setup/oracle-backups
mkdir oracle-setup/oracle-fra

# Directory structure
# kumari-cinemas/
# ├── oracle-setup/
# │   ├── scripts/          (SQL initialization scripts)
# │   ├── backups/          (Database backups)
# │   ├── logs/             (Oracle logs)
# │   ├── config/           (Configuration files)
# │   ├── oracle-data/      (Database files)
# │   └── docker-compose.yml
# └── .NET-Project/
```

### ✓ Copy Files to Correct Locations

- [ ] Copy `docker-compose.yml` → `oracle-setup/`
- [ ] Copy SQL scripts to `oracle-setup/scripts/`:
  - [ ] `01-create-schema.sql`
  - [ ] `02-create-indexes.sql`
  - [ ] `03-create-triggers.sql`
  - [ ] `04-create-procedures.sql`
  - [ ] `05-sample-data.sql`

### ✓ Docker Registry Login

```bash
# Navigate to oracle-setup
cd oracle-setup

# Login to Oracle Container Registry
docker login container-registry.oracle.com

# When prompted:
# Username: your-oracle-email@example.com
# Password: your-oracle-password
# Login Successful!
```

**Verification:**
```bash
# Check if login successful
ls ~/.docker/config.json
# Should contain "container-registry.oracle.com"
```

---

## PHASE 3: PULL ORACLE IMAGE (15 minutes)

### ✓ Download Oracle 26ai Image

```bash
# Pull the Oracle 26ai Enterprise image (large file ~7.5GB)
docker pull container-registry.oracle.com/database/enterprise:26.0.0.0

# This will take 10-15 minutes depending on internet speed
# You'll see:
# Downloading ghcr.io/... [=====>] ...
# Downloaded successfully!
```

### ✓ Verify Image Download

```bash
# List downloaded images
docker images | grep oracle

# Expected output:
# REPOSITORY                              TAG       IMAGE ID     SIZE
# container-registry.oracle.com/database/enterprise  26.0.0.0  xxxxxxxx  7.5GB
```

---

## PHASE 4: START ORACLE CONTAINER (20 minutes)

### ✓ Start the Database

```bash
# Navigate to oracle-setup directory
cd oracle-setup

# Start Oracle container with docker-compose
docker-compose up -d

# Expected output:
# Creating network "kumari-net" with driver "bridge"
# Creating kumari-oracle-26ai ... done
```

### ✓ Monitor Startup Progress

```bash
# Watch the initialization logs (this takes 15-20 minutes)
docker-compose logs -f oracle-26ai

# Look for messages indicating progress:
# - "Creating initial PASSWORD file"
# - "Creating spfile from pfile"
# - "Opening the database"
# - "DATABASE IS READY TO USE!!!" ← SUCCESS INDICATOR
```

### ✓ Verify Container Health

```bash
# Check container status (should be "healthy")
docker-compose ps

# Expected output:
# NAME                 STATUS                    PORTS
# kumari-oracle-26ai   Up 5 minutes (healthy)   1521->1521, 5500->5500

# If NOT healthy, wait 5 more minutes and retry
# The initialization takes a while!
```

**⚠️ IMPORTANT: Wait for "DATABASE IS READY TO USE!!!" message before proceeding**

---

## PHASE 5: DATABASE INITIALIZATION (10 minutes)

### ✓ Access Oracle SQLPlus

```bash
# Option 1: Using docker exec
docker exec -it kumari-oracle-26ai sqlplus sys/YourSecurePassword123!@localhost:1521/ORCL as sysdba

# Option 2: Using docker-compose
docker-compose exec oracle-26ai sqlplus sys/YourSecurePassword123!@localhost:1521/ORCL as sysdba

# If successful, you'll see:
# SQL*Plus: Release 26.0.0.0.0 - Production on ...
# SQL>
```

### ✓ Create Kumari Database User

```sql
-- In SQLPlus, run these commands:

-- Create tablespace
CREATE TABLESPACE kumari_ts 
  DATAFILE '/opt/oracle/oradata/kumari_data.dbf' 
  SIZE 500M 
  AUTOEXTEND ON 
  NEXT 50M;

-- Create user
CREATE USER kumari_user 
  IDENTIFIED BY Kumari@Cinema123
  DEFAULT TABLESPACE kumari_ts 
  QUOTA UNLIMITED ON kumari_ts;

-- Grant privileges
GRANT CONNECT, RESOURCE, CREATE SESSION TO kumari_user;
GRANT CREATE TABLE, CREATE VIEW, CREATE PROCEDURE, CREATE TRIGGER TO kumari_user;
GRANT SELECT ANY TABLE, INSERT ANY TABLE, UPDATE ANY TABLE, DELETE ANY TABLE TO kumari_user;

-- Verify user creation
SELECT username, default_tablespace FROM dba_users WHERE username='KUMARI_USER';

-- Should output:
-- USERNAME      DEFAULT_TABLESPACE
-- KUMARI_USER   KUMARI_TS

-- Exit SQLPlus
EXIT;
```

---

## PHASE 6: LOAD DATABASE SCHEMA (5 minutes)

### ✓ Run SQL Initialization Scripts

```bash
# Connect as kumari_user and run schema creation
docker exec -it kumari-oracle-26ai sqlplus kumari_user/Kumari@Cinema123@ORCL << EOF

@/docker-entrypoint-initdb.d/sql/01-create-schema.sql
@/docker-entrypoint-initdb.d/sql/02-create-indexes.sql
@/docker-entrypoint-initdb.d/sql/03-create-triggers.sql
@/docker-entrypoint-initdb.d/sql/04-create-procedures.sql
@/docker-entrypoint-initdb.d/sql/05-sample-data.sql

EXIT;
EOF
```

### ✓ Verify Schema Creation

```bash
# Connect to database
docker exec -it kumari-oracle-26ai sqlplus kumari_user/Kumari@Cinema123@ORCL

# Run verification queries
SELECT table_name FROM user_tables;
-- Should show: users, theaters, halls, seats, movies, shows, bookings, etc.

SELECT sequence_name FROM user_sequences;
-- Should show: seq_user_id, seq_theater_id, etc.

SELECT object_name FROM user_objects WHERE object_type='PROCEDURE';
-- Should show all stored procedures

EXIT;
```

---

## PHASE 7: .NET PROJECT SETUP (10 minutes)

### ✓ Create .NET Project

```bash
# Navigate to kumari-cinemas directory
cd ..

# Create new ASP.NET Core project
dotnet new web -n KumariCinemas

# Navigate to project
cd KumariCinemas

# Add NuGet packages
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Oracle.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.Extensions.Configuration.Json
```

### ✓ Configure appsettings.json

```json
{
  "ConnectionStrings": {
    "OracleConnection": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=kumari_user;Password=Kumari@Cinema123;Integrated Security=false;Pooling=true;Min Pool Size=5;Max Pool Size=20;"
  },
  "Database": {
    "Provider": "Oracle",
    "Version": "26ai",
    "CommandTimeout": 300
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Debug"
    }
  }
}
```

### ✓ Test Connection from .NET

```csharp
// Program.cs - Add DbContext
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseOracle(connectionString, oracleOptions =>
    {
        oracleOptions.UseOracleSQLCompatibility("23");
    })
);

// In a test endpoint:
app.MapGet("/health/db", async (CinemaDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? "✓ Database Connected" : "✗ Connection Failed";
});
```

### ✓ Test .NET Application

```bash
# Build project
dotnet build

# Run application
dotnet run

# Access health endpoint
curl http://localhost:5000/health/db

# Expected output: ✓ Database Connected
```

---

## PHASE 8: BACKUP & MONITORING (5 minutes)

### ✓ Create Database Backup

```bash
# Create backup directory
mkdir -p oracle-setup/backups

# Backup database (using RMAN)
docker exec kumari-oracle-26ai rman target=/ <<EOF
BACKUP DATABASE PLUS ARCHIVELOG;
EXIT;
EOF
```

### ✓ Enable Monitoring

```bash
# View Oracle alert logs
docker logs -f kumari-oracle-26ai

# Monitor container resources
docker stats kumari-oracle-26ai

# Expected output:
# CONTAINER         CPU %    MEM USAGE / LIMIT
# kumari-oracle-26ai  5.2%   2.8G / 8G
```

---

## PHASE 9: VERIFY COMPLETE SETUP

### ✓ Final Verification Checklist

```bash
# 1. Check all containers running
docker-compose ps
# All containers should show: Up X minutes (healthy)

# 2. Check database accessibility
docker exec -it kumari-oracle-26ai sqlplus kumari_user/Kumari@Cinema123@ORCL -c "SELECT COUNT(*) FROM users;"
# Should return: COUNT(*)
#               --------
#                     8 (or number of sample users)

# 3. Check .NET application connectivity
dotnet run --project KumariCinemas
# No connection errors in logs

# 4. Test API endpoint
curl http://localhost:5000/api/users
# Should return user data in JSON format

# 5. Check disk usage
df -h
# Oracle data should be ~2-3GB
```

---

## PHASE 10: TROUBLESHOOTING

### ✓ Common Issues & Solutions

#### Issue: Container won't start
```bash
# Solution: Check Docker daemon and logs
docker info
docker-compose logs oracle-26ai

# If image pulling failed:
docker pull container-registry.oracle.com/database/enterprise:26.0.0.0

# Restart container:
docker-compose restart oracle-26ai
```

#### Issue: Database not healthy after 30 minutes
```bash
# Solution: Increase wait time and check logs
docker-compose logs --tail=100 oracle-26ai

# Look for errors related to disk space, memory, or password
# Common fix: Increase Docker memory allocation in Docker Desktop
```

#### Issue: .NET connection fails
```bash
# Solution: Verify connection string
# Check appsettings.json has correct:
# - Host: localhost
# - Port: 1521
# - Service Name: ORCL
# - Username: kumari_user
# - Password: Kumari@Cinema123

# Test connection using docker
docker exec -it kumari-oracle-26ai sqlplus kumari_user/Kumari@Cinema123@ORCL -c "SELECT 1 FROM dual;"
```

#### Issue: Port 1521 already in use
```bash
# Solution: Kill process on port 1521
# On Linux/Mac:
lsof -i :1521
kill -9 <PID>

# On Windows:
netstat -ano | findstr :1521
taskkill /PID <PID> /F

# Or change port in docker-compose.yml:
ports:
  - "1522:1521"  # Use 1522 instead
```

---

## PHASE 11: NEXT STEPS

### ✓ After Successful Setup

1. **Explore Database**
   - [ ] Connect via SQL Developer (free download)
   - [ ] Browse tables and data
   - [ ] Run reports

2. **Develop .NET Application**
   - [ ] Create controllers
   - [ ] Implement repositories
   - [ ] Add business logic

3. **Setup Version Control**
   - [ ] Initialize Git
   - [ ] Create GitHub repository
   - [ ] Commit initial code

4. **Configure Authentication**
   - [ ] Implement user login
   - [ ] Setup JWT tokens
   - [ ] Add role-based access

5. **Deploy to Production**
   - [ ] Setup CI/CD pipeline
   - [ ] Configure cloud hosting
   - [ ] Enable monitoring and logging

---

## SUMMARY

| Phase | Task | Duration | Status |
|-------|------|----------|--------|
| 1 | Pre-Setup & Prerequisites | 5 min | ▢ |
| 2 | Docker Setup | 10 min | ▢ |
| 3 | Pull Oracle Image | 15 min | ▢ |
| 4 | Start Container | 20 min | ▢ |
| 5 | Database Init | 10 min | ▢ |
| 6 | Load Schema | 5 min | ▢ |
| 7 | .NET Setup | 10 min | ▢ |
| 8 | Backup & Monitor | 5 min | ▢ |
| 9 | Verify Setup | 5 min | ▢ |
| 10 | Troubleshoot (if needed) | - | ▢ |
| **Total** | | **45-60 min** | ▢ |

---

## IMPORTANT CONTACTS & RESOURCES

- **Oracle Support**: support.oracle.com
- **Docker Documentation**: docs.docker.com
- **.NET Documentation**: learn.microsoft.com/dotnet
- **GitHub Issues**: Open issue in repository
- **Oracle Community**: community.oracle.com

---

**Setup Status**: Ready to begin ✓

Good luck with your Kumari Cinemas project!
