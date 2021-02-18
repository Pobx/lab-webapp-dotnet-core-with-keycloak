using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace myapp {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {

      services.AddAuthentication (options => {
          options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie (CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect (OpenIdConnectDefaults.AuthenticationScheme, (options) => {
          options.Authority = "your-authority";
          options.ClientId = "your-client-id";
          options.ClientSecret = "your-client-secret";
          options.ResponseType = OpenIdConnectResponseType.Code;
          options.RequireHttpsMetadata = false;
          options.SaveTokens = true;
          options.SignedOutRedirectUri = "http://localhost:5000";

          options.Events = new OpenIdConnectEvents {
            OnTokenResponseReceived = context => {
              var ticket = context.ProtocolMessage.AccessToken;
              var json = JsonConvert.SerializeObject (context.ProtocolMessage, Formatting.Indented);
              Console.WriteLine (json);
              return Task.CompletedTask;
            }
          };

        });

      services.AddMvc ()
        .AddRazorPagesOptions (options => {
          options.Conventions.AuthorizePage ("/Privacy");
          options.Conventions.AllowAnonymousToPage ("/index");
        })
        .SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      } else {
        app.UseExceptionHandler ("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts ();
      }

      // app.UseHttpsRedirection ();
      app.UseStaticFiles ();
      app.UseCookiePolicy ();
      app.UseAuthentication ();

      app.UseMvc ();
    }
  }
}