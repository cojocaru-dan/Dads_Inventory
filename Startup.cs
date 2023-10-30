using DadsInventory.Authentication;
using DadsInventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DadsInventory
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
            services.AddAuthentication("BasicAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();


            services.AddScoped<AuthenticationHandler<AuthenticationSchemeOptions>, BasicAuthenticationHandler>();
            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<FamilyRepository, FamilyRepository>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Item}/{action=List}/{id?}");
            });
            StoreRolesAndUsersToDb(app);
        }

        private async void StoreRolesAndUsersToDb(IApplicationBuilder app)
        {
            using var scope1 = app.ApplicationServices.CreateScope();
            var roleManager = scope1.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new string[] { "first", "second" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using var scope2 = app.ApplicationServices.CreateScope();
            var userManager = scope2.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var dadName = "Dad";
            var dadPassword = "Pass#1";
            var momName = "Mom";
            var momPassword = "Pass#2";

            for (int i = 0; i < 2; i++)
            {
                var user = new IdentityUser();
                if (i == 0 && await userManager.FindByNameAsync(dadName) == null)
                {
                    user.UserName = dadName;
                    await userManager.CreateAsync(user, dadPassword);
                    await userManager.AddToRoleAsync(user, "first");                   
                }
                else if (await userManager.FindByNameAsync(momName) == null)
                {
                    user.UserName = momName;
                    await userManager.CreateAsync(user, momPassword);
                    await userManager.AddToRoleAsync(user, "second");
                }
            }
        }
    }
}
