using Asp.Versioning;
using Tus.Api.Application.Behaviors.ExceptionHandling;
using Tus.Api.StartupExtensions;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

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

#if DEBUG
Debugger.Launch();
#endif

// Use cloud events to automatically unpack the message data
// app.UseCloudEvents();

app.UseApplicationEndpoints();
app.UseDefaultOpenApi();

app.UseHttpsRedirection();
app.UseCors("origin-policy");
app.UseStaticFiles();
app.UseExceptionHandler((builder) =>
{
    builder.Run(async (context) =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature?.Error != null)
        {
            await ExceptionBehavior.OnException(context: context, ex: exceptionHandlerFeature.Error).ConfigureAwait(false);
        }
    });
});

app.Run();

return 0;