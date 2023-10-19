using BusinessLayer.Container;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using TraversalCoreProje.CQRS.Handlers.DestinationHandlers;
using TraversalCoreProje.Models;

namespace YourNamespace
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// Bu method, hizmetleri ve bağımlılıkları yapılandırmak için kullanılır.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<GetAllDestinationQueryHandler>();

			services.AddLogging(x =>
			{
				x.ClearProviders();
				x.SetMinimumLevel(LogLevel.Debug);
				x.AddDebug();
			});

			services.AddRazorPages()
				.AddRazorRuntimeCompilation();

			services.AddDbContext<Context>();
			services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<Context>().AddErrorDescriber<CustomIdentityValidator>().AddEntityFrameworkStores<Context>();

			services.AddHttpClient();

			services.ContainerDependencies();

			services.AddAutoMapper(typeof(Startup));

			services.CustomValidator();

			services.AddControllersWithViews().AddFluentValidation();

            services.AddMvc(config =>
			{
				var policy = new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser()
				.Build();
				config.Filters.Add(new AuthorizeFilter(policy));
			});

			services.AddMvc();

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Login/SignIn/";
			});
		}

		// Bu method, HTTP istekleri için bir HTTP pipeline'ı yapılandırmak için kullanılır.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			var path = Directory.GetCurrentDirectory();
			loggerFactory.AddFile($"{path}\\Logs\\Log1.txt");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404", "?code={0}");

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Default}/{action=Index}/{id?}");
			});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
	}
}
