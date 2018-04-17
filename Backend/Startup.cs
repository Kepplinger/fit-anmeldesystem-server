using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Backend.Core.Contracts;
using StoreService.Persistence;
using Swashbuckle.AspNetCore.Swagger;
//using RazorLight;
using System;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Backend.Persistence;
using Backend.Core.Entities.UserManagement;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Controllers.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Backend
{
    public class Startup
    {
        //aus einer text file laden ode enviroment variable und zufällige zeichenkette (secretkey)
        private const string SecretKey = "needtogetthisfromenvironment";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders().AddUserManager<UserManager<IdentityUser>>();
            services.Configure<IdentityOptions>(options => { });
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(IdentityUser), policy => policy.RequireClaim("rol", "admin"));
                //options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).

            });
            //services.AddAuthentication(o => {
            //    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddJwtBearer();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters =
                             new TokenValidationParameters
                             {
                                 ValidateIssuer = false,
                                 ValidateAudience = false,
                                 ValidateLifetime = true,
                                 ValidateIssuerSigningKey = true,

                                 IssuerSigningKey = _signingKey
                             };
                    });
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info { Title = "FIT Anmelde System - V2.0", Version = "v2" });
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            string connectionString = configuration["Urls:ServerUrl"];

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = "FIT-Backend";
                options.Audience = connectionString;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            });
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "FITAS2.0");
            });
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();

            createRoles(provider);

        }

        private async void createRoles(IServiceProvider provider)
        {

            using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    IdentityRole newRole = new IdentityRole() { Name = "Admin" };
                    await roleManager.CreateAsync(newRole);
                    await roleManager.AddClaimAsync(newRole, new Claim("rol", "admin"));
                }

            }
        }
    }
}
