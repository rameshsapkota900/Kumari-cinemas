using Kumari_cinemas.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register Oracle Database Helper as singleton service
builder.Services.AddSingleton<OracleDbHelper>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("OracleDb")!;
    return new OracleDbHelper(connStr);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
