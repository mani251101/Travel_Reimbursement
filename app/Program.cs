// Title       : Travel Reimbursement 
// Author      : Manikandan K
// Created At  : 21/02/2023
// Updated At  : 02/03/2023
// Reviewed At : 03/05/2023
// Reviewed By : 

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using app.Areas.Identity.Data;
using scaflogindbcontext;
using filters;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

    builder.Services.AddDbContext<TravelDbContext>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection"));
});

    builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
options.Filters.Add(typeof(Reimbursementfilters)));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
