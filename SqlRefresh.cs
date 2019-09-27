using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SqlRefresh
{
	public class SqlRefresh
	{
		private readonly IConfiguration _config;

		public SqlRefresh(IConfiguration config)
		{
			 _config = config;
		}

		[FunctionName("Refresh")]
		public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
		{
			log.LogInformation("Logging into Azure");
			var credentials = new AzureCredentialsFactory().FromServicePrincipal(_config["azureClient"], _config["azureSecret"], _config["azureTenant"], AzureEnvironment.AzureGlobalCloud);

			var azure = Azure
				.Configure()
				.WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
				.Authenticate(credentials)
				.WithSubscription(_config["azureSubscription"]);

			// this is a bit stupid that you need to use the full resource id, but it works
			log.LogInformation("Getting SQL Server");			
			string serverId = $"/subscriptions/{_config["azureSubscription"]}/resourceGroups/{_config["azureResourceGroupName"]}/providers/Microsoft.Sql/servers/{_config["sqlServerName"]}";

			var server = await azure.SqlServers.GetByIdAsync(serverId);

			log.LogInformation("Get reference to clean northwind");
			var cleanDB = server.Databases.Get("cleanNorthwind");

			log.LogInformation("Get reference to dirty Northwind");
			var database = server.Databases.Get("Northwind");
			if (database != null)
			{
				log.LogInformation("Deleting dirty Northwind");
				await database.DeleteAsync();
			}

			// Create a new database based on the template database and define your name of your new database
			log.LogInformation("Refreshing Northwind Prod");
			var freshNorthwind = await server.Databases.Define("Northwind")
				.WithSourceDatabase(cleanDB)
				.WithMode(CreateMode.Copy)
				.CreateAsync();

			log.LogInformation("Northwind has been refreshed");
		}
	}
}
