using System.Text;
using System.Text.Json.Serialization;
using AxonPDS.Data;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using AxonPDS.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// // Add services to the container
// builder.Services.AddControllers();

// Configuration
var configuration = builder.Configuration;
var connString = configuration.GetConnectionString("DevDB");

// Add Services

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Database Context
builder.Services.AddDbContext<PdsDbContext>(options =>
    options.UseSqlite(connString)
);

// Repositories
builder.Services.AddScoped<IAuth, AuthRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IPatient, PatientRepo>();
builder.Services.AddScoped<IPatientInfo, PatientInfoRepo>();
builder.Services.AddScoped<IMedicine, MedicineRepo>();
builder.Services.AddScoped<IRecord, RecordRepo>();
builder.Services.AddScoped<ITreatment, TreatmentRepo>();
builder.Services.AddScoped<ITreatmentRecord, TreatmentRecordRepo>();
builder.Services.AddScoped<IMedicineRecord, MedicineRecordRepo>();
builder.Services.AddScoped<SseRepo>();

// Identity & Authentication
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<PdsDbContext>();

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.SaveToken = false;
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(
//             Encoding.UTF8.GetBytes(configuration["AppSettings:JWTSecret"]!)
//         )
//     };
// });

 var secret = builder.Configuration.GetSection("AppSettings:JWTSecret").Value;
 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.RequireHttpsMetadata = false;
       options.SaveToken = true;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
           ValidateLifetime = true,
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidIssuer = builder.Configuration["AppSettings:Issuer"],
           ValidAudience = builder.Configuration["AppSettings:Audience"],
           ClockSkew = TimeSpan.Zero // remove delay of token when expire
       };

   });

builder.Services.AddAuthorization();

var app = builder.Build();


app.UseCors();

// Middleware Configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Apply database migrations
app.MigrateDb();

// API Routes
app.MapGroup("/api");
//    .MapIdentityApi<User>();

app.Run();

// game_graphql.db