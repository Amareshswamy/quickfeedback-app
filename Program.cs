using Microsoft.EntityFrameworkCore;
using QuickFeedback.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.

// Add EF Core and SQL Server
builder.Services.AddDbContext<FeedbackContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add a simple CORS policy (good practice)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//NEW FINE 

var app = builder.Build();

// 2. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// *** THIS IS KEY FOR THE UI ***
// UseDefaultFiles must be called before UseStaticFiles
// This tells the app to look for "index.html" in wwwroot
app.UseDefaultFiles();
// This serves the files from wwwroot
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// *** THIS IS KEY FOR THE DATABASE ***
// Automatically apply EF Core migrations on startup
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FeedbackContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    // In a real app, you might want to handle this more gracefully.
}


app.Run();