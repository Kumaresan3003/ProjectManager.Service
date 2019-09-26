namespace ProjectManager.Service
{
    using System.Buffers;
    using System.Diagnostics.CodeAnalysis;
    using Business;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Repository;

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
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvc(options =>
            {
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }, ArrayPool<char>.Shared));
            });

            services.AddTransient<ITaskManager, TaskManager>();

            services.AddTransient<IProjectManager, ProjectManger>();

            services.AddTransient<IUserManager, UserManager>();

            services.AddTransient<ITaskDetailsRepository, TaskDetailsRepository>();

            services.AddTransient<IProjectDetailsRepository, ProjectDetailsRepository>();

            services.AddTransient<IUserDetailsRepository, UserDetailsRepository>();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ProjectManagerDbContext>(
                    option => option.UseSqlServer(Configuration.GetSection("Database").GetSection("Connection").Value));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [ExcludeFromCodeCoverage]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
