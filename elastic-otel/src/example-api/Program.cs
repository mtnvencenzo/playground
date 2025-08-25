using Asp.Versioning;
using Example.Api.Application.Behaviors.ExceptionHandling;
using Example.Api.StartupExtensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddApplicationServices();

var apiVersioningBuilder = builder.Services.AddApiVersioning((o) =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});

builder.AddDefaultOpenApi(apiVersioningBuilder);

// -------------
// build the app
// -------------
var app = builder.Build();

app.UseApplicationEndpoints();
app.UseDefaultOpenApi();

// Not requiring the dev cert for open api locally
// Had issues with cert trust on ubuntu for some reason.
if (app.Environment.IsEnvironment("local"))
{
    app.UseWhen(context =>
    {
        return !context.Request.Path.Equals("/scalar/v1/openapi.json");
    }, appBuilder =>
    {
        appBuilder.UseHttpsRedirection();
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("origin-policy");
app.UseStaticFiles();
app.UseExceptionHandler((builder) =>
{
    builder.Run(async (context) =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature?.Error != null)
        {
            await ExceptionBehavior.OnException(context: context, ex: exceptionHandlerFeature.Error);
        }
    });
});

app.Run();

return 0;