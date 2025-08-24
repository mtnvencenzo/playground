namespace Example.Api.Application.Behaviors.Telemetry;

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;

public class SyntheticRequestTelemetryFilter(ITelemetryProcessor next) : ITelemetryProcessor
{
    private readonly ITelemetryProcessor next = next;

    public void Process(ITelemetry item)
    {
        if (!string.IsNullOrEmpty(item.Context.Operation.SyntheticSource))
        {
            return;
        }

        if (item is RequestTelemetry request && request?.Url?.LocalPath != null)
        {
            if (ExcludePaths.Contains(request.Url.LocalPath.ToLower()))
            {
                return;
            }
        }

        // Send everything else:
        this.next.Process(item);
    }

    public static IList<string> ExcludePaths => ["", "/", "/diagnostics/server"];
}