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
using System.Threading.Tasks;
using Backend.Utils;
using System.Linq;

namespace Backend {
    public class Startup {
        private static string secretKey = ReadPrivateKey();
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<IdentityOptions>(options => { });

            services.AddIdentity<FitUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<FitUser>>();

            services.AddSingleton<IJwtFactory, JwtFactory>();

            services.AddAuthorization(options => {
                options.AddPolicy("WritableFitAdmin", policy => {
                    policy.RequireClaim("rol", "FitAdmin");
                    policy.AddAuthenticationSchemes("Bearer");
                });
                options.AddPolicy("FitAdmin", policy => {
                    policy.RequireClaim("rol", "FitAdmin", "FitReadOnly");
                    policy.AddAuthenticationSchemes("Bearer");
                });
                options.AddPolicy("WritableAdmin", policy => {
                    policy.RequireClaim("rol", "FitAdmin", "MemberAdmin");
                    policy.AddAuthenticationSchemes("Bearer");
                });
                options.AddPolicy("AnyAdmin", policy => {
                    policy.RequireClaim("rol", "FitAdmin", "FitReadOnly", "MemberAdmin", "MemberReadOnly");
                    policy.AddAuthenticationSchemes("Bearer");
                });
                options.AddPolicy("Member", policy => {
                    policy.RequireClaim("rol", "Member");
                    policy.AddAuthenticationSchemes("Bearer");
                });
                options.AddPolicy("MemberAndWriteableAdmins", policy => {
                    policy.RequireClaim("rol", "Member", "FitAdmin", "MemberAdmin");
                    policy.AddAuthenticationSchemes("Bearer");
                });
                options.AddPolicy("Anyone", policy => {
                    policy.RequireClaim("rol", "FitAdmin", "FitReadOnly", "MemberAdmin", "MemberReadOnly", "Member");
                    policy.AddAuthenticationSchemes("Bearer");
                });
            });

            services.Configure<IdentityOptions>(options => {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.TokenValidationParameters =
                             new TokenValidationParameters {
                                 ValidateIssuer = false,
                                 ValidateAudience = false,
                                 ValidateLifetime = true,
                                 ValidateIssuerSigningKey = true,
                                 IssuerSigningKey = _signingKey
                             };
                    });

            services.AddMvc();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v2", new Info { Title = "FIT Anmelde System - V2.0", Version = "v2" });
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            string connectionString = configuration["Urls:ServerUrl"] + configuration["Urls:ApiPort"];

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options => {
                options.Issuer = "FIT-Backend";
                options.Audience = connectionString;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider) {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            });
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "FITAS2.0");
            });
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();

            InitDb(provider);
            
            // CreateTestPDF();
        }

        private static async Task InitDb(IServiceProvider provider) {
            try {
                using (IUnitOfWork uow = new StoreService.Persistence.UnitOfWork()) {
                    await uow.FillDb(provider);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private static async Task CreateTestPDF() {
            using (IUnitOfWork uow = new StoreService.Persistence.UnitOfWork()) {
                DocumentBuilder builder = new DocumentBuilder();
                builder.CreatePdfOfBooking(uow.BookingRepository.Get().FirstOrDefault());
            }
        }

        private static string ReadPrivateKey() {
            return File.ReadAllText("privateKey.txt", Encoding.Default);
        }
    }
}
