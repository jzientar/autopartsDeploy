# # PostInstallSetup.ps1 # 

$sharepath = "C:\inetpub\AspNetCoreWebApps\app\Images" $Acl = Get-ACL $SharePath $AccessRule= New-Object System.Security.AccessControl.FileSystemAccessRule("DefaultAppPool","full","ContainerInherit,Objectinherit","none","Allow") $Acl.AddAccessRule($AccessRule) Set-Acl $SharePath $Acl