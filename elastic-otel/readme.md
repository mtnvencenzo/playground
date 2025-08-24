0 Elastic OpenTelemetry Collector

This directory contains resources for running an [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/) configured to export telemetry data to [Elastic Observability](https://www.elastic.co/observability). It provides a quick way to collect, process, and forward traces, metrics, and logs from your applications to an Elastic Stack instance for monitoring and analysis.

## 📁 Contents

- **docker.compse.yml**: Docker Compose file to orchestrate the OpenTelemetry Collector and any supporting services.
- **otel-collector-config.yml**: Configuration file for the OpenTelemetry Collector, specifying receivers, processors, and exporters (including Elastic).
- **readme.md**: This documentation file.

## ⚙️ Prerequisites

- [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/) installed on your machine.
- Access to an Elastic Stack instance (Elastic Cloud or self-hosted) with an API key or credentials for ingesting telemetry data.

## 🚀 Setup & Usage

1. **Configure Elastic Exporter:**
	- Edit `otel-collector-config.yml` and update the Elastic exporter section with your Elastic APM Server endpoint and authentication details (API key or username/password).

2. **Start the OpenTelemetry Collector:**
	```bash
	docker compose -p elastic-stack-otel -f docker.compse.yml up -d
	```
	This will start the OpenTelemetry Collector using the provided configuration and group the containers under the project name `elastic-stack-otel` in Docker Desktop and the CLI.

3. **Send Telemetry Data:**
	- Point your application(s) to the OpenTelemetry Collector endpoint (as defined in `otel-collector-config.yml`).
	- The collector will receive, process, and forward telemetry data to your Elastic instance.

4. **Monitor in Elastic:**
	- Log in to your Elastic Observability dashboard to view traces, metrics, and logs.

## 🛠️ Customization

- Modify `otel-collector-config.yml` to add or remove receivers, processors, or exporters as needed for your environment.
- Refer to the [OpenTelemetry Collector documentation](https://opentelemetry.io/docs/collector/configuration/) for advanced configuration options.

## 🐞 Troubleshooting

- Check container logs for errors:
  ```bash
	docker compose -p elastic-stack-otel -f docker.compse.yml logs
  ```
- Ensure network connectivity between the collector and your Elastic instance.
- Validate your credentials and endpoint URLs in the configuration file.

## 📄 License

This project is licensed under the terms of the repository's main LICENSE file.

---
For more information, see the official documentation for [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/) and [Elastic Observability](https://www.elastic.co/guide/en/observability/current/index.html).
