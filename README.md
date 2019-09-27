# SqlRefresh

This sample uses Azure Functions `[TimerTrigger]` and [Azure Management Libraries for .NET](https://github.com/Azure/azure-libraries-for-net) to refresh a target database using a clean source.

NOTE 1: The target database has to be DELETED before the source database is copied into a new/fresh target.

NOTE 2: Sample `TimerTrigger` is set for every 5 minutes. Change this to hourly (0 * * * *) or daily at midnight (0 0 * * *) if you want to use it for real.

## Getting Started

Add your own `local.settings.json` file with contents like this:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "azureClient": "<SERVICE PRINCIPAL CLIENT ID>",
    "azureSecret": "<SERVICE PRINCIPAL SECRET>",
    "azureTenant": "<YOUR AZURE TENANT ID>",
    "azureSubscription": "<YOUR AZURE SUBSCRIPTION ID>",
	"azureResourceGroupName": "<RESOURCE GROUP WHERE THE SQL SERVER WAS CREATED>",
    "sqlServerName": "<NAME OF THE SQL SERVER THAT HOLDS YOUR DATABASES>"
  }
}
```
