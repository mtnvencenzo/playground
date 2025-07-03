namespace Tus.Api.Application.Concerns.TusUpload.Commands;

using MediatR;
using Microsoft.Extensions.Options;
using Tus.Api.Domain.Config;
using tusdotnet.Models.Configuration;

public class TusDocumentCompleteCommand(FileCompleteContext context) : IRequest<Unit>
{
    public FileCompleteContext Context { get; } = context;
}

public class TusDocumentCompleteCommandHandler : IRequestHandler<TusDocumentCompleteCommand, Unit>
{
    public async Task<Unit> Handle(TusDocumentCompleteCommand request, CancellationToken cancellationToken)
    {
        var options = request.Context.HttpContext.RequestServices.GetRequiredService<IOptions<TusApiConfig>>().Value
            ?? throw new InvalidOperationException($"{nameof(TusApiConfig)} is not configured");

        var file = await request.Context.GetFileAsync();
        var fileContent = await file.GetContentAsync(cancellationToken);
        var metadata = await file.GetMetadataAsync(cancellationToken);

        var uploadId = Guid.NewGuid().ToString();
        request.Context.HttpContext.Response.Headers.Append(options.DocumentIdHeaderName, uploadId);

        return Unit.Value;
    }
}