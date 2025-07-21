  param 
	(
	[string]$Projectnamepath,
	[string]$dbserver,
	[string]$dbname,
	[string]$dbuser,
	[string]$dbpasProjrd,
	[string]$insightsinstrumentationkey,
	[string]$insightsapplicationid,
	[string]$b2ctenantid,
	[string]$b2cinstance,
	[string]$b2cclientid,
	[string]$b2cdomain,
	[string]$b2cclientsecret,
	[string]$b2csignupsigninpolicyid
	)


# Muestra las variables de entrada
Write-Host "`n Variables de Entrada: `n" 

Write-Host "ProjectNamePath: $Projectnamepath"  
Write-Host "dbserver: $dbserver"  
Write-Host "dbname: $dbname"  
Write-Host "dbuser: $dbuser"  
Write-Host "dbpasProjrd: $dbpasProjrd"  
Write-Host "insightsinstrumentationkey: $insightsinstrumentationkey"  
Write-Host "insightsapplicationid: $insightsapplicationid"  
Write-Host "b2ctenantid: $b2ctenantid" 
Write-Host "b2cinstance: $b2cinstance" 
Write-Host "b2cclientid: $b2cclientid" 
Write-Host "b2cdomain: $b2cdomain" 
Write-Host "b2cclientsecret: $b2cclientsecret" 
Write-Host "b2csignupsigninpolicyid: $b2csignupsigninpolicyid" 

Write-host "Rutas de carpetas"
Get-ChildItem

#INICIO VALIDACION VARIABLES PROYECTO CORE

# Replace Variables en Archivo appsetting.json
$codeVersionFile = "$Projectnamepath\Offer\src\Web.Api\appsettings.json"
Write-Host "Path complete: $codeVersionFile"  
(get-content $codeVersionFile) | foreach-object {
	$_.replace('__dbserver', $dbserver).replace('__dbname', $dbname).replace('__dbuser', $dbuser).replace('__dbpasProjrd', $dbpasProjrd).replace('__insightsinstrumentationkey', $insightsinstrumentationkey).replace('__insightsapplicationid', $insightsapplicationid).replace('__b2ctenantid', $b2ctenantid).replace('__b2cinstance', $b2cinstance).replace('__b2cclientid', $b2cclientid).replace('__b2cdomain', $b2cdomain).replace('__b2cclientsecret', $b2cclientsecret).replace('__b2csignupsigninpolicyid', $b2csignupsigninpolicyid)
} | set-content $codeVersionFile -Encoding UTF8

#Muestra el resultado de reemplazar las variables
Write-Host "`n Resultado variables appsettings: `n" 
get-content -path "$codeVersionFile"

