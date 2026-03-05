# KUMARI CINEMAS - ADMIN USER GUIDE
## Professional Cinema Management System v2.0

---

## 📖 TABLE OF CONTENTS

1. [Dashboard Overview](#-dashboard-overview)
2. [User Management](#-user-management)
3. [Theater & Hall Management](#-theater--hall-management)
4. [Movie Catalog](#-movie-catalog)
5. [Showtime Management](#-showtime-management)
6. [Ticket Management](#-ticket-management)
7. [Advanced Reports](#-advanced-reports)
8. [Common Tasks Workflows](#-common-tasks-workflows)
9. [Tips & Best Practices](#-tips--best-practices)

---

## 🏠 DASHBOARD OVERVIEW

### Accessing the Dashboard
1. Open the application in your browser
2. You'll land on the **Dashboard** (main home page)
3. The sidebar on the left shows all available options

### Dashboard Components

#### Statistics Cards (Top Section)
**8 colorful cards displaying key metrics:**

| Card | Shows | Purpose |
|------|-------|---------|
| **Total Movies** | Count of all films in the system | Track catalog size |
| **Total Theaters** | Number of cinema locations | Monitor expansion |
| **Total Halls** | Total screening rooms across all theaters | Capacity overview |
| **Total Shows** | Scheduled showtimes | Current schedule load |
| **Registered Users** | Active customer accounts | User base size |
| **Total Bookings** | Tickets sold/reserved | Sales volume |
| **Total Tickets** | With breakdown (Paid/Cancelled) | Revenue tracking |
| **Total Revenue** | Money from paid tickets only | Business metrics |

**Pro Tip**: Click any stat card to drill down to that section

#### Interactive Charts (Middle Section)
- **Genre Distribution** - Which movie genres are most popular
- **Bookings per Theater** - Compare performance across locations
- **Monthly Revenue** - Track business trends over time

#### Quick Access Menu (Bottom Section)
**6 colored navigation buttons** for fast access to:
- Users Management
- Theaters & Halls
- Movies Catalog
- Showtimes Scheduling
- Tickets Management
- Reports & Analytics

---

## 👥 USER MANAGEMENT

### Navigation
**Sidebar** → **Users** OR **Dashboard** → **Users** button

### View All Users
You'll see a table with all registered customers:

| Column | Description |
|--------|-------------|
| UserID | Unique identifier |
| Username | Login username |
| Full Name | Customer's name |
| Email | Contact email |
| Phone | Contact number |
| Address | Residential address |
| Registration Date | When they joined |
| Actions | Edit/Delete buttons |

### Add New User
1. Click **"+ Add New User"** button (top right)
2. Fill in the form:
   - **Username** *(required)* - Unique login name
   - **Password** *(required)* - Secure password
   - **Full Name** *(required)* - Customer's name
   - **Email** *(required)* - Valid email address
   - **Phone** - Contact number (optional)
   - **Address** - Residential address (optional)
3. Click **"Create User"** button
4. Success message appears → User is registered

### Edit User Details
1. Click **Pencil Icon** next to user's name
2. Modify any fields
3. Click **"Update User"** button
4. Changes are saved immediately

### Delete User
1. Click **Trash Icon** next to user's name
2. Confirm deletion (you'll see a confirmation message)
3. User is removed from the system
4. ⚠️ WARNING: This also removes associated bookings/tickets!

### Search Users
- Users table is sortable (click column headers)
- Scroll through the table to find users
- Use browser's Find function (Ctrl+F) to search on current page

---

## 🏢 THEATER & HALL MANAGEMENT

### Navigation
**Sidebar** → **Theaters & Halls** OR **Dashboard** → **Theaters & Halls** button

### View All Theaters
Main page shows all theaters with:
- Theater ID
- Theater Name
- City Location
- Address
- Edit/Delete actions

### Add New Theater
1. Click **"+ Add New Theater"** button
2. Fill in:
   - **Theater Name** *(required)* - Cinema name (e.g., "Kumari Cinemas Downtown")
   - **City** *(required)* - Location city (e.g., "Kathmandu")
   - **Address** *(required)* - Full address
3. Click **"Create Theater"** button
4. Theater is created and appears in the list

### Edit Theater
1. Click **Pencil Icon** next to theater
2. Update name, city, or address
3. Click **"Update Theater"** button

### Delete Theater
1. Click **Trash Icon** next to theater
2. Confirm deletion
3. ⚠️ WARNING: Deleting removes all associated halls and shows!

### Add Screening Hall to Theater
1. From Theaters list, click on a theater's **"Manage Halls"** link
2. Click **"+ Add New Hall"** button
3. Fill in:
   - **Hall Name** *(required)* - Hall name (e.g., "Screen A", "IMAX Hall")
   - **Capacity** *(required)* - Number of seats (e.g., 150, 200)
4. Click **"Create Hall"** button
5. Hall is linked to the theater

### Edit/Delete Halls
- Same process as theaters
- Modify capacity or rename halls
- Delete removes all associated shows and seats

### Manage Seats
1. Go to **Sidebar** → **Seats**
2. Create seats for each hall (automated in some systems)
3. Track seat status: Available, Booked, Reserved

---

## 🎬 MOVIE CATALOG

### Navigation
**Sidebar** → **Movies** OR **Dashboard** → **Movies** button

### View All Movies
Table displays:
- Movie ID
- Title
- Duration (in minutes)
- Language (Hindi, English, etc.)
- Genre (Action, Comedy, Drama, etc.)
- Release Date
- Actions (Edit/Delete)

### Add New Movie
1. Click **"+ Add New Movie"** button
2. Fill in all fields:
   - **Title** *(required)* - Movie name
   - **Duration** *(required)* - Length in minutes (e.g., 150)
   - **Language** *(required)* - Language (Hindi, English, Nepali, etc.)
   - **Genre** *(required)* - Category (Action, Drama, Comedy, Horror, etc.)
   - **Release Date** *(required)* - Official release date
3. Click **"Create Movie"** button
4. Movie appears in catalog and is available for scheduling

### Edit Movie Details
1. Click **Pencil Icon** next to movie
2. Update any field (title, duration, genre, etc.)
3. Click **"Update Movie"** button
4. Changes apply to future shows

### Delete Movie
1. Click **Trash Icon** next to movie
2. Confirm deletion
3. ⚠️ Movie is removed (affects all scheduled shows!)

### Search/Sort Movies
- Click column headers to sort (Title, Genre, Language, etc.)
- Use browser Find (Ctrl+F) to search the current page

---

## ⏰ SHOWTIME MANAGEMENT

### Navigation
**Sidebar** → **Showtimes** OR **Dashboard** → **Showtimes** button

### View All Showtimes
Table shows:
- Show ID
- Movie Title
- Theater & Hall Name
- Show Date
- Show Time
- Status (Active, Scheduled, Cancelled)
- Actions

### Add New Showtime
1. Click **"+ Add New Showtime"** button
2. Fill in:
   - **Movie** *(required)* - Select from dropdown
   - **Theater** *(required)* - Select cinema location
   - **Hall** *(required)* - Select screening room
   - **Show Date** *(required)* - Date of show (select calendar)
   - **Show Time** *(required)* - Time (e.g., 10:00 AM, 2:30 PM)
   - **Status** *(required)* - Active / Scheduled / Cancelled
3. Click **"Create Showtime"** button
4. Show is scheduled and available for bookings

### Edit Showtime
1. Click **Pencil Icon** next to show
2. Change show date, time, or status
3. Click **"Update Showtime"** button
4. Changes take effect immediately

### Cancel a Show
1. Click **Edit** on the show
2. Change Status to **"Cancelled"**
3. Save → Show is cancelled
4. Existing bookings may need refunds (handled in Tickets section)

### Filter Shows by Status
- Use the Status filter dropdown (if available)
- View only Active, Scheduled, or Cancelled shows

**Pro Tip**: Schedule shows during peak hours (Evening: 5-9 PM, Weekend: 2-11 PM) for maximum bookings

---

## 🎟️ TICKET MANAGEMENT

### Navigation
**Sidebar** → **Tickets** OR **Dashboard** → **Tickets** button

### View All Tickets
Table displays:
- Ticket ID
- Issue Date
- Price (in Rs.)
- Category (Adult, Child, Senior, etc.)
- Seat Number
- Seat Status
- Cancellation Status
- Actions

### Issue New Ticket
1. Click **"+ Add Ticket"** button
2. Fill in:
   - **Show** *(required)* - Select a scheduled show
   - **Seat** *(required)* - Choose from available seats
   - **Ticket Category** *(required)* - Adult, Child, Senior, etc.
   - **Price** *(required)* - Price per ticket
3. Click **"Create Ticket"** button
4. Ticket is issued and assigned to a booking

### Manage Ticket Categories/Prices
1. Go to **Sidebar** → **Ticket Prices**
2. View all price categories
3. Add/Edit/Delete categories as needed
4. Standard categories: Adult, Child, Senior, Concession

### Cancel a Ticket
1. Go to **Tickets** list
2. Click **Edit** on the ticket to cancel
3. Mark for cancellation with reason
4. Click **"Save"** → Refund process initiated
5. Seat becomes available again

### Track Paid vs. Cancelled Tickets
- Dashboard shows: **Paid Tickets** (green) | **Cancelled Tickets** (red)
- Only paid tickets count toward occupancy and revenue

---

## 📊 ADVANCED REPORTS

### 1. USER TICKET HISTORY REPORT

**Purpose**: See what tickets a customer has purchased

**Navigation**: 
- **Sidebar** → **User Tickets** 
- **Dashboard** → **Reports** → **User Tickets**

**How to Use**:
1. Click the **"Select User"** dropdown
2. Choose a customer name from the list
3. System automatically shows:
   - **User Details Card**: Name, Email, Phone, Address, Registration Date
   - **Tickets Table**: All tickets purchased in the **last 6 months**
     - Movie name
     - Ticket price and category
     - Seat number
     - Show date and time
     - Booking date
   - **Total Revenue**: Sum of all prices in this period

**Example Workflow**:
- Manager wants to know: "What has customer 'John Doe' bought?"
- Select John Doe from dropdown
- See all 12 tickets he bought in last 6 months = Rs. 3,600 total spent
- Use this for customer loyalty programs or refund disputes

---

### 2. THEATER/HALL MOVIE SCHEDULE REPORT

**Purpose**: See what movies are playing in a specific hall

**Navigation**: 
- **Sidebar** → **Hall Movies** 
- **Dashboard** → **Reports** → **Hall Movies**

**How to Use**:
1. Click the **"Select Hall"** dropdown
2. Choose a theater hall (shows as: "Hall Name — Theater, City (Capacity)")
3. System displays:
   - **Theater/Hall Info Card**: Complete details
     - Theater name
     - City and address
     - Hall name
     - Seating capacity
   - **Movies & Showtimes Table**: All shows in that hall
     - Movie title, genre, language
     - Movie duration and release date
     - Show date and time
     - Show status (Active/Scheduled)
   - **Total Shows**: Count at bottom

**Example Workflow**:
- Theater manager asks: "What's playing in our downtown IMAX hall next week?"
- Select "IMAX Hall — Downtown Cinemas, Kathmandu"
- See 8 shows: 3 Hindi movies, 2 English, 3 Nepali productions
- Plan for staffing, refreshment inventory, parking

---

### 3. MOVIE OCCUPANCY PERFORMANCE REPORT

**Purpose**: Find which theaters are most popular for a specific movie

**Navigation**: 
- **Sidebar** → **Occupancy** 
- **Dashboard** → **Reports** → **Occupancy Analytics**

**How to Use**:
1. Click the **"Select Movie"** dropdown
2. Choose a movie title
3. System analyzes and displays:
   - **Movie Info Card**: Title, Genre, Language, Analysis type
   - **Top 3 Performers**: Ranked with medals
     - 🥇 **GOLD** - Highest occupancy %
     - 🥈 **SILVER** - 2nd highest
     - 🥉 **BRONZE** - 3rd highest

**For Each Theater Shown**:
- Theater name and city
- Hall name
- Occupancy percentage (e.g., 85.5%)
- Paid tickets / Total capacity (e.g., 171 / 200 seats)
- Visual progress bar (color-coded):
  - 🟢 **Green** = Highly popular (>80%)
  - 🔵 **Blue** = Good occupancy (50-80%)
  - 🟠 **Orange** = Low occupancy (<50%)

**Important Note**: 
- ⚠️ **Only PAID tickets count** - Cancelled/Refunded tickets are excluded
- This shows real revenue-generating capacity

**Example Workflow**:
- Management asks: "How did 'Action Hero' movie perform across locations?"
- Select "Action Hero"
- Results show:
  - 🥇 Kathmandu Downtown: 95% (190/200 seats) ✅ Sold out!
  - 🥈 Kathmandu Mall: 72% (144/200 seats) ✓ Good
  - 🥉 Pokhara Center: 58% (87/150 seats) → Could improve
- Decision: Keep showing in Kathmandu, consider special offers in Pokhara

---

## 🛠️ COMMON TASKS WORKFLOWS

### WORKFLOW 1: Opening a New Theater Location

**Scenario**: Kumari Cinemas is opening a new branch in Pokhara

**Steps**:
1. **Add Theater**
   - Go to Theaters & Halls → Add New Theater
   - Name: "Kumari Cinemas Pokhara"
   - City: "Pokhara"
   - Address: "Lakeside Road, Pokhara, Nepal"
   - Submit

2. **Add Screening Halls**
   - Click "Manage Halls" for Pokhara theater
   - Add Hall 1: Name="Screen A", Capacity=150
   - Add Hall 2: Name="Screen B", Capacity=120
   - Add Hall 3: Name="Screen C (IMAX)", Capacity=180

3. **Create Seats**
   - Go to Seats → Create seats for each hall
   - Screen A: 150 seats (A1-A30, B1-B30, C1-C30, D1-D30, E1-E30)
   - Mark all as "Available"

4. **Schedule First Shows**
   - Go to Showtimes → Add shows
   - Select movies from catalog (or add new movies first)
   - Schedule across three halls
   - Evening shows (6 PM, 9 PM)
   - Weekend matinees (2 PM)

---

### WORKFLOW 2: Managing a Movie Release

**Scenario**: A new Bollywood movie "Super Action" is releasing - you want to maximize occupancy

**Steps**:
1. **Add Movie to Catalog**
   - Go to Movies → Add New Movie
   - Title: "Super Action"
   - Duration: 140 minutes
   - Language: "Hindi"
   - Genre: "Action"
   - Release Date: [actual release date]

2. **Schedule Across All Theaters**
   - Go to Showtimes
   - Add show in Kathmandu Downtown - Screen A (2 PM) - Status: Active
   - Add show in Kathmandu Downtown - Screen A (5 PM) - Status: Active
   - Add show in Kathmandu Downtown - Screen A (8 PM) - Status: Active
   - Add show in Kathmandu Mall - Screen 1 (6 PM) - Status: Active
   - Add show in Pokhara Center - Screen A (7 PM) - Status: Active
   - (Schedule 1-2 shows per hall to maximize attendance)

3. **Track Performance**
   - Next day/week: Go to Occupancy Report
   - Select "Super Action"
   - See which halls are filling up
   - Add more shows if 80%+ occupancy in any hall
   - Consider special timing if <50% occupancy

4. **Analyze Results**
   - Dashboard shows updated Revenue chart
   - Compare genre distribution
   - Plan marketing for other halls if performing well

---

### WORKFLOW 3: Processing Customer Refund

**Scenario**: A customer wants to cancel their ticket booking

**Steps**:
1. **Find User's Tickets**
   - Go to User Ticket Report
   - Select the customer
   - Find the ticket to cancel

2. **Cancel Ticket**
   - Go to Tickets
   - Find the ticket ID
   - Click Edit
   - Change status to "Cancelled"
   - Add reason: "Customer requested refund"
   - Save

3. **Process Refund** (manual in this system)
   - Go to Cancellations
   - Record the cancellation with refund amount
   - Process refund through payment system (external)

4. **Verify Impact**
   - Go to Occupancy Report
   - Run for affected movie/show
   - Seat becomes available
   - Occupancy % decreases (now excludes this paid ticket)

---

## 💡 TIPS & BEST PRACTICES

### DASHBOARD
✅ **Check daily**: Monitor today's revenue and bookings  
✅ **Analyze trends**: Look at monthly revenue chart  
✅ **Compare performance**: Use bookings per theater chart  
✅ **Genre insights**: Plan movie selections based on distribution  

### USERS
✅ **Validate email**: Ensure valid contact information  
✅ **Regional diversity**: Track registration by location  
✅ **Customer support**: Use phone/address for refunds  
❌ **Don't delete lightly**: Removes all booking history!  

### MOVIES
✅ **Complete metadata**: Always fill genre, language, duration  
✅ **Realistic durations**: Include intermission time  
✅ **Current catalog**: Remove expired/old movies  
✅ **Seasonality**: Add family movies in holidays  
❌ **Don't list duplicates**: One record per movie  

### SHOWS
✅ **Peak scheduling**: 6 PM, 8 PM, 10 PM on weekdays  
✅ **Matinee shows**: Weekend 2-3 PM for families  
✅ **Stagger timings**: Same movie at different times  
✅ **Monitor occupancy**: Adjust shows based on demand  
❌ **Too many shows**: Can cannibalize attendance  

### TICKETS & PRICING
✅ **Realistic pricing**: Match competitors in your region  
✅ **Categories**: Offer Senior/Child discounts  
✅ **Dynamic pricing**: Premium hours = higher prices  
✅ **Track conversions**: Monitor paid vs. cancelled  
❌ **Overpricing**: Will reduce occupancy  

### REPORTS
✅ **Run weekly**: Check occupancy trends  
✅ **User analysis**: Identify loyal customers  
✅ **Hall performance**: Balance load across screens  
✅ **Export data**: Keep backups of reports  
✅ **Seasonal comparison**: Compare YoY data  

---

## ⚠️ IMPORTANT WARNINGS

| Action | Warning | Impact |
|--------|---------|--------|
| **Delete User** | Removes bookings/tickets | Loss of booking history |
| **Delete Theater** | Cascades to all halls | Complete location removed |
| **Delete Movie** | Removes show records | Cannot view past screenings |
| **Cancel Show** | Affects existing bookings | Customer refunds needed |
| **Change Capacity** | Affects occupancy % | Historical data changes |

---

## 🎯 KEY METRICS TO MONITOR

**Daily**:
- Today's bookings count
- Current occupancy % across halls
- Revenue from paid tickets

**Weekly**:
- Average occupancy per theater
- Revenue trends
- Customer satisfaction (if feedback system available)

**Monthly**:
- Total revenue
- Best-performing movies
- Best-performing locations
- Genre preferences
- Seasonal trends

**Quarterly/Yearly**:
- Year-over-year growth
- Profitability per location
- Market expansion opportunities
- Pricing strategy adjustments

---

## 📞 TROUBLESHOOTING

| Issue | Solution |
|-------|----------|
| **Can't see a user's bookings** | User may have registered but not purchased tickets |
| **Show not appearing in reports** | Mark show status as "Active" |
| **Occupancy shows 0%** | No paid tickets yet - need actual bookings |
| **Missing movie in dropdown** | Movie might not be in database - add it first |
| **Seat shows as occupied but unfilled** | Reserved but not confirmed - may need manual release |

---

**Last Updated**: 2025  
**For Technical Support**: Contact system administrator  
**For Features/Enhancements**: Submit request to development team
