using EntityRelationAPI.DataLayer.Repository;
using EntityRelationAPI.Services.Interface;
using EntityRelationAPI.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EntityRelationAPI.Web
{
    public class Startup
    {
        private readonly string _corsPolicyName = "AllowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(x =>
            {
                x.AddPolicy(_corsPolicyName, builder =>
                {
                    builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    builder.WithOrigins("http://localhost:8081").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    builder.WithOrigins("http://localhost:8082").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddDbContext<EntityRelationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Default")));
            
            services.AddScoped<IDbContext, EntityRelationDbContext>();
            services.AddScoped<IEntityService, EntityService>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo() {Title = "EntityRelation API", Version = "v1"});
            });

            services.AddMvc(config => {
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "EntityRelation API V1");
                x.RoutePrefix = string.Empty;
            });

            app.UseCors(_corsPolicyName);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
