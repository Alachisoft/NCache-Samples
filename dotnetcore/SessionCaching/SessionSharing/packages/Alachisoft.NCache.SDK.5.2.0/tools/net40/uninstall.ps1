param($installPath, $toolsPath, $package, $project)

#Remove the  post build events
$buildEventVal=$project.Properties.Item("PostBuildEvent").Value;
$buildEventVal | out-file D:\temp.txt -append
$buildEventVal=$buildEventVal.Replace("`r`nxcopy `"$installPath\additionalLib\net40\`*.dll`" `"`$`(ProjectDir`)\bin\$`(ConfigurationName`)\`" /Y /I","");
$buildEventVal | out-file D:\temp.txt -append
#remove Oracle.dataAccess if added.
$osType =(Get-WmiObject Win32_OperatingSystem).OSArchitecture;
if($osType -eq "64-bit")
{
	$buildEventVal=$buildEventVal.Replace("`r`nxcopy `"$installPath\additionalLib\Oracle40\x64\Oracle.DataAccess.dll`" `"`$`(ProjectDir`)\bin\$`(ConfigurationName`)\`" /Y /I","");
}
elseif($osType -eq "32-bit")
{
	$buildEventVal=$buildEventVal.Replace("`r`nxcopy `"$installPath\additionalLib\Oracle40\x32\Oracle.DataAccess.dll`" `"`$`(ProjectDir`)\bin\$`(ConfigurationName`)\`" /Y /I","");
}
else
{
	write-host("Error getting OS architecture type.")
}
#replace the modified string
$project.Properties.Item("PostBuildEvent").Value = $buildEventVal;
