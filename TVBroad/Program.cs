using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TVBroad.DataAccess.Data;
using TVBroad.DataAccess.Repository;
using TVBroad.DataAccess.Repository.Admin;
using TVBroad.DataAccess.Repository.Approver;
using TVBroad.DataAccess.Repository.SchedulerRepo;
using TVBroad.DataAccess.Repository.UserR;
using TVBroad.Domain.Interfaces;
using TVBroad.Domain.Interfaces.Approver;
using TVBroad.Domain.Interfaces.IAdmin;
using TVBroad.Domain.Interfaces.IScheduler;
using TVBroad.Domain.Interfaces.IUser;
using TVBroad.Service.Approver;
using TVBroad.Service.SchedulerService;
using TVBroad.Service.Service.Admin;
using TVBroad.Service.Services;
using TVBroad.Service.User;

var builder = WebApplication.CreateBuilder(args);

// Configure your DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

// Register your custom services
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ISchedulerRepository, SchedulerRepository>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();

builder.Services.AddScoped<IBroadcastRepository, BroadcastRepository>();
builder.Services.AddScoped<IBroadcastService, BroadcastService>();

builder.Services.AddScoped<IApproverService, ApproverService>();
builder.Services.AddScoped<IApproverRepository, ApproverRepository>();



//  MVC support
builder.Services.AddControllersWithViews();

var app = builder.Build();

//  Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//  Auth middleware
app.UseAuthentication();
app.UseAuthorization();

//  Default route (Login)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
