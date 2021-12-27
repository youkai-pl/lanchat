runtimes=(win-x86 win-x64 linux-x64 linux-arm linux-arm64 osx-x64 osx-arm64)

rm -r bin/Packages/
mkdir bin/Packages

for rid in ${runtimes[@]};
do
    dotnet publish \
    -c Release \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    -p:GenerateDocumentationFile=false \
    --self-contained \
    -r $rid

    zip -r -j bin/Packages/Lanchat-$rid.zip bin/Release/net6.0/$rid/publish/
done