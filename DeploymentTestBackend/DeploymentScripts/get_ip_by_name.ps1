$ipAddress = (get-vm <name> |Get-VMNetworkAdapter)[0].ipaddresses[0]

while([string]::IsNullOrWhiteSpace($ipAddress)) {
    $ipAddress = (get-vm <name> |Get-VMNetworkAdapter)[0].ipaddresses[0]
}

return $ipAddress