# Terraform IaC for MarketTrading

This folder provisions the minimal Azure resources needed by the MarketTrading services.

Resources created:
- Resource Group
- Azure Event Hubs Namespace (Standard)
- Azure Event Hub for market data
- Event Hub Consumer Group (marketdata-ui)
- Send and Listen SAS policies for the Event Hub

Outputs include connection strings that can be placed in the services' configuration.

## Prerequisites
- Terraform v1.5+
- Azure CLI logged in: `az login`
- Appropriate Azure subscription selected: `az account set --subscription <SUB_ID>`

## Quick start

```bash
cd infra/terraform
terraform init
terraform plan -var "project=markettrading" -var "environment=dev"
terraform apply -auto-approve -var "project=markettrading" -var "environment=dev"
```

After apply, note the outputs:
- `eventhub_producer_connection_string` -> use in the producer (MarketDataIngestionService)
- `eventhub_consumer_connection_string` -> use in the consumer (MarketDataRelayService)
- `eventhub_name` -> the Event Hub name

## Variables
- `project` (string): Short name of the project, used in resource names.
- `environment` (string): Environment suffix, e.g. `dev`, `test`, `prod`.
- `location` (string): Azure region, default `westeurope`.
- `eventhub_name` (string): Name of the Event Hub, default `marketdata`.
- `eh_partitions` (number): Number of partitions, default `4`.

## Clean up

```bash
terraform destroy -auto-approve -var "project=markettrading" -var "environment=dev"
```
