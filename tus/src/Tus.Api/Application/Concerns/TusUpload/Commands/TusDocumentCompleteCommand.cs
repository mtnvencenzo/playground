namespace Tus.Api.Application.Concerns.TusUpload.Commands;

using MediatR;
using tusdotnet.Models.Configuration;

public class TusDocumentCompleteCommand(FileCompleteContext context) : IRequest<Unit>
{
    public FileCompleteContext Context { get; } = context;
}

public class TusDocumentCompleteCommandHandler : IRequestHandler<TusDocumentCompleteCommand, Unit>
{
    public Task<Unit> Handle(TusDocumentCompleteCommand request, CancellationToken cancellationToken) => Task.FromResult(Unit.Value);
}