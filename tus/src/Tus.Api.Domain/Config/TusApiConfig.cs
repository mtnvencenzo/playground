namespace Tus.Api.Domain.Config;

public class TusApiConfig
{
    public const string SectionName = "TusApi";

    public string BaseOpenApiUri { get; set; } = string.Empty;

    public string DocumentIdHeaderName { get; set; } = "x-tus-docid";
}
