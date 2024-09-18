// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.Identity.Web;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
//
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
// app.UseAuthorization();
//
// app.MapControllers();
//
// app.Run();


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Netherite.Service;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Netherite.Data;
using Netherite.Interface;
using Netherite.Repository;
using Netherite.Service;

#nullable enable
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
CorsServiceCollectionExtensions.AddCors(builder.Services, (Action<CorsOptions>) (options => options.AddDefaultPolicy((Action<CorsPolicyBuilder>) (policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()))));
MvcServiceCollectionExtensions.AddControllers(builder.Services);
EndpointMetadataApiExplorerServiceCollectionExtensions.AddEndpointsApiExplorer(builder.Services);
SwaggerGenServiceCollectionExtensions.AddSwaggerGen(builder.Services, (Action<SwaggerGenOptions>) (c =>
{
  SwaggerGenOptionsExtensions.SwaggerDoc(c, "v1", new OpenApiInfo()
  {
    Version = "v1",
    Title = "Netherite API",
    Description = "API for Netherite app"
  });
  SwaggerGenOptionsExtensions.TagActionsBy(c, (Func<ApiDescription, IList<string>>) (api => (IList<string>) new string[1]
  {
    ((ControllerActionDescriptor) api.ActionDescriptor).ControllerName
  }));
  // SwaggerGenOptionsExtensions.DocInclusionPredicate(c, (Func<string, ApiDescription, bool>) ((name, api) => true));
  // MethodInfo methodInfo;
  // SwaggerGenOptionsExtensions.CustomOperationIds(c, (Func<ApiDescription, string>) (apiDesc => Swashbuckle.AspNetCore.SwaggerGen.ApiDescriptionExtensions.TryGetMethodInfo(apiDesc, out methodInfo) ? methodInfo.Name : (string) null));
  // string filePath = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
  // SwaggerGenOptionsExtensions.IncludeXmlComments(c, filePath);
}));
EntityFrameworkServiceCollectionExtensions.AddDbContext<NetheriteDbContext>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IReferalBonusesServices, ReferalBonusesServices>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<ITasksServices, TasksServices>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<ITasksRepository, TasksRepository>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IMinerServices, MinerServices>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IMinerRepository, MinerRepository>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IUserServices, UserServices>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IUserRepository, UserRepository>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IIntervalServices, IntervalServices>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IIntervalsRepository, IntervalsRepository>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<ICurrencyPairsService, CurrencyPairsServices>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<ICurrencyPairsRepository, CurrencyPairsRepository>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IFavoritesService, FavoritesService>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IFavoritesRepository, FavoritesRepository>(builder.Services);
ServiceCollectionServiceExtensions.AddScoped<IOrderServices, OrderService>(builder.Services);
HttpClientFactoryServiceCollectionExtensions.AddHttpClient<IOrderRepository, OrderRepository>(builder.Services);
ServiceCollectionHostedServiceExtensions.AddHostedService<OrderBackgroundService>(builder.Services);
ServiceCollectionServiceExtensions.AddSingleton<OrderBackgroundService>(builder.Services);
WebApplication webApplication = builder.Build();
if (!Directory.Exists("storage"))
  Directory.CreateDirectory("storage");
WebApplication app = webApplication;
StaticFileOptions options1 = new StaticFileOptions();
options1.FileProvider = (IFileProvider) new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "storage"));
options1.RequestPath = (PathString) "/storage";
StaticFileExtensions.UseStaticFiles(app, options1);
SwaggerBuilderExtensions.UseSwagger(webApplication);
SwaggerUIBuilderExtensions.UseSwaggerUI(webApplication, (Action<SwaggerUIOptions>) (c => SwaggerUIOptionsExtensions.SwaggerEndpoint(c, "/swagger/v1/swagger.json", "Netherite API v1")));
HttpsPolicyBuilderExtensions.UseHttpsRedirection(webApplication);
CorsMiddlewareExtensions.UseCors(webApplication);
EndpointRoutingApplicationBuilderExtensions.UseRouting(webApplication);
AuthAppBuilderExtensions.UseAuthentication(webApplication);
AuthorizationAppBuilderExtensions.UseAuthorization(webApplication);
ControllerEndpointRouteBuilderExtensions.MapControllers(webApplication);
webApplication.Run();

