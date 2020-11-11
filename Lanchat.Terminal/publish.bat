dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r win-x86
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r win-x64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r linux-x64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r linux-arm
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r linux-arm64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r osx-x64

rename bin\Release\net5.0\win-x86\publish\Lanchat.exe Lanchat_win-x86.exe
rename bin\Release\net5.0\win-x64\publish\Lanchat.exe Lanchat_win-x64.exe
rename bin\Release\net5.0\linux-x64\publish\Lanchat Lanchat_linux-x64
rename bin\Release\net5.0\linux-arm\publish\Lanchat Lanchat_linux-arm
rename bin\Release\net5.0\linux-arm64\publish\Lanchat Lanchat_linux-arm64
rename bin\Release\net5.0\osx-x64\publish\Lanchat Lanchat_osx-x64