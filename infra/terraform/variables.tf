variable "project" {
  description = "Short project name used in resource names"
  type        = string
}

variable "environment" {
  description = "Environment suffix (dev/test/prod)"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "westeurope"
}

variable "eventhub_name" {
  description = "Event Hub name"
  type        = string
  default     = "marketdata"
}

variable "eh_partitions" {
  description = "Number of Event Hub partitions"
  type        = number
  default     = 4
}
