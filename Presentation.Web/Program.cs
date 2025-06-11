using Application.Mappings;
using Application.Validators;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Transactions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
{
    options.UseSqlServer("Server=.;Database=EfCoreBenchmark;Trusted_Connection=True;TrustServerCertificate=True;", opt =>
    {
        opt.MigrationsAssembly("Infrastructure");
        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    
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
});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(UpdateCustomerCommandValidator).Assembly);

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
