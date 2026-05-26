using AspNetCoreRateLimit;
using CalendarGroupMeeting.Api.API;
using CalendarGroupMeeting.Api.API.Middleware;
using CalendarGroupMeeting.Api.Infra.Data;
using CalendarGroupMeeting.Api.Infra.DependencyInjection;
using CalendarGroupMeeting.Api.Infra.DI;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddSecurity();
builder.Services.AddApplication();
var allowedOrigins = Env.GetString("ALLOWED_ORIGINS")?.Split(',')
    ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();
app.UseCors("AllowSpecificOrigins");
app.MapEventEndpoints();

app.Run();
