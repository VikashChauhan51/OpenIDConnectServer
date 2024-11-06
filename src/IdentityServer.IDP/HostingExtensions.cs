using Duende.IdentityServer;
using IdentityServer.IDP;
using IdentityServer.IDP.DbContexts;
using IdentityServer.IDP.Entities;
using IdentityServer.IDP.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Serilog;

namespace IdentityServer.IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(
               builder.Configuration
               .GetConnectionString("IdentityServerDatabase"),
               dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));
        });

        builder.Services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var isBuilder = builder.Services.AddIdentityServer(options =>
            {
                options.Caching.ClientStoreExpiration = TimeSpan.FromMinutes(5);
                options.Caching.ResourceStoreExpiration = TimeSpan.FromMinutes(5);

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<ApplicationUser>()
             .AddConfigurationStore(options =>
             {
                 options.ConfigureDbContext = opt =>
                 opt.UseSqlite(builder.Configuration
               .GetConnectionString("IdentityServerDatabase"),
                         sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
             })
             .AddConfigurationStoreCache()
             .AddOperationalStore(options =>
             {
                 options.ConfigureDbContext = opt =>
                 opt.UseSqlite(builder.Configuration
               .GetConnectionString("IdentityServerDatabase"),
                         sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
                 // this enables automatic token cleanup. this is optional.
                 options.EnableTokenCleanup = true;
                 options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
             })
             .AddDeveloperSigningCredential();



        // if you want to use server-side sessions: https://blog.duendesoftware.com/posts/20220406_session_management/
        // then enable it
        isBuilder.AddServerSideSessions();
        //
        // and put some authorization on the admin/management pages
        //builder.Services.AddAuthorization(options =>
        //       options.AddPolicy("admin",
        //           policy => policy.RequireClaim("sub", "1"))
        //   );
        //builder.Services.Configure<RazorPagesOptions>(options =>
        //    options.Conventions.AuthorizeFolder("/ServerSideSessions", "admin"));


        builder.Services
           .AddAuthentication()
           .AddOpenIdConnect("AAD", "Azure Active Directory", options =>
           {
               options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
               options.Authority = "https://login.microsoftonline.com/621cb4b2-eeb8-4699-913d-a651c392babd/v2.0";
               options.ClientId = "df55658d-e228-4f72-9f11-b60334edb0e2";
               options.ClientSecret = "Tkt8Q~gJ9yez1EGA6BqFJfCVWIzM_B.bCAUx8bkB";
               options.ResponseType = "code";
               options.CallbackPath = new PathString("/signin-aad/");
               options.SignedOutCallbackPath = new PathString("/signout-aad/");
               options.Scope.Add("email");
               options.Scope.Add("offline_access");
               options.SaveTokens = true;
           });
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            });

        builder.Services.AddAuthentication()
            .AddFacebook("Facebook",
               options =>
               {
                   options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                   options.AppId = "864396097871039";
                   options.AppSecret = "11015f9e340b0990b0e50f39dd8a4e9a";
               });

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
              | ForwardedHeaders.XForwardedProto;
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}