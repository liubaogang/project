param($installPath, $toolsPath, $package, $project)

$configItem = $project.ProjectItems.Item("NLog.config")

$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = 0

$buildAction = $configItem.Properties.Item("BuildAction")
$buildAction.Value = 2

$configItem = $project.ProjectItems.Item("Cicada.properties")

$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = 0

$buildAction = $configItem.Properties.Item("BuildAction")
$buildAction.Value = 2

