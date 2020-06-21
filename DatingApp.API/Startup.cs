using System.Net;
using System.Text;
using DatingApp.API.Data;
using DatingApp.API.Helper;
using DatingApp.API.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;


namespace DatingApp.API
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
            services.AddDbContext<DataContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefualtConnectionString")));
            services.AddControllers();
            services.AddCors();
            //Here we have three a) AddSinglton b) AddTransient c)AddScope 
            //Singllton will create only one instance of the object in entier lifcyce of an application 
            //Transient will create instance for each request 
            //Will create instance for session 
            services.AddScoped<IAuthRepo, AuthRepo>();
            //Add authentication and authorization 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSetting:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
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
                //Middle layer for global exception handler
                app.UseExceptionHandler(builder =>
               {
                   builder.Run(async handler =>
                   {
                       handler.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                       var error = handler.Features.Get<IExceptionHandlerFeature>();
                       if (error != null)
                       {
                           handler.Response.AddApplicationError(error.Error.Message);
                           await handler.Response.WriteAsync(error.Error.Message);
                       }
                   });

               });
            }
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
