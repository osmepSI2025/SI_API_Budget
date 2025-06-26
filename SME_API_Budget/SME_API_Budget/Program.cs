
using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Repository;
using SME_API_Budget.Services;
using SME_API_SMEBudget.Entities;

namespace SME_API_Budget
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // ✅ Register Database Context
            builder.Services.AddDbContext<SMEBudgetDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // ✅ Add NSwag Swagger 2.0
            builder.Services.AddOpenApiDocument(config =>
            {
                config.DocumentName = "API_SME_BUDGET_v1";
                config.Title = "API SME Budget";
                config.Version = "v1";
                config.Description = "API documentation using Swagger 2.0";
                config.SchemaType = NJsonSchema.SchemaType.Swagger2;
            });



            //add service
            builder.Services.AddScoped<IApiInformationRepository, ApiInformationRepository>();
            builder.Services.AddScoped<IApiInformationService, ApiInformationService>();
            builder.Services.AddScoped<IRecPRsRepository, RecPRsRepository>();
            builder.Services.AddScoped<IRecPRsService, RecPRsService>();
            builder.Services.AddScoped<IReturnPActivityRepository, ReturnPActivityRepository>();
            builder.Services.AddScoped<IReturnPActivityService, ReturnPActivityService>();
            builder.Services.AddScoped<IReturnPAreaRepository, ReturnPAreaRepository>();
            builder.Services.AddScoped<IReturnPAreaService, ReturnPAreaService>();
            builder.Services.AddScoped<IReturnPExpectedRepository, ReturnPExpectedRepository>();
            builder.Services.AddScoped<IReturnPExpectedService, ReturnPExpectedService>();
            builder.Services.AddScoped<IReturnPOutcomeRepository, ReturnPOutcomeRepository>();
            builder.Services.AddScoped<IReturnPOutcomeService, ReturnPOutcomeService>();
            builder.Services.AddScoped<IReturnPOutputRepository, ReturnPOutputRepository>();
            builder.Services.AddScoped<IReturnPOutputService, ReturnPOutputService>();
            builder.Services.AddScoped<IReturnPPayRepository, ReturnPPayRepository>();
            builder.Services.AddScoped<IReturnPPayService, ReturnPPayService>();
            builder.Services.AddScoped<IReturnPPlanBdgRepository, ReturnPPlanBdgRepository>();
            builder.Services.AddScoped<IReturnPPlanBdgService, ReturnPPlanBdgService>();
            builder.Services.AddScoped<IReturnPRiskRepository, ReturnPRiskRepository>();
            builder.Services.AddScoped<IReturnPRiskService, ReturnPRiskService>();

            builder.Services.AddScoped<IReturnProjectRepository, ReturnProjectRepository>();
            builder.Services.AddScoped<IReturnProjectService, ReturnProjectService>();

            //service call api
            builder.Services.AddScoped<ICallAPIService, CallAPIService>();
            //    builder.Services.AddScoped<IReturnProjectService, ReturnProjectService>();
            builder.Services.AddHttpClient<CallAPIService>();
            builder.Services.AddSingleton<CallAPIService>();

            builder.Services.AddHostedService<ScheduledJobService>();
            // Cron job
            //builder.Services.AddHostedService<CronBackgroundService>();
            //builder.Services.AddScoped<IJobService, JobService>();
            var app = builder.Build();

            if (app.Environment.IsDevelopment()
                  || app.Environment.IsProduction()
                  )
            {
                app.UseOpenApi();     // ✅ Serve Swagger 2.0 JSON
                app.UseSwaggerUi3();  // ✅ Use Swagger UI 3
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
