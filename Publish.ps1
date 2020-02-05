echo Cleaning

rm -r -fo "Lanchat.Console\bin\Publish"

echo Building

dotnet publish Lanchat.Console\Lanchat.Console.csproj /p:PublishProfile=Lanchat.Console\Properties\PublishProfiles\Windows.pubxml -f net472 -c Release
dotnet publish Lanchat.Console\Lanchat.Console.csproj /p:PublishProfile=Lanchat.Console\Properties\PublishProfiles\Linux.pubxml -f netcoreapp3.0 -c Release
dotnet publish Lanchat.Console\Lanchat.Console.csproj /p:PublishProfile=Lanchat.Console\Properties\PublishProfiles\Mac.pubxml -f netcoreapp3.0 -c Release

echo Packing

$7zipPath = "$env:ProgramFiles\7-Zip\7z.exe"

if (-not (Test-Path -Path $7zipPath -PathType Leaf)) {
    throw "7 zip file '$7zipPath' not found"
}

Set-Alias 7zip $7zipPath

$CurrentDir = (Get-Item -Path '.\' -Verbose)
$Version = [System.Reflection.Assembly]::LoadFrom("$CurrentDir\Lanchat.Console\bin\Publish\Windows\Lanchat.exe").GetName().Version.ToString()

7zip a -r "Lanchat.Console\bin\Publish\Lanchat_Windows_$Version.zip" "$CurrentDir\Lanchat.Console\bin\Publish\Windows\*"
7zip a -r "Lanchat.Console\bin\Publish\Lanchat_Linux_$Version.zip" "$CurrentDir\Lanchat.Console\bin\Publish\Linux\*"
7zip a -r "Lanchat.Console\bin\Publish\Lanchat_Mac_$Version.zip" "$CurrentDir\Lanchat.Console\bin\Publish\Mac\*"