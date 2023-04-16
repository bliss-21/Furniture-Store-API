using API.FurnitureStore.API.Configuration;
using API.FurnitureStore.API.Services;
using API.FurnitureStore.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using NLog;
using NLog.Web;

// Nlog Init
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    #region [Swagger Config]
    builder.Services.AddSwaggerGen(x =>
    {
        x.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Furniture_store_API",
            Version = "v1"
        });

        x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer cheme. Enter prefix (Bearer), space, and then yout Token. Example: 'Bearer 123456abcdef'"
        });

        x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });
    #endregion

    #region [Bd SQLite]
    builder.Services.AddDbContext<APIFurnitureStoreContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("APIFurnitureStoreContext")));
    #endregion

    #region[Smtp Email]
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
    builder.Services.AddSingleton<IEmailSender, EmailService>();
    #endregion

    #region [Autentificacion jwt]
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
    var tokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, //valida quien emitio el token, queda en falso porque es un entorno de desarrollo
        ValidateAudience = false, //valida que la audiencia sea el mismo que el que lo recibe 
        RequireExpirationTime = false,
        ValidateLifetime = true
    };

    builder.Services.AddSingleton(tokenValidationParameters);
    builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(jwt =>
        {
            jwt.SaveToken = true;//nos permite decirle al Identity Framework que almacene el token una vez que la autenticacion sea exitosa
            jwt.TokenValidationParameters = tokenValidationParameters;
        });

    //señalamos que modelo de usuario usara
    builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            options.SignIn.RequireConfirmedAccount = false) //La validacion que tendra que hacer
        .AddEntityFrameworkStores<APIFurnitureStoreContext>();// señalamos que contexto tiene que usar


    #endregion

    #region [NLog Config]
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #endregion

    var app = builder.Build();

    #region [Init data BD]
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<APIFurnitureStoreContext>();
        context.Database.Migrate();
        APIFurnitureStoreContext.SetInitialize(context, dataFake:true);
    }
    #endregion

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseReDoc(options =>
        {
            options.DocumentTitle = "Swagger Demo Documentation";
            options.SpecUrl = "/swagger/v1/swagger.json";
        });
    }

    app.UseReDoc(c =>
    {
        c.RoutePrefix = "docs";
        c.EnableUntrustedSpec();
        c.ScrollYOffset(10);
        c.HideHostname();
        c.HideDownloadButton();
        c.ExpandResponses("200,201");
        c.RequiredPropsFirst();
        c.NoAutoAuth();
        c.PathInMiddlePanel();
        c.HideLoading();
        c.NativeScrollbars();
        c.DisableSearch();
        c.OnlyRequiredInSamples();
        c.SortPropsAlphabetically();
    });

    app.UseHttpsRedirection();

    app.UseAuthentication();//[Autentificacion jwt]
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception e)
{
    logger.Error(e, "There has been an error");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

