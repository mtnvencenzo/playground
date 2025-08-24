namespace Example.Api.Domain.Config;

public class ExampleApiConfig
{
    public const string SectionName = "ExampleApi";

    public string BaseImageUri { get; set; }

    public string BaseOpenApiUri { get; set; }

    public string ApimHostKey { get; set; }
}
