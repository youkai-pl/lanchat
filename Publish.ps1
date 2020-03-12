Write-Output "Preparing..."
$CurrentDir = (Get-Item -Path '.\' -Verbose)
$ILMergePath = "$env:USERPROFILE\.nuget\packages\ilmerge\3.0.29\tools\net452\ILMerge.exe"
$WindowsBuildPath = "$CurrentDir\Lanchat.Console\bin\Publish\Windows"
$7zipPath = "$env:ProgramFiles\7-Zip\7z.exe"

Set-Alias ILMerge $ILMergePath
Set-Alias 7zip $7zipPath

if (-not (Test-Path -Path $7zipPath -PathType Leaf)) {
    throw "7 zip file '$7zipPath' not found"
}

Write-Output "Cleaning..."

rm -r -fo "Lanchat.Console\bin\Publish"

Write-Output "Building..."

dotnet publish Lanchat.Console\Lanchat.Console.csproj /p:PublishProfile=Lanchat.Console\Properties\PublishProfiles\Windows.pubxml -f net472 -c Release -v m
dotnet publish Lanchat.Console\Lanchat.Console.csproj /p:PublishProfile=Lanchat.Console\Properties\PublishProfiles\Linux.pubxml -f netcoreapp3.0 -c Release -v m
dotnet publish Lanchat.Console\Lanchat.Console.csproj /p:PublishProfile=Lanchat.Console\Properties\PublishProfiles\Mac.pubxml -f netcoreapp3.0 -c Release -v m

Write-Output "Merging..."

ILMerge /out:"$WindowsBuildPath\Lanchat.exe" "$WindowsBuildPath\Lanchat.exe" "$WindowsBuildPath\Colorful.Console.dll" "$WindowsBuildPath\Lanchat.Common.dll" "$WindowsBuildPath\Newtonsoft.Json.dll"

Write-Output "Removing unnecessary files..." 

Get-ChildItem -Path $WindowsBuildPath -Recurse | Remove-Item -Exclude "Lanchat.exe" -Recurse -force 

Write-Output "Packing..." 

$Version = (get-item -Path "$CurrentDir\Lanchat.Console\bin\Publish\Windows\Lanchat.exe").VersionInfo.FileVersion.ToString()

7zip a -r -bb "Lanchat.Console\bin\Publish\Lanchat_Windows_$Version.zip" "$CurrentDir\Lanchat.Console\bin\Publish\Windows\*"
7zip a -r -bb "Lanchat.Console\bin\Publish\Lanchat_Linux_$Version.zip" "$CurrentDir\Lanchat.Console\bin\Publish\Linux\*"
7zip a -r -bb "Lanchat.Console\bin\Publish\Lanchat_Mac_$Version.zip" "$CurrentDir\Lanchat.Console\bin\Publish\Mac\*"

Write-Output "All done!!!" 