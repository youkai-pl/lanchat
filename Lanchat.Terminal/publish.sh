rm -r bin/Packages/
mkdir bin/Packages

for rid in win-x86 win-x64 linux-x64 linux-arm linux-arm64 osx-x64 osx-arm64;
do
    dotnet publish \
    -c Release \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    -p:GenerateDocumentationFile=false \
    --self-contained true \
    -r $rid

    zip -r -j bin/Packages/$rid.zip bin/Release/net6.0/$rid/publish/
done