namespace Tus.Api.Infrastructure;

using System.Text;

public static class ExtensionMethods
{
    public static string FromBytes(this byte[] bytes) => Encoding.UTF8.GetString(bytes);
}
