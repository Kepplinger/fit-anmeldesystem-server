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
using System.IO;

namespace Backend
{
    public class Startup
    {
        private static string secretKey = ReadPrivateKey();
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddIdentity<FitUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders().AddUserManager<UserManager<FitUser>>();
            services.Configure<IdentityOptions>(options => { });
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(FitUser), policy => policy.RequireClaim("rol", "admin"));
                //options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).

            });

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
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
            
            InitDatabase(provider);
        }

        private async void InitDatabase(IServiceProvider provider)
        {
            using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<FitUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                using (IUnitOfWork uow = new StoreService.Persistence.UnitOfWork()) {
                    uow.FillDb();
                }

                string[] roles = new string[] { "FitAdmin", "FitReadOnly", "MemberAdmin", "MemberReadOnly" };

                foreach (var role in roles) {
                    if (!await roleManager.RoleExistsAsync(role)) {
                        IdentityRole newRole = new IdentityRole() { Name = role };
                        await roleManager.CreateAsync(newRole);
                        await roleManager.AddClaimAsync(newRole, new Claim("rol", role));
                    }
                }

                FitUser fitUser = new FitUser();
                fitUser.Email = "simon.kepplinger@gmail.com";
                fitUser.UserName = fitUser.Email;
                fitUser.Role = "FitAdmin";

                await userManager.CreateAsync(fitUser, "test123");
            }
        }

        private static string ReadPrivateKey() {
            return File.ReadAllText("privateKey.txt", Encoding.Default);
        }
    }
}
