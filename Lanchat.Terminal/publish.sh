runtimes=(win-x86 win-x64 linux-x64 linux-arm linux-arm64 osx-x64 osx-arm64)

rm -r bin/Packages/
mkdir bin/Packages

# Build self contained packages
for rid in ${runtimes[@]};
do
    dotnet publish \
    -c Release \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    -p:GenerateDocumentationFile=false \
    --self-contained \
    -r $rid

    zip -r -j bin/Packages/Lanchat-$rid-Self-Contained.zip bin/Release/net6.0/$rid/publish/
done

# Build runtime dependent packages
for rid in ${runtimes[@]};
do
    dotnet publish \
    -c Release \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    -p:GenerateDocumentationFile=false \
    --no-self-contained \
    -r $rid

    zip -r -j bin/Packages/Lanchat-$rid-Runtime-Dependent.zip bin/Release/net6.0/$rid/publish/
done