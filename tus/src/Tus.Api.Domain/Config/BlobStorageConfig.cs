namespace Tus.Api.Domain.Config;

public class BlobStorageConfig
{
    public const string SectionName = "BlobStorage";

    public string CdnHostName { get; set; }
}
