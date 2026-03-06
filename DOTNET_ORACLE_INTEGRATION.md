# .NET & Oracle 26ai Integration Guide

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [NuGet Package Installation](#nuget-package-installation)
3. [Connection Configuration](#connection-configuration)
4. [Entity Framework Core Setup](#entity-framework-core-setup)
5. [DbContext Implementation](#dbcontext-implementation)
6. [Migrations Guide](#migrations-guide)
7. [Data Access Layer](#data-access-layer)
8. [Code Examples](#code-examples)
9. [Performance Optimization](#performance-optimization)
10. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required Software
- **.NET 8.0 SDK** or higher
- **Oracle 26ai Database** (Docker container running)
- **Visual Studio 2022** or **VS Code**
- **SQL Developer** or **DBeaver** (optional, for database exploration)

### Verify .NET Installation
```bash
dotnet --version
# Output: 8.x.x or higher
```

---

## NuGet Package Installation

### Step 1: Install Core Entity Framework Package

```bash
# Navigate to your project directory
cd YourProject

# Install EF Core
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0

# Install design tools
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# Install Oracle Data Provider for .NET
dotnet add package Oracle.EntityFrameworkCore --version 8.0.0
```

### Step 2: Alternative NuGet Installation (Via Package Manager Console)

```powershell
# In Visual Studio Package Manager Console
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.0
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.0
Install-Package Oracle.EntityFrameworkCore -Version 8.0.0

# For Dapper (if using micro-ORM)
Install-Package Dapper -Version 2.0.0

# For connection pooling
Install-Package Microsoft.EntityFrameworkCore.Proxies -Version 8.0.0
```

### Step 3: Verify Installation

```bash
# List installed packages
dotnet package list
# or in Package Manager Console
Get-Package
```

---

## Connection Configuration

### Step 1: Update appsettings.json

```json
{
  "ConnectionStrings": {
    "OracleConnection": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=kumari_user;Password=Kumari@Cinema123;Integrated Security=false;Pooling=true;Min Pool Size=5;Max Pool Size=20;Connection Lifetime=300;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Debug"
    }
  },
  "Database": {
    "Provider": "Oracle",
    "Version": "26ai",
    "CommandTimeout": 300,
    "LazyLoadingEnabled": true
  }
}
```

### Step 2: Connection String Breakdown

```
Host: localhost
Port: 1521
Service Name: ORCL
Username: kumari_user
Password: Kumari@Cinema123

Connection Pooling:
- Min Pool Size: 5 (minimum connections to maintain)
- Max Pool Size: 20 (maximum connections allowed)
- Connection Lifetime: 300 seconds (recycle after 5 minutes)
```

### Step 3: Secure Connection String (User Secrets)

For development, use user secrets to avoid committing passwords:

```bash
# Initialize user secrets
dotnet user-secrets init

# Set connection string
dotnet user-secrets set "ConnectionStrings:OracleConnection" "Data Source=...;User Id=kumari_user;Password=Kumari@Cinema123;"

# View all secrets
dotnet user-secrets list
```

---

## Entity Framework Core Setup

### Step 1: Create Data Models

Create models for each table:

```csharp
// Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KumariCinemas.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Column("password_hash")]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [Column("full_name")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Column("phone")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Column("user_role")]
        [StringLength(20)]
        public string UserRole { get; set; } = "customer";

        [Column("is_active")]
        public int IsActive { get; set; } = 1;

        [Column("is_verified")]
        public int IsVerified { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        // Navigation properties
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Approval> Approvals { get; set; } = new List<Approval>();
    }
}
```

### Step 2: Create Additional Models

```csharp
// Models/Theater.cs
[Table("theaters")]
public class Theater
{
    [Key]
    [Column("theater_id")]
    public int TheaterId { get; set; }

    [Required]
    [Column("theater_name")]
    [StringLength(100)]
    public string TheaterName { get; set; }

    [Column("city")]
    [StringLength(50)]
    public string City { get; set; }

    [Column("address")]
    [StringLength(200)]
    public string Address { get; set; }

    [Column("phone")]
    [StringLength(20)]
    public string Phone { get; set; }

    [Column("manager_id")]
    public int? ManagerId { get; set; }

    [ForeignKey(nameof(ManagerId))]
    public User Manager { get; set; }

    [Column("total_capacity")]
    public int? TotalCapacity { get; set; }

    [Column("is_active")]
    public int IsActive { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Hall> Halls { get; set; } = new List<Hall>();
}

// Models/Movie.cs
[Table("movies")]
public class Movie
{
    [Key]
    [Column("movie_id")]
    public int MovieId { get; set; }

    [Required]
    [Column("title")]
    [StringLength(200)]
    public string Title { get; set; }

    [Column("genre")]
    [StringLength(50)]
    public string Genre { get; set; }

    [Column("language")]
    [StringLength(30)]
    public string Language { get; set; }

    [Column("duration_minutes")]
    public int? DurationMinutes { get; set; }

    [Column("release_date")]
    public DateTime? ReleaseDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("rating")]
    [StringLength(10)]
    public string Rating { get; set; }

    [Column("director")]
    [StringLength(100)]
    public string Director { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("poster_url")]
    [StringLength(500)]
    public string PosterUrl { get; set; }

    [Column("is_active")]
    public int IsActive { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Show> Shows { get; set; } = new List<Show>();
}

// Models/Booking.cs
[Table("bookings")]
public class Booking
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Column("show_id")]
    [ForeignKey(nameof(Show))]
    public int ShowId { get; set; }

    [Column("booking_date")]
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    [Column("number_of_seats")]
    public int NumberOfSeats { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("booking_status")]
    [StringLength(20)]
    public string BookingStatus { get; set; } = "Pending";

    [Column("special_requests")]
    [StringLength(500)]
    public string SpecialRequests { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; }
    public Show Show { get; set; }
    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

// Models/Show.cs
[Table("shows")]
public class Show
{
    [Key]
    [Column("show_id")]
    public int ShowId { get; set; }

    [Column("movie_id")]
    [ForeignKey(nameof(Movie))]
    public int MovieId { get; set; }

    [Column("hall_id")]
    [ForeignKey(nameof(Hall))]
    public int HallId { get; set; }

    [Column("show_date")]
    public DateTime ShowDate { get; set; }

    [Column("show_time")]
    [StringLength(5)]
    public string ShowTime { get; set; }

    [Column("end_time")]
    [StringLength(5)]
    public string EndTime { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("available_seats")]
    public int? AvailableSeats { get; set; }

    [Column("booked_seats")]
    public int BookedSeats { get; set; } = 0;

    [Column("show_status")]
    [StringLength(20)]
    public string ShowStatus { get; set; } = "Scheduled";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Movie Movie { get; set; }
    public Hall Hall { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

// Models/Payment.cs
[Table("payments")]
public class Payment
{
    [Key]
    [Column("payment_id")]
    public int PaymentId { get; set; }

    [Column("booking_id")]
    [ForeignKey(nameof(Booking))]
    public int BookingId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("payment_method")]
    [StringLength(30)]
    public string PaymentMethod { get; set; }

    [Column("payment_status")]
    [StringLength(20)]
    public string PaymentStatus { get; set; } = "Pending";

    [Column("transaction_id")]
    [StringLength(100)]
    public string TransactionId { get; set; }

    [Column("payment_date")]
    public DateTime? PaymentDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Booking Booking { get; set; }
}

// Models/Ticket.cs
[Table("tickets")]
public class Ticket
{
    [Key]
    [Column("ticket_id")]
    public int TicketId { get; set; }

    [Column("booking_id")]
    [ForeignKey(nameof(Booking))]
    public int BookingId { get; set; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Column("seat_id")]
    [ForeignKey(nameof(Seat))]
    public int SeatId { get; set; }

    [Column("show_id")]
    [ForeignKey(nameof(Show))]
    public int ShowId { get; set; }

    [Column("ticket_number")]
    [StringLength(20)]
    public string TicketNumber { get; set; }

    [Column("qr_code")]
    [StringLength(500)]
    public string QrCode { get; set; }

    [Column("ticket_status")]
    [StringLength(20)]
    public string TicketStatus { get; set; } = "Active";

    [Column("check_in_time")]
    public DateTime? CheckInTime { get; set; }

    [Column("is_used")]
    public int IsUsed { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Booking Booking { get; set; }
    public User User { get; set; }
    public Seat Seat { get; set; }
    public Show Show { get; set; }
}
```

---

## DbContext Implementation

### Create CinemaDbContext

```csharp
// Data/CinemaDbContext.cs
using Microsoft.EntityFrameworkCore;
using KumariCinemas.Models;

namespace KumariCinemas.Data
{
    public class CinemaDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CinemaDbContext(DbContextOptions<CinemaDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Theater>()
                .HasOne(t => t.Manager)
                .WithMany()
                .HasForeignKey(t => t.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Hall>()
                .HasOne(h => h.Theater)
                .WithMany(t => t.Halls)
                .HasForeignKey(h => h.TheaterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Seats)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Show)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure value conversions for Oracle compatibility
            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasConversion<int>();

            modelBuilder.Entity<User>()
                .Property(u => u.IsVerified)
                .HasConversion<int>();

            // Configure decimal precision for currency
            modelBuilder.Entity<Show>()
                .Property(s => s.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            // Configure indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Show>()
                .HasIndex(s => new { s.MovieId, s.ShowDate });

            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.UserId, b.BookingDate });

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => t.TicketNumber)
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Set audit timestamps
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
```

### Register in Startup

```csharp
// Program.cs
var builder = WebApplicationBuilder.CreateBuilder(args);

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<CinemaDbContext>(options =>
{
    options.UseOracle(
        connectionString,
        oracleOptions =>
        {
            oracleOptions.UseOracleSQLCompatibility("23");
            oracleOptions.UseRowNumberForPaging();
        }
    );
});

// Add services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

var app = builder.Build();
app.Run();
```

---

## Migrations Guide

### Step 1: Create Initial Migration

```bash
# Create migration from existing database (reverse engineering)
dotnet ef dbcontext scaffold \
    "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=kumari_user;Password=Kumari@Cinema123;" \
    Oracle.EntityFrameworkCore \
    -o Models \
    -c CinemaDbContext \
    --force
```

### Step 2: Create Code-First Migration

```bash
# Create migration for code-first approach
dotnet ef migrations add InitialCreate

# View pending migrations
dotnet ef migrations list

# Update database
dotnet ef database update
```

### Step 3: Migration Best Practices

```bash
# Generate SQL script before applying
dotnet ef migrations script --output migrations.sql

# Rollback last migration
dotnet ef database update PreviousMigrationName

# Remove pending migration
dotnet ef migrations remove
```

---

## Data Access Layer

### Repository Pattern Implementation

```csharp
// Repositories/IGenericRepository.cs
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync();
}

// Repositories/GenericRepository.cs
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly CinemaDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(CinemaDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

// Repositories/IUserRepository.cs
public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetByRoleAsync(string role);
}

// Repositories/UserRepository.cs
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(CinemaDbContext context) : base(context) { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        return await _dbSet.Where(u => u.UserRole == role).ToListAsync();
    }
}
```

---

## Code Examples

### Example 1: Create Booking

```csharp
public async Task<BookingDto> CreateBookingAsync(CreateBookingRequest request)
{
    var booking = new Booking
    {
        UserId = request.UserId,
        ShowId = request.ShowId,
        NumberOfSeats = request.NumberOfSeats,
        TotalAmount = request.TotalAmount,
        BookingStatus = "Pending"
    };

    await _bookingRepository.AddAsync(booking);

    return new BookingDto
    {
        BookingId = booking.BookingId,
        Status = booking.BookingStatus
    };
}
```

### Example 2: Get User Tickets (Report)

```csharp
public async Task<IEnumerable<UserTicketDto>> GetUserTicketsAsync(int userId)
{
    var tickets = await _context.Tickets
        .Include(t => t.Booking)
            .ThenInclude(b => b.Show)
                .ThenInclude(s => s.Movie)
        .Include(t => t.Booking)
            .ThenInclude(b => b.Show)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Theater)
        .Include(t => t.Seat)
        .Where(t => t.UserId == userId && t.Booking.BookingStatus == "Confirmed")
        .OrderByDescending(t => t.Booking.BookingDate)
        .Select(t => new UserTicketDto
        {
            TicketId = t.TicketId,
            TicketNumber = t.TicketNumber,
            MovieTitle = t.Booking.Show.Movie.Title,
            ShowDate = t.Booking.Show.ShowDate,
            ShowTime = t.Booking.Show.ShowTime,
            TheaterName = t.Booking.Show.Hall.Theater.TheaterName,
            SeatInfo = $"{t.Seat.SeatRow}{t.Seat.SeatNumber}",
            Price = t.Booking.TotalAmount,
            TicketStatus = t.TicketStatus
        })
        .ToListAsync();

    return tickets;
}
```

### Example 3: Theater Occupancy Report

```csharp
public async Task<IEnumerable<OccupancyReportDto>> GetTheaterOccupancyAsync(int movieId)
{
    var occupancyData = await _context.Shows
        .Where(s => s.MovieId == movieId)
        .GroupBy(s => new { s.Hall.Theater.TheaterName, s.Hall.HallName, s.Hall.Capacity })
        .Select(g => new OccupancyReportDto
        {
            TheaterName = g.Key.TheaterName,
            HallName = g.Key.HallName,
            TotalCapacity = g.Key.Capacity,
            BookedSeats = g.Sum(s => s.BookedSeats),
            OccupancyPercentage = (decimal)g.Sum(s => s.BookedSeats) / g.Key.Capacity * 100
        })
        .OrderByDescending(o => o.OccupancyPercentage)
        .Take(3)
        .ToListAsync();

    return occupancyData;
}
```

---

## Performance Optimization

### Enable Query Optimization

```csharp
// CinemaDbContext.cs - Constructor
public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
    : base(options)
{
    // Enable query tags for better monitoring
    this.ChangeTracker.LazyLoadingEnabled = true;
    this.ChangeTracker.AutoDetectChangesEnabled = true;
}

// Disable auto-detection for bulk operations
ChangeTracker.AutoDetectChangesEnabled = false;
// ... bulk operations ...
ChangeTracker.AutoDetectChangesEnabled = true;
```

### Use AsNoTracking for Read-Only Queries

```csharp
// More efficient for large result sets
var movies = await _context.Movies
    .AsNoTracking()
    .Where(m => m.IsActive == 1)
    .OrderByDescending(m => m.ReleaseDate)
    .ToListAsync();
```

### Index on Frequently Queried Columns

```csharp
// Configure in OnModelCreating
modelBuilder.Entity<Booking>()
    .HasIndex(b => b.UserId);

modelBuilder.Entity<Booking>()
    .HasIndex(b => b.BookingStatus);

modelBuilder.Entity<Ticket>()
    .HasIndex(t => t.TicketNumber)
    .IsUnique();
```

---

## Troubleshooting

### Connection Issues

```bash
# Test Oracle connection
sqlplus kumari_user/Kumari@Cinema123@localhost:1521/ORCL

# Check Docker container status
docker ps | grep oracle

# View Oracle logs
docker logs kumari-oracle-26ai
```

### Migration Issues

```bash
# If migration fails, drop and recreate
dotnet ef database drop --force
dotnet ef database update

# List all migrations
dotnet ef migrations list --detailed
```

### Performance Issues

```csharp
// Enable SQL logging to diagnose slow queries
var connection = _context.Database.GetDbConnection();
var optionsBuilder = new DbContextOptionsBuilder<CinemaDbContext>();
optionsBuilder
    .UseOracle(connection.ConnectionString)
    .LogTo(Console.WriteLine);
```

---

## Next Steps

1. Run migrations to initialize database
2. Seed sample data
3. Implement business logic
4. Create API endpoints
5. Add authentication/authorization
6. Deploy to production

---

**Integration Status**: Ready for .NET Development ✓
