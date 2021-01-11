rm -r bin/Packages/
mkdir bin/Packages

dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -r win-x86
dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -r win-x64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -r linux-x64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -r linux-arm
dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -r linux-arm64
dotnet publish -c Release -p:PublishSingleFile=true  --self-contained true -r osx-x64

zip -r -j bin/Packages/win-x86.zip bin/Release/net5.0/win-x86/publish/
zip -r -j bin/Packages/win-x64.zip bin/Release/net5.0/win-x64/publish/
zip -r -j bin/Packages/linux-x64.zip bin/Release/net5.0/linux-x64/publish/
zip -r -j bin/Packages/linux-arm.zip bin/Release/net5.0/linux-arm/publish/
zip -r -j bin/Packages/linux-arm64.zip bin/Release/net5.0/linux-arm64/publish/
zip -r -j bin/Packages/osx-x64.zip bin/Release/net5.0/osx-x64/publish/