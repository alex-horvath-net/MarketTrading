param(
    [switch]$Upgrade
)

# Always run in the folder of this script
Set-Location -Path $PSScriptRoot

# Ensure Terraform is available
$tf = Get-Command terraform -ErrorAction SilentlyContinue
if (-not $tf) {
    Write-Error "Terraform is not installed or not in PATH. Install it (e.g., winget install Hashicorp.Terraform) and re-run."
    exit 1
}

Write-Host "Terraform version:" -ForegroundColor Cyan
terraform -version

$initArgs = @('init','-input=false')
if ($Upgrade) { $initArgs += '-upgrade' }

Write-Host "Running: terraform $($initArgs -join ' ')" -ForegroundColor Cyan
terraform @initArgs

if ($LASTEXITCODE -ne 0) {
    Write-Error "terraform init failed with exit code $LASTEXITCODE"
    exit $LASTEXITCODE
}

Write-Host "terraform init completed successfully." -ForegroundColor Green
