using FinRost.BL.Infrastructure;
using FinRost.DAL.Extensions;
using FinRost.Web.Api.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddServiceConfig(builder.Configuration)
    .Configure<FormOptions>(x =>
    {
        x.ValueLengthLimit = int.MaxValue;
        x.MultipartBodyLengthLimit = int.MaxValue;

    });

builder.Services.AddCors();

builder.Services.AddAuthentication("CustomAuthScheme")
                .AddScheme<CustomAuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuthScheme", opts => { });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes("CustomAuthScheme")
                            .RequireAuthenticatedUser()
                            .Build();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "FinRost", Version = "v1", Description = "FinRost" });
    x.AddSecurityDefinition("CustomAuthScheme", new OpenApiSecurityScheme
    {
        Description = "FinRost",
        Name = "token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "CustomAuthScheme",
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id = "CustomAuthScheme",
                }
            },
            new string[]{}
        }
    });

    var basePath = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(basePath, "FinRost.Web.Api.xml");
    x.IncludeXmlComments(xmlPath);

});

builder.Services.AddSignalR();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandling>();
app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<NotifyHub>("/notifyHub");

app.Services.GetService<TelegramBotService>().Start();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(options => options.AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials().AllowAnyHeader());

app.UseHttpsRedirection();

app.MapControllers();

app.Run();