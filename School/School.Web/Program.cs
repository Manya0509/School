using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using School.Web.Data;
using School.Web.Data.Services;
using MatBlazor;
using School.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddDbContext<SchoolDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentContext"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<CabinetService>();
builder.Services.AddScoped<ClassModelService>();
builder.Services.AddScoped<ManagementService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<ScheduleService>();

builder.Services.AddMatBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
