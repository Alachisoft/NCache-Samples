param($installPath, $toolsPath, $package, $project)


#Include Config
$configItem = $project.ProjectItems.Item("config.ncconf")
$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")

$configItem2 = $project.ProjectItems.Item("client.ncconf")

#Set Config to CopyAlways
$copyToOutput2 = $configItem2.Properties.Item("CopyToOutputDirectory")


$configItem3 = $project.ProjectItems.Item("tls.ncconf")
#Set Config to CopyAlways
$copyToOutput3 = $configItem3.Properties.Item("CopyToOutputDirectory")

$copyToOutput.Value = 1
$copyToOutput2.Value = 1

