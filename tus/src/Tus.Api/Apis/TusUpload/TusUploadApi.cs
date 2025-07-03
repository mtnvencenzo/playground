namespace Tus.Api.Apis.TusUpload;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Stores;
using tusdotnet.Models.Configuration;
using MediatR;
using Tus.Api.Application.Concerns.TusUpload.Commands;

public static class TusUploadApi
{
    private readonly static string TemporaryFilePath = Path.Combine(Path.GetTempPath(), "tus-file-uploads");

    public static RouteGroupBuilder MapTusApiV1(this IEndpointRouteBuilder app)
    {
        Directory.CreateDirectory(TemporaryFilePath);

        var groupBuilder = app.MapGroup("/file-uploads")
            .WithTags("Tus")
            .AllowAnonymous();

        groupBuilder.MapTus("/", UploadFile)
            .WithName(nameof(UploadFile))
            .WithDisplayName(nameof(UploadFile))
            .WithDescription("Uploads resumeable files");

        return groupBuilder;
    }

    public static Task<DefaultTusConfiguration> UploadFile(HttpContext context)
    {
        var sender = context.RequestServices.GetRequiredService<ISender>();

        return Task.FromResult(new DefaultTusConfiguration
        {
            Store = new TusDiskStore(TemporaryFilePath),
            Events = new Events
            {
                OnBeforeCreateAsync = async (context) => await sender.Send(new BeforeCreateTusDocumentCommand(context), context.CancellationToken),
                OnFileCompleteAsync = async (context) => await sender.Send(new TusDocumentCompleteCommand(context), context.CancellationToken)
            }
        });
    }
}
