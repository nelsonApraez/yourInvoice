///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Identity.Web;
using yourInvoice.Common;
using yourInvoice.Common.Constant;
using yourInvoice.Common.EF;
using yourInvoice.Link;
using yourInvoice.Offer.Application;
using yourInvoice.Offer.Infrastructure;
using yourInvoice.Offer.Web.API;
using yourInvoice.Offer.Web.API.Extensions;
using yourInvoice.Offer.Web.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddPresentation()
                .AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddApplicationInsightsTelemetry()
                .AddLogging(logBuilder => logBuilder.AddApplicationInsights())
                .AddApplicationLink(builder.Configuration)
                .AddyourInvoiceCommon(builder.Configuration)
                .AddyourInvoiceCommonEF(builder.Configuration)
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options =>
                {
                    builder.Configuration.Bind(ConstantCode_AppSection.AzureAdB2C, options);
                },
                options => { builder.Configuration.Bind(ConstantCode_AppSection.AzureAdB2C, options); });
//TO DO Manejo de sesion
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-CO");
});

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; // 2 gb
});

// Configurar Kestrel para permitir solicitudes m�s grandes
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue; // 2 gb
});

builder.Services.AddControllers(options =>
{
    options.MaxModelBindingCollectionSize = int.MaxValue; // L�mite de tama�o para colecciones en el binding de modelos.
})
.AddJsonOptions(options =>
{
    // Configura el l�mite del tama�o del cuerpo JSON.
    options.JsonSerializerOptions.MaxDepth = 64;
});

builder.Services.Configure<IISServerOptions>(settings =>
{
    settings.MaxRequestBodySize = long.MaxValue;
});

//TO DO Final manejo de session

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.ApplyMigrationsLink();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler("/error");

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpManage();

app.UseMiddleware<GloblalExceptionHandlingMiddleware>();

app.MapControllers();
//TO DO Manejo de session
app.UseSession();
//TO DO Final manejo de session
app.Run();