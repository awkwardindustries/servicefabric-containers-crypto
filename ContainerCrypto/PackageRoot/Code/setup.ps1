# Generate a random password. This is used to secure the exported PFX
# and will be added to the generated password file.
[Reflection.Assembly]::LoadWithPartialName("System.Web")
$password = [System.Web.Security.Membership]::GeneratePassword(20,2)

# Find the cert with the thumbprint matching that provided in the
# Custom_CertThumbprint environment variable. Additional
# checks such as finding the one with the latest expiration date
# may be preferred.
$matchedCerts = Get-ChildItem -Path cert:\LocalMachine\My | ?{ $_.Thumbprint -eq "$env:Custom_CertThumbprint" }
$certToExport = $matchedCerts[0]

# Build the destination full paths for the certificate and password
# file using Service Fabric's work directory for the service and
# provided Custom_PfxFileName and Custom_PfxPasswordFileName
# environment variables
$certDestinationPath = Join-Path -Path $env:Fabric_Folder_App_Work -ChildPath $env:Custom_PfxFileName
$passwordDestinationPath = Join-Path -Path $env:Fabric_Folder_App_Work -ChildPath $env:Custom_PfxPasswordFileName

# Write out the plain text password to file
$password | Out-File $passwordDestinationPath

# Export the PFX with private key
$securePassword = ConvertTo-SecureString -String $password -Force –AsPlainText
Export-PfxCertificate -Cert $certToExport -FilePath $certDestinationPath -Password $securePassword -Verbose
