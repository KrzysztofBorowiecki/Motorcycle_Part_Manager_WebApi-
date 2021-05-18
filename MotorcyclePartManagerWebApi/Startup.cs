using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MotorcyclePartManagerWebApi.Data;
using MotorcyclePartManagerWebApi.Helpers.AutoMapperConfig;
using MotorcyclePartManagerWebApi.JwtAuthentication;
using MotorcyclePartManagerWebApi.Middleware;
using MotorcyclePartManagerWebApi.Models;
using MotorcyclePartManagerWebApi.Repositories;
using MotorcyclePartManagerWebApi.Seeders;
using MotorcyclePartManagerWebApi.Services.UserServices;
using MotorcyclePartManagerWebApi.Validators;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using MotorcyclePartManagerWebApi.Authorization;

namespace MotorcyclePartManagerWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddControllers().AddFluentValidation();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MotorcyclePartManagerWebApi", Version = "v1" });
            });
            services.AddDbContext<ProjectContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IPartRepository, PartRepository>();

            services.AddScoped<IAddUserService, AddUserService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RoleSeeder>();
            //services.AddScoped<IPasswordHasher<Singup>, PasswordHasher<Singup>>(); 
            services.AddScoped<IValidator<Singup>, SingupEntityValidator>();
            services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
            services.AddScoped<IUserContextService , UserContextService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IValidator<MotorcycleQuery>, MotorcycleQueryValidator>();
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient",
                    builder => builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(Configuration["AllowedOrigins"])
                );
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleSeeder seeder)
        {
            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseCors("FrontEndClient");

            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotorcyclePartManagerWebApi v1"));
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
