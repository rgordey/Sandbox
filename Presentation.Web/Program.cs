using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Features.Customers.Queries;
using Application.Mappings;
using Application.Validators;
using Domain;
using FluentValidation;
using Infrastructure;
using Infrastructure.CompiledModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Presentation.Services;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using Serilog.Debugging;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");;

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, opt =>
    {
        opt.MigrationsAssembly("Infrastructure");
        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseModel(AppDbContextModel.Instance);
});



builder.Services
     .AddIdentity<ApplicationUser, ApplicationRole>(options =>
     {
         options.SignIn.RequireConfirmedAccount = true;
     })
     .AddDefaultTokenProviders()
     .AddRoles<ApplicationRole>()
     .AddEntityFrameworkStores<AppDbContext>()
     .AddRoleManager<RoleManager<ApplicationRole>>();

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(GetCustomersQuery).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.LicenseKey = builder.Configuration["ApiKeys:AutoMapperApiKey"];
});


SelfLog.Enable(Console.Error);

builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddSendGrid(options =>
{
    options.ApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
});


builder.Services.AddAutoMapper(action => 
{ 
    action.AddMaps(typeof(MappingProfile).Assembly);
    action.LicenseKey = builder.Configuration["ApiKeys:AutoMapperApiKey"];
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(UpdateCustomerCommandValidator).Assembly);

// Add services to the container.
builder.Services.AddRazorPages();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
