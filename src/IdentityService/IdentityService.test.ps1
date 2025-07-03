cls
$base = 'https://localhost:5001'
Invoke-RestMethod -Uri "$base/auth/register" -Method POST -ContentType 'application/json' -Body (@{ email = 'alice@example.com'; password = 'P@ssw0rd!' } | ConvertTo-Json)
$loginResponse = Invoke-RestMethod -Uri "$base/auth/login" -Method POST -ContentType 'application/json' -Body (@{ email = 'alice@example.com'; password = 'P@ssw0rd!' } | ConvertTo-Json)
Invoke-RestMethod -Uri "$base/ping" -Method GET
Invoke-RestMethod -Uri "$base/sping" -Method GET -Headers @{ Authorization = "Bearer $($loginResponse.token)" }
