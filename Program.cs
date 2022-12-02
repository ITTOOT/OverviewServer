using Blazored.Modal;
using BlazorPrettyCode;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OverviewServer.Data;
using OverviewServer.Services;
using OverviewServer.Services.Background.Workers;
using Radzen;


//Web Builder
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");
//var connectionString = builder.Configuration.GetConnectionString("DefaultContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

//Identity Contexts
builder.Services.AddDbContext<IdentityContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<IdentityContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//Single instance throughout project
builder.Services.AddSingleton<WorkerService>();
builder.Services.AddSingleton<RockwellDataService>();
builder.Services.AddSingleton<RockwellConnectionService>();
builder.Services.AddSingleton<EquipmentMessagesService>();

//Background services
builder.Services.AddHostedService(provider => provider.GetService<RockwellDataService>());

//Worker Service hosting is working
//builder.Services.AddHostedService(provider => provider.GetService<WorkerService>());

//Programmer services
builder.Services.AddLogging();

//Usability services
builder.Services.AddBlazorPrettyCode();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddBlazoredModal();

// Configure to run as Windows service
builder.Host.UseWindowsService();

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

//Security
app.UseAuthentication();
app.UseAuthorization();

//Endpoints
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Run();
