cls
$base = 'http://localhost:5002'
Invoke-RestMethod -Uri "$base/ping" -Method GET
