Param([string]$SolutionDir)
New-Item -ItemType Directory -Force -Path "$SolutionDir\TorchBinaries\Plugins\IdentityNameSync"
copy-item -path "$SolutionDir\IdentityNameSync\bin\Debug\*" -Destination "$SolutionDir\TorchBinaries\Plugins\IdentityNameSync" -Force