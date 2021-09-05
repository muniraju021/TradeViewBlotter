$base_url = "http://localhost:8090/api/v1"
Try
{
  write-Host "Executing Health Check - Starting.. $base_url/healthcheck"
  Invoke-RestMethod -Uri "$base_url/healthcheck"
}
catch
{
  write-Host "Error calling in Health Check Script"
  $ErrorMessage = $_.Exception.Message
  $InnerException = $_.Exception.$InnerException
  $StackTraces = $_.Exception.StackTrace
  write-Host "Error: $ErrorMessage $InnerException $StackTraces"
}