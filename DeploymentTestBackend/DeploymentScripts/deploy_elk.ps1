
$guid1 = [guid]::NewGuid()
$guid2 = [guid]::NewGuid()
$guid3 = [guid]::NewGuid()
$guid4 = [guid]::NewGuid()
$name = "<name>"

Import-Module virtualmachinemanager
Get-VMMServer bajor

New-SCVirtualScsiAdapter -VMMServer bajor -JobGroup $guid1 -AdapterID 7 -ShareVirtualScsiAdapter $false -ScsiControllerType DefaultTypeNoType 


New-SCVirtualDVDDrive -VMMServer bajor -JobGroup $guid1 -Bus 0 -LUN 1 

$VMNetwork = Get-SCVMNetwork -VMMServer bajor -Name "Trust" -ID "3610244d-0727-4239-84e0-f41496cc4e30"

New-SCVirtualNetworkAdapter -VMMServer bajor -JobGroup $guid1 -MACAddressType Dynamic -VirtualNetwork "TrustSwitch" -VLanEnabled $false -Synthetic -EnableVMNetworkOptimization $false -EnableMACAddressSpoofing $false -EnableGuestIPNetworkVirtualizationUpdates $false -IPv4AddressType Dynamic -IPv6AddressType Dynamic -VMNetwork $VMNetwork -DevicePropertiesAdapterNameMode Disabled 

$CPUType = Get-SCCPUType -VMMServer bajor | where {$_.Name -eq "3.60 GHz Xeon (2 MB L2 cache)"}

New-SCHardwareProfile -VMMServer bajor -CPUType $CPUType -Name $guid4 -Description "Profile used to create a VM/Template" -CPUCount 4 -MemoryMB 2048 -DynamicMemoryEnabled $true -DynamicMemoryMinimumMB 512 -DynamicMemoryMaximumMB 8192 -DynamicMemoryBufferPercentage 20 -MemoryWeight 5000 -CPUExpectedUtilizationPercent 20 -DiskIops 0 -CPUMaximumPercent 100 -CPUReserve 0 -NumaIsolationRequired $false -NetworkUtilizationMbps 0 -CPURelativeWeight 100 -HighlyAvailable $false -DRProtectionRequired $false -SecureBootEnabled $true -SecureBootTemplate "MicrosoftWindows" -CPULimitFunctionality $false -CPULimitForMigration $false -CheckpointType Production -Generation 2 -JobGroup $guid1 



$Template = Get-SCVMTemplate -VMMServer bajor -ID "5a9e8364-df7c-4f69-bd8f-4477f3eecc58" | where {$_.Name -eq "elk_template"}
$HardwareProfile = Get-SCHardwareProfile -VMMServer bajor | where {$_.Name -eq $guid4}

New-SCVMTemplate -Name $guid2 -Template $Template -HardwareProfile $HardwareProfile -JobGroup $guid3



$template = Get-SCVMTemplate -All | where { $_.Name -eq $guid2 }
$virtualMachineConfiguration = New-SCVMConfiguration -VMTemplate $template -Name $name
Write-Output $virtualMachineConfiguration
$vmHost = Get-SCVMHost -ID "c9d4689b-8444-4290-91f3-ea6489fc1391"
Set-SCVMConfiguration -VMConfiguration $virtualMachineConfiguration -VMHost $vmHost
Update-SCVMConfiguration -VMConfiguration $virtualMachineConfiguration

$AllNICConfigurations = Get-SCVirtualNetworkAdapterConfiguration -VMConfiguration $virtualMachineConfiguration



Update-SCVMConfiguration -VMConfiguration $virtualMachineConfiguration
New-SCVirtualMachine -Name $name -VMConfiguration $virtualMachineConfiguration -Description "" -BlockDynamicOptimization $false -StartVM -JobGroup $guid3 -ReturnImmediately -UseDiffDiskOptimization -StartAction "NeverAutoTurnOnVM" -StopAction "SaveVM"



return $name