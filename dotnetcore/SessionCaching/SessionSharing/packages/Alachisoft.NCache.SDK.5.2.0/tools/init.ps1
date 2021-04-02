param($installPath, $toolsPath, $package, $project)

# Get Project if not specified by the user
if(!$project)
{
	$project = Get-project # -Name TestingNugetsNetCore
}
# Checking if core
$CORE = [bool]0
if($project.Type -eq "C#")
{
	$CORE = [bool]0
}
else	# Assuming its NET Framework by default
{
	$CORE = [bool]1
}
# Get project dir
# ================================================================================================
$projectFullName = $project.FullName
$fileInfo = new-object -typename System.IO.FileInfo -ArgumentList $projectFullName
$projectDirectory = $fileInfo.DirectoryName
write-host "projectDirectory = " $projectDirectory

# Get Install Path if not specified by the user
if(!$installPath)
{
	if($CORE)
	{
		$installPath = "$env:UserProfile\.nuget\packages\alachisoft.ncache.sdk\5.2.0"
	}
	else
	{
		$installPath = "$projectDirectory\..\packages\alachisoft.ncache.sdk.5.2.0"
	}
}

# Debugggin purpose comment after use
# ================================================================================================
write-host "installpath = " $installPath
write-host "toolsPath = " $toolsPath
write-host "package = " $package
write-host "project = " $project
write-host "CORE = " $CORE

# Copy Configs to projectDir
# ================================================================================================
copy-item $installPath\content\net40\client.ncconf $projectDirectory
copy-item $installPath\content\net40\config.ncconf $projectDirectory
copy-item $installPath\content\net40\tls.ncconf $projectDirectory

# Add Item to project
# ================================================================================================
$file = $projectDirectory + '\' + 'config.ncconf'
$project.ProjectItems.AddFromFile($file)
$file = $projectDirectory + '\' + 'client.ncconf'
$project.ProjectItems.AddFromFile($file)
$file = $projectDirectory + '\' + 'tls.ncconf'
$project.ProjectItems.AddFromFile($file)



# Set Config to copy always
# Get Config
# ================================================================================================
$configItem = $project.ProjectItems.Item("config.ncconf")
# Set Config to CopyAlways
$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = [uint32]1

# Get Client Config
# ================================================================================================
$configItem = $project.ProjectItems.Item("client.ncconf")
# Set Config to CopyAlways
$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = [uint32]1


# Get tls Config
# ================================================================================================
$configItem = $project.ProjectItems.Item("tls.ncconf")
# Set Config to CopyAlways
$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = [uint32]1

write-host "Done adding configs."

