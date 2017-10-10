using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace ServerlessSpaWithDotNet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // add our Cognito group authorization requirement, specifying CalendarWriter as the group
            services.AddAuthorization(
                options => options.AddPolicy("InCalendarWriterGroup", policy => policy.Requirements.Add(new CognitoGroupAuthorizationRequirement("CalendarWriter")))
            );
            // add a singleton of our cognito authorization handler
            services.AddSingleton<IAuthorizationHandler, CognitoGroupAuthorizationHandler>();
            
            services.AddMvc();

            // Pull in any SDK configuration from Configuration object
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLambdaLogger(Configuration.GetLambdaLoggerOptions());

            app.UseDefaultFiles();  //needs to be before the app.UseStaticFiles() call below
            app.UseStaticFiles();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Audience = "5uibfabea1gvq8t8ivou1bge50",
                Authority = "https://cognito-idp.us-west-2.amazonaws.com/us-west-2_PoTkPSgPb",
                AutomaticAuthenticate = true,
                RequireHttpsMetadata = false    //for dev only, for production the JWT token should only be sent via https
            });

            app.UseMvc();
        }
    }
}
