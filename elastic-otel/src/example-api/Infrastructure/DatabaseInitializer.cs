namespace Example.Api.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Example.Api.Domain.Config;
using System.Threading.Tasks;
using System;

public class DatabaseInitializer(
    IOptions<CosmosDbConfig> config,
    HealthDbContext exampleDbContext,
    ILogger<DatabaseInitializer> logger)
{
    public async Task InitializeAsync()
    {
        try
        {
            logger.LogInformation("Starting database initialization for database: {DatabaseName}", config.Value.DatabaseName);

            var exampleCosmosClient = exampleDbContext.Database.GetCosmosClient();

            // Create database if it doesn't exist
            try
            {
                logger.LogInformation("Creating database if it doesn't exist...");
                var exampleDatabase = await exampleCosmosClient.CreateDatabaseIfNotExistsAsync(config.Value.DatabaseName);
                logger.LogInformation("Database created/verified: {DatabaseId}", exampleDatabase.Database.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating database: {Message}", ex.Message);
                throw;
            }

            // Create containers if they don't exist
            logger.LogInformation("Creating containers if they don't exist...");

            try
            {
                var exampleContainer = await exampleCosmosClient
                    .GetDatabase(config.Value.DatabaseName)
                    .CreateContainerIfNotExistsAsync(new ContainerProperties
                    {
                        Id = "health",
                        PartitionKeyPath = "/subjectId",
                        PartitionKeyDefinitionVersion = PartitionKeyDefinitionVersion.V2
                    });
                logger.LogInformation("Health container created/verified: {ContainerId}", exampleContainer.Container.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating health container: {Message}", ex.Message);
                throw;
            }

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database initialization");
            throw;
        }
    }
}