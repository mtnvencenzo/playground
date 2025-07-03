namespace Tus.Api.Application.Concerns.TusUpload.Commands;

using MediatR;
using System.Net;
using tusdotnet.Models.Configuration;

public class BeforeCreateTusDocumentCommand(BeforeCreateContext context) : IRequest<Unit>
{
    public BeforeCreateContext Context { get; } = context;
}

public class BeforeCreateTusDocumentCommandHandler : IRequestHandler<BeforeCreateTusDocumentCommand, Unit>
{
    public Task<Unit> Handle(BeforeCreateTusDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!request.Context.Metadata.ContainsKey("name"))
        {
            request.Context.FailRequest(HttpStatusCode.UnprocessableEntity, "Name is required");
        }

        return Task.FromResult(Unit.Value);
    }
}