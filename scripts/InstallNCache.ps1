$array=1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30

$fileName="";
$filePath="";
$firstName="";
$lastName="";
$installMode= 0;
$key = "";
$email = "";
$company = "";
$installpath="C:\Program Files\NCache";
$setpermission=1;

$bKey=$false;
$bfileName=$false;
$bfilePath=$false;
$bEmail=$false;
$bFname=$false;
$bLname=$false;
$bCname=$false;

$server_name=get-childitem env:computername;
$servers=$server_name.value;

$i=0;

$args | foreach-object { $array[$i]=$_;   $i++ }

if ($args -contains "-?" -or $args -contains "/?" -or $help)
{
@"
NAME
    InstallNCache
	
SYNOPSIS
    Install NCache on target machines in quite mode 
	
SYNTAX
    IntallNCache.ps1 /k key /f first-name /l last-name /company company-name /file file-name /file-path complete-file-path [option[...]]. 

DETAILED DESCRIPTION
    This script install NCache on multiple machines in quite mode. This script uses win32_Product class
	which is a [WMICLASS].It uses the install method of this class.

PARAMETERS
    /file file-name
        Allows the user to specify the name of the setup file with its extension.
		
    /file-path complete-file-path
        Allows the user to specify the complete file-path/file-location of the setup file.
		
	/f first-name
		Allows the user to specify the his first name.
	
    /l last-name
		Allows the user to specify the his last name.
	
	/company company-name
		Allows the user to specify the company name.
	
    /k key
        Allows the user to specify a valid license key.
	    
	/email email-address
		Allows the user to specify the his email address.
OPTIONS	
	
    /s server-names
	Allows the user to specify a comma separated  list of names or IPs of the machines
	on which NCache is to be install.
	
    /i install-mode
	Allows the user to specify the edition of NCache. Default is "0".
	Specify "0" for Server Edition.
	Specify "2" for Developer Edition.
	Specify "3" for Client Edition.
	
    /company company-name
	Allows the user to specify the company name.
	
    /install-dir installation-path
	Allows the user to specify the path where to install NCache. Default is "C:\Program Files\NCache".
	
    /set-per permissions
	Allows the user to specify the permissions for quite mode. Default is "1".
	
	/? 
	Show this help information.

"@
exit 1
}


$i=0;

while($i -lt $args.length)
{
 switch ($array[$i])
  { 
	"/s"         	{$servers=$array[$i+1]; $i++; $i++; }
	
	"/k" 	     	{$key=$array[$i+1];$i++; $bKey=$true;}
	"/key"  		{$key=$array[$i+1];$i++; $bKey=$true;}
	
	"/f" 	        {$firstName=$array[$i+1];$i++;$bFname=$true; }
	"/first-name"   {$firstName=$array[$i+1];$i++;$bFname=$true; }
	
	"/l" 	     	{$lastName=$array[$i+1];$i++;$bLname=$true; }
	"/last-name"    {$lastName=$array[$i+1];$i++;$bLname=$true; }
	
	"/i" 	        {$installMode=$array[$i+1];$i++; }
	"/install-mode"      {$installMode=$array[$i+1];$i++; }
	
	"/file" 	    {$fileName=$array[$i+1];$i++; $bfileName=$true;}
	"/file-name"    {$fileName=$array[$i+1];$i++; $bfileName=$true;}
	
	"/file-path" 	{$filePath=$array[$i+1];$i++; $bfilePath=$true;}
	
	"/email" 	    {$email=$array[$i+1];$i++; $bEmail=$true;}
	
	"/company"      {$company=$array[$i+1];$i++;$bCname=$true; }
	
	"/install-dir" 	{$installpath=$array[$i+1];$i++; }
	
	"/set-per"      {$setpermission=$array[$i+1];$i++; }
	
	default {$i++;}
  }
}

$Options = "INSTALLMODE=""$installMode"" KEY=""$key"" USERFIRSTNAME=""$firstName"" USERLASTNAME=""$lastName"" COMPANYNAME=""$company"" EMAILADDRESS=""$email"" INSTALLDIR=""$installpath"" SETPERMISSION=""$setpermission"""

if ($bfileName) 
{
	if($bfilePath)
    {
		if ($bKey) 
		{
			if ($bEmail)
			{	
				if ($bFname)
				{
					if($bLname)
					{
						if($bCname)
						{
							$i=0;
							$check = $servers -is [array] ;
							if($check)
							{
								while($i -lt $servers.length)
								{
										$System = $servers[$i];
										echo "Installing NCache on Machine $System";
										echo "Status:"									
										$Setup_obj = [WMICLASS]"\\$System\ROOT\CIMV2:win32_Product"
										copy-item "$filePath\$fileName" \\$System\c$\ -force
										$Setup_obj.install("\\$System\C$\$fileName","$Options",$true)
										$i++;						
								}
							}
							else
							{		    
								$System = $servers;
								echo "installing NCache on Machine $System";
								echo "Status:"						
								$Setup_obj = [WMICLASS]"\\$System\ROOT\CIMV2:win32_Product"
								#copy-item "$path\$f" \\$System\c$\ -force
								copy-item "$filePath\$fileName" \\$System\c$\ -force			
								$Setup_obj.install("\\$System\C$\$fileName","$Options",$true)
							}
						}
						else {write-host "Argument '/company' for company-name is invalid or missing" -foregroundcolor  RED;}
					}
					else {write-host "Argument '/l' for last-name is invalid or missing" -foregroundcolor  RED;}
				}
				else {write-host "Argument '/f' for first-name is invalid or missing" -foregroundcolor  RED;}
			}
			else {write-host "Argument '/email' for email is invalid or missing" -foregroundcolor  RED;}
		}
		else {write-host "Argument '/k' for key is invalid or missing" -foregroundcolor  RED;}
	}
	else {write-host "Argument '/file-path' for file path is invalid or missing" -foregroundcolor  RED;}
}
else {write-host "Argument '/file' for file name is invalid or missing" -foregroundcolor  RED;}
