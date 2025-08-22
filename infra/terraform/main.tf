locals {
  name_prefix = "${var.project}-${var.environment}"
}

resource "azurerm_resource_group" "rg" {
  name     = "${local.name_prefix}-rg"
  location = var.location
}

# Random suffix for globally-unique Event Hubs namespace names
resource "random_string" "suffix" {
  length  = 4
  upper   = false
  special = false
}

resource "azurerm_eventhub_namespace" "ehns" {
  name                = "${local.name_prefix}-ehns-${random_string.suffix.result}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
  capacity            = 1
  tags = {
    project     = var.project
    environment = var.environment
  }
}

resource "azurerm_eventhub" "eh" {
  name                = var.eventhub_name
  namespace_name      = azurerm_eventhub_namespace.ehns.name
  resource_group_name = azurerm_resource_group.rg.name
  partition_count     = var.eh_partitions
  message_retention   = 1
}

# Consumer group for UI
resource "azurerm_eventhub_consumer_group" "cg_ui" {
  name                = "marketdata-ui"
  namespace_name      = azurerm_eventhub_namespace.ehns.name
  eventhub_name       = azurerm_eventhub.eh.name
  resource_group_name = azurerm_resource_group.rg.name
}

# Shared access policies
resource "azurerm_eventhub_authorization_rule" "producer" {
  name                = "producer-send"
  namespace_name      = azurerm_eventhub_namespace.ehns.name
  eventhub_name       = azurerm_eventhub.eh.name
  resource_group_name = azurerm_resource_group.rg.name
  send                = true
}

resource "azurerm_eventhub_authorization_rule" "consumer" {
  name                = "consumer-listen"
  namespace_name      = azurerm_eventhub_namespace.ehns.name
  eventhub_name       = azurerm_eventhub.eh.name
  resource_group_name = azurerm_resource_group.rg.name
  listen              = true
}

# Outputs for connection strings
output "eventhub_name" {
  value = azurerm_eventhub.eh.name
}

# Authorization rule data sources to expose connection strings
data "azurerm_eventhub_authorization_rule" "producer" {
  name                = azurerm_eventhub_authorization_rule.producer.name
  eventhub_name       = azurerm_eventhub.eh.name
  namespace_name      = azurerm_eventhub_namespace.ehns.name
  resource_group_name = azurerm_resource_group.rg.name
}

data "azurerm_eventhub_authorization_rule" "consumer" {
  name                = azurerm_eventhub_authorization_rule.consumer.name
  eventhub_name       = azurerm_eventhub.eh.name
  namespace_name      = azurerm_eventhub_namespace.ehns.name
  resource_group_name = azurerm_resource_group.rg.name
}

output "eventhub_producer_connection_string" {
  sensitive = true
  value     = data.azurerm_eventhub_authorization_rule.producer.primary_connection_string
}

output "eventhub_consumer_connection_string" {
  sensitive = true
  value     = data.azurerm_eventhub_authorization_rule.consumer.primary_connection_string
}
