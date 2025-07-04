cls
$base = 'https://localhost:5003'
Invoke-RestMethod -Uri "$base/trading/ping" -Method GET
