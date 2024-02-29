using InternetBillingSystem.Data;
using InternetBillingSystem.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options => //Db Context for connecting to the database
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Name_of_connection_string"));  //Change the name of the connection string to your name of the connection string in the appsettings.json
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => //adding authetication to the app using jwt token
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer Scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddCors(options => options.AddPolicy(name: "ClientBilling", //adding cors policy
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer( options => //adding authetication
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value))
    };
});


var app = builder.Build();

app.UseAuthorizationMiddleware();// add isAuthorized header to response

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("ClientBilling");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCheckJwtExpirationMiddleware();//checks for expiration time oj jwt when sending a request

app.UseGlobalExceptionMiddleware();//handles exceptions in the whole application

app.MapControllers();

app.Run();
