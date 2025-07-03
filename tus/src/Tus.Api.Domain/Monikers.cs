namespace Tus.Api.Domain;

using Cezzi.Applications.Logging;

public static class Monikers
{
    static Monikers()
    {
        App = new AppMonikers();
        Api = new ApiMonikers();
        ServiceBus = new ServiceBusMonikers();
        Tus = new TusMonikers();
        Azure = new AzMonikers();
    }

    public static AppMonikers App { get; }

    public static ApiMonikers Api { get; }

    public static AzMonikers Azure { get; }

    public static ServiceBusMonikers ServiceBus { get; }

    public static TusMonikers Tus { get; }
}
