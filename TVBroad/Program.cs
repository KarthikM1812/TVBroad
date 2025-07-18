using Broadcast.DataAccess.Repository;
using Broadcast.DataService.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TVBroad.DataAccess.Data;
using TVBroad.Domain.Interfaces.Broadcast;

var builder = WebApplication.CreateBuilder(args);

// ?? Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBroadcastRepository, BroadcastRepository>();
builder.Services.AddScoped<IBroadcastService, BroadcastService>();


// ?? Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ?? Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ?? Middleware setup
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Required for static files like CSS/JS

app.UseRouting();

app.UseAuthentication(); // ? Must come before UseAuthorization
app.UseAuthorization();

// ??? Route config


// ? Seed Roles and Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    

    string[] roles = new[] { "Admin", "Scheduler", "Approver" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Admin user details
    string adminEmail = "admin@tv.com";
    string adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            Console.WriteLine("Error creating admin user:");
            foreach (var error in result.Errors)
                Console.WriteLine($"- {error.Description}");
        }
    }
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
