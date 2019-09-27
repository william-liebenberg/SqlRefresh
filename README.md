# SqlRefresh

On a schedule, refresh the target database using a clean source database.

NOTE 1: The target database gets DELETED before the source database is copied into a new/fresh target.

NOTE 2: Sample `TimerTrigger` is set for every 5 minutes. Change this to hourly or daily if you want to use it for real.

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
    "azureSubscription": "<YOUR AZURE SUBSCRIPTION ID>"
  }
}
```
